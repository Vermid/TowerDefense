using System.Security.Policy;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneUI : MonoBehaviour
{

    void Start()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    public void Pause()
    {
        gameObject.SetActive(true);
        Time.timeScale = 0;
    }

    public void Continue()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    public void Restart()
    {
        //implement later
        int sceneBuildIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(sceneBuildIndex);
    }

    public void Exit()
    {
        //implement later
    }

    public void Menu()
    {
        //implement later
    }
}
