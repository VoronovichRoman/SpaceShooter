using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    static public Main S;
    static Dictionary<WeaponType, WeaponDefinition> WEAP_DICT;

    [Header("Set in Inspector")]
    public GameObject[] prefabEnemies;
    public float enemySpawnPerSecond = 0.5f;
    public float enemyDefaultPadding = 1.5f;
    public WeaponDefinition[] weaponDefinitions;
    public GameObject prefabPowerUp;
    public float levelTimer = 30f;
    public WeaponType[] powerUpFrequency = new WeaponType[] { WeaponType.blaster, WeaponType.blaster, WeaponType.spread, WeaponType.shield };
    private BoundsCheck bndCheck;
    private Text levelTimerText;
    public GameObject levelNumberGo;
    public GameObject pauseMenuPanel;
    public GameObject controlsMenu;
    public GameObject gameOverMenu;
    public GameObject flyOverCooldownGO;
    public Image flyOverCooldownImage;
    public GameObject firewaveCooldownGO;
    public Image firewaveCooldownImage;

    [Header("Set Dynamically")]
    public GameObject[] enemiesOnLevel;
    public int bossesCount = 0;
    public int aliveBossesCount;
    public int level;
    public bool levelTimerOn = true;
    public bool gameIsPaused = false;
    public void ShipDestroyed(Enemy e)
    {
        if (Random.value <= e.powerUpDropChance)
        {
            int ndx = Random.Range(0, powerUpFrequency.Length);
            WeaponType puType = powerUpFrequency[ndx];
            GameObject go = Instantiate(prefabPowerUp) as GameObject;
            PowerUp pu = go.GetComponent<PowerUp>();
            pu.SetType(puType);
            pu.transform.position = e.transform.position;
        }
        if (level % 10 == 0)
        {
            aliveBossesCount--;
        }
    }
    void Awake()
    {
        S = this;
        bndCheck = GetComponent<BoundsCheck>();
        gameOverMenu.SetActive(false);
        levelTimerText = GameObject.Find("Timer").GetComponent<Text>();
        flyOverCooldownImage = flyOverCooldownGO.GetComponent<Image>();
        firewaveCooldownImage = firewaveCooldownGO.GetComponent<Image>();
        Cursor.visible = false;
        pauseMenuPanel.SetActive(false);
        level = 0;
        LevelUp();
        WEAP_DICT = new Dictionary<WeaponType, WeaponDefinition>();
        foreach (WeaponDefinition def in weaponDefinitions)
        {
            WEAP_DICT[def.type] = def;
        }
    }
    public void SpawnEnemy()
    {
        int ndx = Random.Range(0, enemiesOnLevel.Length);
        GameObject go = Instantiate<GameObject>(enemiesOnLevel[ndx]);
        float enemyPadding = enemyDefaultPadding;
        if (go.GetComponent<BoundsCheck>() != null)
        {
            enemyPadding = Mathf.Abs(go.GetComponent<BoundsCheck>().radius);
        }
        Vector3 pos = Vector3.zero;
        float xMin = -bndCheck.camWidth + enemyPadding;
        float xMax = bndCheck.camWidth - enemyPadding;
        pos.x = Random.Range(xMin, xMax);
        pos.y = bndCheck.camHeight + enemyPadding;
        go.transform.position = pos;
        if (ThisIsTheBossLevel())
        {
            if (bossesCount > 0)
            {
                bossesCount--;
                Invoke("SpawnEnemy", 1f / enemySpawnPerSecond);
            }
        }
        else if (levelTimer > 3)
        {
            Invoke("SpawnEnemy", 1f / enemySpawnPerSecond);
        }
    }
    public void DelayedRestart(float delay)
    {
        Invoke("Restart", delay);
    }
    public void Restart()
    {
        Player.scoreCount = 0;
        SceneManager.LoadScene("Game");
    }
    static public WeaponDefinition GetWeaponDefinition(WeaponType wt)
    {
        if (WEAP_DICT.ContainsKey(wt))
        {
            return (WEAP_DICT[wt]);
        }
        return (new WeaponDefinition());
    }
    public void PauseMenu()
    {
        gameIsPaused = !gameIsPaused;
        if (gameIsPaused)
        {
            Time.timeScale = 0;
            Cursor.visible = true;
        }
        else
        {
            Time.timeScale = 1;
            Cursor.visible = false;
        }
        pauseMenuPanel.SetActive(gameIsPaused);
        if (gameIsPaused)
        {
            controlsMenu.SetActive(false);
        }
    }
    public void EndGame()
    {
        Destroy(GameObject.Find("SoundManager"));
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }
    public void GameOver()
    {
        Cursor.visible = true;
        string scoreShow = Player.scoreCount.ToString();
        string highScoreShow = Player.highScore.ToString();
        Text gameResults = gameOverMenu.GetComponentInChildren<Text>();
        if (Player.scoreCount >= 1000000)
        {
            float score = Player.scoreCount;
            scoreShow = score / 1000 + "k";
        }
        if (Player.highScore >= 1000000)
        {
            float highScore = Player.highScore;
            highScoreShow = highScore / 1000 + "k";
        }
        gameOverMenu.SetActive(true);
        if (Player.scoreCount > Player.playersBestScore)
        {
            Player.playersBestScore = Player.scoreCount;
        }
        if (Player.scoreCount > Player.highScore)
        {
            Player.highScore = Player.scoreCount;
            gameResults.text = "GAME OVER\n\nNew High Score!!!\n" + scoreShow;
        }
        else
        {
            gameResults.text = "GAME OVER\n\nYour Score: " + scoreShow + "\nHigh Score: " + highScoreShow;
        }
        if (level > Player.playersBestLevel)
        {
            Player.playersBestLevel = level;
        }
        if (level > Player.maxLevel)
        {
            Player.maxLevel = level;
            gameResults.text += "\n\nNew Max Level!!!\n" + level;
        }
        else
        {
            gameResults.text += "\n\nYour Level: " + level + "\nMax Level: " + Player.maxLevel;
        }
    }
    public void LevelUp()
    {
        level++;
        levelTimer = 30f;
        levelTimerOn = true;

        if (level % 2 == 0)
        {
            enemySpawnPerSecond += 0.15f;
        }
        if (ThisIsTheBossLevel())
        {
            for (int i = 0; i < enemiesOnLevel.Length; i++)
            {
                enemiesOnLevel[i] = prefabEnemies[9];
            }
            bossesCount = level / 10 - 1;
            aliveBossesCount = level / 10;
        }
        else
        {
            enemiesOnLevel = new GameObject[level % 10];
            for (int i = 0; i < enemiesOnLevel.Length; i++)
            {
                enemiesOnLevel[i] = prefabEnemies[i];
            }
        }
        Invoke("SpawnEnemy", 1f / enemySpawnPerSecond);
    }
    public void LevelTimer()
    {
        if (levelTimerOn)
        {
            if (levelTimer > 0)
            {
                UpdateLevelNumberText();
                levelTimer -= Time.deltaTime;
                UpdateTimerText();
            }
            else
            {
                if (ThisIsTheBossLevel())
                {
                    Hero.S.shieldLevel -= 2; //Hero.S.LoseOneLife();
                    levelTimer = 30f;
                }
                else
                {
                    levelTimerOn = false;
                    foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
                    {
                        Destroy(enemy);
                    }
                    LevelUp();
                }
            }
            if (levelTimer < 27 && level % 10 == 0 && aliveBossesCount <= 0)
            {
                LevelUp();
            }
        }
    }
    public void UpdateTimerText()
    {
        if (levelTimer < 0)
        {
            levelTimer = 0;
        }

        float minutes = Mathf.FloorToInt(levelTimer / 60);
        float seconds = Mathf.FloorToInt(levelTimer % 60);
        levelTimerText.text = string.Format("{0:00} : {1:00}", minutes, seconds);
    }
    public void UpdateLevelNumberText()
    {
        if (levelTimer < 27)
        {
            levelNumberGo.SetActive(false);
        }
        else
        {
            levelNumberGo.SetActive(true);
            if (ThisIsTheBossLevel())
            {
                levelNumberGo.GetComponent<Text>().text = "Boss Level!!!\n If the timer reaches zero and\nthe enemies are still alive,\nyou will take 2 damage!";
            }
            else
            {
                levelNumberGo.GetComponent<Text>().text = "Level: " + level;
            }
        }
    }
    public bool ThisIsTheBossLevel()
    {
        if (level % 10 == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

