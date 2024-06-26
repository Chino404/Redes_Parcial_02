using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

//Crear una barra de vida por player
public class LifeHostHandler : NetworkBehaviour
{

    public static LifeHostHandler Instance;

    private float _maxLife = 100;

    List<LifeBarItem> _lifeBarList = new List<LifeBarItem>();
    [SerializeField] LifeBarItem _prefabLifeBar;

    [Networked(OnChanged =nameof(OnLifeChanged))]
    public float CurrentLife { get; set; }

    [SerializeField] GameObject _visualObject;
    [SerializeField] byte _liveAmount = 3;

    [Networked(OnChanged = nameof(OnDeadChanged))]
    private bool IsDead { get; set; }

    public event Action<float> OnLifeUpdate = delegate { };
    public event Action OnRespawn = delegate { };
    public event Action<bool> OnEnableControls = delegate { };

    private void Awake()
    {
        //if (Instance == null) Instance = this;
        //else Destroy(gameObject);

        Instance = this;
    }

    private void Start()
    {
        CurrentLife = _maxLife;
    }

    public LifeBarItem CreateBarLife(NetworkHostPlayer player)
    {
        LifeBarItem lifeBar = Instantiate(_prefabLifeBar, transform).SetTarget(player).SetLife(this);
        _lifeBarList.Add(lifeBar);

        player.OnPlayerDespawn += () =>
        {
            _lifeBarList.Remove(lifeBar);
            Destroy(lifeBar.gameObject);

        };

        return lifeBar;
    }

    void LateUpdate()
    {
        foreach (var item in _lifeBarList) item.UpdatePosition();
    }

    public void TakeDamage(float damage)
    {
        if (damage > CurrentLife) damage = CurrentLife;

        //CurrentLife -= damage;
        RPC_TakeDamage(damage);
        AudioManager.instance.PlaySFX(AudioManager.instance.takeDamage);


        if (CurrentLife != 0) return;

        _liveAmount--;
        if(_liveAmount==0)
        {
           
            //GameManager.instance.RPC_IsLose();

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
        CurrentLife = _maxLife;
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
            //Runner.Despawn(Object);
            Runner.Disconnect(Object.InputAuthority);
            
        }
        else
        {
            //Activar canvas derrota
            GameManager.instance.CanvasLost();
        }

        Runner.Despawn(Object);
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_TakeDamage(float damage)
    {
        CurrentLife -= damage;
    }

    static void OnLifeChanged(Changed<LifeHostHandler> changed)
    {
        //todo lo que vimos de barra de vida
        var updateLife = changed.Behaviour;  
        updateLife.OnLifeUpdate(updateLife.CurrentLife / updateLife._maxLife);       
    }
}
