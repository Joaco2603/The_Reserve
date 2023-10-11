using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Assertions;


public class GarbageDeposit : NetworkBehaviour
{

    private Puntaje puntaje;

    //Agrega los tipos de reciclajes
    public enum GarbageType
    {
        Plastic,
        Organic,
        Glass,
        Dangerous,
        Paper
    }


    //Serializacion en lista de la estructura TrashConfig
    [SerializeField]
    private List<GarbageConfig> GarbageData;

    public String GetGarbageType() 
    {
        return GarbageData[GarbageData.Count-1].garbageType.ToString();
    }

    //Estructura con el valor del damage, su aporte(value) y una descipcion
    [Serializable]
    public struct GarbageConfig
    {
        public GarbageType garbageType;
    }

    private NetworkObjectPool objectPool;
    private Recicle recicleComponent;

    private void OnTriggerStay(Collider other)
    {
        if(!IsClient) return;
        
        recicleComponent = other.gameObject.GetComponent<Recicle>();
        var networkObject = other.gameObject.GetComponent<NetworkObject>();
        if (recicleComponent != null && networkObject != null)
        {
            String GarbageConfigInstance = this.GetGarbageType();
            String RecicleTypeInstance = recicleComponent.GetRecicleType();
            if(GarbageConfigInstance == RecicleTypeInstance)
            {
                DesespawnTrashServerRpc(networkObject.NetworkObjectId);
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void DesespawnTrashServerRpc(ulong objectId)
    { 
        NetworkObject targetObject;
        if (NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(objectId, out targetObject))
            {
                targetObject.gameObject.transform.SetParent(null);
                DespawnRecicleClientRpc(objectId);
            }
    }


    [ClientRpc]
    private void DespawnRecicleClientRpc(ulong objectId)
    {
        if (NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(objectId, out NetworkObject targetObject))
        {
            puntaje = Puntaje.Instance;
            puntaje.points.Value += 10;
            Debug.Log(puntaje.points.Value);
            objectPool = NetworkObjectPool.Instance;
            objectPool.ReturnNetworkObject(targetObject, targetObject.gameObject);
        }
    }



}