using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Fusion;

public class PauseManager : NetworkBehaviour
{
    

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_BacktoMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void BackToMainMenu()
    {
        if (Runner.IsServer)
        {
            RPC_BacktoMainMenu();
            StartCoroutine(ShutDownServer());
        }
        else
            SceneManager.LoadScene(0);
    }

    IEnumerator ShutDownServer()
    {
        yield return new WaitForSeconds(1f);
        DisconnectAllClients();
        yield return new WaitForSeconds(1f);
        Runner.Shutdown();
    }

    private void DisconnectAllClients()
    {
        foreach (var player in Runner.ActivePlayers)
        {
            if (player != Runner.LocalPlayer)
            {
                Runner.Disconnect(player);
            }
        }
    }
}
