using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool_Zaim : MonoBehaviour
{
    #region Inspector
    [Tooltip("How much Objects to hold at the Beginning")]
    [SerializeField]
    private int objectCounter = 10;
    [Tooltip("List of All Gamobjects")]
    [SerializeField]
    private List<GameObject> listOfGameObjects = new List<GameObject>();
    #endregion
    private Dictionary<string, List<GameObject>> ObjectPoolByName;
    private List<GameObject> gameObjectsList;

    public static ObjectPool_Zaim current;

    private GameObject gobjHolder;

    void Awake()
    {
        // get acces onto this script
        current = this;
    }

    // Use this for initialization
    void Start()
    {
        gobjHolder = GameObject.FindGameObjectWithTag("Finish");

        InitializeDictionary();
    }

    private void InitializeDictionary()
    {
        ObjectPoolByName = new Dictionary<string, List<GameObject>>();
        //loop the Inspector list
        foreach (GameObject gobj in listOfGameObjects)
        {
            //check if the list is hight than the gameobjects set
            if (gobj != null)
            {
                gameObjectsList = new List<GameObject>();
                for (int i = 0; i < objectCounter; i++)
                {
                    GameObject obj = (GameObject)Instantiate(gobj);
                    obj.SetActive(false);
                    gameObjectsList.Add(obj);
                    obj.transform.parent = gobjHolder.transform;
                }
                ObjectPoolByName.Add(gobj.name, gameObjectsList);
            }
        }
    }

    /// <summary>
    /// search for the "wantedGameObjectName" and returns a Gameobject if there is one that isn't activated else null
    /// </summary>
    /// <param name="wantedGameObject"></param>
    /// <returns></returns>
    public GameObject GetPoolObject(string wantedGameObject)
    {
        if (ObjectPoolByName.ContainsKey(wantedGameObject))
        {
            var value = ObjectPoolByName[wantedGameObject];
            for (int i = 0; i < value.Count; i++)
            {
                //check if the elementList[i] is activ or not
                if (value[i].activeInHierarchy == false)
                {
                    return value[i];
                }
            }
         
            //If the there are no objetcs generate new objects and add them to the list 

            //save the value from the dictionary by writing the wanted key
            //Instantiate from the value  with the index 0 (it doesn't matter waht index you pick they are all the same in this value
            GameObject obj = (GameObject)Instantiate(value[0]);
            obj.SetActive(false);
            //add the new map to the list from the value
            // wyh did i add this?
            value.Add(obj);
            obj.transform.parent = gobjHolder.transform;
            return obj;
        }
        return null;
    }
}