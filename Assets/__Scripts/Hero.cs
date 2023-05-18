using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    static public Hero S;

    [Header("Set in Inspector")]
    public float speed = 30;
    public float rollMult = -45;
    public float pitchMult = 30;
    public float gameRestartDelay = 2f;
    public GameObject projectilePrefab;
    public float projectileSpeed = 40f;
    public Weapon[] weapons;
    public float flipDuration = 3f;
    public float waveFireDelay = 5f;

    [Header("Set Dynamically")]
    [SerializeField]
    private float _shieldLevel;
    public int livesCount;
    public bool flipIsActive = false;
    public float flipCooldown = 5f;
    public float flipTime;
    public float waveCooldown = 5f;
    public static float immortalityDuration = 5f;
    public bool immortalityIsActive = false;
    GameObject goImmortality;
    private GameObject lastTriggerGo = null;
    WeaponType reservedWeapon = WeaponType.blaster;
    public delegate void WeaponFireDelegate();
    public WeaponFireDelegate fireDelegate;
    public float tempWeaponDelay;
    WeaponType tempWeaponType;
    public int tempWeaponsCount = 0;
    public bool autoAttackIsOn = false;
    public bool autoAttackSave = false;
    void Start()
    {
        if (S == null)
        {
            S = this;
        }
        else
        {
            Debug.LogError("Hero.Awake() - Attempted to assign second Hero.S!");
        }

        goImmortality = GameObject.Find("ImmortalSphere");
        immortalityIsActive = true;

        livesCount += Player.startLivesCount;
        flipTime = 0f;

        ClearWeapons();
        weapons[0].SetType(WeaponType.blaster);

        FirewaveCooldownShow(waveCooldown);
        FlyOverCooldownShow(flipCooldown);
    }
    void Update()
    {
        if (immortalityIsActive)
        {
            ImmortalityMode();
        }


        if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.C))
        {
            autoAttackIsOn = !autoAttackIsOn;
        }

        Main.S.LevelTimer();

        SoundEffectsMuteCheck();

        float xAxis = Input.GetAxis("Horizontal");
        float yAxis = Input.GetAxis("Vertical");
        Vector3 pos = transform.position;
        pos.x += xAxis * speed * Time.deltaTime;
        pos.y += yAxis * speed * Time.deltaTime;
        transform.position = pos;
        transform.rotation = Quaternion.Euler(yAxis * pitchMult, xAxis * rollMult, 0);

        if (Input.GetAxis("Fire1") == 1 && fireDelegate != null)
        {
            fireDelegate();
            autoAttackIsOn = false;
        }

        flipCooldown -= Time.deltaTime;
        if (flipCooldown >= 0)
        {
            FlyOverCooldownShow(flipCooldown);
        }
        else
        {
            Main.S.flyOverCooldownGO.SetActive(false);
        }

        if (Input.GetAxis("Jump") == 1 && !flipIsActive && flipCooldown <= 0)
        {
            flipIsActive = true;
            if (autoAttackIsOn)
            {
                autoAttackIsOn = false;
                autoAttackSave = true;
            }
            AudioSource flyOverSound = GameObject.Find("Shield").GetComponent<AudioSource>();
            flyOverSound.clip = Resources.Load<AudioClip>("Sound/heroFlyOverSound");
            flyOverSound.Play();
        }
        if (flipIsActive)
        {
            Flip();
        }
        waveCooldown -= Time.deltaTime;
        if (waveCooldown >= 0)
        {
            FirewaveCooldownShow(waveCooldown);
        }
        else
        {
            Main.S.firewaveCooldownGO.SetActive(false);
        }

        if (Input.GetAxis("Fire2") == 1 && Player.fireWavesCount > 0 && waveCooldown <= 0 && !flipIsActive)
        {
            TempWeaponSet();
            weapons[0].SetType(WeaponType.fireWave);
            fireDelegate();
            Player.fireWavesCount--;
            waveCooldown = waveFireDelay;
            tempWeaponDelay = 1.5f;
            ClearWeapons();
            Main.S.firewaveCooldownGO.SetActive(true);
        }

        tempWeaponDelay -= Time.deltaTime;
        if (weapons[0].type == WeaponType.none && tempWeaponDelay <= 0 && !flipIsActive)
        {
            TempWeaponGet();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Main.S.PauseMenu();
        }

        if (autoAttackIsOn)
        {
            fireDelegate();
        }
    }
    void OnTriggerEnter(Collider other)
    {
        Transform rootT = other.gameObject.transform.root;
        GameObject go = rootT.gameObject;

        if (go == lastTriggerGo)
        {
            return;
        }
        lastTriggerGo = go;
        if (go.tag == "Enemy")
        {
            if (flipIsActive || immortalityIsActive)
            {
                lastTriggerGo = null;
            }
            else
            {
                shieldLevel--;
                if (!Main.S.ThisIsTheBossLevel())
                {
                    Destroy(go);
                }
            }
        }
        else if (go.tag == "PowerUp")
        {
            if (!flipIsActive)
            {
                AbsorbPowerUp(go);
            }
            lastTriggerGo = null;
        }
        else
        {
            print("Triggered by non-Enemy:" + go.name);
        }
    }
    public void AbsorbPowerUp(GameObject go)
    {
        PowerUp pu = go.GetComponent<PowerUp>();
        switch (pu.type)
        {
            case WeaponType.shield:
                shieldLevel++;
                break;

            case WeaponType.coins:
                Player.coinsCount += 10;
                break;

            default:
                if (pu.type == weapons[0].type)
                {
                    Weapon w = GetEmptyWeaponSlot();
                    if (w != null)
                    {
                        w.SetType(pu.type);
                    }
                    else
                    {
                        Player.coinsCount += 20;
                    }
                }
                else
                {
                    ClearWeapons();
                    weapons[0].SetType(pu.type);
                }
                reservedWeapon = pu.type;
                break;
        }

        pu.AbsorbedBy(this.gameObject);
    }
    public float shieldLevel
    {
        get
        {
            return (_shieldLevel);
        }
        set
        {
            _shieldLevel = Mathf.Min(value, 4);
            if (value > 4)
            {
                Player.coinsCount += 20;
            }
            if (value < 0)
            {
                LoseOneLife();
            }
        }
    }
    Weapon GetEmptyWeaponSlot()
    {
        for (int i = 0; i < Player.weaponsCount; i++)
        {
            if (weapons[i].type == WeaponType.none)
            {
                return (weapons[i]);
            }
        }
        return (null);
    }
    void ClearWeapons()
    {
        foreach (Weapon w in weapons)
        {
            w.SetType(WeaponType.none);
        }
    }
    void Flip()
    {
        if (weapons[0].type != WeaponType.none)
        {
            TempWeaponSet();
            ClearWeapons();
        }
        if (flipTime < flipDuration)
        {
            flipTime += Time.deltaTime;
            Vector3 flipScale = transform.localScale;
            if (flipTime < flipDuration / 2)
            {
                flipScale.x += 0.01f;
                flipScale.y += 0.01f;
                transform.localScale = flipScale;
            }
            else
            {
                flipScale.x -= 0.01f;
                flipScale.y -= 0.01f;
                transform.localScale = flipScale;
            }
        }
        else
        {
            TempWeaponGet();
            transform.localScale = Vector3.one;
            flipIsActive = false;
            flipCooldown = 5f;
            flipTime = 0;
            if (autoAttackSave)
            {
                autoAttackIsOn = true;
                autoAttackSave = false;
            }
            Main.S.flyOverCooldownGO.SetActive(true);
        }

    }
    public void FlyOverCooldownShow(float timeLeft)
    {
        float normalizedValue = Mathf.Clamp(timeLeft / 5f, 0.0f, 1.0f);
        Main.S.flyOverCooldownImage.fillAmount = normalizedValue;
    }
    public void FirewaveCooldownShow(float timeLeft)
    {
        float normalizedValue = Mathf.Clamp(timeLeft / waveFireDelay, 0.0f, 1.0f);
        Main.S.firewaveCooldownImage.fillAmount = normalizedValue;
    }
    void ImmortalityMode()
    {
        immortalityDuration -= Time.deltaTime;
        if (immortalityDuration >= 0)
        {
            if (immortalityDuration <= 2f && goImmortality.activeInHierarchy)
            {
                goImmortality.SetActive(false);
            }
            else
            {
                goImmortality.SetActive(true);
            }
        }
        else
        {
            goImmortality.SetActive(false);
            immortalityIsActive = false;
            immortalityDuration = 5f;
        }
    }
    void SoundEffectsMuteCheck()
    {
        if (SoundManager.soundIsMuted)
        {
            GetComponent<AudioSource>().mute = true;
            GameObject.Find("Shield").GetComponent<AudioSource>().mute = true;
        }
        else
        {
            GetComponent<AudioSource>().mute = false;
            GameObject.Find("Shield").GetComponent<AudioSource>().mute = false;
        }
    }
    public void LoseOneLife()
    {
        ClearWeapons();
        weapons[0].SetType(reservedWeapon);
        _shieldLevel = 1;
        livesCount--;
        goImmortality.SetActive(true);
        immortalityIsActive = true;

        if (livesCount <= 0)
        {
            Destroy(this.gameObject);
            Main.S.GameOver();
        }
    }
    public void TempWeaponSet()
    {
        if (weapons[0].type != WeaponType.none)
        {
            for (int i = 0; i < weapons.Length; i++)
            {
                if (weapons[i].type == weapons[0].type)
                {
                    tempWeaponsCount++;
                }
            }
            tempWeaponType = weapons[0].type;
        }
    }
    public void TempWeaponGet()
    {
        for (int i = 0; i < tempWeaponsCount; i++)
        {
            weapons[i].SetType(tempWeaponType);
        }
        tempWeaponsCount = 0;
    }
}