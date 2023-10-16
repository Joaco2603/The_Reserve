using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SeedPlants : NetworkBehaviour
{
    
    void Start()
    {
        StartCoroutine(RepeatFunction());
    }


    IEnumerator RepeatFunction()  // Asegúrate de que sea IEnumerator y no IEnumerator<T>
    {
        while (true)
        {
            yield return new WaitForSeconds(15);
            destroySeed();
        }
    }


    private void destroySeed()
    {
        
    }

}
