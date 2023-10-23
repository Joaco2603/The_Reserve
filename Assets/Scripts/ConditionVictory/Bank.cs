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
        StartCoroutine(WaitAndEndEvent(600f)); // 10f significa 10 segundos.
    }

    IEnumerator WaitAndEndEvent(float waitTime)
    {
        yield return new WaitForSeconds(waitTime); // Espera el tiempo especificado.
        EndEvent();
    }

    [ClientRpc]
    void EndEventClientRPC()
    {
        endTime = DateTime.Now;
        endTimeString = endTime.ToString();
        Debug.Log("Evento finalizado a las: " + endTimeString);
        duration = endTime - startTime;

        EndEvent();
    }

    // RPC para notificar a todos los clientes que el evento ha comenzado
    [ClientRpc]
    void StartEventClientRPC()
    {
        if (IsClient)
        {
            // Aquí puedes añadir código que solo quieras que se ejecute en los clientes 
            // cuando comienza el evento.
            Debug.Log("Evento iniciado a las: " + startTimeString);
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
                DisconnectAllClientsAndStopServer(true);
            }else{
                Debug.Log("Perdiste");
                // var sceneManagementNetworkBehaviour = SceneManagementNetworkBehaviour.Instance;
                // sceneManagementNetworkBehaviour.ChangeSceneServerRpc(false);
                DisconnectAllClientsAndStopServer(false);
            }
        }
        }
    }

    
    private void DisconnectAllClientsAndStopServer(bool state)
    {
        foreach (var client in NetworkManager.Singleton.ConnectedClientsList)
        {
            if (client.ClientId != NetworkManager.Singleton.ServerClientId) //Evita desconectar el host
            {
                NetworkManager.Singleton.DisconnectClient(client.ClientId);
            }
        }
        NetworkManager.Singleton.Shutdown();

        string stateString;
        stateString = state ? "timeline" : "timelineBad";

        SceneManager.LoadScene(stateString);
    }


}



