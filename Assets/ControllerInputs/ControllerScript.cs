using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Netcode;


public class ControllerScript : NetworkBehaviour
{

    PlayerInput _playerInput;

    // Start is called before the first frame update
    void Start()
    {
        _playerInput = GetComponent<PlayerInput>();
    }

    public PlayerInput ReturnPlayerInput()
    {
        return _playerInput;
    }
}
