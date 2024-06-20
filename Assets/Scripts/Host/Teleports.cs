using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Fusion;

public class Teleports : NetworkBehaviour
{
    public Teleports _nextTeleport;
    public bool _isTeleporting = false;
    public event Action Teleport = delegate { };



    public override void FixedUpdateNetwork()
    {
        if (_nextTeleport._isTeleporting == true && _isTeleporting == false)
        {
            _isTeleporting = true;
            StartCoroutine(TeleportCoolDown());
        }
    }
    
        
    

    private void OnTriggerEnter(Collider other)
    {
        //var player = other.gameObject.GetComponent<PlayerHostModel>();
        if (other.gameObject.layer==3  && !_isTeleporting)
        {
           
            _isTeleporting = true;
            float z = other.transform.position.z;
            other.transform.position = new Vector3(_nextTeleport.transform.position.x, _nextTeleport.transform.position.y, z);
            //Teleport();
            

            StartCoroutine(TeleportCoolDown());

        }
    }

    

    IEnumerator TeleportCoolDown()
    {
        yield return new WaitForSeconds(1f);
        _nextTeleport._isTeleporting = false;
        _isTeleporting = false;
    }
}
