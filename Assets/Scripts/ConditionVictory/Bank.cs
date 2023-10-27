using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;


public class Bank : NetworkBehaviour
{

    private Puntaje puntaje;

    [HideInInspector] // Escondemos el DateTime real, para que no haya confusión.
    public DateTime startTime;

    [HideInInspector] // Escondemos el DateTime real, para que no haya confusión.
    public DateTime endTime;

    TimeSpan duration;


    private string startTimeString;

    [SerializeField]  // Asignamos el tiempo que debe trascurrir
    private string endTimeString;

    List<ulong> connectedClientIds;

    void Start()
    {
        if (IsServer)
        {
            OnNetworkSpawn();
            StartEvent();
        }
    }

    void StartEvent() 
    {
        startTime = DateTime.Now;
        startTimeString = startTime.ToString();

        // Notifica a todos los clientes que el evento ha comenzado
        StartEventClientRPC();

        // Iniciar la corutina para esperar 10 segundos (o el tiempo que desees) antes de llamar a EndEvent
        StartCoroutine(WaitAndEndEvent(120f)); // 10f significa 10 segundos.
    }

    IEnumerator WaitAndEndEvent(float waitTime)
    {
        yield return new WaitForSeconds(waitTime); // Espera el tiempo especificado.
        EndEvent();
    }

    [ClientRpc]
    void EndEventClientRPC()
    {
        bool disable = false;
        if(!disable)
        {
            endTime = DateTime.Now;
            endTimeString = endTime.ToString();
            Debug.Log("Evento finalizado a las: " + endTimeString);
            duration = endTime - startTime;

            EndEvent();
            disable = true;
        }
    }

    // RPC para notificar a todos los clientes que el evento ha comenzado
    [ClientRpc]
    void StartEventClientRPC()
    {
        bool disable = false;
        if(!disable)
        {
            if (IsClient)
            {
                // Aquí puedes añadir código que solo quieras que se ejecute en los clientes 
                // cuando comienza el evento.
                Debug.Log("Evento iniciado a las: " + startTimeString);
            }
            disable = true;
        }
    }

    // RPC para notificar a todos los clientes que el evento ha terminado
    void EndEvent()
    {
        if (IsClient)
        {
            // Aquí puedes añadir código que solo quieras que se ejecute en los clientes 
            // cuando el evento termina.
            Debug.Log("Duración del evento: " + duration.ToString());
            Debug.Log("Tiempo terminado");
            puntaje = Puntaje.Instance;
        if (IsServer)
        { 
            if(puntaje.points.Value>100)
            {
                Debug.Log("Ganaste");
                // var sceneManagementNetworkBehaviour = SceneManagementNetworkBehaviour.Instance;
                // sceneManagementNetworkBehaviour.ChangeSceneServerRpc(true);
                DisconnectAllClientsAndStopServerRpc(true);
            }else{
                Debug.Log("Perdiste");
                // var sceneManagementNetworkBehaviour = SceneManagementNetworkBehaviour.Instance;
                // sceneManagementNetworkBehaviour.ChangeSceneServerRpc(false);
                DisconnectAllClientsAndStopServerRpc(false);
            }
        }
        }
    }


    [ServerRpc]
    private void DisconnectAllClientsAndStopServerRpc(bool state)
    {
        // Notificar a todos los clientes (incluido el host) para que cambien la escena
        ChangeSceneClientRpc(state);

        // Iniciar una corutina para desconectar a los clientes y cerrar el servidor después de un breve retraso
        StartCoroutine(ShutdownServerAfterDelay());
    }

    private IEnumerator ShutdownServerAfterDelay()
    {
        // Espera un breve retraso (por ejemplo, 5 segundos) para dar tiempo a los clientes a cambiar de escena
        yield return new WaitForSeconds(5f);

        // Desconectar a todos los clientes
        foreach (var client in NetworkManager.Singleton.ConnectedClientsList)
        {
            if (client.ClientId != NetworkManager.Singleton.ServerClientId) //Evita desconectar el host
            {
                NetworkManager.Singleton.DisconnectClient(client.ClientId);
            }
        }

        // Cerrar el servidor
        NetworkManager.Singleton.Shutdown();
    }

    [ClientRpc]
    private void ChangeSceneClientRpc(bool state)
    {
        bool disable = false;
        if(!disable)
        {
            string stateString = state ? "timeline" : "timelineBad";
            SceneManager.LoadScene(stateString);
            disable = true;
        }
    }

}



