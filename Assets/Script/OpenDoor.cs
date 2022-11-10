using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpenDoor : MonoBehaviour
{
    [SerializeField] private AudioSource openDoorSoundEffect;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Door")  )
        {
            openDoorSoundEffect.Play();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +  1);
        }
    }
}
