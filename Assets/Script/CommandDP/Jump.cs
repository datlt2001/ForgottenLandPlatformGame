using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class JumpCommand : ICommand
{
   

    public override void Execute(AudioSource audio)
    {
        audio.Play();
    }

    
}


