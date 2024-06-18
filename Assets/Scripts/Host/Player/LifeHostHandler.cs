using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class LifeHostHandler : MonoBehaviour
{

    public event Action OnRespawn = delegate { };
    public event Action<bool> OnEnableControls = delegate { };

    public void TakeDamage(byte damage)
    {
        
    }

}
