using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject PauseUI;

    public bool isPaused;
    
    void Start()
    {
        PauseUI.SetActive(false);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }
    public void PauseGame()
    {
        
        PauseUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;

    }
    public void ResumeGame()
    {
        
        PauseUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void GoMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
    public void Quit()
    {
        Application.Quit();
    }
}
