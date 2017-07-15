using UnityEngine;

public class ShopMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject ui;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            Toggle();
        }
    }

    private void Toggle()
    {
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
