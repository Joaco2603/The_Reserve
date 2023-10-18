using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;
using System.Collections.Generic;

public class RecolectPlant : NetworkBehaviour
{
    
    [SerializeField]
    private bool Type = true;
    private bool isReady = false;

    private Transform position;
    private Quaternion rotation;

    private void OnTriggerStay(Collider other) 
    {
        var player = other.gameObject.GetComponent<PlayerHud>();
        var ObjectNet = other.gameObject.GetComponent<NetworkObject>();
        if(Input.GetKey(KeyCode.E) && player != null && ObjectNet != null && Type)
        {
            RecolectServerRpc();
        }

        if(Input.GetKey(KeyCode.Q) && player != null && ObjectNet != null && this.transform.childCount == 0 && !Type)
        {
            if(isReady) return;
            isReady = true;
            PlantServerRpc();
            Invoke("ReloadPlant",5f);
        }
    }

    private void ReloadPlant()
    {
        isReady = false;
    }

    [ServerRpc(RequireOwnership = false)]
    private void RecolectServerRpc()
    {
        this.gameObject.transform.parent = null;
        RecolectClientRpc();
    }

    [ClientRpc]
    private void RecolectClientRpc()
    {
        var objectPool = NetworkObjectPool.Instance;
        objectPool.ReturnNetworkObject(this.gameObject.GetComponent<NetworkObject>(),this.gameObject);   
    }

    [ServerRpc(RequireOwnership = false)]
    private void PlantServerRpc()
    {
        PlantClientRpc();
    }

    [ClientRpc]
    private void PlantClientRpc()
    {
        var spawner = SpawnerControl.Instance;
        spawner.PlantSeed(this.transform,this.rotation);
    }

}
