using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;


public class PauseManager : MonoBehaviour
{
    
    public void BacktoMainMenu()
    {
        SceneManager.LoadScene(0);

        //if (Runner.ActivePlayers.Count() == 1)
        //{
        //    Runner.Disconnect(Object.InputAuthority);
        //    Runner.Shutdown();

        //    print("ir al menu");
        //}
        //else
        //{
        //    Runner.Disconnect(Object.InputAuthority);
        //    SceneManager.LoadScene(0);
        //    print("ir al menu");
        //}
    }
}
