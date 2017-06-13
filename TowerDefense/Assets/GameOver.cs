using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameOver : MonoBehaviour
{
    #region Inspector
    [SerializeField]
    private SceneFader sceneFader;

    [SerializeField]
    private GameObject ui;

    [SerializeField]
    private Text score;

    #endregion

    // Update is called once per frame
    void Update()
    {
        if (PlayerStarts.lives <= 0)
        {
            ui.SetActive(true);
            Time.timeScale = 0;
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

    public void Exit()
    {
        //implement later
    }

    public void Menu()
    {
        Toggle();
        sceneFader.FadeTo(ConstNames.MainMenu);
    }


    private void GetScore()
    {
        // add score to the game and cal it here (playerstarts [change also the class name])
        //score.text = 
    }
}
