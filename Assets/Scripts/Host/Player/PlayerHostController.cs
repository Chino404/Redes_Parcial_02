using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

[RequireComponent(typeof(PlayerHostModel))]
[RequireComponent(typeof(PlayerHostGun))]
[RequireComponent(typeof(LifeHostHandler))]
public class PlayerHostController : NetworkBehaviour
{
    PlayerHostModel _playerModel;
    PlayerHostGun _playerHostGun;
    NetworkInputData _localInputs;

    Vector3 _direction;

    // Start is called before the first frame update
    public override void Spawned()
    {
        _playerModel = GetComponent<PlayerHostModel>();
        _playerHostGun = GetComponent<PlayerHostGun>();

        GetComponent<LifeHostHandler>().OnEnableControls += (controls) => enabled = controls;
    }

    private void OnEnable() => PlayerController(true);
    private void OnDisable() => PlayerController(false);

    void PlayerController(bool value)
    {
        if (!_playerModel.Controller) return;
        _playerModel.Controller.enabled = value;
    }

    public override void FixedUpdateNetwork()
    {
        //Debug.Log("Previo obtener inputs en controller");
        if (!GetInput(out _localInputs)) return;
        //Debug.Log("Obteniendo inputs en controller");
        //Movement
        _direction = Vector3.forward * _localInputs.xMovement;
        _playerModel.Move(_direction);

        //Jump
        if(_localInputs.isJumpPressed) _playerModel.Jump();

        //Shoot
        if (_localInputs.isFirePressed) _playerHostGun.Shoot();

        //Dash
        if (_localInputs.isDashPressed) _playerModel.Dash();
    }

}
