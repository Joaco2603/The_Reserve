using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DilmerGames.Core.Singletons;

public class CallSound : Singleton<CallSound>
{
    // AudioSource para reproducir el efecto de sonido
    private AudioSource audioSource;

    // Clip de audio que contiene el efecto de sonido
    public AudioClip soundEffect;

    // Se llama al despertar, antes del primer frame
    private void Awake()
    {
        // Configura el AudioSource
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    // Método público para reproducir el efecto de sonido
    public void PlaySoundEffect()
    {
        // Asignar el clip de audio al audio source
        audioSource.clip = soundEffect;
        
        // Reproducir el sonido
        audioSource.Play();
    }

    // Método para asignar un nuevo efecto de sonido
    public void SetSoundEffect(AudioClip newSoundEffect)
    {
        if (newSoundEffect != null)
        {
            soundEffect = newSoundEffect;
        }
        else
        {
            Debug.LogError("Invalid sound effect assigned.");
        }
    }
}
