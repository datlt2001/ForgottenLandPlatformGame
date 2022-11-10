using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Difficulty : MonoBehaviour
{
    public GameObject enemy;
    public GameObject health;
    // Start is called before the first frame update
    void Start()
    {

        string difficult = PlayerPrefs.GetString("difficult");
        Debug.Log(difficult);
        if (difficult == "easy")
        {
            enemy.SetActive(false);
            health.SetActive(false);
            //EventsManager.StartEasyLevel += EasyLevel;            
        }
        else
        {
            enemy.SetActive(true);
            health.SetActive(true);
            // EventsManager.StartHardLevel += HardLevel;
        }
    }
    

}
