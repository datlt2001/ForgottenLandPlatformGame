using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoBackMenu : MonoBehaviour
{
    public void GoToMainMenu()
    {
        SceneManager.LoadScene("Start Screen");
    }
}
