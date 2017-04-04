using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool_Zaim : MonoBehaviour
{
    #region Inspector
    [SerializeField]
    private string objectOneString;
    [SerializeField]
    private string objectTwoString;
    [SerializeField]
    private string objectThreeString;
    [SerializeField]
    private string objectFourString;
    [SerializeField]
    private GameObject objectOneGameObject;
    [SerializeField]
    private GameObject objectTwoGameObject;
    [SerializeField]
    private GameObject objectThreeGameObject;
    [SerializeField]
    private GameObject objectFourGameObject;
    [SerializeField]
    private int listSize = 10;
    [SerializeField]
    private bool canGrow = false;

    #endregion
    private List<GameObject> enemyOne = new List<GameObject>();
    private Dictionary<string, List<GameObject>> ObjectPoolByName;
    public static ObjectPool_Zaim current;


    //damn dont forget this again 
    //if you use a dictionary list for more enemys
    private List<GameObject> enemyTwo = new List<GameObject>();
    private List<GameObject> enemyThree = new List<GameObject>();
    private List<GameObject> enemyFour = new List<GameObject>();

    void Awake()
    {
        // get acces onto this script
        current = this;
    }

    // Use this for initialization
    void Start()
    {
        InitializeDictionary();
    }

    private void InitializeDictionary()
    {
        ObjectPoolByName = new Dictionary<string, List<GameObject>>();
        for (int i = 0; i < listSize; i++)
        {
            GameObject obj = (GameObject)Instantiate(objectOneGameObject);
            obj.SetActive(false);
            enemyOne.Add(obj);
        }
        ObjectPoolByName.Add(objectOneString, enemyOne);

        for (int i = 0; i < listSize; i++)
        {
            GameObject obj = (GameObject)Instantiate(objectTwoGameObject);
            obj.SetActive(false);
            enemyTwo.Add(obj);
        }
        ObjectPoolByName.Add(objectTwoString, enemyTwo);

        for (int i = 0; i < listSize; i++)
        {
            GameObject obj = (GameObject)Instantiate(objectThreeGameObject);
            obj.SetActive(false);
            enemyThree.Add(obj);
        }
        ObjectPoolByName.Add(objectThreeString, enemyThree);

        for (int i = 0; i < listSize; i++)
        {
            GameObject obj = (GameObject)Instantiate(objectFourGameObject);
            obj.SetActive(false);
            enemyFour.Add(obj);
        }
        ObjectPoolByName.Add(objectFourString, enemyFour);
    }

    /// <summary>
    /// search for the "wantedGameObjectName" and returns a Gameobject if there is one that isn't activated else null
    /// </summary>
    /// <param name="wantedGameObjectName"></param>
    /// <returns></returns>
    public GameObject GetPoolObject(string wantedGameObjectName)
    {
        if (ObjectPoolByName.ContainsKey(wantedGameObjectName))
        {
            foreach (var keyVal in ObjectPoolByName)
            {
                if (keyVal.Key == wantedGameObjectName)
                {
                    var elementList = ObjectPoolByName[wantedGameObjectName];
                    for (int i = 0; i < elementList.Count; i++)
                    {
                        //check if the elementList[i] is activ or not
                        if (elementList[i].activeInHierarchy == false)
                        {
                            return elementList[i];
                        }
                    }
                }
            }
        }
        return null;
    }
}