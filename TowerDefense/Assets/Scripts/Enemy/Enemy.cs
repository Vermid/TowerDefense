using System;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    #region Inspector

    [Header("Attributes")]
    [SerializeField]
    public float startSpeed = 10;

    [SerializeField]
    private float health = 100;

    [SerializeField]
    private int monsterHeadBounty = 50;

    [HideInInspector]
    public float speed;

    [SerializeField]
    private GameObject deathEffect;

    [SerializeField]
    private Image healthBar;
    #endregion

    private float startHealth;
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

        healthBar.fillAmount = health/startHealth; // change this to interpolation
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


        health = startHealth;

        //   StartCoroutine(RespawnTimer());
        Invoke("SetSpawn", respawn);

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

    //void OnParticleCollision(GameObject other)
    //{
    //    Debug.Log("OnParticleCollision");
    //    //   Destroy(laserEffect);
    //}

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter");
        Debug.Log(other.gameObject.name);
        //        Destroy(other);
    }

    //void OnCollisionEnter(Collision other)
    //{
    //    Debug.Log("OnCollisionEnter");
    //    Debug.Log(other.gameObject.name);

    //}

}
