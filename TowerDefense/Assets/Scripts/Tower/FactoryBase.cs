﻿using UnityEngine;
using System.Collections;
using System.Linq;
using System.Runtime.Remoting;
using UnityEngine.UI;
using Assets.Scripts;

public class FactoryBase : MonoBehaviour
{
    #region Inspector
    [SerializeField]
    private GameObject mine;

    [Header("Attributes")]
    [SerializeField]
    private float timeBetweenSpawn = 5F;

    [SerializeField]
    public float damage = Mine.damage;

    [SerializeField]
    private int maxMines = 5;

    [SerializeField]
    private int mineRadius = 5;

    [SerializeField]
    private bool groundType;

    [SerializeField]
    private bool airType;

    [SerializeField]
    private Enums.WeaponType wType;

    #endregion

    private float countDown = 2F;
    private Transform bulletHolder;
    private Transform objectPool;
    private Vector3 offsett;
    private float radius;
    private Transform[] targets;

    void Start()
    {
        bulletHolder = transform.Find("BulletHolder").transform;
        objectPool = GameObject.FindGameObjectWithTag("ObjectPool").transform;

        radius = GetComponentInChildren<SphereCollider>().radius = mineRadius;
        radius /= 2;

        SpawnPoint();
    }
    void Update()
    {
        //spawn enemys only if the coundown reaches 0
        if (countDown <= 0F && bulletHolder.childCount < maxMines)
        {
            StartCoroutine(Spawn());
            //set the coundown back to any time you want  in this chase tmeBetweenWaves
            countDown = timeBetweenSpawn;
        }
        //count in seconds?
        countDown -= Time.deltaTime; //time past since last frames
    }

    IEnumerator Spawn()
    {
        //call the funktion and pas the wanted enemysName into
        SpawnMine(mine.name);
        //wait .5 seconds for the next Enemy
        yield return new WaitForSeconds(.5F);
    }

    /// <summary>
    /// This will set the mine around the next Waypoint
    /// </summary>
    void SpawnPoint()
    {
        if (groundType)
        {
            targets = Waypoints.GroundPoints;

            float distance = Mathf.Infinity;

            foreach (Transform waypoint in targets)
            {
                if (Vector3.Distance(waypoint.position, transform.position) <= distance)
                {
                    distance = Vector3.Distance(waypoint.position, transform.position);
                    bulletHolder.transform.position = waypoint.position;
                }
            }
        }
        if (airType)
        {
            targets = Waypoints.AirPoints;

            float distance = Mathf.Infinity;

            foreach (Transform waypoint in targets)
            {
                if (Vector3.Distance(waypoint.position, transform.position) <= distance)
                {
                    distance = Vector3.Distance(waypoint.position, transform.position);
                    bulletHolder.transform.position = waypoint.position;
                }
            }
        }
    }

    /// <summary>
    /// Spawns the wanted Object 
    /// </summary>
    /// <param name="name"></param>
    void SpawnMine(string name)
    {
        GameObject obj = ObjectPool_Zaim.current.GetPoolObject(name);
        {
            Debug.LogWarning(name + " is NULL");
            return;
        }
        float offsetX = Random.Range(-radius, radius);
        float offsetZ = Random.Range(-radius, radius);

        offsett = new Vector3(offsetX, 0, offsetZ);

        obj.transform.position = bulletHolder.transform.position + offsett;
        obj.transform.rotation = bulletHolder.transform.rotation;
        obj.SetActive(true);

       // obj.GetComponent<Mine>().SetWeapontType(wType,this);
        obj.transform.parent = bulletHolder;
    }

    public void RejectAmmo()
    {

        for (int i = 0; i < bulletHolder.childCount; i++)
        {
            bulletHolder.GetChild(i).parent = objectPool;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        if (bulletHolder != null)
            Gizmos.DrawWireSphere(bulletHolder.transform.position, mineRadius * 2);
    }
}
