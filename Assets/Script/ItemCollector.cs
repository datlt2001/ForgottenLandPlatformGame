using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class ItemCollector : MonoBehaviour
{

   
    public GameObject player;
    private int score = 0;




    [SerializeField] private Text gemText;

    [SerializeField] private AudioSource collectionSoundEffect;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        

        if (collision.gameObject.CompareTag("Gem"))
        {

            collectionSoundEffect.Play();

            Destroy(collision.gameObject);
            score += 50;
            gemText.text = "Score : " + score;
            PlayerPrefs.SetInt("Gem_Score", score);
            
        }
    }
    
}
