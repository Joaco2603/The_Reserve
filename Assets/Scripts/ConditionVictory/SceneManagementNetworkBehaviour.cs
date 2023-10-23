using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;
using DilmerGames.Core.Singletons;

public class SceneManagementNetworkBehaviour : Singleton<SceneManagementNetworkBehaviour>
{

    [ServerRpc(RequireOwnership = false)]
    public void ChangeSceneServerRpc(bool didWin, ServerRpcParams rpcParams = default)
    {
        string sceneName = didWin ? "timeline" : "timelineBad";

        // Obtén la instancia de NetworkManager y luego llama a LoadScene en ella
        NetworkManager networkManager = NetworkManager.Singleton;
        if (networkManager != null)
        {
            networkManager.SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        }
        else
        {
            Debug.LogError("NetworkManager Singleton is null.");
        }
    }
}
