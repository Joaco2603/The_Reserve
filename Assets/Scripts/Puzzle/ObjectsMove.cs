using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class ObjectsMove : NetworkBehaviour
{
    public bool mode = false;
    private int speed = 2; 

    private float zRotation = 0; // Almacena la rotación acumulada en el eje Z
    Quaternion originalRotation;


    //Player
    [SerializeField]
    private int vectorXPlayer;
    [SerializeField]
    private int vectorYPlayer;
    //Glass180
    [SerializeField]
    private int vectorX180;
    [SerializeField]
    private int vectorY180;
    //Glass0
    [SerializeField]
    private int vectorX0;
    [SerializeField]
    private int vectorY0;

    Floor floorObject;

    
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
        floorObject = GameObject.Find("Plataforma").GetComponent<Floor>();
    }

    // Update is called once per frame
    void Update()
    {
        if(mode && IsClient && IsOwner)
        {
            MoveObjects();
        }
    }


    //private void OnCollisionEnter(Collision other) 
    //{
        //if(other.gameObject.GetComponent<Floor>())
        //{
		
        //}
    //}



    private void MoveObjects()
    {
        if (Input.GetKey(KeyCode.W))
        {
              switch(objectType)
        		{
            case ObjectType.Player:
                // Player movement
                if(zRotation == 0){
				vectorYPlayer -= 1;
					if(vectorYPlayer == floorObject.vectorY){
					    transform.position = Vector3.Lerp(new Vector3(vectorXPlayer,vectorYPlayer+1,0), new Vector3(vectorXPlayer,vectorYPlayer,0), 3f);
					}else
                    {
					    vectorYPlayer +=1;
					}
				}
				if(zRotation == 90){
				    vectorXPlayer += 1;
					if(vectorXPlayer == floorObject.vectorX){
					    transform.position = Vector3.Lerp(new Vector3(vectorXPlayer-1,vectorYPlayer,0), new Vector3(vectorXPlayer,vectorYPlayer,0), 3f);
					}else
                    {
					    vectorYPlayer -=1;
					} 
				}
				if(zRotation == 180){
				    vectorYPlayer += 1;
					if(vectorYPlayer == floorObject.vectorY){
					    transform.position = Vector3.Lerp(new Vector3(vectorXPlayer,vectorYPlayer-1,0), new Vector3(vectorXPlayer,vectorYPlayer,0), 3f);
					}else
                    {
					    vectorYPlayer-=1;
					}
				if(zRotation == 270){
				    vectorXPlayer -= 1;
					if(vectorXPlayer == floorObject.vectorX){
				        transform.position = Vector3.Lerp(new Vector3(vectorXPlayer,vectorYPlayer,0), new Vector3(vectorXPlayer,vectorYPlayer,0), 3f);
					}else{
					    vectorXPlayer += 1;
					} 
				}
			}
                break;
            case ObjectType.Glass180:
                // Object1 movement
                	if(zRotation == 0){
				    vectorYPlayer -= 1;
					if(vectorYPlayer == floorObject.vectorY){
					    transform.position = Vector3.Lerp(new Vector3(vectorXPlayer,vectorYPlayer+1,0), new Vector3(vectorXPlayer,vectorYPlayer,0), 3f);
					}else{
					    vectorYPlayer +=1;
					}
				}
				if(zRotation == 90){
				    vectorXPlayer += 1;
					if(vectorXPlayer == floorObject.vectorX){
					    transform.position = Vector3.Lerp(new Vector3(vectorXPlayer-1,vectorYPlayer,0), new Vector3(vectorXPlayer,vectorYPlayer,0), 3f);
					}else{
					    vectorYPlayer -=1;
					} 
				}
				if(zRotation == 180){
				    vectorYPlayer += 1;
					if(vectorYPlayer == floorObject.vectorY){
					    transform.position = Vector3.Lerp(new Vector3(vectorXPlayer,vectorYPlayer-1,0), new Vector3(vectorXPlayer,vectorYPlayer,0), 3f);
					}else
                    {
					    vectorYPlayer-=1;
					}
				if(zRotation == 270){
				    vectorXPlayer -= 1;
					if(vectorXPlayer == floorObject.vectorX){
				        transform.position = Vector3.Lerp(new Vector3(vectorXPlayer,vectorYPlayer,0), new Vector3(vectorXPlayer,vectorYPlayer,0), 3f);
					}else{
					    vectorXPlayer += 1;
					} 
				}
			}
                break;
            case ObjectType.Glass0:
                // Object2 movement
                if(zRotation == 0){
				    vectorYPlayer += 1;
				if(vectorYPlayer == floorObject.vectorY){
				    transform.position = Vector3.Lerp(new Vector3(vectorXPlayer,vectorYPlayer-1,0), new Vector3(vectorXPlayer,vectorYPlayer,0), 3f);
				}else
                {
				    vectorYPlayer -= 1;
				}
				}
				if(zRotation == 90){
				    vectorXPlayer -= 1;
				if(vectorXPlayer == floorObject.vectorX){
				    transform.position = Vector3.Lerp(new Vector3(vectorXPlayer-1,vectorYPlayer,0), new Vector3(vectorXPlayer,vectorYPlayer,0), 3f); 
				}else
                {
				    vectorXPlayer += 1;
				}
				}
				if(zRotation == 180){
				    vectorYPlayer -= 1;
				if(vectorYPlayer == floorObject.vectorY){
				    transform.position = Vector3.Lerp(new Vector3(vectorXPlayer,vectorYPlayer+1,0), new Vector3(vectorXPlayer,vectorYPlayer,0), 3f);
				}else{
				    vectorYPlayer += 1;
				}
				}
				if(zRotation == 270){
				    vectorXPlayer += 1;
				if(vectorXPlayer == floorObject.vectorX){
				    transform.position = Vector3.Lerp(new Vector3(vectorXPlayer-1,vectorYPlayer,0), new Vector3(vectorXPlayer,vectorYPlayer,0), 3f); 
				}else
                {
				    vectorXPlayer -=1;
				}
				}
                break;
        	}
        }
        
        if(Input.GetKey(KeyCode.S))
        {
            switch(objectType)
        	{
            case ObjectType.Player:
                // Player movement
                if(zRotation == 0){
				    vectorYPlayer += 1;
				if(vectorYPlayer == floorObject.vectorY){
				    transform.position = Vector3.Lerp(new Vector3(vectorXPlayer,vectorYPlayer-1,0), new Vector3(vectorXPlayer,vectorYPlayer,0), 3f);
				}else
                {
				    vectorYPlayer -= 1;
				}
				}
				if(zRotation == 90){
				    vectorXPlayer -= 1;
				if(vectorXPlayer == floorObject.vectorX){
				    transform.position = Vector3.Lerp(new Vector3(vectorXPlayer-1,vectorYPlayer,0), new Vector3(vectorXPlayer,vectorYPlayer,0), 3f); 
				}else
                {
				    vectorXPlayer += 1;
				}
				}
				if(zRotation == 180){
				    vectorYPlayer -= 1;
				if(vectorYPlayer == floorObject.vectorY){
				    transform.position = Vector3.Lerp(new Vector3(vectorXPlayer,vectorYPlayer+1,0), new Vector3(vectorXPlayer,vectorYPlayer,0), 3f);
				}else{
				    vectorYPlayer += 1;
				}
				}
				if(zRotation == 270){
				    vectorXPlayer += 1;
				if(vectorXPlayer == floorObject.vectorX){
				    transform.position = Vector3.Lerp(new Vector3(vectorXPlayer-1,vectorYPlayer,0), new Vector3(vectorXPlayer,vectorYPlayer,0), 3f); 
				}else{
				    vectorXPlayer -=1;
				    }
				}
                break;
            case ObjectType.Glass180:
                // Object1 movement
                if(zRotation == 0){
				    vectorYPlayer += 1;
				if(vectorYPlayer == floorObject.vectorY)
                {
				    transform.position = Vector3.Lerp(new Vector3(vectorXPlayer,vectorYPlayer-1,0), new Vector3(vectorXPlayer,vectorYPlayer,0), 3f);
				}else
                {
				    vectorYPlayer -= 1;
                }
			    }
				if(zRotation == 90){
				    vectorXPlayer -= 1;
				if(vectorXPlayer == floorObject.vectorX){
				    transform.position = Vector3.Lerp(new Vector3(vectorXPlayer-1,vectorYPlayer,0), new Vector3(vectorXPlayer,vectorYPlayer,0), 3f); 
				}else
                {
				    vectorXPlayer += 1;
				}
				}
				if(zRotation == 180)
                {
				    vectorYPlayer -= 1;
				if(vectorYPlayer == floorObject.vectorY)
                {
				    transform.position = Vector3.Lerp(new Vector3(vectorXPlayer,vectorYPlayer+1,0), new Vector3(vectorXPlayer,vectorYPlayer,0), 3f);
				}else
                {
				    vectorYPlayer += 1;
				}
				}
				if(zRotation == 270)
                {
				vectorXPlayer += 1;
				if(vectorXPlayer == floorObject.vectorX)
                {
				    transform.position = Vector3.Lerp(new Vector3(vectorXPlayer-1,vectorYPlayer,0), new Vector3(vectorXPlayer,vectorYPlayer,0), 3f); 
				}else
                {
				    vectorXPlayer -=1;
				}
				}
                break;
            case ObjectType.Glass0:
                // Object2 movement
                if(zRotation == 0)
                {
				    vectorYPlayer -= 1;
					if(vectorYPlayer == floorObject.vectorY)
                    {
					    transform.position = Vector3.Lerp(new Vector3(vectorXPlayer,vectorYPlayer+1,0), new Vector3(vectorXPlayer,vectorYPlayer,0), 3f);
					}else
                    {
					    vectorYPlayer +=1;
					}
				}
				if(zRotation == 90)
                {
				    vectorXPlayer += 1;
					if(vectorXPlayer == floorObject.vectorX)
                    {
					    transform.position = Vector3.Lerp(new Vector3(vectorXPlayer-1,vectorYPlayer,0), new Vector3(vectorXPlayer,vectorYPlayer,0), 3f);
					}else
                    {
					    vectorYPlayer -=1;
					} 
				}
				if(zRotation == 180)
                {
				    vectorYPlayer += 1;
					if(vectorYPlayer == floorObject.vectorY)
                    {
					    transform.position = Vector3.Lerp(new Vector3(vectorXPlayer,vectorYPlayer-1,0), new Vector3(vectorXPlayer,vectorYPlayer,0), 3f);
					}else
                    {
					    vectorYPlayer-=1;
					}
                }
				if(zRotation == 270)
                {
				    vectorXPlayer -= 1;
					if(vectorXPlayer == floorObject.vectorX)
                    {
				        transform.position = Vector3.Lerp(new Vector3(vectorXPlayer,vectorYPlayer,0), new Vector3(vectorXPlayer,vectorYPlayer,0), 3f);
					}else
                    {
					    vectorXPlayer += 1;
					} 
				}
                break;
            }
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
