﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using DragonBones;
using UnityEngine;
using UnityEngine.Serialization;
using Transform = UnityEngine.Transform;

public class Turret : MonoBehaviour
{
    #region Inspector
    [Header("General")]
    [SerializeField]
    private float range = 15f;

    [Header("Bullets(Default)")]
    [SerializeField]
    private float fireRate = 1f;

    [SerializeField]
    private GameObject gobjName;

    [Header("Laser")]
    [SerializeField]
    private bool useLaser = false;

    [SerializeField]
    private float slowAmount = .5f;

    [SerializeField]
    private int damageOverTime = 30;

    [SerializeField]
    private LineRenderer lineRenderer;

    [SerializeField]
    private ParticleSystem impactEffect;

    [SerializeField]
    private Light impactlight;

    [SerializeField]
    private ParticleSystem laserEffect;

    [Header("Unity Setup Fields")]

    [SerializeField]
    private Transform partToRotate;

    [SerializeField]
    private Transform firePoint;

    [SerializeField]
    private float turnSpeed = 10f;
    #endregion

    public GameObject LaserGameObject;

    private float fireCoundown = 0f;
    private Transform target;
    private Enemy targetEnemy;

    void Start()
    {
        //updates the target
        InvokeRepeating("UpdateTarget", 0f, 0.1f);
        lineRenderer = GetComponent<LineRenderer>();

        if (!useLaser)
        {
            var laserHolder = partToRotate.gameObject.transform.Find("LaserObjectHolder");
            laserHolder.gameObject.SetActive(false);
        }
        else
        {
            var enemyList = GameObject.FindGameObjectsWithTag("Enemy");
            ParticleSystem ptsystem = LaserGameObject.GetComponent<ParticleSystem>();

            for (int i = 0; i < enemyList.Length; i++)
            {
                BoxCollider targetCollider = enemyList[i].GetComponent<BoxCollider>();
                ptsystem.trigger.SetCollider(i, targetCollider);
            }
        }
    }


    void UpdateTarget()
    {
        //find all gameobjects with the wanted tag 
        //this will you ned maybe of you want to set an enemy type to prior heavy > fast
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(ConstNames.Enemy);
        //sets the shortesDistenace to infiity. you get here he max float number
        float shortestDistance = Mathf.Infinity;
        //sets the nearest enemy
        GameObject nearestEnemy = null;
        //search throught the enmies array
        foreach (GameObject enemy in enemies)
        {
            //you get the distance if you sub the turret position with the enemy.position
            //this is the distance to enemy
            float distanceToEnemys = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemys < shortestDistance)
            {
                shortestDistance = distanceToEnemys;
                //set the found enemy to nearestEnemy
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null && shortestDistance <= range)
        {
            //set the transform from the nearestEnemys to the target
            target = nearestEnemy.transform;
            targetEnemy = nearestEnemy.GetComponent<Enemy>();
        }
        else
        {
            target = null;
        }
    }

    void Update()
    {
        if (target == null || targetEnemy != null && targetEnemy.GetHealth() <= 0)
        {
            if (useLaser)
            {
                if (lineRenderer.enabled)
                {
                    lineRenderer.enabled = false;
                    //use play for particle effect or they will just despawn 
                    impactEffect.Stop();
                    impactlight.enabled = false;
                    LaserGameObject.SetActive(false);
                }
            }
            return;
        }
        if (targetEnemy != null && targetEnemy.GetHealth() >= 0)
        {
            LockOnTarget();

            if (useLaser)
            {
                Laser();
            }
            else
            {
                if (fireCoundown <= 0f)
                {
                    Shoot();
                    //why 1/1 ? this dont make sense try somewthing out with the variables!!
                    fireCoundown = 1f / fireRate;
                }
                //subs a seconds(framerate) away frm the firecoundown
                fireCoundown -= Time.deltaTime;
            }
        }
    }

    void LockOnTarget()
    {
        if (targetEnemy.gameObject.activeInHierarchy)
        {
            //gives the current dir  sub target from current
            Vector3 dir = target.position - transform.position;
            //saves the dir as quaternion
            Quaternion lookRotation = Quaternion.LookRotation(dir);
            //with lerp you can make things smoother
            //with  Time.deltaTime * turnSpeed you make the rotation turn in seconds
            //The three angles giving the three rotation matrices are called Euler angles
            Vector3 rotation =
                Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
            //set the partToRotate.rotation  with the euler. Watch out Euler rotates  X Y and Z !! 
            partToRotate.rotation = Quaternion.Euler(0F, rotation.y, 0F);

        }
        else
        {
            //this looks like the right way to fix the bug 
            var bulletHolder = gameObject.transform.Find("BulletHolder");
            if (useLaser)
            {
                lineRenderer.SetPosition(0, transform.position);
                lineRenderer.SetPosition(1, transform.position);
                return;
            }
            if (bulletHolder.childCount > 0)
            {
                for (int i = 0; i < bulletHolder.childCount; i++)
                {
                    bulletHolder.GetChild(i).gameObject.SetActive(false);
                }
            }
        }
    }

    private int particleSystemCounter = 0;
    void Laser()
    {
        ParticleSystem ptsystem = LaserGameObject.GetComponent<ParticleSystem>();

        BoxCollider targetCollider = target.GetComponent<BoxCollider>();
        ptsystem.trigger.SetCollider(particleSystemCounter++, targetCollider);

        LaserGameObject.SetActive(true);

        targetEnemy.TakeDamage(damageOverTime * Time.deltaTime);
        targetEnemy.Slow(slowAmount);

        if (!lineRenderer.enabled)
        {
            lineRenderer.enabled = true;
            impactEffect.Play();
            impactlight.enabled = true;
        }

        lineRenderer.SetPosition(0, firePoint.position);
        lineRenderer.SetPosition(1, target.position);

        Vector3 dir = firePoint.position - target.position;

        var offsett = new Vector3(0, 2f, 0);

        impactEffect.transform.position = target.position + offsett; //+ dir.normalized;

        impactEffect.transform.rotation = Quaternion.LookRotation(dir);
    }

    void Shoot()
    {
        GameObject bulletObj = ObjectPool_Zaim.current.GetPoolObject(gobjName.name);

        if (bulletObj == null)
            return;

        bulletObj.transform.position = firePoint.position;
        bulletObj.transform.rotation = firePoint.rotation;

        //var bulletHolder = gameObject.transform.parent.FindChild("BulletHolder");
        var bulletHolder = gameObject.transform.Find("BulletHolder");

        bulletObj.transform.parent = bulletHolder.transform;
        bulletObj.SetActive(true);

        Bullet bullet = bulletObj.GetComponent<Bullet>();
        if (bullet != null)
            bullet.Seek(target);
    }


    //draw a gizmo when the gameobject is selected to see the range from the turret
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        //draw a DrawWireSphere  
        Gizmos.DrawWireSphere(transform.position, range);
    }
}