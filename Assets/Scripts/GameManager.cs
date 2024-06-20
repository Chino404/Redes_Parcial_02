using Fusion;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    public static GameManager instance;

    public List<PlayerHostModel> players;
    [SerializeField] bool _isGameStarting;

    private void Awake()
    {
        instance = this;
    }

    public override void Spawned()
    {
        //Time.timeScale = 0;
    }

    public override void FixedUpdateNetwork()
    {
        if (Runner.ActivePlayers.Count() >= 2)
        {
            _isGameStarting = true;
        }

        if (_isGameStarting == true)
        {

            //Time.timeScale = 1;
        }
    }
}
