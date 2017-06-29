using UnityEngine;
using System.Collections;
using System.Linq;
using System.Runtime.Remoting;
using UnityEngine.UI;

public class FactoryBase : MonoBehaviour
{
    #region Inspector
    [SerializeField]
    private GameObject mine;

    [Header("Attributes")]
    [SerializeField]
    private float timeBetweenSpawn = 5F;

    [SerializeField]
    private int maxMines = 5;

    [SerializeField]
    private int mineRadius = 5;

    [SerializeField]
    private bool groundType;

    [SerializeField]
    private bool airType;

    #endregion

    private float countDown = 2F;
    private Transform children;
    private Vector3 offsett;
    private float radius;
    private Transform[] targets;

    void Start()
    {
        children = transform.Find(ConstNames.SpawnPoint).transform;

        radius = GetComponentInChildren<SphereCollider>().radius = mineRadius;
        radius /= 2;

        SpawnPoint();
    }
    void Update()
    {
        //spawn enemys only if the coundown reaches 0
        if (countDown <= 0F && children.childCount <= maxMines)
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
                    children.transform.position = waypoint.position;
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
                    children.transform.position = waypoint.position;
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

        obj.transform.position = children.transform.position + offsett;
        obj.transform.rotation = children.transform.rotation;
        obj.SetActive(true);

        obj.transform.parent = children;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        if (children != null)
            Gizmos.DrawWireSphere(children.transform.position, mineRadius * 2);
    }
}
