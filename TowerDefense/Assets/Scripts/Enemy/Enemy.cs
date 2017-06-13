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

    [HideInInspector]
    public float speed;

    [SerializeField]
    private GameObject deathEffect;

    [SerializeField]
    private Image healthBar;


    [SerializeField]
    private Enums.ArmorType armorType;
    #endregion

    private float health;
    private float respawn = 5;
    private bool spawn = true;

    private Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
        health = startHealth;
        speed = startSpeed;
        current = this;
        spawn = false;
    }

    private void TakeRealDamage(float amount, Enums.ArmorType aType)
    {
        if (aType == Enums.ArmorType.Light)
        {
        }
        if (aType == Enums.ArmorType.Magic)
        {
        }
        if (aType == Enums.ArmorType.Heavy)
        {
        }
    }

    public float GetHealth()
    {
        return health;
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        healthBar.fillAmount = health / startHealth; // change this to interpolation
        if (health <= 0)
        {
            Die();
        }
        if (spawn)
        {
            spawn = false;
        }
    }

    public static Enemy current;

    public bool GetRespawnTimer()
    {
        return spawn;
    }

    public void Slow(float pct)
    {
        //slows the enemy  speed * percentage 
        speed = startSpeed * (1f - pct);
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

        Invoke("SetSpawn", respawn);

        PlayerStarts.money += monsterHeadBounty;
    }

    private void SetSpawn()
    {
        spawn = true;
        anim.SetBool("IsDead", false);
        health = startHealth;
        gameObject.SetActive(false);
    }

    void OnParticleCollision(GameObject other)
    {
        //dont destroy the laser you have to destroy the particle. One by one
        //Destroy(other);
    }
}
