using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class StartPuzzle : NetworkBehaviour
{
    private void OnTriggerStay(Collider other) 
    {
        if(Input.GetKey(KeyCode.E))
        {
            // Encuentra todos los objetos con el componente ObjectsMove
            ObjectsMove[] allObjectMoves = GameObject.FindObjectsOfType<ObjectsMove>();

            // Para cada objeto con el componente ObjectsMove, cambia mode a true
            foreach (ObjectsMove objectMove in allObjectMoves)
            {
                objectMove.mode = true;
            }
        }
    }
}
