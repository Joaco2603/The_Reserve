using System.Collections;
// using System.Collections.Generic;
// using Unity.Services.Authentication;
// using Unity.Services.Core;
// using Unity.Services.Lobbies;
// using Unity.Services.Lobbies.Models;
using UnityEngine;

public class Lobbys : MonoBehaviour
{


//     private Lobby hostLobby;
//     private Lobby joinedLobby;
//     private float heartbeatTimer;
//     private float lobbyUpdateTimer;
//     private string playerName;
//     private string gameMode;
//     private int minValue = 0;
//     private int maxValue = 101;
//     Dictionary<string, PlayerDataObject> Data;

//     private async void Awake()
//     {
//         try
//         {
//         await UnityServices.InitializeAsync();

//         AuthenticationService.Instance.SignedIn += ()=>{
//             Debug.Log("Signed in " + AuthenticationService.Instance.PlayerId);
//         };

//         await AuthenticationService.Instance.SignInAnonymouslyAsync();

//         playerName = "Player" + UnityEngine.Random.Range(minValue, maxValue);
//         Debug.Log(playerName);
//         }catch(LobbyServiceException e)
//         {
//             Debug.Log(e);
//         }
//     }

//     private void Update()
//     {
//         HandleLobbyHeartbeat();
//         HandleLobbyPollForUpdates();

//         // if(Input.GetKeyDown(KeyCode.C))
//         // {
//         //     CreateLobby();
//         // }
//         // if(Input.GetKeyDown(KeyCode.V))
//         // {
//         //     ListLobbies();
//         // }
//     }



//     private async void HandleLobbyHeartbeat()
//     {
//         if(hostLobby!=null){
//             heartbeatTimer -= Time.deltaTime;
//             if(heartbeatTimer<0f){
//                 float heartbeatTimerMax = 15;
//                 heartbeatTimer = heartbeatTimerMax;

//                 await LobbyService.Instance.SendHeartbeatPingAsync(hostLobby.Id);
//             }
//         }
//     }




//     private async void CreateLobby()
//     {
//         try
//         {
//         string lobbyName = "MyLobby";
//         int maxPlayers = 4;
//         CreateLobbyOptions createLobbyOptions = new CreateLobbyOptions{
//             IsPrivate = true,
//             Player = GetPlayer(),
//                     // Id = AuthenticationService.Instance.PlayerId,
//                     Data = new Dictionary<string,DataObject>{
//                     {"GameMode", new DataObject(DataObject.VisibilityOptions.Public,"Normal")},
//                     {"Map", new DataObject(DataObject.VisibilityOptions.Public,"Mapa_1")}
//                     }
//         };
//         Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName,maxPlayers,createLobbyOptions);

//         hostLobby = lobby;
//         joinedLobby = hostLobby;
//         Debug.Log("Aqui");
//         Debug.Log("Created Lobby "+ lobby.Name + " " + lobby.MaxPlayers +" "+ lobby.Id + " "+ lobby.LobbyCode);
//         PrintPlayers(hostLobby);
//         }catch(LobbyServiceException e)
//         {
//             Debug.Log(e);
//         }
//     }

//     private async void ListLobbies()
//     {

//         try{

//             QueryLobbiesOptions queryLobbiesOptions = new QueryLobbiesOptions
//             {
//                 Count = 25,
//                 Filters = new List<QueryFilter>
//                 {
//                     new QueryFilter(QueryFilter.FieldOptions.AvailableSlots, "0", QueryFilter.OpOptions.GT),
//                 },
//                 Order = new List<QueryOrder>
//                 {
//                     new QueryOrder(false, QueryOrder.FieldOptions.Created)
//                 }
//             };


//             QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync();

//             Debug.Log("Lobbies found: "+queryResponse.Results.Count);
//             foreach(Lobby lobby in queryResponse.Results)
//             {
//                 Debug.Log(lobby.Name +" "+ lobby.MaxPlayers + " " + lobby.Data["GameMode"].Value);
//             }
//         }catch(LobbyServiceException e)
//         {
//             Debug.Log(e);
//         }
//     }


// //    public async void JoinLobbyById()
// //     {
// //         try
// //         {
// //             QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync();
    
// //             if (queryResponse.Results != null && queryResponse.Results.Count > 0)
// //             {
// //                 string lobbyId = queryResponse.Results[0].Id;
// //                 await Lobbies.Instance.JoinLobbyByIdAsync(lobbyId);
// //             }
// //             else
// //             {
// //                 Debug.Log("No se encontraron lobbies disponibles.");
// //             }
// //         }
// //         catch (LobbyServiceException e)
// //         {
// //             Debug.Log(e);
// //         }
// //     }

//     public async void JoinLobbyByCode(string lobbyCode)
//     {
//         try
//         {
//                 JoinLobbyByCodeOptions joinLobbyByCodeOptions = new JoinLobbyByCodeOptions {
//                     Player = GetPlayer()
//                 };

//                 Lobby joinedLobby = await Lobbies.Instance.JoinLobbyByCodeAsync(lobbyCode, joinLobbyByCodeOptions);

//                 Debug.Log("Joined Lobby with code " + lobbyCode);

//                 PrintPlayers(joinedLobby);
//         }
//         catch (LobbyServiceException e)
//         {
//             Debug.Log(e);
//         }
//     }

//     private async void QuickJoinLobby()
//     {
//         try
//         {
//             await LobbyService.Instance.QuickJoinLobbyAsync();

//         } catch(LobbyServiceException e){
//             Debug.Log(e);
//         }
//     }



//     private async void UpdateLobbyGameMode()
//     {
//         try
//         {
//         hostLobby = await Lobbies.Instance.UpdateLobbyAsync(hostLobby.Id, new UpdateLobbyOptions{
//             Data = new Dictionary<string,DataObject>{
//                 {"GameMode", new DataObject(DataObject.VisibilityOptions.Public,gameMode)}
//             }
//         });
//         }catch(LobbyServiceException e)
//         {
//             Debug.Log(e);
//         }
//     }

//     private async void HandleLobbyPollForUpdates()
//     {
//         if(joinedLobby!=null){
//             lobbyUpdateTimer -= Time.deltaTime;
//             if(lobbyUpdateTimer<0f){
//                 float heartbeatTimerMax = 1.1f;
//                 lobbyUpdateTimer = heartbeatTimerMax;

//                 Lobby lobby = await LobbyService.Instance.GetLobbyAsync(joinedLobby.Id);
//                 joinedLobby = lobby;
//             }
//         }
//     }


//     private async void UpdatePlayerName(string newPlayerName)
//     {
//         try
//         { 
//         playerName = newPlayerName;
//         await LobbyService.Instance.UpdatePlayerAsync(joinedLobby.Id,AuthenticationService.Instance.PlayerId,new UpdatePlayerOptions{
//             Data = new Dictionary<string, PlayerDataObject>{
//                 {"PlayerName",new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member,playerName)}
//             }
//         });
//         }
//         catch(LobbyServiceException e)
//         {
//             Debug.Log(e);
//         }
//     }

//     private async void LeaveLobby()
//     {
//         try
//         {
//             await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id,AuthenticationService.Instance.PlayerId);
//         }
//         catch(LobbyServiceException e)
//         {
//             Debug.Log(e);
//         }
//     }

//     private async void KickPlayer()
//     {
//         try
//         {
//             await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id,joinedLobby.Players[1].Id);
//         }
//         catch(LobbyServiceException e)
//         {
//             Debug.Log(e);
//         }
//     }


//     private async void MigrateLobbyHost()
//     {
//         try
//         {
//         hostLobby = await Lobbies.Instance.UpdateLobbyAsync(hostLobby.Id, new UpdateLobbyOptions{
//             HostId = joinedLobby.Players[1].Id
//         });
        
//         joinedLobby = hostLobby;

//         PrintPlayers(hostLobby);

//         }
//         catch(LobbyServiceException e)
//         {
//             Debug.Log(e);
//         }
//     }

//     private async void DeleteLobby()
//     {
//         try
//         {
//         await LobbyService.Instance.DeleteLobbyAsync(joinedLobby.Id);
//         }
//         catch(LobbyServiceException e)
//         {
//             Debug.Log(e);
//         }
//     }


//     private Player GetPlayer()
//     {
//         return new Player{
//             Data = new Dictionary<string,PlayerDataObject>{
//                 {"PlayerName",new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member,playerName)}
//             }
//         };
//     }

//     private void PrintPlayers(Lobby lobby)
//     {
//         Debug.Log("Players in Lobby "+lobby.Name + " " + lobby.Data["GameMode"].Value + " " + lobby.Data["Map"].Value);
//         foreach(Player player in lobby.Players)
//         {
//             Debug.Log(player.Id + " " + player.Data["PlayerName"].Value);
//         }
//     }

//     private void PrintPlayers()
//     {
//         PrintPlayers(joinedLobby);
//     }

}
