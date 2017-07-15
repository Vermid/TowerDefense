using System;
using System.Collections;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    #region Inspector

    [Header("Attributes")]
    [SerializeField]
    public float startSpeed = 10;

    [SerializeField]
    private float startHealth = 100;

    [SerializeField]
    private int monsterHeadBounty = 50;

    [SerializeField]
    private float healthSpeed = 100;

    [HideInInspector]
    public float speed = 0;

    [SerializeField]
    private GameObject deathEffect;

    [SerializeField]
    private Image healthBar;

    [SerializeField]
    private bool groundType;
    [SerializeField]

    private bool airType;
    [SerializeField]
    private Enums.ArmorType armorType;
    #endregion

    public static Enemy current;

    private float fullDamage = 1;
    private float halfDamage = .5F;
    private float lowDamage = .25F;

    private float health;
    private float respawn = 5;
    private bool spawn = true;
    private Animator anim;
    private bool isDead = false;

    public EnemyMovement enemyMovement;
    void Start()
    {
        anim = GetComponent<Animator>();
        enemyMovement = GetComponent<EnemyMovement>();

        current = this;
        isDead = false;
        health = startHealth;
        speed = startSpeed;
       // spawn = false;
    }

    private void Awake()
    {
        isDead = false;
        health = startHealth;
        speed = startSpeed;
    }
    private void OnEnable()
    {
        isDead = false;
        //health = startHealth;
        //speed = startSpeed;
        // spawn = false;
    }

    public bool GetAirType()
    {
        return airType;
    }

    public bool GetGroundType()
    {
        return groundType;
    }

    public bool GetDamage(float amount, Enums.WeaponType wType)
    {
        switch (wType)
        {
            case Enums.WeaponType.Pierce:
                if (armorType == Enums.ArmorType.Light)
                {
                   return TakeDamage(amount * fullDamage);
                }
                else if (armorType == Enums.ArmorType.Heavy)
                {
                    return TakeDamage(amount * halfDamage);
                }
                else if (armorType == Enums.ArmorType.Magic)
                {
                    return TakeDamage(amount * lowDamage);
                }
                break;

            case Enums.WeaponType.Normal:
                if (armorType == Enums.ArmorType.Light)
                {
                    return TakeDamage(amount * halfDamage);
                }
                else if (armorType == Enums.ArmorType.Heavy)
                {
                    return TakeDamage(amount * fullDamage);
                }
                else if (armorType == Enums.ArmorType.Magic)
                {
                    return TakeDamage(amount * lowDamage);
                }
                break;

            case Enums.WeaponType.Chaotic:
                if (armorType == Enums.ArmorType.Light)
                {
                    return TakeDamage(amount * halfDamage);
                }
                else if (armorType == Enums.ArmorType.Heavy)
                {
                    return TakeDamage(amount * halfDamage);
                }
                else if (armorType == Enums.ArmorType.Magic)
                {
                    return TakeDamage(amount * fullDamage);
                }
                break;
        }
        return false;
    }

    private void Update()
    {
        var result = fillbar(health, 0, startHealth, 0, 1);

        if (result != healthBar.fillAmount)
            healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, result, Time.deltaTime * healthSpeed);
    }

    public float GetHealth()
    {
        return health;
    }

    public bool TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0 && !isDead)
        {
            Die();
            return isDead = true;
        }
        return false;
    }
    private float fillbar(float value, float inMin, float inMax, float outMin, float outMax)
    {
        return (value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
    }

    public bool GetRespawnTimer()
    {
        return spawn;
    }

    void Die()
    {
        GameObject gobj = ObjectPool_Zaim.current.GetPoolObject(deathEffect.name);

        if (gobj == null)
            return;

        gobj.transform.position = transform.position;
        gobj.transform.rotation = transform.rotation;
        gobj.SetActive(true);

        anim.SetBool("IsDead", true);
        StartCoroutine(SetSpawn(respawn));
        PlayerStarts.money += monsterHeadBounty;

        WaveManager.EnemysInScene--;
        enemyMovement.LastBreath();
    }

    IEnumerator SetSpawn(float time)
    {
        for (int i = 0; i <= 1; i++)
        {
            if (i == 1)
            {
                gameObject.SetActive(false);
                Invoke("ResetStats", .5f);
            }
            yield return new WaitForSeconds(time);
        }
    }

    private void ResetStats()
    {
        health = startHealth;
        healthBar.fillAmount = startHealth;
        spawn = true;
    }
}
