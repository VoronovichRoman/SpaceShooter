using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroPannelUI : MonoBehaviour
{
    GameObject goHeroScore;
    GameObject goHeroLives;
    GameObject goHeroCoins;
    GameObject goHeroFireWaves;
    GameObject goHighScore;
    GameObject goMaxLevel;
    GameObject goHeroLevel;
    Text score;
    Text lives;
    Text coins;
    Text fireWaves;
    Text highScore;
    Text maxLevel;
    Text level;
    public void Start()
    {
        goHeroScore = GameObject.Find("HeroScore");
        score = goHeroScore.GetComponent<Text>();
        goHeroLives = GameObject.Find("HeroLives");
        lives = goHeroLives.GetComponent<Text>();
        goHeroCoins = GameObject.Find("HeroCoins");
        coins = goHeroCoins.GetComponent<Text>();
        goHeroFireWaves = GameObject.Find("HeroFireWaves");
        fireWaves = goHeroFireWaves.GetComponent<Text>();
        goHighScore = GameObject.Find("HighScore");
        highScore = goHighScore.GetComponent<Text>();
        goMaxLevel = GameObject.Find("MaxLevel");
        maxLevel = goMaxLevel.GetComponent<Text>();
        goHeroLevel = GameObject.Find("CurrentLevel");
        level = goHeroLevel.GetComponent<Text>();
    }
    void Update()
    {
        if (GameObject.Find("PlayerButton_1"))
        {
            score.text = "Score: " + "0";
            lives.text = "Lives: " + "0";
            coins.text = "Coins: " + "0";
            fireWaves.text = "Fire Waves: " + "0";
            if (PlayerPrefs.HasKey("BestLevelPl_1") && PlayerPrefs.GetInt("BestLevelPl_1") > 0)
            {
                GameObject.Find("PlayerButton_1").GetComponentInChildren<Text>().text = "Best Score:\n" + PlayerPrefs.GetInt("BestScorePl_1") + "\nBest Level:\n" + PlayerPrefs.GetInt("BestLevelPl_1");
            }
            if (PlayerPrefs.HasKey("BestLevelPl_2") && PlayerPrefs.GetInt("BestLevelPl_2") > 0)
            {
                GameObject.Find("PlayerButton_2").GetComponentInChildren<Text>().text = "Best Score:\n" + PlayerPrefs.GetInt("BestScorePl_2") + "\nBest Level:\n" + PlayerPrefs.GetInt("BestLevelPl_2");
            }
            if (PlayerPrefs.HasKey("BestLevelPl_3") && PlayerPrefs.GetInt("BestLevelPl_3") > 0)
            {
                GameObject.Find("PlayerButton_3").GetComponentInChildren<Text>().text = "Best Score:\n" + PlayerPrefs.GetInt("BestScorePl_3") + "\nBest Level:\n" + PlayerPrefs.GetInt("BestLevelPl_3");
            }
        }
        else
        {
            fireWaves.text = "Fire Waves: " + Player.fireWavesCount;
            score.text = "Score: " + Player.scoreCount;
            coins.text = "Coins: " + Player.coinsCount;
            if (Hero.S != null)
            {
                lives.text = "Lives: " + Hero.S.livesCount;
            }
            else if (GameObject.Find("TitleText") != null)
            {
                lives.text = "Lives: " + Player.startLivesCount;
            }
            else
            {
                lives.text = "Lives: " + 0;
            }
        }
        highScore.text = "High Score: " + Player.highScore;
        maxLevel.text = "Max Level: " + Player.maxLevel;
        if (Main.S != null)
        {
            level.text = "Level: " + Main.S.level;
        }
    }
}
