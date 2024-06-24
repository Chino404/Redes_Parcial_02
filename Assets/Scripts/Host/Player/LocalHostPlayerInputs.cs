using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalHostPlayerInputs : MonoBehaviour
{
    NetworkInputData _inputData;

    bool _isJumpPressed;
    bool _isFirePressed;
    bool _isDashPressed;

    void Awake()
    {
        _inputData = new NetworkInputData();
    }

    void Update()
    {
        if (Time.timeScale == 0) return;
        _inputData.xMovement = Input.GetAxisRaw("Horizontal");

        if(Input.GetKeyDown(KeyCode.W)) 
        { 
            _isJumpPressed = true;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _isFirePressed = true;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            _isDashPressed=true;
        }
    }

    public NetworkInputData GetLocalInputs()
    {
        _inputData.isJumpPressed = _isJumpPressed;
        _inputData.isFirePressed = _isFirePressed;
        _inputData.isDashPressed = _isDashPressed;

        _isJumpPressed = _isFirePressed =_isDashPressed = false;
        //_isJumpPressed = false;
        //_isFirePressed = false;

        return _inputData;
    }
}
