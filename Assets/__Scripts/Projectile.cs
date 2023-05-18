using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public BoundsCheck bndCheck;
    private Renderer rend;

    [Header("Set Dynamically")]
    public Rigidbody rigid;
    public float laserTimer = 0f;
    public AudioSource weaponSound;

    [SerializeField]
    private WeaponType _type;
    public WeaponType type
    {
        get
        {
            return _type;
        }
        set
        {
            SetType(value);
        }
    }
    void Awake()
    {
        bndCheck = GetComponent<BoundsCheck>();
        rend = GetComponent<Renderer>();
        rigid = GetComponent<Rigidbody>();
    }
    public void Start()
    {
        if (Hero.S != null)
        {
            weaponSound = Hero.S.GetComponent<AudioSource>();
            weaponSound.clip = Hero.S.weapons[0].def.shotSound;
            if (Hero.S.weapons[0].type == WeaponType.laser)
            {
                if (!Hero.S.GetComponent<AudioSource>().isPlaying)
                {
                    weaponSound.Play();
                }
            }
            else
            {
                weaponSound.Play();
            }
        }
    }
    void Update()
    {
        if (this._type == WeaponType.fireWave)
        {
            if (this.rigid.velocity.y < 50)
            {
                this.rigid.velocity = Vector3.zero;
                this.rigid.velocity = Vector3.up * 50;
            }
        }
        if (this._type == WeaponType.laser)
        {
            laserTimer += Time.deltaTime;
            if ((Input.GetAxis("Fire1") == 0 && !Hero.S.autoAttackIsOn) || laserTimer > Hero.S.weapons[0].def.delayBetweenShots)
            {
                Destroy(gameObject);
                laserTimer = 0;
            }
        }
        if (bndCheck.offUp)
        {
            Destroy(gameObject);
        }
    }
    public void SetType(WeaponType eType)
    {
        _type = eType;
        WeaponDefinition def = Main.GetWeaponDefinition(_type);
        rend.material.color = def.projectileColor;
    }
}
