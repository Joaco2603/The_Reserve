using DilmerGames.Core.Singletons;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;


public class UIManager : Singleton<UIManager>
{
    // [SerializeField]
    // private Button startServerButton;

    // [SerializeField]
    // private Button startHostButton;

    // [SerializeField]
    // private Button startClientButton;

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

    private string timeliType;


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

        startGame?.onClick.AddListener(async () =>
        {
            if (!NetworkManager.Singleton.IsServer && NetworkManager.Singleton.StartHost())
            {
                // No hay servidor, iniciar como host
                if (RelayManager.Instance.IsRelayEnabled) 
                await RelayManager.Instance.SetupRelay();
                    Logger.Instance.LogInfo("Host started...");
                NetworkManager.Singleton.StartHost();
                EnabledMainMenu();
            }
            else
            {
                if (RelayManager.Instance.IsRelayEnabled && !string.IsNullOrEmpty(joinCodeInput.text))
                    await RelayManager.Instance.JoinRelay(joinCodeInput.text);

                    if(NetworkManager.Singleton.StartClient())
                        Logger.Instance.LogInfo("Client started...");
                    else
                        Logger.Instance.LogInfo("Unable to start client...");
                EnabledMainMenu();
            } 
        });




        // // START SERVER
        // startServerButton?.onClick.AddListener(() =>
        // {
        //     if (NetworkManager.Singleton.StartServer())
        //         Logger.Instance.LogInfo("Server started...");
        //     else
        //         Logger.Instance.LogInfo("Unable to start server...");
        // });

        // // START HOST
        // startHostButton?.onClick.AddListener(async () =>
        // {
        //     // this allows the UnityMultiplayer and UnityMultiplayerRelay scene to work with and without
        //     // relay features - if the Unity transport is found and is relay protocol then we redirect all the 
        //     // traffic through the relay, else it just uses a LAN type (UNET) communication.
        //     if (RelayManager.Instance.IsRelayEnabled) 
        //         await RelayManager.Instance.SetupRelay();

        //     if (NetworkManager.Singleton.StartHost())
        //         Logger.Instance.LogInfo("Host started...");
        //     else
        //         Logger.Instance.LogInfo("Unable to start host...");
        // });

        // // START CLIENT
        // startClientButton?.onClick.AddListener(async () =>
        // {
        //     if (RelayManager.Instance.IsRelayEnabled && !string.IsNullOrEmpty(joinCodeInput.text))
        //         await RelayManager.Instance.JoinRelay(joinCodeInput.text);

        //     if(NetworkManager.Singleton.StartClient())
        //         Logger.Instance.LogInfo("Client started...");
        //     else
        //         Logger.Instance.LogInfo("Unable to start client...");
        // });

        // STATUS TYPE CALLBACKS
        NetworkManager.Singleton.OnClientConnectedCallback += (id) =>
        {
            Logger.Instance.LogInfo($"{id} just connected...");
        };
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
