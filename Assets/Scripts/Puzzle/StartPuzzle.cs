using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.InputSystem;

public class StartPuzzle : NetworkBehaviour
{

    private bool mode = false;
    private ObjectsMove[] allObjectMoves;

    public Camera firstCamera; // Asigna la primera cámara aquí a través del inspector
    public Camera secondCamera; // Asigna la segunda cámara aquí a través del inspector
    public float transitionDuration = 0.5f; // Duración de la transición en segundos

    private PlayerWithRaycastControl player;

    private PlayerInput _playerInput;

    private void Start()
    {
        _playerInput = GetComponent<PlayerInput>();
    }


    private void OnTriggerStay(Collider other) 
    {
        if(_playerInput.actions["collect"].ReadValue<float>() > 0 && IsClient)
        {
            var callSound = CallSound.Instance;
            // Reproducir el sonido
            callSound.PlaySoundEffect();
            // Encuentra todos los objetos con el componente ObjectsMove
            allObjectMoves = GameObject.FindObjectsOfType<ObjectsMove>();

            player = other.gameObject.GetComponent<PlayerWithRaycastControl>();

            // Para cada objeto con el componente ObjectsMove, cambia mode a true
            foreach (ObjectsMove objectMove in allObjectMoves)
            {
                objectMove.mode = true;
                objectMove.playerInUse = this.gameObject.GetComponent<NetworkObject>().OwnerClientId;
            }
            player.GameMode = true;
            mode = true;
            StartCoroutine(Transition(false,true));
        }

        if(_playerInput.actions["Seed"].ReadValue<float>() > 0 && mode)
        {
            foreach (ObjectsMove objectMove in allObjectMoves)
            {
                objectMove.mode = false;
            }
            player = other.gameObject.GetComponent<PlayerWithRaycastControl>();
            player.GameMode = false;
            StartCoroutine(Transition(true,false));
        }
    }

    IEnumerator Transition(bool firstBoolean,bool SecondBoolean)
    {
        float t = 0.0f;
        Vector3 startPosition = firstCamera.transform.position;
        Quaternion startRotation = firstCamera.transform.rotation;

        while (t < 1.0f)
        {
            t += Time.deltaTime * (1.0f / transitionDuration);

            // Interpolación entre las posiciones de las cámaras
            firstCamera.transform.position = Vector3.Lerp(startPosition, secondCamera.transform.position, t);
            // Interpolación entre las rotaciones de las cámaras
            firstCamera.transform.rotation = Quaternion.Slerp(startRotation, secondCamera.transform.rotation, t);

            // Esperar hasta el próximo frame
            yield return null;
        }

        // Desactivar la primera cámara y activar la segunda
        firstCamera.enabled = firstBoolean;
        secondCamera.enabled = SecondBoolean;

        // Opcionalmente, mover la primera cámara de regreso a su posición inicial
        firstCamera.transform.position = startPosition;
        firstCamera.transform.rotation = startRotation;
    }
}
