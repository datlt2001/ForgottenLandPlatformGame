using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ResultScene : MonoBehaviour
{
    [SerializeField] private Text finalText;
    public GameObject panel1;
    public GameObject panel2;
    private int score = 0;


    public void PlayAgain()
    {
        SceneManager.LoadScene("Level");
    }
    public void Continue()
    {
        SceneManager.LoadScene("End Scene");
    }
    
    void Start()
    {
        var final = PlayerPrefs.GetInt(GameUtility.Final);
        Debug.Log(final);
        score += final;
        finalText.text = "Score : " + score;
        if(final > 850)
        {
            panel1.SetActive(true);
            panel2.SetActive(false);
        }
        else
        {
            panel1.SetActive(false);
            panel2.SetActive(true);
        }
    }


}
        
