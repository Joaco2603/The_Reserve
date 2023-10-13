using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Floor : NetworkBehaviour
{
    [SerializeField]
    private bool isWalking = true;
    [SerializeField] 
    private bool WinCondition = false;

    public int vectorY;

    public int vectorX;
}