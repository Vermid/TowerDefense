using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//TODO: RENAME CLASS
public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private SceneFader sceneFader;

    [SerializeField]
    private GameObject ui;

    [SerializeField]
    private Text score;

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
        WaveManager.EnemysInScene = 0;
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

    private void GetScore()
    {

        GameObject[] allTurrets = GameObject.FindGameObjectsWithTag(ConstNames.Turret);
        int counter = 0;
        foreach (GameObject turret in allTurrets)
        {
          counter +=  turret.GetComponent<Turret>().GetKillCounter();
        }
        score.text = counter.ToString();
        // add score to the game and cal it here (playerstarts [change also the class name])
        //score.text = 
    }
}
