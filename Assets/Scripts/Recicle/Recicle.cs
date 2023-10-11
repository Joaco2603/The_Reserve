using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Assertions;

public class Recicle : NetworkBehaviour
{
    //Agrega los tipos de reciclajes
    public enum RecicleType
    {
        Plastic,
        Organic,
        Glass,
        Paper
    }


    //Serializacion en lista de la estructura TrashConfig
    [SerializeField]
    private List<RecicleConfig> RecicleData;

    public List<RecicleConfig> GetRecicleData() 
    {
        return RecicleData;
    }

    public String GetRecicleType()
    {
        if (RecicleData.Count > 0)
        {
            return RecicleData[RecicleData.Count - 1].recicleType.ToString(); // Accede al último elemento
        }
        else
        {
            return "No data available";
        }
    }

    //Estructura con el valor del damage, su aporte(value) y una descipcion
    [Serializable]
    public struct RecicleConfig
    {
        public int damage;
        public int value;
        public string descripcion;
        public RecicleType recicleType;
    }

}
