using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using DragonBones;
using UnityEngine;
using UnityEngine.Serialization;
using Assets.Scripts;
using Transform = UnityEngine.Transform;

public class Turret : MonoBehaviour
{
    #region Inspector
    [Header("General")]
    [SerializeField]
    private float range = 15f;

    [SerializeField]
    private bool groundType;

    [SerializeField]
    private bool airType;

    [SerializeField]
    private Enums.WeaponType waeponType;

    [Header("Bullets(Default)")]
    [SerializeField]
    private float fireRate = 1f;

    [SerializeField]
    private GameObject bullet;

    [Header("Laser")]
    [SerializeField]
    private bool useLaser = false;

    [SerializeField]
    private float slowAmount = .5f;

    [SerializeField]
    private int damageOverTime = 30;

    //[SerializeField]
    //private LineRenderer lineRenderer;

    [SerializeField]
    private ParticleSystem impactEffect;

    //[SerializeField]
    //private Light impactlight;

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
    private int particleSystemCounter = 0;
    private Transform startPosition;
    private Transform bulletHolder;
    private Transform objectPool;
    private List<Component> laserColliders = new List<Component>();
    private bool startwave = true;
    private GameObject[] enemysThisWave;

    void Start()
    {
        startPosition = GameObject.FindGameObjectWithTag(ConstNames.Start).transform;
        bulletHolder = gameObject.transform.Find("BulletHolder");
        objectPool = GameObject.FindGameObjectWithTag("ObjectPool").transform;

        if (!useLaser)
        {
            var laserHolder = partToRotate.gameObject.transform.Find("LaserObjectHolder");
            laserHolder.gameObject.SetActive(false);
        }
    }

    void UpdateTarget()
    {
        //find all gameobjects with the wanted tag 
        //this will you need maybe if you want to set an enemy type to prior heavy > fast
        // GameObject[] enemies = GameObject.FindGameObjectsWithTag(ConstNames.Enemy);
        //sets the shortesDistenace to infiity. you get here he max float number
        float shortestDistance = Mathf.Infinity;
        //sets the nearest enemy
        GameObject nearestEnemy = null;
        //search throught the enmies array
        if (enemysThisWave != null)
        {
            foreach (GameObject enemy in enemysThisWave)
            {
                if (enemy.GetComponent<Enemy>().GetHealth() > 0 && enemy.gameObject.activeInHierarchy)
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
            }
        }

        if (nearestEnemy != null && shortestDistance <= range)
        {
            if (groundType == nearestEnemy.GetComponent<Enemy>().GetGroundType())
            {
                //set the transform from the nearestEnemys to the target
                target = nearestEnemy.transform;
            }
            if (airType == nearestEnemy.GetComponent<Enemy>().GetAirType())
            {
                //set the transform from the nearestEnemys to the target
                target = nearestEnemy.transform;
            }
        }
        else
        {
            target = null;
        }
    }

    void Update()
    {
        if (WaveManager.EnemysInScene > 0 && startwave)
        {
            enemysThisWave = GameObject.FindGameObjectsWithTag(ConstNames.Enemy);
            startwave = false;
            InvokeRepeating("UpdateTarget", 0, 0.1f);
        }

        if (WaveManager.EnemysInScene == 0 && !startwave)
        {
            startwave = true;
            LockOnTarget(startPosition);
            enemysThisWave = null;
            CancelInvoke("UpdateTarget");
            target = null;
        }

        if (target != null)
        {
            LockOnTarget(target);

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
        else
        {
            var bulletHolder = gameObject.transform.Find("BulletHolder");
            if (useLaser)
            {
                //       if (lineRenderer.enabled)
                //{
                //  lineRenderer.enabled = false;
                //use play for particle effect or they will just despawn 
                //         impactEffect.Stop();
                // impactlight.enabled = false;
                //      LaserGameObject.SetActive(false);
                //  }
                //  lineRenderer.SetPosition(0, transform.position);
                //  lineRenderer.SetPosition(1, transform.position);
                impactEffect.Stop();
                // impactlight.enabled = false;
                LaserGameObject.SetActive(false);
            }
            if (!useLaser && bulletHolder.childCount > 0)
            {
                for (int i = 0; i < bulletHolder.childCount; i++)
                {
                    bulletHolder.GetChild(i).gameObject.SetActive(false);
                }
            }
            UpdateTarget();
        }
    }

    void LockOnTarget(Transform _target)
    {
        //gives the current dir  sub target from current
        Vector3 dir = _target.position - transform.position;
        dir.y = dir.y - 5f;

        //saves the dir as quaternion
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        //with lerp you can make things smoother
        //with  Time.deltaTime * turnSpeed you make the rotation turn in seconds
        //The three angles giving the three rotation matrices are called Euler angles
        Vector3 rotation =
            Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        partToRotate.rotation = Quaternion.Euler(rotation.x, rotation.y, 0f);
    }

    void Laser()
    {
        BoxCollider targetCollider = target.GetComponent<BoxCollider>();
        ParticleSystem ptsystem = LaserGameObject.GetComponent<ParticleSystem>();

        for (int i = 0; i < ptsystem.trigger.maxColliderCount; i++)
        {
            laserColliders.Add(ptsystem.trigger.GetCollider(i));
        }

        if (!laserColliders.Contains(target.GetComponent<BoxCollider>()))
        {
            ptsystem.trigger.SetCollider(particleSystemCounter++, targetCollider);
        }

        LaserGameObject.SetActive(true);
        target.GetComponent<Enemy>().GetDamage(damageOverTime * Time.deltaTime, waeponType);

        target.GetComponent<EnemyMovement>().Slow(slowAmount);

        //  if (!lineRenderer.enabled)
        //if (LaserGameObject.activeInHierarchy)
        //{
        //     lineRenderer.enabled = true;
        if (!impactEffect.isPlaying)
        {
            impactEffect.Play();
        }
        // impactlight.enabled = true;
        // }

        //  lineRenderer.SetPosition(0, firePoint.position);
        // lineRenderer.SetPosition(1, target.position);

        // Vector3 dir = firePoint.position - target.position;

        var offSett = new Vector3(0, -2f, 0);

        impactEffect.transform.position = target.position + offSett;
        // impactEffect.transform.rotation = Quaternion.LookRotation(dir);
    }

    void Shoot()
    {
        if (bullet == null)
            return;
        GameObject bulletObj = ObjectPool_Zaim.current.GetPoolObject(bullet.name);

        if (bulletObj == null)
            return;

        bulletObj.transform.position = firePoint.position;
        bulletObj.transform.rotation = firePoint.rotation;

        bulletObj.transform.parent = bulletHolder.transform;
        bulletObj.SetActive(true);

        Bullet b = bulletObj.GetComponent<Bullet>();
        if (b != null)
            b.Seek(target, waeponType);
    }

    public void RejectAmmo()
    {
        CancelInvoke("UpdateTarget");
        if (!useLaser)
        {
            for (int i = 0; i < bulletHolder.childCount; i++)
            {
                bulletHolder.GetChild(i).parent = objectPool;
            }
        }
    }

    //draw a gizmo when the gameobject is selected to see the range from the turret
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        //draw a DrawWireSphere  
        Gizmos.DrawWireSphere(transform.position, range);
    }
}