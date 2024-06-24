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
    }
}
