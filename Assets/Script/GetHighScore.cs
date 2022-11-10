using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GetHighScore : MonoBehaviour
{
    [SerializeField] public Text scores;
    // Start is called before the first frame update
    void Start()
    {
        string path = Directory.GetCurrentDirectory() + "/HighScore.txt";
        if (System.IO.File.Exists(path))
        {
            string[] a = File.ReadAllLines(path);
            for (int i = 0; i < 3; i++)
            {
                scores.text += a[i] + "\n";
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
