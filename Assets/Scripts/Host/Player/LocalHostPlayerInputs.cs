using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalHostPlayerInputs : MonoBehaviour
{
    NetworkInputData _inputData;

    bool _isJumpPressed;
    bool _isFirePressed;

    void Awake()
    {
        _inputData = new NetworkInputData();
    }

    void Update()
    {
        _inputData.xMovement = Input.GetAxis("Horizontal");

        if(Input.GetKeyDown(KeyCode.W)) 
        { 
            _isJumpPressed = true;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _isFirePressed = true;
        }
    }

    public NetworkInputData GetLocalInputs()
    {
        _inputData.isJumpPressed = _isJumpPressed;
        _inputData.isFirePressed = _isFirePressed;

        _isJumpPressed = _isFirePressed = false;
        //_isJumpPressed = false;
        //_isFirePressed = false;

        Debug.Log("Obteniendo inputs");

        return _inputData;
    }
}
