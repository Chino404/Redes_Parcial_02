using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public void BacktoMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
