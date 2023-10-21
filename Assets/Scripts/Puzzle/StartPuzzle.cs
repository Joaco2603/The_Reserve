using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class StartPuzzle : NetworkBehaviour
{

    private bool mode = false;
    private ObjectsMove[] allObjectMoves;

    private void OnTriggerStay(Collider other) 
    {
        if(Input.GetKey(KeyCode.E))
        {
            var callSound = CallSound.Instance;
            // Reproducir el sonido
            callSound.PlaySoundEffect();
            // Encuentra todos los objetos con el componente ObjectsMove
            allObjectMoves = GameObject.FindObjectsOfType<ObjectsMove>();

            // Para cada objeto con el componente ObjectsMove, cambia mode a true
            foreach (ObjectsMove objectMove in allObjectMoves)
            {
                objectMove.mode = true;
            }
            mode = true;
        }

        if(Input.GetKey(KeyCode.Q) && mode)
        {
            foreach (ObjectsMove objectMove in allObjectMoves)
            {
                objectMove.mode = false;
            }
        }
    }
}
