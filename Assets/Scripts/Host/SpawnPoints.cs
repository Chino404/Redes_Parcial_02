using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class SpawnPoints : NetworkBehaviour
{
    public bool isUsed;

    public override void Spawned()
    {
        foreach (var item in GameManager.instance.players)
        {

            if (isUsed == false)
                item.transform.position = transform.position;

            else return;
        }

        if (SpawnHostPlayer.spawn1 != null) SpawnHostPlayer.spawn2 = transform;
        else SpawnHostPlayer.spawn1 = transform;
    }
}
