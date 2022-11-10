using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] private float startingHealth;
    public float currentHealth { get; private set; }
    private Animator anim;
    private bool dead;

    private void Awake()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
    }
    public void TakeDamage(float _damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);

        if (currentHealth > 0)
        {
            //anim.SetTrigger("death");
            //iframes
        }
        else
        {
            if (!dead)
            {
                anim.SetTrigger("death");
                Debug.Log("aaaaa");
                SceneManager.LoadScene("Death Scene");
            }
        }
    }
    //private void Update()
    //{
    //    if (Input.GetKeyUp(KeyCode.E))
    //    {
    //       TakeDamage(1);
    //    }
    //}
    public void AddHealth(float _value)
    {
        currentHealth = Mathf.Clamp(currentHealth + _value, 0, startingHealth);
    }
}
