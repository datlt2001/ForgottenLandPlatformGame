using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public KeyCode keyJump;
    public GameObject player;

    private JumpCommand jumpCmd = new JumpCommand();

    [SerializeField] private AudioSource jumpSoundEffect;

    private void Update()
    {
        ProcessInput();
    }

    private void ProcessInput()
    {
        if (Input.GetKeyDown(keyJump))
        {
            jumpCmd.Execute(jumpSoundEffect);
            player.GetComponent<Rigidbody2D>().velocity = new Vector2(player.GetComponent<Rigidbody2D>().velocity.x, 6.5f);
        }
    }

    public void ChangeKeyCode()
    {
        if(keyJump != KeyCode.K) { keyJump = KeyCode.K; }
        else { keyJump = KeyCode.Space; }
        
    }

   
}

