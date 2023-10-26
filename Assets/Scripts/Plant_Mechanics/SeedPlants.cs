using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;


public class SeedPlants : NetworkBehaviour
{

    private bool status = false;

    private int number = 0;
    
    private PlayerInput _playerInput;
    private PlayerWithRaycastControl player;


    void Start()
    {
        StartCoroutine(RepeatFunction());
    }


    private void TimerButton()
    {
        number += 1;
        Debug.Log("Timer");
    }

    private void OnTriggerStay(Collider other)
    {

        if (!IsClient && !IsOwner) return;
        var player = other.GetComponent<PlayerWithRaycastControl>();
        var controller = other.GetComponent<PlayerInput>();
        if(controller != null && player != null && controller.actions["Seed"].ReadValue<float>() > 0)
        {
            TimerButton();
        }
        else
        {
            if(number>=100)
            {
                status = true;
            }
            else
            {
                status = false;
            }
        }
    }


    IEnumerator RepeatFunction()  // Asegúrate de que sea IEnumerator y no IEnumerator<T>
    {
        while (true)
        {
            yield return new WaitForSeconds(7);
            if(status)
            {
                PlantsServerRpc();
            }
            else
            {
                destroySeedServerRpc();
            }
        }
    }


    [ServerRpc(RequireOwnership = false)]
    private void PlantsServerRpc()
    {
        PlantsClientRpc();
    }

    [ClientRpc]
    private void PlantsClientRpc()
    {
        var callSound = CallSound.Instance;
        // Reproducir el sonido
        callSound.PlaySoundEffect();
        var objectPool = NetworkObjectPool.Instance;
        objectPool.ReturnNetworkObject(this.NetworkObject,this.gameObject);
        var spawner = SpawnerControl.Instance;
        spawner.PlantSweath(this.transform,this.transform.rotation * Quaternion.Euler(new Vector3(0,-90,0)));
    }


    [ServerRpc(RequireOwnership = false)]
    private void destroySeedServerRpc()
    {
        destroySeedClientRpc();
    }


    [ClientRpc]
    private void destroySeedClientRpc()
    {
        var objectPool = NetworkObjectPool.Instance;
        objectPool.ReturnNetworkObject(this.NetworkObject,this.gameObject);
    }

}
