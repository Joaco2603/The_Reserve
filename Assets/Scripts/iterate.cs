using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class iterate : NetworkBehaviour
{
    public GameObject playerPrefab; // Prefab del jugador
    public GameObject[] assets; // Lista de assets/modelos a asignar

    private int currentIndex = 0; // Índice para llevar un registro del asset actual

    public void SpawnPlayer()
    {
        // Instanciar el jugador
        GameObject newPlayer = Instantiate(playerPrefab, transform.position, Quaternion.identity);

        // Asignar el asset/modelo al jugador
        if (assets.Length > 0 && currentIndex < assets.Length)
        {
            GameObject assetInstance = Instantiate(assets[currentIndex], newPlayer.transform);
            // Aquí puedes hacer cualquier configuración adicional si es necesario

            // Actualizar el índice para el próximo jugador
            currentIndex++;
            if (currentIndex >= assets.Length)
            {
                currentIndex = 0; // Reiniciar el índice si hemos usado todos los assets
            }
        }
    }
}
