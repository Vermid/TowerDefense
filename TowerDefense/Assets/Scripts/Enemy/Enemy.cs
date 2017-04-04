using UnityEngine;


public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float speed = 10F;

    private Transform target;
    private int wavePointIndex = 0;

    void OnEnable()
    {
        //this resets the Gameobject with the start Values
        target = Waypoints.Points[0];
        wavePointIndex = 0;
    }

    void Update()
    {
        //when you sub the target place - the current you get the dir where you need to go
        Vector3 dir = target.position - transform.position;
        //add some movement with Translate.
        //normalize the dir before you * speed it or the end result can change and the Obejects move faster or slower
        transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);
        //check if the distance is close so he can get the next wayPoint
        if (Vector3.Distance(transform.position, target.position) <= 0.6F)
        {
            GetNextWayPoint();
        }
    }
    void GetNextWayPoint()
    {
        //if wavePoiintIndex is greater then the waypoints.points.lenght than the gameobject arrieved at the end and can be set to false
        if (wavePointIndex >= Waypoints.Points.Length - 1)
        {
            gameObject.SetActive(false);
            //Destroy(gameObject); // do not use this!! we use a POOL
            return;
        }
        //if he isnt at the end increment the wavePointIndex
        wavePointIndex++;
        // and give him the next wavePoint
        target = Waypoints.Points[wavePointIndex];
    }
}
