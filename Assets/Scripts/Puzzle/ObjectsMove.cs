using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class ObjectsMove : NetworkBehaviour
{
    public bool mode = false;
	private bool YouCan = false;
    private int speed = 2; 

    private float zRotation = 0; // Almacena la rotación acumulada en el eje Z
    Quaternion originalRotation;

    Floor floorObject;

	Vector3 positionAfter;

	Vector3 player = new Vector3(0.03f,-0.08f,3.45f);
	Vector3 glass180 = new Vector3(0.02916622f,-0.335f,5.910002f);
	Vector3 glass0 = new Vector3(0.02916622f,-0.335f,1.08f);

    
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

	private void OnTriggerStay(Collider other)
	{
		Debug.Log(other);
		var obj = other.gameObject.GetComponent<Floor>();
		if(mode && obj != null)
        {
			YouCan = true;
		}
		var colisionWithPlayer = other.gameObject.tag;
		Debug.Log(colisionWithPlayer);
		if(mode && colisionWithPlayer == "Puzzle")
		{
			Debug.Log("Colision con ObjectsMove detectada.");
        	if (objectType == ObjectType.Player)
			{
				restart(player);
			}
			if(objectType == ObjectType.Glass180)
			{
				restart(glass180);
			}
			if(objectType == ObjectType.Glass0)
			{
				restart(glass0);
			}
		}
	}

	private void restart(Vector3 vector3)
	{
		this.transform.position = vector3;
	}


	private void OnTriggerExit(Collider other)
	{
		if(mode)
        {
			YouCan = false;
		}
	}


    private void MoveObjects()
    {

		if(mode && YouCan && Input.GetKeyDown(KeyCode.W))
		{
			this.transform.position = Vector3.Lerp(this.transform.position, this.transform.position + this.transform.forward, 3f);
		}

        // Rota a la izquierda con la letra 'A'
        if (mode && Input.GetKeyDown(KeyCode.A))
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
        if (mode && Input.GetKeyDown(KeyCode.D))
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


		if (objectType == ObjectType.Player || objectType == ObjectType.Glass0)
		{
			RotationUpdate(1);
		}else{
			RotationUpdate(-1);
		}	
    }
	private void RotationUpdate(int sign)
	{
		transform.rotation = Quaternion.Euler(originalRotation.eulerAngles.x, sign * zRotation, originalRotation.eulerAngles.z);
	}
}
