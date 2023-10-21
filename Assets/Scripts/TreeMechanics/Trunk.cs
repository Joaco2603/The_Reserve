using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

public class Trunk : NetworkBehaviour
{    
    [SerializeField]
    private GameObject treeAsset;

    Collider collider;
    Rigidbody rb;

    private void OnCollisionEnter(Collision other)
    {
        if (!IsClient && !IsOwner) return;

        collider = this.GetComponent<Collider>();

        rb = this.GetComponent<Rigidbody>();

        Invoke("Disable",1f);
    }

    void Disable()
    {
        rb.useGravity = false;
        rb.isKinematic = false;
        collider.isTrigger = true;
    }

    public void OnTriggerStay(Collider other)
    {
        if (!IsClient && !IsOwner) return;

        var playerHud = other.gameObject.GetComponent<PlayerHud>();
        //Si este componente es nulo no aplicar esta aplicacion por defecto
        var networkObject = other.gameObject.GetComponent<NetworkObject>();
        if(networkObject != null && playerHud != null && Input.GetKey(KeyCode.E))
        {
            DespawnServerRpc(); 
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void DespawnServerRpc()
    {
        CallReturnNetworkClientRpc();
    }



    [ClientRpc]
    private void CallReturnNetworkClientRpc()
    {

        var callSound = CallSound.Instance;
        // Reproducir el sonido
        callSound.PlaySoundEffect();

        var spawnControl = SpawnerControl.Instance;
        spawnControl.RespawnTree(this.transform,this.transform.rotation);

        var objectPool = NetworkObjectPool.Instance;
        objectPool.ReturnNetworkObject(this.NetworkObject, this.gameObject);
    }

}
