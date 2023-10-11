using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;
using DilmerGames.Core.Singletons;

public class LiveTree : NetworkSingleton<LiveTree>
{
    [SerializeField]
    private float networkPlayerHealthInspector = 500;

    [SerializeField]
    public NetworkVariable<float> networkPlayerHealth;

    private Vector3 transformPlayer;

    private Rigidbody rb;

    private NetworkObjectPool objectPool;

    private void Awake()
    {
        networkPlayerHealth = new NetworkVariable<float>(networkPlayerHealthInspector);
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true; // Desactivar la física al inicio.
    }

    public void CheckTreeHealth(Vector3 transformPlayerParam)
    {
        if(networkPlayerHealth.Value <= 0)
        {   transformPlayerParam = transformPlayer;
            FallTreeServerRpc();
        }
    }


    [ServerRpc(RequireOwnership = false)]
    private void FallTreeServerRpc()
    {
        // Activar la física.
        rb.isKinematic = false;
        rb.AddForce(transformPlayer * 10f); // Agregar una pequeña fuerza hacia atrás para iniciar la caída.
        Invoke("CallReturnNetworkClientRpc",10f);
    }


    [ClientRpc]
    private void CallReturnNetworkClientRpc()
    {   
        objectPool = NetworkObjectPool.Instance;
        objectPool.ReturnNetworkObject(this.NetworkObject, this.gameObject);
    }
    
}
