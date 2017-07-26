using System.Security.Policy;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private SceneFader sceneFader;

    [SerializeField]
    private GameObject ui;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            Toggle();
        }
    }

    private void Toggle()
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

    public void Pause()
    {
        Toggle();
    }

    public void Continue()
    {
        Toggle();
    }

    public void Restart()
    {
        Toggle();
        sceneFader.FadeTo(SceneManager.GetActiveScene().name);
    }

    public void GetSettings()
    {
        Toggle();
        Options.current.Toggle();
    }

    public void Exit()
    {
        //implement later
        Application.Quit();
    }

    public void Menu()
    {
        Toggle();
        sceneFader.FadeTo(ConstNames.MainMenu);
    }
}
