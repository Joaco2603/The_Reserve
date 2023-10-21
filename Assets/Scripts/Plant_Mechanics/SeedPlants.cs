using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SeedPlants : NetworkBehaviour
{

    private bool status = false;

    private int number = 0;
    
    
    void Start()
    {
        StartCoroutine(RepeatFunction());
    }


    private void TimerButton()
    {
        number += 1;
        Debug.Log(number);
    }

    private void OnTriggerStay(Collider other)
    {
        if(Input.GetAxis("Fire1") > 0)
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
