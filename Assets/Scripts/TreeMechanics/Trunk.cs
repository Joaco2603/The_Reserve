using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

public class Trunk : NetworkBehaviour
{    
    [SerializeField]
    private GameObject treeAsset;

    private void OnCollisionEnter(Collision other)
    {
        if(Input.GetKey(KeyCode.R))
        {
            var spawnControl = SpawnerControl.Instance;
            spawnControl.RespawnTree(this.transform,Quaternion.identity,treeAsset);
        }
    }
}
