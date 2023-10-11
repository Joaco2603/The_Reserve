using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Assertions;


public class recolectRecicle : NetworkBehaviour
{
    public float distanceInFront = 1.0f; // distancia en la que el objeto será colocado al frente del jugador.
    public GameObject pickedObject = null;
    private Recicle recicleComponent;
    private NetworkObject networkObject;

    void FixedUpdate()
    {
        if(pickedObject != null)
        {
            if(Input.GetKey(KeyCode.X) && NetworkObject.IsLocalPlayer)
            {
                // Solicita al servidor cambiar el parentesco
                NetworkObject otherNetworkObject = pickedObject.gameObject.GetComponent<NetworkObject>();
                if (otherNetworkObject != null)
                {
                    DeselectPickedServerRpc(otherNetworkObject.NetworkObjectId);
                }
    
                pickedObject = null;
            }
        }
    }


    [ServerRpc(RequireOwnership = false)]
    void DeselectPickedServerRpc(ulong parentObjectId)
    {
        NetworkObject parentObject;
        if (NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(parentObjectId, out parentObject))
        {                 
            DeselectPickedClientRpc(parentObjectId);
            parentObject.transform.SetParent(null);
        }
    }


    [ClientRpc]
    void DeselectPickedClientRpc(ulong parentObjectId)
    {
        NetworkObject parentObject;
        if (NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(parentObjectId, out parentObject))
        {                 
            Rigidbody pickedRb = parentObject.GetComponent<Rigidbody>();
            pickedRb.useGravity = true;
            pickedRb.isKinematic = false;
        }
    }



    private void OnTriggerStay(Collider other)
    {

        networkObject = this.gameObject.GetComponent<NetworkObject>();

        if(!IsClient) return;
        
        recicleComponent = other.gameObject.GetComponent<Recicle>();
        if (recicleComponent != null && networkObject.IsSpawned)
        {
            if (Input.GetKey(KeyCode.C) && this.gameObject.GetComponentInChildren<Recicle>() == null && NetworkObject.IsLocalPlayer)
            {
                // Solicita al servidor cambiar el parentesco y la nueva posicion
                NetworkObject otherNetworkObject = other.gameObject.GetComponent<NetworkObject>();
                if (otherNetworkObject != null)
                {
                    ChangeObjectPositionServerRpc(otherNetworkObject.NetworkObjectId);
                    ReparentObjectServerRpc(otherNetworkObject.NetworkObjectId, this.gameObject.GetComponent<NetworkObject>().NetworkObjectId);
                }

                pickedObject = other.gameObject;
            }
        }
    }


    [ServerRpc(RequireOwnership = false)]
    void ChangeObjectPositionServerRpc(ulong objectId)
    {
            NetworkObject targetObject;
            if (NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(objectId, out targetObject))
            {
                Rigidbody otherRb = targetObject.GetComponent<Rigidbody>();
                otherRb.useGravity = false;
                otherRb.isKinematic = true;

                Vector3 pickupPosition = transform.position + transform.forward * distanceInFront;
                Vector3 newPosition = pickupPosition + new Vector3(0,1,0);

                targetObject.transform.position = newPosition;
            }
    }


    [ServerRpc(RequireOwnership = false)]
    void ReparentObjectServerRpc(ulong childObjectId, ulong parentObjectId)
    {
        NetworkObject childObject;
        NetworkObject parentObject;

        // Verifica que ambos objetos existen.
        if (NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(childObjectId, out childObject) &&
            NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(parentObjectId, out parentObject))
        {
            // Solo reparenta en el servidor.
            childObject.transform.SetParent(parentObject.transform);
        }
    }
}