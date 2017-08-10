using UnityEngine;

public class ShopMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject ui;

    private GameObject world;
    public LayerMask layer;
    private LayerMask startLayer;

    public static ShopMenu current;

    private void Awake()
    {
        current = this;
    }
    private void Start()
    {
        world = GameObject.FindGameObjectWithTag("World");
        startLayer = world.layer;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            Toggle();
        }
    }
    public void Toggle()
    {
<<<<<<< HEAD:TowerDefense/Assets/Scripts/UI/ShopMenu.cs
       // world.layer = layer;
=======
>>>>>>> refs/remotes/origin/master:TowerDefense/Assets/Scripts/ShopMenu.cs
        ui.SetActive(!ui.activeSelf);
        Debug.Log(ui.activeInHierarchy);
        //world.layer = layer;
         world.layer = (ui.activeInHierarchy ? layer : startLayer);

    }

    public void Pause()
    {
        Toggle();
    }


    public void Shop()
    {
        Toggle();
    }
}
