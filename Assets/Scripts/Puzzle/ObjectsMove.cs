using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class ObjectsMove : NetworkBehaviour
{
    public bool mode = false;
    private int speed = 2; 
    private int rotationSpeed = 500;

    private float zRotation = 0; // Almacena la rotación acumulada en el eje Z
    Quaternion originalRotation;

    //Player
    private int vectorXPlayer;
    private int vectorYPlayer;
    //Glass180

    //Glass0


    
    public enum ObjectType
    {
        Player,
        Glass180,
        Glass0
    }
    
    public ObjectType objectType;

    void Start()
    {
        originalRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if(mode && IsClient && IsOwner)
        {
            MoveObjects();
        }
    }


    private void OnCollisionEnter(Collision other) 
    {
        if(other.gameObject.GetComponent<Floor>())
        {

        }
    }



    private void MoveObjects()
    {
        if (Input.GetKey(KeyCode.W))
        {
            // Debug.Log("W is pressed");
        }
        
        if (Input.GetKey(KeyCode.S))
        {
            // Debug.Log("S is pressed");
        }

        // Rota a la izquierda con la letra 'A'
        if (Input.GetKeyDown(KeyCode.A))
        {

            if(zRotation == 360)
            {
                zRotation -= 360;
            }
            zRotation += 90;
            // Restablece la rotación
            transform.rotation = Quaternion.Euler(90, 0, 0);

            transform.Rotate(0, 0, zRotation, Space.Self); // Rota en torno al eje   Y     del mundo hacia la izquierda.
        }

        // Rota a la derecha con la letra 'D'
        if (Input.GetKeyDown(KeyCode.D))
        {
            if(zRotation == 0)
            {
                 zRotation += 360;
            }
            zRotation -= 90;
            // Restablece la rotación
            transform.rotation = Quaternion.Euler(-90, 0, 0);
            transform.Rotate(0, 0, -zRotation, Space.Self); // Rota en torno al eje Y  del  mundo hacia la derecha.
        }


        switch(objectType)
        {
            case ObjectType.Player:
                // Player movement
                transform.rotation = Quaternion.Euler(originalRotation.eulerAngles.x, -zRotation, 0);
                break;
            case ObjectType.Glass180:
                // Object1 movement
                transform.rotation = Quaternion.Euler(originalRotation.eulerAngles.x, -180 + zRotation, 0);
                break;
            case ObjectType.Glass0:
                // Object2 movement
                transform.rotation = Quaternion.Euler(originalRotation.eulerAngles.x, zRotation, 0);
                break;
        }
    }
} 