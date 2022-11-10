using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveAIEnemy : MonoBehaviour
{
    public GameObject enemyGFX;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if(enemyGFX.activeSelf == false)
            {
                enemyGFX.SetActive(true);
            }
            else
            {
                enemyGFX.SetActive(false);
            }
            
        }
    }


}
