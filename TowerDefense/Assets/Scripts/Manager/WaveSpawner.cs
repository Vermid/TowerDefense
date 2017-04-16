using UnityEngine;
using System.Collections;
using System.Runtime.Remoting;
using UnityEngine.UI;
public class WaveSpawner : MonoBehaviour
{
    #region Inspector
    [SerializeField]
    private Transform enemyPrefab;
    [SerializeField]
    private Transform spawnPoint;
    [SerializeField]
    private string enemyName = "";
    [SerializeField]
    private string enemyCapsule = "";
    [SerializeField]
    private float timeBetweenWaves = 5F;
    [SerializeField]
    private bool canGrow = false;
    #endregion

    private float countDown = 2F;
    private int waveIndex = 0;

    public Text WaveCoundownText;

    void Update()
    {
        //spawn enemys only if the coundown reaches 0
        if (countDown <= 0F)
        {
            StartCoroutine(SpawnWave());
            //set the coundown back to any time you want  in this chase tmeBetweenWaves
            countDown = timeBetweenWaves;
        }
        //count in seconds?
        countDown -= Time.deltaTime; //time past since last frames
        //show the countdown into the text Object
        WaveCoundownText.text = Mathf.Round(countDown).ToString();
    }


    IEnumerator SpawnWave()
    {
        //increment the waveIndex
        waveIndex++;
        //check if the enemyName is Set
        if (enemyName.Length != 0)
        {
            for (int i = 0; i < waveIndex; i++)
            {
                //call the funktion and pas the wanted enemysName into
                SpawnEnemy(enemyName);
                //wait .5 seconds for the next Enemy
                yield return new WaitForSeconds(.5F);
            }
        }
        //wait 1 Second for the next type of enemys
        yield return new WaitForSeconds(1);
        if (enemyCapsule.Length != 0)
        {
            for (int i = 0; i < waveIndex; i++)
            {
                SpawnEnemy(enemyCapsule);
                yield return new WaitForSeconds(.5F);
            }
        }
    }
    /// <summary>
    /// Wants the Enemy Name "Fast" or "Heavy".
    /// this gets than a gameobject of the enemy type and move the positiona and rotaten and set it than to true
    /// </summary>
    /// <param name="name"></param>
    void SpawnEnemy(string name)
    {
        //get from the pool a single gameobject from Type of the wanted name  "Fast","Heavy",etc
        GameObject obj = ObjectPool_Zaim.current.GetPoolObject(name, canGrow);
        if (obj == null)
            return;
        //sets the position from the map
        obj.transform.position = spawnPoint.transform.position;
        //sets the Rotation from the map
        obj.transform.rotation = spawnPoint.transform.rotation;
        //activates the map
        obj.SetActive(true);
        //Instantiate(enemyPrefab,spawnPoint.position,spawnPoint.rotation);
    }
}
