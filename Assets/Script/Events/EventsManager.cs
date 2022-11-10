using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventsManager : MonoBehaviour
{
    public delegate void EasyLevel();
    public static event EasyLevel StartEasyLevel;

    public delegate void HardLevel();
    public static event HardLevel StartHardLevel;

    public void Easy()
    {
        if (StartEasyLevel != null)
        {
            StartEasyLevel();
        }
    }

    public void Hard()
    {
        if (StartHardLevel != null)
        {
            StartHardLevel();
        }
    }
}
