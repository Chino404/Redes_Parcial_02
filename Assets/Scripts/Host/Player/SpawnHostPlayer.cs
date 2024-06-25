using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Sockets;
using System;
using System.Linq;

public class SpawnHostPlayer : MonoBehaviour, INetworkRunnerCallbacks
{
    [SerializeField] NetworkHostPlayer _playerPrefab;
    LocalHostPlayerInputs _localInputs;
    public static Transform spawn1, spawn2;


    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player) 
    { 
        if(runner.IsServer)
        {
            if(runner.ActivePlayers.Count()>1)
                runner.Spawn(_playerPrefab, spawn1.position, Quaternion.identity, player);

            else
                runner.Spawn(_playerPrefab, spawn2.position, Quaternion.identity, player);

        }
    }
    public void OnInput(NetworkRunner runner, NetworkInput input) 
    {
        if (!NetworkHostPlayer.Local) return;

        if (!_localInputs) _localInputs = NetworkHostPlayer.Local.GetComponent<LocalHostPlayerInputs>();
        else input.Set(_localInputs.GetLocalInputs());
    }

    #region callbacks sin usar
    public void OnConnectedToServer(NetworkRunner runner) { }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    { }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    { }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    { }
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }

    public void OnDisconnectedFromServer(NetworkRunner runner) { }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    { }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) { }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data) { }
    public void OnSceneLoadDone(NetworkRunner runner) { }

    public void OnSceneLoadStart(NetworkRunner runner) { }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
    #endregion
}
