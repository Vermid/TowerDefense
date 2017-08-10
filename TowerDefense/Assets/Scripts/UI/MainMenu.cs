using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private string levelLoad = "Level01";
    [SerializeField]
    private Scene nextLevel;

    [SerializeField] private SceneFader sceneFader;
    public void Play()
    {
        sceneFader.FadeTo(levelLoad);
        SceneManager.LoadScene(levelLoad);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
