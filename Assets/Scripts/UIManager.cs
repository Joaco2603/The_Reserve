using DilmerGames.Core.Singletons;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Net;
using System.Net.Sockets;


public class UIManager : NetworkSingleton<UIManager>
{
    // [SerializeField]
    // private Button startServerButton;

    [SerializeField]
    private Button startHostButton;

    [SerializeField]
    private Button startClientButton;

    [SerializeField]
    private Button Instructions;

    [SerializeField]
    private Image MainImage;

    [SerializeField]
    private List<Button> buttonHiden;

    [SerializeField]
    private Button startGame;

    [SerializeField]
    private TextMeshProUGUI playersInGameText;

    [SerializeField]
    private TMP_InputField joinCodeInput;

    [SerializeField]
    private Button ButtonMore;

    [SerializeField]
    private Button ButtonExit;

    [SerializeField]
    private TextMeshProUGUI StringWinCodition;

    [SerializeField]
    private Image restarMenu;

    [SerializeField]
    private GameObject IntructionsMenu;

    [SerializeField]
    private Button exitInstructions;

    private string timeliType;

    // Puerto que quieres verificar
    private const int port = 7777;



    // [SerializeField]
    // private Button executePhysicsButton;


    private void Awake() 
    {
        Cursor.visible = true;
    }

    void Update()
    {
        playersInGameText.text = $"Players in game: {PlayersManager.Instance.PlayersInGame}";
    }

    void Start()
    {

        
    // startGame.onClick.AddListener(async () => 
    // {
    //     EnabledMainMenu();

    //     IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
    //     IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, port);


    //     using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
    //     {
    //         try
    //         {
    //             socket.Connect(ipEndPoint);
    //             Debug.Log("Puerto " + port + " está en uso.");
    //             bool clientStarted = NetworkManager.Singleton.StartClient();
    //             if (clientStarted)
    //             {
    //                 Logger.Instance.LogInfo("Client started...");
    //             }
    //             else
    //             {
    //                 Logger.Instance.LogInfo("Unable to start client...");
    //             }
    //         }
    //         catch (SocketException)
    //         {
    //             Debug.Log("Puerto " + port + " está disponible.");
    //             if (RelayManager.Instance.IsRelayEnabled) 
    //             {
    //                 await RelayManager.Instance.SetupRelay();
    //             }
        
    //             // Intenta iniciar el host
    //             bool hostStarted = NetworkManager.Singleton.StartHost();
    //             if (!hostStarted)
    //             {
    //                 throw new Exception("Unable to start host");
    //             }
        
    //             Logger.Instance.LogInfo("Host started...");
    //         }
    //     }
    // });


        
        




        // // START SERVER
        // startServerButton?.onClick.AddListener(() =>
        // {
        //     if (NetworkManager.Singleton.StartServer())
        //         Logger.Instance.LogInfo("Server started...");
        //     else
        //         Logger.Instance.LogInfo("Unable to start server...");
        // });

        // // START HOST
        startHostButton?.onClick.AddListener(async () =>
        {
            EnabledMainMenu();
            // this allows the UnityMultiplayer and UnityMultiplayerRelay scene to work with and without
            // relay features - if the Unity transport is found and is relay protocol then we redirect all the 
            // traffic through the relay, else it just uses a LAN type (UNET) communication.
            if (RelayManager.Instance.IsRelayEnabled) 
                await RelayManager.Instance.SetupRelay();

            if (NetworkManager.Singleton.StartHost())
                Logger.Instance.LogInfo("Host started...");
            else
                Logger.Instance.LogInfo("Unable to start host...");
        });

        // // START CLIENT
        startClientButton?.onClick.AddListener(async () =>
        {
            EnabledMainMenu();
            if (RelayManager.Instance.IsRelayEnabled && !string.IsNullOrEmpty(joinCodeInput.text))
                await RelayManager.Instance.JoinRelay(joinCodeInput.text);

            if(NetworkManager.Singleton.StartClient())
                Logger.Instance.LogInfo("Client started...");
            else
                Logger.Instance.LogInfo("Unable to start client...");
        });


        Instructions?.onClick.AddListener(async () =>
        {
            EnabledButtons(false);
            exitInstructions.gameObject.SetActive(true);
            IntructionsMenu.SetActive(true);
        });

        exitInstructions?.onClick.AddListener(async () =>
        {
            EnabledButtons(true);
            IntructionsMenu.SetActive(false);
            exitInstructions.gameObject.SetActive(false);
        });


        // STATUS TYPE CALLBACKS
        NetworkManager.Singleton.OnClientConnectedCallback += (id) =>
        {
            Logger.Instance.LogInfo($"{id} just connected...");
        };
    }

    private void EnabledButtons(bool type)
    {
        foreach(var item in buttonHiden)
        {
            item.gameObject.SetActive(type);
        }
    }


    private void EnabledMainMenu()
    {
        MainImage.enabled = false;
        foreach(var item in buttonHiden)
        {
            item.gameObject.SetActive(false);
        }
    }

}
