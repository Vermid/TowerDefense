using System;
using UnityEngine;
using UnityEngine.UI;

///TODO: think how to use keybinding shop ||build turret || upgrade
public class Settings : MonoBehaviour
{
    [SerializeField]
    private Text soundText;

    [SerializeField]
    private GameObject ui;

    [SerializeField]
    private bool overallSound;
    [SerializeField]
    private bool music;
    [SerializeField]
    private bool specialEffects;

    private int qLevel;
    public static Settings current;

    private void Awake()
    {
        current = this;
    }

    private void Start()
    {
        if (PlayerPrefs.GetInt("SavedSettings") != 0)
        {
            qLevel = PlayerPrefs.GetInt("UnityGraphicsQuality");
        }
        else
        {
            qLevel = QualitySettings.GetQualityLevel();
        }
    }

    public void ChangeSpeed()
    {
        if (Time.timeScale == 2)
        {
            Time.timeScale = 1;
        }
        if (Time.timeScale == 1)
        {
            Time.timeScale = 2;
        }
    }


    public void QualityLow()
    {
        QualitySettings.SetQualityLevel(0);
    }

    public void QualityMedium()
    {
        QualitySettings.SetQualityLevel(2);
    }

    public void QualityHigh()
    {
        QualitySettings.SetQualityLevel(5);
    }

    private float soundVolume;

    public void ChangeSoundVolume()
    {


        int i;
        if (Int32.TryParse(soundText.text, out i))
        {
            AudioListener.volume = i;
            soundVolume = i;
        }
        else
            Console.WriteLine("String could not be parsed.");
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetInt("UnityGraphicsQuality", qLevel);
        PlayerPrefs.SetFloat("Sound", soundVolume);

        PlayerPrefs.SetInt("SavedSettings", 1);

        PlayerPrefs.Save();
    }

    public void Toggle()
    {
        ui.SetActive(!ui.activeSelf);
    }
}
