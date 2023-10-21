using DilmerGames.Core.Singletons;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine.InputSystem;
using UnityEngine;


public class recolectTrash : NetworkBehaviour
{

    private NetworkObjectPool objectPool;
    private Puntaje puntaje;
    private Trash trashComponent;

    private void OnTriggerStay(Collider other)
    {
        // Solo actuar si es el cliente el que detecta el trigger
        if (!IsClient && !IsOwner) return;
        //Trata de conseguir el componente Trash del trigger
        trashComponent = other.gameObject.GetComponent<Trash>();
        //Si este componente es nulo no aplicar esta aplicacion por defecto
        var networkObject = other.gameObject.GetComponent<NetworkObject>();
        if (networkObject != null && trashComponent != null && Input.GetKey(KeyCode.E))
        {
            DesespawnTrashServerRpc(networkObject.NetworkObjectId);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void DesespawnTrashServerRpc(ulong objectId)
    { 
        NetworkObject targetObject;
        if (NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(objectId, out targetObject))
            {
                DespawnTrashClientRpc(objectId);
            }
    }

    [ClientRpc]
    private void DespawnTrashClientRpc(ulong objectId)
    {
        if (NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(objectId, out NetworkObject targetObject))
        {
            puntaje = Puntaje.Instance;
            puntaje.points.Value += 10;
            var callSound = CallSound.Instance;
            // Reproducir el sonido
            callSound.PlaySoundEffect();
            Debug.Log(puntaje.points.Value);
            objectPool = NetworkObjectPool.Instance;
            objectPool.ReturnNetworkObject(targetObject, targetObject.gameObject);
        }
    }
}