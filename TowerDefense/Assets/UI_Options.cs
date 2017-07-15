using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_Options : MonoBehaviour
{

    [SerializeField]
    private SceneFader sceneFader;


    public UI_Options current;

    void OnAwake()
    {
        current = this;
    }

    private void Toggle(GameObject ui)
    {
        ui.SetActive(!ui.activeSelf);

        if (ui.activeSelf)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    public void Pause(GameObject ui)
    {
        Toggle(ui);
    }

    public void Continue(GameObject ui)
    {
        Toggle(ui);
    }

    public void Restart(GameObject ui)
    {
        Toggle(ui);
        sceneFader.FadeTo(SceneManager.GetActiveScene().name);
    }

    public void Exit()
    {
        //implement later
    }

    public void Menu(GameObject ui)
    {
        Toggle(ui);
        sceneFader.FadeTo(ConstNames.MainMenu);
    }

}
