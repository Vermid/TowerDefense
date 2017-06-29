using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class WaveManager : MonoBehaviour
{
    #region Inspector
    [SerializeField]
    private Transform spawnPoint;
    //  [SerializeField]
    // private float timeBetweenWaves = 5F;
    [SerializeField]
    private Text WaveCoundownText;

    [Tooltip("List of all Enemys")]
    [SerializeField]
    private List<WaveGenerator> listOfGameObjects = new List<WaveGenerator>();
    #endregion
    private bool nextWave = false;
    private int waveIndex = 0;
    //private float countDown = 2F;
    public static int EnemysInScene = 0;

    void Update()
    {
        WaveCoundownText.text = "Enemys Left: \n" + EnemysInScene;

        if (Input.GetKeyDown(KeyCode.Space) && EnemysInScene == 0 && waveIndex < listOfGameObjects.Count)
        {
            Debug.Log("Start Next Wave");
            SpawnWave();
        }

        #region EndlessWave
        //spawn enemys only if the coundown reaches 0
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

    void SpawnWave()
    {
        //loop the Inspector list
        //foreach (GameObject gobj in listOfGameObjects)
        if (listOfGameObjects[waveIndex].AirMonster != null)
        {
            for (int i = 0; i < listOfGameObjects[waveIndex].AirCounter;)
            {
                if (SpawnEnemy(listOfGameObjects[waveIndex].AirMonster.name))
                {
                    i++;
                    EnemysInScene++;
                }
            }
        }
        if (listOfGameObjects[waveIndex].GroundMonster != null)
        {
            for (int i = 0; i < listOfGameObjects[waveIndex].GroundCounter;)
            {
                if (SpawnEnemy(listOfGameObjects[waveIndex].GroundMonster.name))
                {
                    i++;
                    EnemysInScene++;
                }
            }
        }
        if (listOfGameObjects[waveIndex].BossMonster != null)
        {
            for (int i = 0; i < listOfGameObjects[waveIndex].BossCounter;)
            {
                if (SpawnEnemy(listOfGameObjects[waveIndex].BossMonster.name))
                {
                    i++;
                    EnemysInScene++;
                }
            }
        }
        waveIndex++;
        StartCoroutine(MoveEnemys());
    }

    IEnumerator MoveEnemys()
    {
        GameObject[] EnemysAlive = GameObject.FindGameObjectsWithTag(ConstNames.Enemy);

        //        foreach (GameObject obj in EnemysAlive)
        for (int i = 0; i < EnemysAlive.Length; i++)
        {
            if (EnemysAlive[i].GetComponent<Enemy>().GetHealth() > 0)
            {
                EnemysAlive[i].GetComponent<EnemyMovement>().SetMovement();
                yield return new WaitForSeconds(1F);
            }
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

        if (!obj.GetComponent<Enemy>().GetRespawnTimer())
            return false;
        var enemy = obj.GetComponent<Enemy>();

        //sets the position from the map
        obj.transform.position = spawnPoint.transform.position;
        //sets the Rotation from the map
        obj.transform.rotation = spawnPoint.transform.rotation;
        //activates the map
        obj.SetActive(true);
        return true;
    }
}
