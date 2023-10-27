using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.InputSystem;

public class ObjectsMove : NetworkBehaviour
{
    public bool mode = false;
	private bool YouCan = false;

    private bool enabled = true;

    private float zRotation = 0; // Almacena la rotación acumulada en el eje Z
    Quaternion originalRotation;

    Floor floorObject;

	[SerializeField]
	private Vector3 initialPosition = new Vector3(0, 0, 0);

    // Lista estática para rastrear todas las instancias de este script
    private static List<ObjectsMove> allInstances = new List<ObjectsMove>();

	public ulong playerInUse;

	private HashSet<ObjectType> objectsInContact = new HashSet<ObjectType>();

    private PlayerInput _playerInput;


    public enum ObjectType
    {
        Player,
        Glass180,
        Glass0
    }
    
    public ObjectType objectType;


    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _playerInput.actions["MoveUp"].performed += ctx => OnMoveUp();
        _playerInput.actions["MoveLeft"].performed += ctx => OnMoveLeft();
        _playerInput.actions["MoveRight"].performed += ctx => OnMoveRight();
    }

    void Start()
    {
        allInstances.Add(this);
    }

    private void OnDestroy()
    {
        _playerInput.actions["MoveUp"].performed -= ctx => OnMoveUp();
        _playerInput.actions["MoveLeft"].performed -= ctx => OnMoveLeft();
        _playerInput.actions["MoveRight"].performed -= ctx => OnMoveRight();
        allInstances.Remove(this);
    }

    private bool hasProcessedInputThisFrame = false;

    private void Update()
    {
        hasProcessedInputThisFrame = false;
    }

    private void OnMoveUp()
    {
        if (mode && YouCan)
        {
            MoveObjectServerRpc();
        }
    }

    private void OnMoveLeft()
    {
        Debug.Log("Entro");
        if (mode)
        {
            RotateObjectLeftServerRpc();
        }
    }

    private void OnMoveRight()
    {
        if (mode)
        {
            RotateObjectRightServerRpc();
        }
    }


    // Update is called once per frame
    // void Update()
    // {
    //     if (mode && YouCan && _playerInput.actions["MoveUp"].ReadValue<float>() > 0 )
    //     {
    //         MoveObjectServerRpc();
    //     }

    //     if (mode && _playerInput.actions["MoveLeft"].ReadValue<float>() > 0)
    //     {
    //         RotateObjectLeftServerRpc();
    //     }

    //     if (mode && _playerInput.actions["MoveRight"].ReadValue<float>() > 0)
    //     {
    //         RotateObjectRightServerRpc();
    //     }
    // }

	private void OnTriggerEnter(Collider other)
    {
        var obj = other.gameObject.GetComponent<Floor>();
        if (mode && obj != null)
        {
            YouCan = true;
        }

        var colisionWithPlayer = other.gameObject.tag;

        // Si el collider que fue golpeado tiene el componente "ObjectsMove"
        if (other.gameObject.GetComponent<ObjectsMove>() != null)
        {
            // Restablece todos los objetos con este script
            foreach (var instance in ObjectsMove.allInstances)
            {
                instance.ResetPosition();
                // Si es el servidor, sincronizamos la posición con los clientes
                if (IsServer)
                {
                    instance.RpcSyncPositionClientRpc(instance.initialPosition);
                }
            }
        }
    }

    private void ResetPosition()
    {
        transform.position = initialPosition;
    }

    [ClientRpc]
    private void RpcSyncPositionClientRpc(Vector3 newPosition)
    {
        transform.position = newPosition;
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
        if(enabled)
        {
            this.transform.position += this.transform.forward * 0.9f;
            enabled = false;
            Invoke("EnabledAgain",0.1f);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void RotateObjectLeftServerRpc()
    {
        if(enabled)
        {
            if (zRotation == 360)
            {
                zRotation -= 360;
            }
            zRotation += 90;
            SetRotation();
            enabled = false;
            Invoke("EnabledAgain",0.3f);
        }
            
    }

    [ServerRpc(RequireOwnership = false)]
    private void RotateObjectRightServerRpc()
    {
        if(enabled)
        {
            if (zRotation == 0)
            {
                zRotation += 360;
            }
            zRotation -= 90;
            SetRotation();
            enabled = false;
            Invoke("EnabledAgain",0.3f);
        }
    }

    private void EnabledAgain()
    {
        enabled = true;
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