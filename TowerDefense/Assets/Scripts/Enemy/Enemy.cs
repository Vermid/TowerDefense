using System;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    public float startSpeed = 10;
    [SerializeField]
    private float health = 100;

    [SerializeField]
    private int monsterHeadBounty = 50;

    [SerializeField]
    private GameObject deathEffect;

    private float startHealth;
    [HideInInspector]
    public float speed;


    private float respawn = 5;
    private bool spawn = true;
    void Start()
    {
        startHealth = health;
        speed = startSpeed;
        current = this;
        spawn = false;
    }

    public void TakeDamage(float amount)
    {
        health -= amount;

        if (health <= 0)
        {
            Die();
        }
        if(spawn)
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


        health = startHealth;

     //   StartCoroutine(RespawnTimer());
        Invoke("SetSpawn",respawn);

        gameObject.SetActive(false);

        PlayerStarts.money += monsterHeadBounty;
    }

    IEnumerator RespawnTimer()
    {
        SetSpawn();
        yield return new WaitForSeconds(respawn);
    }

    private void SetSpawn()
    {
        spawn = true;
    }
}
