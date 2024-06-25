using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using System;

public class NetworkHostPlayer : NetworkBehaviour
{
    public static NetworkHostPlayer Local;
    public event Action OnPlayerDespawn;

    private NicknameItem _myNickname;
    private LifeBarItem _lifeBar;

    [Networked(OnChanged = nameof(OnNickNameChanged))]
    private string Nickname {  get; set; }

    public override void Spawned()
    {
        _myNickname = NickNameHandler.Instance.CreateNewNickName(this);
        _lifeBar = LifeHostHandler.Instance.CreateBarLife(this);

        if (Object.HasInputAuthority)
        {
            Local = this;

            RPC_SetNewNickName(PlayerPrefs.GetString("NickNameSave","Ricardo"));

            //GetComponentInChildren<SkinnedMeshRenderer>().material.color = Color.blue;
        }
        else
        {
            GetComponentInChildren<SkinnedMeshRenderer>().material.color = Color.blue;
        }
    }

    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        OnPlayerDespawn();
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    void RPC_SetNewNickName(string newName)
    {
       Nickname = newName;
    }

    static void OnNickNameChanged(Changed<NetworkHostPlayer> changed)
    {
        var behaviour = changed.Behaviour;

        behaviour._myNickname.UpdateName(behaviour.Nickname);
    }

}
