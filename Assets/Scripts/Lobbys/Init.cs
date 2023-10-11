using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using UnityEngine.SceneManagement;


public class Init : MonoBehaviour
{
    // Start is called before the first frame update
     private async void Awake()
    {
        try
        {
        await UnityServices.InitializeAsync();

        if(UnityServices.State == ServicesInitializationState.Initialized)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();

            AuthenticationService.Instance.SignedIn += OnSignedIn;

            if(AuthenticationService.Instance.IsSignedIn)
            {
                string username = PlayerPrefs.GetString(key:"Username");
                if(username == "")
                {
                    username = "Player";
                    PlayerPrefs.SetString("Username", username);
                }

                SceneManager.LoadSceneAsync("MenuInicio");
            }
        }


        }catch(System.Exception e)
        {
            Debug.Log(e);
        }
    }

    private void OnSignedIn()
    {
        // Debug.Log(message: $"Player Id: {AuthenticationService.Instance.PlayerId}");
        // Debug.Log(message: $"Token: {AuthenticationService.Instance.AccessToken}");
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
