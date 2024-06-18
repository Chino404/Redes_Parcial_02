using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleports : MonoBehaviour
{
    [SerializeField] Teleports _nextTeleport;
    public bool _isTeleporting = false;

    //private void Update()
    //{
    //    if (_nextTeleport._isTeleporting == true && _isTeleporting == false)
    //    {
    //        _isTeleporting = true;
    //        StartCoroutine(TeleportCoolDown());
    //    }
    //}

    private void OnTriggerEnter(Collider other)
    {
        var player = other.gameObject.GetComponent<PlayerHostModel>();
        if (player != null && !_isTeleporting)
        {
            _nextTeleport._isTeleporting = true;
            _isTeleporting = true;
            float z = other.transform.position.z;
            other.transform.position = new Vector3(_nextTeleport.transform.position.x, _nextTeleport.transform.position.y,z);
            

            StartCoroutine(TeleportCoolDown());

        }
    }

    

    IEnumerator TeleportCoolDown()
    {
       

        yield return new WaitForSeconds(2f);
        _nextTeleport._isTeleporting = false;
        _isTeleporting = false;

    }
}
