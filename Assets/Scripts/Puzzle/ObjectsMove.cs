using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class ObjectsMove : NetworkBehaviour
{
    public bool mode = false;
	private bool YouCan = false;

    private float zRotation = 0; // Almacena la rotación acumulada en el eje Z
    Quaternion originalRotation;

    Floor floorObject;

	Vector3 positionAfter;

	Transform player;
	Transform glass180;
	Transform glass0;

    
    public enum ObjectType
    {
        Player,
        Glass180,
        Glass0
    }
    
    public ObjectType objectType;

    void Start()
    {

		if(this.objectType == ObjectType.Player)
		{
			Debug.Log("Entro Player");
			Debug.Log(this.transform.position);
			player = this.transform;
		}
		if(this.objectType == ObjectType.Glass180)
		{
			Debug.Log("Entro Glass180");
			Debug.Log(this.transform.position);
			glass180 = this.transform;
		}
		if(this.objectType == ObjectType.Glass0)
		{
			Debug.Log("Entro Glass0");
			Debug.Log(this.transform.position);
			glass0 = this.transform;
		}
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
		var obj = other.gameObject.GetComponent<Floor>();
		if(mode && obj != null)
        {
			YouCan = true;
		}
		var colisionWithPlayer = other.gameObject.tag;
		if(mode && Input.GetKeyDown(KeyCode.W) && colisionWithPlayer == "Puzzle")
		{
			var tagsObjects = GameObject.FindGameObjectsWithTag("Puzzle");

			foreach(var tags in tagsObjects)
			{
				Debug.Log(tags.GetComponent<ObjectsMove>().objectType);
				if(tags.GetComponent<ObjectsMove>().objectType == ObjectType.Player && tags.GetComponent<ObjectsMove>() != null)
				{
					Debug.Log("Entro al player");
					restart(player);
				}
				if(tags.GetComponent<ObjectsMove>().objectType == ObjectType.Glass180 && tags.GetComponent<ObjectsMove>() != null)
				{
					restart(glass180);
				}
				if(tags.GetComponent<ObjectsMove>().objectType == ObjectType.Glass0 && tags.GetComponent<ObjectsMove>() != null)
				{
					restart(glass0);
				}
			}
		}
	}

	private void OnCollisionStay(Collision other)
	{
		var obj = other.gameObject.GetComponent<Floor>();
		if(obj.Winner && other.gameObject.GetComponent<ObjectsMove>().objectType == ObjectType.Glass0 && other.gameObject.GetComponent<ObjectsMove>().objectType == ObjectType.Glass180)
		{
			var puntaje = Puntaje.Instance;
            puntaje.points.Value += 50;
			mode = false;
		}
	}

	private void restart(Transform newPosition)
	{
		Debug.Log("Restart");
		this.transform.position = newPosition.position + new Vector3(0,10,0);
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

		//objectType == ObjectType.Player ||
		//objectType == ObjectType.Player ||  
		if (objectType == ObjectType.Glass0)
		{
			transform.rotation = Quaternion.Euler(0, zRotation,0);
		}else{
			transform.rotation = Quaternion.Euler(0,-180 + zRotation,0);
		}	
    }
}
