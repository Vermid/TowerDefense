using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Enemy))]
public class EnemyMovement : MonoBehaviour
{
    [SerializeField]
    private GameObject partToRotate;

    [SerializeField]
    private float changeDirectionSpeed;

    private Enemy enemy;
    private Transform target;
    private int wavePointIndex = 0;

    private float speed = 10;
    void Start()
    {
        enemy = GetComponent<Enemy>();
    }
    void OnEnable()
    {
        //this resets the Gameobject with the start Values
        target = Waypoints.Points[0];
        wavePointIndex = 0;
    }

    void Update()
    {

        if (enemy.GetHealth() >= 0)
        {
            //when you sub the target place - the current you get the dir where you need to go
            Vector3 dir = target.position - transform.position;
            //add some movement with Translate.
            //normalize the dir before you * speed it or the end result can change and the Obejects move faster or slower
            transform.Translate(dir.normalized * enemy.speed * Time.deltaTime, Space.World);
            //check if the distance is close so he can get the next wayPoint
            if (Vector3.Distance(transform.position, target.position) <= 0.6F)
            {
                GetNextWayPoint();
            }

            Quaternion lookRotation = Quaternion.LookRotation(dir);
            Vector3 rotation =
                Quaternion.Lerp(partToRotate.transform.rotation, lookRotation, Time.deltaTime * changeDirectionSpeed)
                    .eulerAngles;
            partToRotate.transform.rotation = Quaternion.Euler(0F, rotation.y, 0F);
        }
    }

    void LateUpdate()
    {
        speed = enemy.speed;
    }
    void GetNextWayPoint()
    {
        //if wavePoiintIndex is greater then the waypoints.points.lenght than the gameobject arrieved at the end and can be set to false
        if (wavePointIndex >= Waypoints.Points.Length - 1)
        {
            EndPath();
            return;
        }
        //if he isnt at the end increment the wavePointIndex
        wavePointIndex++;
        // and give him the next wavePoint
        target = Waypoints.Points[wavePointIndex];
    }

    void EndPath()
    {
        gameObject.SetActive(false);

        PlayerStarts.lives--;
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
}