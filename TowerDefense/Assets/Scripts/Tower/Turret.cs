using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.Serialization;

public class Turret : MonoBehaviour
{
    #region Inspector
    [Header("Attribute")]
    [SerializeField]
    private float range = 15f;
    [SerializeField]
    private float fireRate = 1f;
    [SerializeField]
    private float turnSpeed = 10f;
    [SerializeField]
    private GameObject gobjName;
    [Header("Unity Setup Fileds")]
    [SerializeField]
    private string enemyTag = "Enemy";
    [SerializeField]
    private Transform partToRotate;
    [SerializeField]
    private Transform firePoint;
    [SerializeField]
    private bool canGrow = false;

    #endregion

    private LineRenderer tracer;

    private float fireCoundown = 0f;
    private Transform target;
    void Start()
    {
        //updates the target
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
            tracer = GetComponent<LineRenderer>();
    }

    void UpdateTarget()
    {
        //find all gameobjects with the wanted tag 
        //this will you ned maybe of you want to set an enemy type to prior heavy > fast
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
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
        }
        else
        {
            target = null;
        }
    }

    void Update()
    {
        if (target == null)
            return;

        //gives the current dir  sub target from current
        Vector3 dir = target.position - transform.position;
        //saves the dir as quaternion
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        //with lerp you can make things smoother
        //with  Time.deltaTime * turnSpeed you make the rotation turn in seconds
        //The three angles giving the three rotation matrices are called Euler angles
        Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        //set the partToRotate.rotation  with the euler. Watch out Euler rotates  X Y and Z !! 
        partToRotate.rotation = Quaternion.Euler(0F, rotation.y, 0F);

        if (fireCoundown <= 0f)
        {
            Shoot();
            //why 1/1 ? this dont make sense try somewthing out with the variables!!
            fireCoundown = 1f / fireRate;
        }
        //subs a seconds away frm the firecoundown
        fireCoundown -= Time.deltaTime;
    }

    void Shoot()
    {

        if (gobjName.name == "Laser")
        {
            LaserGun();
        }
        else
        {
            ProjectileGun();
        }
    }
    public float shotDistance = 200;

    public void LaserGun()
    {
        Ray ray = new Ray(firePoint.position, firePoint.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, shotDistance))
        {
            shotDistance = hit.distance;
        }


        if (tracer)
        {
            StartCoroutine("RenderTracer", ray.direction * shotDistance);
        }

        Debug.DrawRay(ray.origin, ray.direction * shotDistance, Color.red, 3);
        //    Debug.DrawLine(firePoint.position,s);
    }

    IEnumerator RenderTracer(Vector3 hitPoint)
    {
        tracer.enabled = true;
        tracer.SetPosition(0, firePoint.position);
        tracer.SetPosition(1, firePoint.position + hitPoint);

        yield return null;
        tracer.enabled = false;
    }

    private void ProjectileGun()
    {
        GameObject bulletObj = ObjectPool_Zaim.current.GetPoolObject(gobjName.name, canGrow);

        if (bulletObj == null)
            return;

        bulletObj.transform.position = firePoint.position;
        bulletObj.transform.rotation = firePoint.rotation;
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
