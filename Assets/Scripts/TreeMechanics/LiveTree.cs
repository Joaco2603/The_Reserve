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

    [SerializeField]
    private GameObject treePlaceholderPrefab;


    // [Header("List of Prefabs")]
    // public List<GameObject> prefabs = new List<GameObject>();


    private Vector3 transformPlayer;

    private Rigidbody rb;

    private NetworkObjectPool objectPool;
    private Puntaje puntaje;

    private Quaternion transformRotation;

    private bool destroyCalls = false;

    private void Awake()
    {
        networkPlayerHealth = new NetworkVariable<float>(networkPlayerHealthInspector);
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true; // Desactivar la física al inicio.
    }


    public void CheckTreeHealth(Vector3 transformPlayerParam, Quaternion rotation)
    {
        if(networkPlayerHealth.Value <= 0)
        {   
            transformPlayer = transformPlayerParam;
            transformRotation = rotation;
            FallTreeServerRpc();
            Invoke("CreateTreePlaceholder",6f);//Cuando el árbol cae, crea un marcador
        }
    }


    [ServerRpc(RequireOwnership = false)]
    private void FallTreeServerRpc()
    {
        // Activar la física.
        rb.isKinematic = false;
        rb.AddForce(transformPlayer * 100f); // Agregar una pequeña fuerza hacia atrás para iniciar la caída.
        Invoke("CallReturnNetworkClientRpc",5f);
    }


    private void CreateTreePlaceholder()
    {
        if (destroyCalls) return;
            var CallInstanceTree = SpawnerControl.Instance;
            CallInstanceTree.SpawnTreeCall(transformPlayer,transformRotation);
            Invoke("ConvertDestroyCalls",2f);
        destroyCalls = true;
    }

    private void ConvertDestroyCalls()
    {
        destroyCalls = false;
    }



    [ClientRpc]
    private void CallReturnNetworkClientRpc()
    {   
        puntaje = Puntaje.Instance;
        puntaje.wood.Value += 10;
        objectPool = NetworkObjectPool.Instance;
        objectPool.ReturnNetworkObject(this.NetworkObject, this.gameObject);
    }



    [ServerRpc(RequireOwnership = false)]
    private void PlantTreeAtPlaceholderServerRpc(Vector3 position,ulong treePrefab)
    {

        // Solicitar un objeto de la piscina
        // var spawnerControl = SpawnerControl.Instance;
        // spawnerControl.SpawnTreeCall(position, Quaternion.identity);

        // Si tu versión de Netcode requiere que llames a Spawn después de sacar un objeto de la piscina, hazlo aquí.
        // if(!treePrefab.IsSpawned)
        // {
        //     networkTree.Spawn();
        // }

        // Eliminar el marcador actual
    }
}
