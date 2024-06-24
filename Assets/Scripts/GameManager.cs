using Fusion;
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
    [SerializeField] Canvas _canvas;

    private void Awake()
    {
        instance = this;
        Time.timeScale = 0;


    }

    public override void Spawned()
    {
        
       
    }

    public override void FixedUpdateNetwork()
    {
        if (Runner.ActivePlayers.Count() >= 2)
        {
            _isGameStarting = true;
        }

        if (_isGameStarting == true)
        {

            _canvas.gameObject.SetActive(false);
            Time.timeScale = 1;

            if(!_playMusicGame)
            {
                _playMusicGame = true;
                AudioManager.instance.StopMusic();
                AudioManager.instance.PlayMusic(AudioManager.instance.musicGame);
            }
        }
    }
}
