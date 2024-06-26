using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    public static GameManager instance;

    public List<PlayerHostModel> players;
    [SerializeField] private bool _isGameStarting;
    private bool _playMusicGame;

    [Header("Canvas")]
    [SerializeField] Canvas _waitingCanvas;
    public Canvas loseCanvas;
    public Canvas winCanvas;

    //[Networked(OnChanged = nameof(OnCondisionChanged))]
    public bool Win { get; set; }


    private void Awake()
    {
        instance = this;
        Time.timeScale = 0;

    }

    public override void FixedUpdateNetwork()
    {

        if (_isGameStarting)
        {

            _waitingCanvas.gameObject.SetActive(false);
            Time.timeScale = 1;

            if(!_playMusicGame)
            {
                _playMusicGame = true;
                AudioManager.instance.StopMusic();
                AudioManager.instance.PlayMusic(AudioManager.instance.musicGame);
            }
        }

     

        else if (players.Count() >= 2)
        {
            _isGameStarting = true;
        }
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.Proxies)]
    public void RPC_IsLose()
    {

        if (Object.HasInputAuthority)
        {
            CanvasLost();
        }
    }


    public void CanvasLost()
    {
        Debug.Log("PERDI");
        loseCanvas.gameObject.SetActive(true);
    }

   public void CheckWin()
    {
        if (_isGameStarting && players.Count() == 1) CanvasWin();
    }
    public void CanvasWin()
    {
        //Time.timeScale = 0;
        Debug.Log("GANE");
        winCanvas.gameObject.SetActive(true);
    }

}
