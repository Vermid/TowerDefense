using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyMovement : MonoBehaviour
{
    #region Inspector
    [SerializeField]
    private GameObject partToRotate;

    [SerializeField]
    private float changeDirectionSpeed;

    [SerializeField]
    private float resetSpeed;
    #endregion
    private Transform target;
    private int wavePointIndex = 0;
    private float speed = 10;
    private bool groundType = false;
    private bool airType = false;
    private bool moving = false;
    private Animator anim;
    private bool lastBreath = false;
    public Enemy enemy;

    void Start()
    {
        anim = GetComponent<Animator>();
        enemy = gameObject.GetComponent<Enemy>();

        if (enemy.GetGroundType())
        {
            groundType = true;
            target = Waypoints.GroundPoints[0];
        }
        else if (enemy.GetAirType())
        {
            airType = true;
            target = Waypoints.AirPoints[0];
        }
    }

    void OnEnable()
    {
        //this resets the Gameobject with the start Values
        if (groundType)
        {
            target = Waypoints.GroundPoints[0];
        }
        if (airType)
        {
            target = Waypoints.AirPoints[0];
        }
        wavePointIndex = 0;
        speed = enemy.startSpeed;
        moving = false;
    }

    public void SetMovement()
    {
        moving = true;
    }

    void Update()
    {
        if (moving)
        {
            anim.SetBool("isRun", true);
            if (lastBreath)
            {
                target = Waypoints.GroundPoints[wavePointIndex];
            }

            //when you0 sub the target place - the current you get the dir where you need to go
            Vector3 dir = target.position - transform.position;
            //add some movement with Translate.
            //normalize the dir before you * speed it or the end result can change and the Obejects move faster or slower
            transform.Translate(dir.normalized * (lastBreath ? (speed / 2) : speed) * Time.deltaTime, Space.World);
            //check if the distance is close so he can get the next wayPoint
            if (Vector3.Distance(transform.position, target.position) <= 0.3F && !lastBreath)
            {
                GetNextWayPoint();
            }

            Quaternion lookRotation = Quaternion.LookRotation(dir);
            Vector3 rotation = Quaternion.Lerp(partToRotate.transform.rotation, lookRotation, Time.deltaTime * changeDirectionSpeed).eulerAngles;
            partToRotate.transform.rotation = Quaternion.Euler(0F, rotation.y, 0F);

            if (enemy.GetHealth() <= 0 && Vector3.Distance(transform.position, target.position) <= 0.3F)
            {
                transform.Translate(Vector3.zero);
                lastBreath = false;
                moving = false;
                anim.SetBool("isRun", false);
            }
        }
        if (enemy.GetHealth() > 0)
        {
            lastBreath = false;
        }
    }

    public void Slow(float pct)
    {
        CancelInvoke("ResetSpeed");
        //slows the enemy  speed * percentage 
        speed = enemy.startSpeed * (1f - pct);

        Invoke("ResetSpeed", resetSpeed);
    }

    private void ResetSpeed()
    {
        speed = enemy.startSpeed;
    }

    void GetNextWayPoint()
    {
        if (groundType)
        {
            //if wavePoiintIndex is greater then the waypoints.points.lenght than the gameobject arrieved at the end and can be set to false
            if (wavePointIndex >= Waypoints.GroundPoints.Length - 1)
            {
                EndPath();
                return;
            }
            //if he isnt at the end increment the wavePointIndex
            wavePointIndex++;
            // and give him the next wavePoint
            target = Waypoints.GroundPoints[wavePointIndex];
        }
        if (airType)
        {
            //if wavePoiintIndex is greater then the waypoints.points.lenght than the gameobject arrieved at the end and can be set to false
            if (wavePointIndex >= Waypoints.AirPoints.Length - 1)
            {
                EndPath();
                return;
            }
            //if he isnt at the end increment the wavePointIndex
            wavePointIndex++;
            // and give him the next wavePoint
            target = Waypoints.AirPoints[wavePointIndex];
        }
    }

    void EndPath()
    {
        gameObject.SetActive(false);
        //WaveManager.EnemysInScene--;
        PlayerStarts.lives--;
        var pathStart = GameObject.FindGameObjectWithTag(ConstNames.Start);

        transform.position = pathStart.transform.position;
        wavePointIndex = 0;
        gameObject.SetActive(true);
        moving = true;
    }

    void LockOnTarget()
    {
        //gives the current dir  sub target from current
        Vector3 dir = target.position - transform.position;
        //saves the dir as quaternion
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        //with lerp you can make things smoother
        //with  Time.deltaTime * turnSpeed you make the rotation turn in seconds
        //The three angles giving the three rotation matrices are called Euler angles
        Vector3 rotation =
            Quaternion.Lerp(partToRotate.transform.rotation, lookRotation, Time.deltaTime * changeDirectionSpeed).eulerAngles;
        //set the partToRotate.rotation  with the euler. Watch out Euler rotates  X Y and Z !! 
        partToRotate.transform.rotation = Quaternion.Euler(0F, rotation.y, 0F);
    }

    public void LastBreath()
    {
        lastBreath = true;
    }
}