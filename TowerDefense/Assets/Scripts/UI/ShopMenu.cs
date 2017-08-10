using UnityEngine;

public class ShopMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject ui;

    private GameObject world;
    public LayerMask layer;

    private void Start()
    {
        world = GameObject.FindGameObjectWithTag("World");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            Toggle();
        }
    }
    private void Toggle()
    {
       // world.layer = layer;
        ui.SetActive(!ui.activeSelf);

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
