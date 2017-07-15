using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class WaveSpawner : MonoBehaviour
{
    #region Inspector

    [SerializeField]
    private Transform spawnPoint;
    [SerializeField]
    private float timeBetweenWaves = 5F;
    [SerializeField]
    private int objectCounter = 10;
    [Tooltip("List of all Enemys")]
    [SerializeField]
    private List<GameObject> listOfGameObjects = new List<GameObject>();
    [Tooltip("List of how much Enemys will spawn in each Wave")]
    [SerializeField]
    private List<int> enemyCounter = new List<int>();

    [SerializeField]
    public struct Elements
    {
        public GameObject g;
        public int inte;
    }

    #endregion
    private bool nextWave = false;

    private float countDown = 2F;

    public Text WaveCoundownText;
    private int waveIndex = 0;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            nextWave = true;
        }
        Debug.Log("NextWave Middle " + nextWave);

        //spawn enemys only if the coundown reaches 0
        if (nextWave)
        {
            StartCoroutine(SpawnWave());
            nextWave = false;
        }


        #region EndlessWave
        //if (countDown <= 0F)
        //if (nextWave)
        //{
        //    StartCoroutine(SpawnWave());
        //    //set the coundown back to any time you want  in this chase tmeBetweenWaves
        //    countDown = timeBetweenWaves;
        //}
        //count in seconds?
        //countDown -= Time.deltaTime; //time past since last frames

        //countDown = Mathf.Clamp(countDown, 0f, Mathf.Infinity);

        //show the countdown into the text Object
        // WaveCoundownText.text = Mathf.Round(countDown).ToString();
        //string format to show a better contdown 
        // WaveCoundownText.text = string.Format("{0:00.00}", countDown);
        #endregion
    }


    IEnumerator SpawnWave()
    {
        //TODO: this works for now change it later 
        //loop the Inspector list
        //foreach (GameObject gobj in listOfGameObjects)
        for (int x = 0; x < listOfGameObjects.Count; x++)
        {
            if (listOfGameObjects[x] != null)
            {
                for (int i = 0; i < enemyCounter[x];)
                {
                    if (SpawnEnemy(listOfGameObjects[x].name))
                        i++;
                    yield return new WaitForSeconds(.1F);
                }
            }
            waveIndex++;
        }
    }

    /// <summary>
    /// Wants the Enemy Name "Fast" or "Heavy".
    /// this gets than a gameobject of the enemy type and move the positiona and rotaten and set it than to true
    /// </summary>
    /// <param name="name"></param>
    bool SpawnEnemy(string name)
    {
        //get from the pool a single gameobject from Type of the wanted name  "Fast","Heavy",etc
        GameObject obj = ObjectPool_Zaim.current.GetPoolObject(name);
        if (obj == null)
            return false;
        var spawn = obj.GetComponent<Enemy>().GetRespawnTimer();
        if (!spawn)
            return false;

        //sets the position from the map
        obj.transform.position = spawnPoint.transform.position;
        //sets the Rotation from the map
        obj.transform.rotation = spawnPoint.transform.rotation;
        //activates the map
        obj.SetActive(true);
        return true;
    }
}
