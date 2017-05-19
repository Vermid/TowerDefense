using UnityEngine;
using System.Collections;
using System.Linq;
using System.Runtime.Remoting;
using UnityEngine.UI;

public class FactoryBase : MonoBehaviour
{
    #region Inspector
    [Header("Attributes")]
    [SerializeField]
    private GameObject mines;
    [SerializeField]
    private float timeBetweenSpawn = 5F;
    [SerializeField]
    private int maxMines = 5;
    [SerializeField]
    private int mineRadius = 5;
    [Header("ObjectPool")]
    #endregion
    private float countDown = 2F;
    private Transform target;
    private Vector3 offsett;
    private float radius;

    void Start()
    {
        target = transform.FindChild(ConstNames.SpawnPoint).transform;

        radius = GetComponentInChildren<SphereCollider>().radius = mineRadius;
        radius /= 2;

        SpawnPoint();
    }
    void Update()
    {
        //spawn enemys only if the coundown reaches 0
        if (countDown <= 0F && target.childCount < maxMines)
        {
            StartCoroutine(SpawnWave());
            //set the coundown back to any time you want  in this chase tmeBetweenWaves
            countDown = timeBetweenSpawn;
        }
        //count in seconds?
        countDown -= Time.deltaTime; //time past since last frames
    }

    IEnumerator SpawnWave()
    {
        //call the funktion and pas the wanted enemysName into
        SpawnEnemy(mines.name);
        //wait .5 seconds for the next Enemy
        yield return new WaitForSeconds(.5F);
    }

    public Transform[] targets;
    void SpawnPoint()
    {
        targets = Waypoints.Points;

        float distance = Mathf.Infinity;

        foreach (Transform waypoint in targets)
        {
            if (Vector3.Distance(waypoint.position, transform.position) <= distance)
            {
                distance = Vector3.Distance(waypoint.position, transform.position);
                target.transform.position = waypoint.position;
            }
        }
    }

    /// <summary>
    /// Wants the Soldier Name.
    /// this gets than a gameobject and move the position and rotation and set it than to true
    /// </summary>
    /// <param name="name"></param>
    void SpawnEnemy(string name)
    {
        GameObject obj = ObjectPool_Zaim.current.GetPoolObject(name);
        if (obj == null)
            return;

        float offsetX = Random.Range(-radius, radius);
        float offsetZ = Random.Range(-radius, radius);

        offsett = new Vector3(offsetX, 0, offsetZ);

        obj.transform.position = target.transform.position + offsett;
        obj.transform.rotation = target.transform.rotation;
        obj.SetActive(true);

        obj.transform.parent = target;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        if (target != null)
            Gizmos.DrawWireSphere(target.transform.position, mineRadius * 2);
    }

}
