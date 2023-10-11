using DilmerGames.Core.Singletons;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Assertions;

public class Puntaje : NetworkSingleton<Puntaje>
{
    public NetworkVariable<int> points = new NetworkVariable<int>(0);
}