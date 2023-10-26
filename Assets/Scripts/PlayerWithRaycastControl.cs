using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(NetworkTransform))]
[RequireComponent(typeof(NetworkObject))]
public class PlayerWithRaycastControl : NetworkBehaviour
{
    [SerializeField]
    private float walkSpeed = 3.5f;

    [SerializeField]
    private float runSpeedOffset = 2.0f;

    [SerializeField]
    private float rotationSpeed = 3.5f;

    [SerializeField]
    private Vector2 defaultInitialPositionOnPlane = new Vector2(-4, 4);

    [SerializeField]
    private NetworkVariable<Vector3> networkPositionDirection = new NetworkVariable<Vector3>();

    [SerializeField]
    private NetworkVariable<Vector3> networkRotationDirection = new NetworkVariable<Vector3>();

    [SerializeField]
    private NetworkVariable<PlayerState> networkPlayerState = new NetworkVariable<PlayerState>();


    [SerializeField]
    private NetworkVariable<float> networkPlayerHealth = new NetworkVariable<float>(1000);

    [SerializeField]
    private NetworkVariable<float> networkPlayerPunchBlend = new NetworkVariable<float>();

    [SerializeField]
    private GameObject leftHand;

    [SerializeField]
    private GameObject rightHand;

    [SerializeField]
    private float minPunchDistance = 30.0f;

    private float targetLiveTree;

    private CharacterController characterController;

    public bool GameMode = false;

    // client caches positions
    private Vector3 oldInputPosition = Vector3.zero;
    private Vector3 oldInputRotation = Vector3.zero;
    private PlayerState oldPlayerState = PlayerState.Idle;

    private Animator animator;

    private LiveTree treeLiveComponent;
    private Vector3 treePosition;
    private Quaternion treeRotation;

    private PlayerInput _playerInput;

    public ulong PlayerHostId;
    public NetworkObject PlayerHost;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        _playerInput = GetComponent<PlayerInput>();
    }

    void Start()
    {
        if (IsClient && IsOwner)
        {

            transform.position = new Vector3(Random.Range(defaultInitialPositionOnPlane.x, defaultInitialPositionOnPlane.y), 0,
                   Random.Range(defaultInitialPositionOnPlane.x, defaultInitialPositionOnPlane.y));

            PlayerCameraFollow.Instance.FollowPlayer(transform.Find("PlayerCameraRoot"));
        }
        if (NetworkManager.Singleton.IsServer)
        {
            // Código del servidor
            Debug.Log("Es el servidor");
            PlayerHostId = GetComponent<NetworkObject>().OwnerClientId;
            PlayerHost = GetComponent<NetworkObject>();
        }
    }

    void Update()
    {
        if (IsClient && IsOwner)
        {
            ClientInput();
        }

        ClientMoveAndRotate();
        ClientVisuals();
    }

    private void FixedUpdate()
    {
        if (IsClient && IsOwner)
        {
            if (networkPlayerState.Value == PlayerState.Punch && ActivePunchActionKey())
            {
                // CheckPunch(leftHand.transform, Vector3.up);
                CheckPunch(rightHand.transform, Vector3.up);
            }
        }
    }


    private void CheckPunch(Transform hand, Vector3 aimDirection)
    {
        RaycastHit hit;

        int layerMask = LayerMask.GetMask("Tree");

        if (Physics.Raycast(hand.position, hand.transform.TransformDirection(aimDirection), out hit, minPunchDistance, layerMask))
        {
            Debug.DrawRay(hand.position, hand.transform.TransformDirection(aimDirection) * minPunchDistance, Color.yellow);

            var TreeHit = hit.transform.GetComponent<NetworkObject>();
            if (TreeHit != null)
            { 
                ulong clientId = GetComponent<NetworkObject>().OwnerClientId; // Obtener el clientId del jugador que está golpeando
                UpdateHealthServerRpc(10, TreeHit.NetworkObjectId, clientId);
            }
        }
        else
        {
            Debug.DrawRay(hand.position, hand.transform.TransformDirection(aimDirection) * minPunchDistance, Color.red);
        }
    }


    private void ClientMoveAndRotate()
    {
        if (networkPositionDirection.Value != Vector3.zero)
        {
            characterController.SimpleMove(networkPositionDirection.Value);
        }
        if (networkRotationDirection.Value != Vector3.zero)
        {
            transform.Rotate(networkRotationDirection.Value, Space.World);
        }
    }

    private void ClientVisuals()
    {
        if (oldPlayerState != networkPlayerState.Value)
        {
            oldPlayerState = networkPlayerState.Value;
            animator.SetTrigger($"{networkPlayerState.Value}");
            if (networkPlayerState.Value == PlayerState.Punch)
            {
                animator.SetFloat($"{networkPlayerState.Value}Blend", networkPlayerPunchBlend.Value);
            }
        }
    }

    private void ClientInput()
    {

        if(GameMode) return;

        // left & right rotation
        // Suponiendo que tienes una acción "Horizontal" configurada en tu Input Action Map.
        Vector2 rotationInput = _playerInput.actions["Move"].ReadValue<Vector2>();
        Vector3 inputRotation = new Vector3(0, rotationInput.x, 0); // Usamos solo la entrada en el eje X para la rotación en Y

        // forward & backward direction
        Vector3 direction = transform.TransformDirection(Vector3.forward);
        // Suponiendo que tienes una acción "Vertical" configurada en tu Input Action Map.
        float forwardInput = _playerInput.actions["Move"].ReadValue<Vector2>().y;
        Vector3 inputPosition = direction * forwardInput;

        // change fighting states
        if (ActivePunchActionKey() && forwardInput == 0)
        {
            UpdatePlayerStateServerRpc(PlayerState.Punch);
            return;
        }
    
        //_playerInput.actions["Seed"].ReadValue<float>() > 0
        if(Input.GetKeyDown(KeyCode.Q))
        {
            UpdatePlayerStateServerRpc(PlayerState.Water);
        }

        // change motion states
        if (forwardInput == 0)
            UpdatePlayerStateServerRpc(PlayerState.Idle);
        else if (!ActiveRunningActionKey() && forwardInput > 0 && forwardInput <= 1)
            UpdatePlayerStateServerRpc(PlayerState.Walk);
        else if (ActiveRunningActionKey() && forwardInput > 0 && forwardInput <= 1)
        {
            inputPosition = direction * runSpeedOffset;
            UpdatePlayerStateServerRpc(PlayerState.Run);
        }
        else if (forwardInput < 0)
        {
            UpdatePlayerStateServerRpc(PlayerState.ReverseWalk);
        }

        // let server know about position and rotation client changes
        if (oldInputPosition != inputPosition ||
            oldInputRotation != inputRotation)
        {
            oldInputPosition = inputPosition;
            oldInputRotation = inputRotation;
            UpdateClientPositionAndRotationServerRpc(inputPosition * walkSpeed, inputRotation * rotationSpeed);
        }
    }

    private bool ActiveRunningActionKey()
    {
        bool status;
        if(_playerInput.actions["Run"].ReadValue<float>() > 0)
        {
            status = true;
        }
        else
        {
            status = false;
        }

        return status;
    }

    private bool ActivePunchActionKey()
    {
        bool status;
        if(_playerInput.actions["Hit"].ReadValue<float>() > 0)
        {
            status = true;
        }
        else
        {
            status = false;
        }

        return status;
    }

    [ServerRpc]
    public void UpdateClientPositionAndRotationServerRpc(Vector3 newPosition, Vector3 newRotation)
    {
        networkPositionDirection.Value = newPosition;
        networkRotationDirection.Value = newRotation;
    }

    [ServerRpc]
    public void UpdateHealthServerRpc(int takeAwayPoint, ulong objectId, ulong clientId)
    {

        NetworkObject targetObject;
        if(NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(objectId, out targetObject))
        { 
            treePosition = targetObject.transform.position;   // Aquí obtenemos la posición
            treeRotation = targetObject.transform.rotation; // Aquí obtenemos la rotación

            
            targetLiveTree = targetObject.GetComponent<LiveTree>().networkPlayerHealth.Value;

            treeLiveComponent = targetObject.GetComponent<LiveTree>();

            if (targetObject != null && targetLiveTree > 0)
            {
                targetLiveTree -= takeAwayPoint;
                targetObject.GetComponent<LiveTree>().networkPlayerHealth.Value = targetLiveTree;
            }

            // execute method on a client getting punch
            NotifyHealthChangedClientRpc(takeAwayPoint,targetLiveTree, new ClientRpcParams
            {
                Send = new ClientRpcSendParams
                {
                    TargetClientIds = new ulong[] { clientId }
                }
            });
        }
    }

    [ClientRpc]
    public void NotifyHealthChangedClientRpc(int takeAwayPoint,float liveTree, ClientRpcParams clientRpcParams = default)
    {
        if (!IsOwner && !IsClient) return;

        Logger.Instance.LogInfo($"Client got punch {takeAwayPoint}");
        Logger.Instance.LogInfo($"Live of tree {liveTree}");
        if(liveTree<1f)
        {   
            ChangeTreeServerRpc();
        }
    }

    [ServerRpc]
    private void ChangeTreeServerRpc()
    {
        treeLiveComponent.CheckTreeHealth(treePosition,treeRotation);
    }
    
    [ServerRpc]
    public void UpdatePlayerStateServerRpc(PlayerState state)
    {
        Debug.Log(state);
        networkPlayerState.Value = state;
        if (state == PlayerState.Punch)
        {
            networkPlayerPunchBlend.Value = Random.Range(0.0f, 2.0f);
        }
    }
}