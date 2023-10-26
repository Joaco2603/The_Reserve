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

	[SerializeField]
	private Vector3 initialPosition;

	Transform player;
	Transform glass180;
	Transform glass0;

	public ulong playerInUse;

	private HashSet<ObjectType> objectsInContact = new HashSet<ObjectType>();

    public enum ObjectType
    {
        Player,
        Glass180,
        Glass0
    }
    
    public ObjectType objectType;

    // Update is called once per frame
    void Update()
    {
        if (mode && YouCan && Input.GetKeyDown(KeyCode.W))
        {
            MoveObjectServerRpc();
        }

        if (mode && Input.GetKeyDown(KeyCode.A))
        {
            RotateObjectLeftServerRpc();
        }

        if (mode && Input.GetKeyDown(KeyCode.D))
        {
            RotateObjectRightServerRpc();
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

			foreach (var tags in tagsObjects)
			{
    		ObjectsMove objMove = tags.GetComponent<ObjectsMove>();
    		if (objMove != null)
    		{
    		    Debug.Log(objMove.objectType);
    		    switch (objMove.objectType)
    		    {
    		        case ObjectType.Player:
    		        case ObjectType.Glass180:
    		        case ObjectType.Glass0:
    		            restartServerRpc();
    		            break;
        		}
    		}
			}
		}
	}

	[ServerRpc(RequireOwnership = false)]
	private void restartServerRpc()
	{
	        if (initialPosition != null)
	        {
	            transform.position = initialPosition;
	        }
	}



private void OnCollisionEnter(Collision other)
{
    var objMove = other.gameObject.GetComponent<ObjectsMove>();
    if (objMove != null)
    {
        objectsInContact.Add(objMove.objectType);
    }
    CheckForWinnerCondition(other);
}

private void OnCollisionExit(Collision other)
{
    var objMove = other.gameObject.GetComponent<ObjectsMove>();
    if (objMove != null)
    {
        objectsInContact.Remove(objMove.objectType);
    }
}

private void CheckForWinnerCondition(Collision other)
{
    var floor = other.gameObject.GetComponent<Floor>();
    if (floor != null && floor.Winner && 
        objectsInContact.Contains(ObjectType.Glass0) && 
        objectsInContact.Contains(ObjectType.Glass180))
    {
        var puntaje = Puntaje.Instance;
        puntaje.points.Value += 50;
        mode = false;
    }
}

	private void OnTriggerExit(Collider other)
	{
		if(mode)
        {
			YouCan = false;
		}
	}


	[ServerRpc(RequireOwnership = false)]
    private void MoveObjectServerRpc()
    {
        this.transform.position += this.transform.forward * 3f;
    }

    [ServerRpc(RequireOwnership = false)]
    private void RotateObjectLeftServerRpc()
    {
        if (zRotation == 360)
        {
            zRotation -= 360;
        }
        zRotation += 90;
        SetRotation();
    }

    [ServerRpc(RequireOwnership = false)]
    private void RotateObjectRightServerRpc()
    {
        if (zRotation == 0)
        {
            zRotation += 360;
        }
        zRotation -= 90;
        SetRotation();
    }

    private void SetRotation()
    {
        if (objectType == ObjectType.Glass0)
        {
            transform.rotation = Quaternion.Euler(0, zRotation, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, -180 + zRotation, 0);
        }
    }
}