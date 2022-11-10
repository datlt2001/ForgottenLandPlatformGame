using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using static EventsManager;

public class MainMenu : MonoBehaviour
{
    private void Start()
    {
        EventsManager.StartEasyLevel += EasyLevel;
        EventsManager.StartHardLevel += HardLevel;
    }

    private void EasyLevel()
    {
        PlayerPrefs.SetString("difficult", "easy");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    private void HardLevel()
    {
        PlayerPrefs.SetString("difficult", "hard");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void StartGameHard()
    {
        PlayerPrefs.SetString("difficult", "hard");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void StartGameEasy()
    {
        PlayerPrefs.SetString("difficult", "easy");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void Quit()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }
    public void Back()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
