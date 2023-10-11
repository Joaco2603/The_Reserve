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
    [SerializeField]
    private int vectorY;
    [SerializeField]
    private int vectorX;
}