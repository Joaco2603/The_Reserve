using DilmerGames.Core.Singletons;
using Unity.Netcode;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

public class SpawnerControl : NetworkSingleton<SpawnerControl>
{

    public enum ObjectType
    {
        Trash,
        Recycle
    }

    [Serializable]
    public struct SpawnableItem
    {
        public GameObject Prefab;
        public int PrewarmCount;
        public ObjectType Type;

        public void Spawn()
        {
            for(int i = 0; i < PrewarmCount; i++)
            {
                GameObject go = NetworkObjectPool.Instance.GetNetworkObject(Prefab).gameObject;
                go.transform.position = new Vector3(UnityEngine.Random.Range(-10, 10), 10.0f, UnityEngine.Random.Range(-10, 10));
                go.GetComponent<NetworkObject>().Spawn();
            }
        }
    }

    [SerializeField]
    private List<SpawnableItem> spawnableItems = new List<SpawnableItem>();

    private void Start()
    {
        NetworkManager.Singleton.OnServerStarted += () =>
        {
            NetworkObjectPool.Instance.InitializePool();
        };

        StartCoroutine(RepeatFunction());
    }

    public void SpawnObjects()
    {
        if (!IsServer) return;

        foreach (var item in spawnableItems)
        {
            item.Spawn();
        }
    }

    IEnumerator RepeatFunction()  // Asegúrate de que sea IEnumerator y no IEnumerator<T>
    {
        while (true)
        {
            yield return new WaitForSeconds(15);
            SpawnObjects();
        }
    }


}

