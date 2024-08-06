using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Fusion;
using System.Linq;

public class PauseManager : NetworkBehaviour
{
    
    public void BackToMainMenu()
    {
        if (Runner.ActivePlayers.Count() == 1)
        {
            Runner.Disconnect(Object.InputAuthority);
            Runner.Shutdown();
            print("ir al menu");
            SceneManager.LoadScene(0);
        }
        else
        {
            Runner.Disconnect(Object.InputAuthority);
            print("ir al menu");
            SceneManager.LoadScene(0);
        }
    }
}

    

