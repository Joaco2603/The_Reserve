using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Assertions;

public class Trash : NetworkBehaviour
{
    //Serializacion en lista de la estructura TrashConfig
    [SerializeField]
    private List<TrashConfig> TrashData;
    

    public List<TrashConfig> GetTrashData() 
    {
        return TrashData;
    }

    //Estructura con el valor del damage, su aporte(value) y una descipcion
    [Serializable]
    public struct TrashConfig
    {
        public int damage;
        public int value;
        public string descripcion;
    }
}
