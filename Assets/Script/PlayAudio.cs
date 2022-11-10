using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudio : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Play");
        Audio.Instance.PlayAudio(Audio.Instance.Clip);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
