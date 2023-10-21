using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Floor : NetworkBehaviour
{
    public bool isWalking = true;

    public bool Winner = false;
}