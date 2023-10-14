using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

public class Trunk : NetworkBehaviour
{    
    [SerializeField]
    private GameObject treeAsset;


    public void OnTriggerStay(Collider other)
    {
        if (!IsClient && !IsOwner) return;

        var playerHud = other.gameObject.GetComponent<PlayerHud>();
        //Si este componente es nulo no aplicar esta aplicacion por defecto
        var networkObject = other.gameObject.GetComponent<NetworkObject>();
        if(networkObject != null && playerHud != null && Input.GetKey(KeyCode.E))
        {
            Debug.Log("Funciono");
            var spawnControl = SpawnerControl.Instance;
            spawnControl.RespawnTree(this.transform,this.transform.rotation);
        }
    }
}
