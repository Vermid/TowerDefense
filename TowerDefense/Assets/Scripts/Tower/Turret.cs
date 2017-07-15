using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;
using Transform = UnityEngine.Transform;
using System.Collections;

public class Turret : MonoBehaviour
{
    #region Inspector
    [Header("General")]
    [SerializeField]
    private float range = 15f;

    [SerializeField]
    private float damage;
    [SerializeField]
    private bool groundType;

    [SerializeField]
    private bool airType;

    [SerializeField]
    private Enums.WeaponType weaponType;

    [SerializeField]
    private float fireRate = 1f;

    [SerializeField]
    private GameObject ammo;

    [SerializeField]
    private int cost;

    [SerializeField]
    private GameObject nextUpgrade;

    [SerializeField]
    private int upgradeCost;

    [Header("Laser")]
    [SerializeField]
    private bool useLaser = false;

    [SerializeField]
    private float slowAmount = .5f;

    [SerializeField]
    private ParticleSystem impactEffect;

    [SerializeField]
    private ParticleSystem laserEffect;

    //[SerializeField]
    //private Light impactlight;
    //[SerializeField]
    //private LineRenderer lineRenderer;

    [Header("Mine Factory")]
    [SerializeField]
    private bool useFactory = false;

    [SerializeField]
    private float timeBetweenSpawn = 5F;

    [SerializeField]
    private int maxMines = 5;

    [SerializeField]
    private int mineRadius = 5;

    [Header("Unity Setup Fields")]

    [SerializeField]
    private Transform partToRotate;

    [SerializeField]
    private Transform firePoint;

    [SerializeField]
    private float turnSpeed = 10f;
    #endregion

    // public GameObject LaserGameObject;
    private float fireCoundown = 0f;
    private Transform target;
    private int particleSystemCounter = 0;
    private Transform startPosition;
    private Transform bulletHolder;
    private Transform objectPool;
    private List<Component> laserColliders = new List<Component>();
    private bool startwave = true;
    private GameObject[] enemysThisWave;

    private float countDown = 2F;
    private Vector3 offsett;
    private float radius;
    private Transform[] targets;

    #region Prooooops
    public int Price { get { return cost; } }
    public int UpgradePrice { get { return upgradeCost; } }
    public float Range { get { return range; } }
    public float Damage { get { return damage; } }
    public float FireRate { get { return fireRate; } }
    public bool AirType { get { return airType; } }
    public bool GroundType { get { return groundType; } }
    public Enums.WeaponType WeaponType { get { return weaponType; } }
    #endregion

    void Start()
    {
        startPosition = GameObject.FindGameObjectWithTag(ConstNames.Start).transform;
        bulletHolder = gameObject.transform.Find("BulletHolder");
        objectPool = GameObject.FindGameObjectWithTag("ObjectPool").transform;

        if (!useLaser && !useFactory)
        {
            var laserHolder = partToRotate.gameObject.transform.Find("LaserObjectHolder");
            laserHolder.gameObject.SetActive(false);
            Bullet.damage = damage;
        }
        if (useFactory)
        {
            radius = GetComponentInChildren<SphereCollider>().radius = mineRadius;
            radius /= 2;
            Mine.damage = damage;
            SpawnPoint();
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
        if (useFactory)
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
        else
        {
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
                    ammo.SetActive(false);
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

    #region Gun
    void Shoot()
    {
        if (ammo == null)
            return;
        GameObject bulletObj = ObjectPool_Zaim.current.GetPoolObject(ammo.name);

        if (bulletObj == null)
            return;

        bulletObj.transform.position = firePoint.position;
        bulletObj.transform.rotation = firePoint.rotation;

        bulletObj.transform.parent = bulletHolder.transform;
        bulletObj.SetActive(true);

        Bullet b = bulletObj.GetComponent<Bullet>();
        if (b != null)
            b.Seek(target, weaponType, this);
    }
    #endregion

    #region Laser
    void Laser()
    {
        BoxCollider targetCollider = target.GetComponent<BoxCollider>();
        ParticleSystem ptsystem = ammo.GetComponent<ParticleSystem>();

        for (int i = 0; i < ptsystem.trigger.maxColliderCount; i++)
        {
            laserColliders.Add(ptsystem.trigger.GetCollider(i));
        }

        if (!laserColliders.Contains(target.GetComponent<BoxCollider>()))
        {
            ptsystem.trigger.SetCollider(particleSystemCounter++, targetCollider);
        }

        ammo.SetActive(true);
        target.GetComponent<Enemy>().GetDamage(damage * Time.deltaTime, weaponType);

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
    #endregion

    #region Mine Factory
    IEnumerator Spawn()
    {
        //call the funktion and pas the wanted enemysName into
        SpawnMine(ammo.name);
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
        if (obj == null)
            return;

        float offsetX = Random.Range(-radius, radius);
        float offsetZ = Random.Range(-radius, radius);

        offsett = new Vector3(offsetX, 0, offsetZ);

        obj.transform.position = bulletHolder.transform.position + offsett;
        obj.transform.rotation = bulletHolder.transform.rotation;
        obj.SetActive(true);

        obj.GetComponent<Mine>().SetWeapontType(weaponType);
        obj.transform.parent = bulletHolder;
    }
    #endregion

    #region General
    //draw a gizmo when the gameobject is selected to see the range from the turret
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        //draw a DrawWireSphere  
        Gizmos.DrawWireSphere(transform.position, range);
    }

    public void Kill()
    {
        killCounter++;
    }
    private int killCounter = 0;

    public int GetKillCounter()
    {
        return killCounter;
    }

    public GameObject GetUpgrade()
    {
        if (nextUpgrade != null)
        {
            return nextUpgrade;
        }
        else
        {
            return null;
        }
    }
    #endregion  
}