using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Fusion;
using Fusion.Sockets;
using System.Threading.Tasks;
using System;

public class NetworkRunnerHandler : MonoBehaviour, INetworkRunnerCallbacks
{
    [SerializeField] NetworkRunner _networkPrefab;
    NetworkRunner _currentNetwork;

    public event Action OnLobbyConected = delegate { };
    public event Action<List<SessionInfo>> OnSessionListUpdate = delegate { };

    #region Lobby
    public void JoinLobby()
    {
        if(_currentNetwork) Destroy(_currentNetwork);

        _currentNetwork = Instantiate(_networkPrefab);

        _currentNetwork.AddCallbacks(this);

        var clientTask = JoinLobbyTask();
    }

    async Task JoinLobbyTask()
    {
        var result = await _currentNetwork.JoinSessionLobby(SessionLobby.Custom, "Normal Lobby");

        if(result.Ok)
        {
            OnLobbyConected();
        }
        else
        {
            Debug.Log("Unable to join");
        }
    }

    #endregion

    #region Create/Join Session
    public void CreateSession(string sessionName, string sceneName)
    {
        var clientTask = InitializeGame(GameMode.Host, sessionName,
            SceneUtility.GetBuildIndexByScenePath($"Scenes/{sceneName}"));
    }

    public void JoinSession(SessionInfo session)
    {
        var clientTask = InitializeGame(GameMode.Client, session.Name);
    }

    async Task InitializeGame(GameMode gameMode,string sessionName, SceneRef? sceneToLoad = null)
    {
        var sceneManager = GetComponent<NetworkSceneManagerDefault>();

        _currentNetwork.ProvideInput = true;

        var result = await _currentNetwork.StartGame(new StartGameArgs()
        {
            GameMode = gameMode,
            SessionName = sessionName,
            Scene = sceneToLoad,
            CustomLobbyName = "Normal Lobby",
            SceneManager = sceneManager
        });

        if(result.Ok) 
        {
            Debug.Log("Game created/joined");
        }
    }
    #endregion


    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        OnSessionListUpdate(sessionList);
    }

    #region callbacks sin usar
    public void OnConnectedToServer(NetworkRunner runner) { }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    { }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    { }
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data){ }

    public void OnDisconnectedFromServer(NetworkRunner runner){ }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    { }

    public void OnInput(NetworkRunner runner, NetworkInput input) { }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player) { }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) { }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data) { }
    public void OnSceneLoadDone(NetworkRunner runner) { }

    public void OnSceneLoadStart(NetworkRunner runner) { }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
    #endregion
}
