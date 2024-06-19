using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class LifeHostHandler : NetworkBehaviour
{
    private const byte _fullLife = 100;

    [Networked(OnChanged =nameof(OnLifeChanged))]
    private byte CurrentLife { get; set; }

    [SerializeField] GameObject _visualObject;
    [SerializeField] byte _liveAmount = 3;

    [Networked(OnChanged = nameof(OnDeadChanged))]
    private bool IsDead { get; set; }

    public event Action OnRespawn = delegate { };
    public event Action<bool> OnEnableControls = delegate { };

    public override void Spawned()
    {
        CurrentLife = _fullLife;
    }

    public void TakeDamage(byte damage)
    {
        if (damage > CurrentLife) damage = CurrentLife;
        CurrentLife -= damage;

        if (CurrentLife != 0) return;

        _liveAmount--;
        if(_liveAmount==0)
        {
            Disconnect();
            return;
        }
        StartCoroutine(RespawnCoolDown());
        
    }

    IEnumerator RespawnCoolDown()
    {
        IsDead = true;

        yield return new WaitForSeconds(3f);
        IsDead = false;
        CurrentLife = _fullLife;
        OnRespawn();
    }

    static void OnDeadChanged(Changed<LifeHostHandler> changed)
    {
        bool currentDead = changed.Behaviour.IsDead;
        changed.LoadOld();
        bool oldDead = changed.Behaviour.IsDead;

        if (currentDead)
        {
            changed.Behaviour.ChangeRemoteDeadRespawnValues(false);
        }

        else if (oldDead)
        {
            changed.Behaviour.ChangeRemoteDeadRespawnValues(true);

        }
    }

    void ChangeRemoteDeadRespawnValues(bool value)
    {
        _visualObject.SetActive(value);
        OnEnableControls(value);
    }

    private void Disconnect()
    {
        if (!Object.HasInputAuthority)
        {
            Runner.Disconnect(Object.InputAuthority);
        }

        else
        {
            //Activar canvas derrota
        }

        Runner.Despawn(Object);
    }

    static void OnLifeChanged(Changed<LifeHostHandler> changed)
    {
        //todo lo que vimos de barra de vida
    }
}
