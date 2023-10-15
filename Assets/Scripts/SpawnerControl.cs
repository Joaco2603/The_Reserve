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
        Recycle,
        Tree,
        TreeBroken,
        seed,
        sweath
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

        public void SpawnTrunk(Vector3 position, Quaternion rotation)
        {
            for(int i = 0; i < PrewarmCount; i++)
            {
                GameObject go = NetworkObjectPool.Instance.GetNetworkObject(Prefab).gameObject;
                go.transform.position = position + new Vector3(0,1,0);
                go.transform.rotation = rotation;
                go.GetComponent<NetworkObject>().Spawn();
            }
        }

        public void SpawnTree(Transform position,Quaternion rotation)
        {
            for(int i = 0; i < PrewarmCount; i++)
            {
                GameObject go = NetworkObjectPool.Instance.GetNetworkObject(Prefab).gameObject;
                go.transform.position = position.position + new Vector3(0,-1.6f,0);
                go.transform.rotation = rotation;
                go.GetComponent<NetworkObject>().Spawn();
            }
        }

        public void SpawnSeed(Transform position,Quaternion rotation)
        {
            for(int i = 0; i < PrewarmCount; i++)
            {
                GameObject go = NetworkObjectPool.Instance.GetNetworkObject(Prefab).gameObject;
                go.transform.position = position.position + new Vector3(0,0.5f,0);
                go.transform.rotation = rotation;
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
            if(item.Type == SpawnerControl.ObjectType.Recycle || item.Type == SpawnerControl.ObjectType.Trash)
            {
                item.Spawn();
            }
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

    public void SpawnTreeCall(Vector3 position,Quaternion rotation)
    {
        if (!IsServer) return;

        foreach (var item in spawnableItems)
        {
            if(item.Type == SpawnerControl.ObjectType.Tree)
            {
                item.SpawnTrunk(position,rotation);
            }
        }
    }


    public void RespawnTree(Transform position,Quaternion rotation)
    {
        if (!IsServer) return;

        foreach (var item in spawnableItems)
        {
            if(item.Type == SpawnerControl.ObjectType.TreeBroken)
            {
                item.SpawnTree(position,rotation);
            }
        }
    }


    public void PlantSeed(Transform position,Quaternion rotation)
    {
        if (!IsServer) return;

        foreach (var item in spawnableItems)
        {
            if(item.Type == SpawnerControl.ObjectType.seed)
            {
                item.SpawnSeed(position,rotation);
            }
        }
    }

}

