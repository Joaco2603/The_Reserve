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

    private NetworkObject NetworkObjectToDestroy;
    private GameObject gameObjectToDestroy;

    private bool destroyCalls = false;
    private bool destroyCallsCreatePlaceHolder = false;

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
            CreateTreePlaceholderServerRpc();//Cuando el árbol cae, crea un marcador
            FallTreeServerRpc();
        }
    }


    [ServerRpc(RequireOwnership = false)]
    private void FallTreeServerRpc()
    {
        if (destroyCallsCreatePlaceHolder) return;
            // Activar la física.
            rb.isKinematic = false;
            rb.AddForce(transformPlayer * 2f); // Agregar una pequeña fuerza hacia atrás para iniciar la caída.
            Invoke("CallReturnNetworkClientRpc",2f);
        destroyCallsCreatePlaceHolder = true;
        Invoke("ConvertDestroyCalls",3f);
    }

    [ServerRpc(RequireOwnership = false)]
    private void CreateTreePlaceholderServerRpc()
    {
        if (destroyCalls) return;
            CreateTreePlaceholderClientRpc();
            Invoke("ConvertDestroyCalls",3f);
        destroyCalls = true;
    }

    [ClientRpc]
    private void CreateTreePlaceholderClientRpc()
    {
        var CallInstanceTree = SpawnerControl.Instance;
        CallInstanceTree.SpawnTreeCall(transformPlayer,transformRotation);
    }

    private void ConvertDestroyCalls()
    {
        destroyCalls = false;
    }



    [ClientRpc]
    private void CallReturnNetworkClientRpc()
    {   
        NetworkObjectToDestroy = this.NetworkObject;
        gameObjectToDestroy = this.gameObject;
        puntaje = Puntaje.Instance;
        puntaje.wood.Value += 10;
        objectPool = NetworkObjectPool.Instance;
        objectPool.ReturnNetworkObject(NetworkObjectToDestroy ,gameObjectToDestroy);
    }

}
