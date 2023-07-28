using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class TestLobby : MonoBehaviour
{
    public const string KEY_START_GAME = "Start";
    public static TestLobby Instance { get; private set; }
    private Lobby hostLobby;
    private Lobby joinedLobby;
    private float heartbeatTimer;
    private float lobbyUpdateTimer;
    private string playerName = "Shorty Dodom";

    public event EventHandler<LobbyEventArgs> OnJoinedLobby;
    public event EventHandler<LobbyEventArgs> OnJoinedLobbyUpdate;
    [SerializeField] TMP_InputField codeInput;
    [SerializeField] TMP_Text usernameUI;
    [SerializeField] TMP_InputField username;
    [SerializeField] TextMeshProUGUI codeText;
    [SerializeField] GameObject LobbyUI;
    [SerializeField] GameObject MainLobbyUI;
    [SerializeField] GameObject StartButton;
    [SerializeField] GameObject GameMap;
   


    public class LobbyEventArgs : EventArgs
    {
        public Lobby lobby;
    }


    private void Awake()
    {
   
            Instance = this;

    }
    public Lobby GetJoinedLobby()
    {
        return joinedLobby;
    }


    private void Update()
    {
        HandleLobbyHeartbeat();
        HandleLobbyPollForUpdate();
        if (IsLobbyHost())
        {
            StartButton.SetActive(true);
        }
    }
    private async void HandleLobbyHeartbeat()
    {
        if (hostLobby != null)
        {
            heartbeatTimer -= Time.deltaTime;
            if (heartbeatTimer < 0f)
            {
                float heartbeatTimerMax = 30;
                heartbeatTimer = heartbeatTimerMax;

               await LobbyService.Instance.SendHeartbeatPingAsync(hostLobby.Id);
            }
        }
    }

    private async void HandleLobbyPollForUpdate()
    {
        if (joinedLobby != null)
        {
            lobbyUpdateTimer -= Time.deltaTime;
            if (lobbyUpdateTimer < 0f)
            {
                float lobbyUpdateTimerMax = 1.4f;
                lobbyUpdateTimer = lobbyUpdateTimerMax;

                joinedLobby = await LobbyService.Instance.GetLobbyAsync(joinedLobby.Id);

                OnJoinedLobbyUpdate?.Invoke(this, new LobbyEventArgs { lobby = joinedLobby });
                if (joinedLobby.Data[KEY_START_GAME].Value != "0")
                {
                    if (!IsLobbyHost())
                        TestRelay.Instance.JoinRelay(joinedLobby.Data[KEY_START_GAME].Value);
                        GamaManager.Instance.StartGame();
                    joinedLobby = null;
                }
            }
        }

    }

    public bool IsLobbyHost()
    {
        return joinedLobby != null && joinedLobby.HostId == AuthenticationService.Instance.PlayerId;
    }

    public async void CreateLobby()
    {
        try {
            Debug.Log("STARTED");
            string lobbyname = "MyNewLobby";
            int maxPlayers = 4;
            CreateLobbyOptions createLobbyOptions = new CreateLobbyOptions
            {
                Player =  GetPlayer(),
                IsPrivate = false,
                Data = new Dictionary<string, DataObject>
                {
                    { KEY_START_GAME, new DataObject(DataObject.VisibilityOptions.Member, "0") }
                }

            };
            Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyname, maxPlayers, createLobbyOptions);

            hostLobby = lobby;
            joinedLobby = hostLobby;
            Debug.Log("Created Lobby! " + lobby.Name + " " + lobby.MaxPlayers + " " + lobby.Id + " " + lobby.LobbyCode);
            PrintPlayers(hostLobby);
            OnJoinedLobby?.Invoke(this, new LobbyEventArgs { lobby = lobby });
            MainLobbyUI.SetActive(true);
            FindObjectOfType<LobbyUIEvents>().ToggleLobby();
            codeText.SetText(lobby.LobbyCode);
            LobbyUI.SetActive(false);
        } catch (LobbyServiceException e){
            Debug.Log(e);
        }
        
    }

    private async void ListLobbies()
    {
        try{
            
            QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync();
            Debug.Log("Lobbies found: " + queryResponse.Results);
            foreach (Lobby lobby in queryResponse.Results)
            {
                Debug.Log(lobby.Name + " " + lobby.MaxPlayers);
            }
        } 
        catch (LobbyServiceException e){
            Debug.Log(e);
        }
        
     }
    

    public void JoinLobbyUI()
    {
        JoinLobbyByCode(codeInput.text);
        LobbyUI.SetActive(false);
        MainLobbyUI.SetActive(true);
        FindObjectOfType<LobbyUIEvents>().ToggleLobby();

    }
    private async void JoinLobbyByCode(string lobbyCode)
    {
        try
        {

            JoinLobbyByCodeOptions joinLobbyByCodeOptions = new JoinLobbyByCodeOptions
            {
                Player = GetPlayer()
            };
            Lobby lobby = await Lobbies.Instance.JoinLobbyByCodeAsync(lobbyCode, joinLobbyByCodeOptions);
            joinedLobby = lobby;
            Debug.Log("Joined Lobby with code " + lobbyCode);
            PrintPlayers(lobby); 

            OnJoinedLobby?.Invoke(this, new LobbyEventArgs {lobby = lobby});
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
      
    }

    private async void QuickJoinLobby()
    {
        try
        {
            await  LobbyService.Instance.QuickJoinLobbyAsync();
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    private Player GetPlayer()
    {
        return new Player
        {
            Data = new Dictionary<string, PlayerDataObject>{
                        {"PlayerName", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, playerName)}
                    }
        };
    }
    private void PrintPlayers(Lobby lobby)
    {
        Debug.Log("Players in Lobby " + lobby.Name);
        foreach(Player player in lobby.Players)
        {
            Debug.Log(player.Id + " " + player.Data["PlayerName"].Value);
        }
    }

   


    private async void UpdatePlayerName(string newPlayerName)
    {
        try
        {
            playerName = newPlayerName;
            await LobbyService.Instance.UpdatePlayerAsync(joinedLobby.Id, AuthenticationService.Instance.PlayerId, new UpdatePlayerOptions
            {
                Data = new Dictionary<string, PlayerDataObject>{
                        {"PlayerName", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, playerName)}
                    }
            });
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    public async void LeaveLobby()
    {
        try
        {
            await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id, AuthenticationService.Instance.PlayerId);
            FindObjectOfType<LobbyUIEvents>().ToggleLobby();
            MainLobbyUI.SetActive(false);
            LobbyUI.SetActive(true);
            joinedLobby = null;
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    private async void KickPlayer()
    {
        try
        {
            await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id, joinedLobby.Players[1].Id);

        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    private async void MigrateLobbyHost()
    {
        try
        {
            hostLobby = await Lobbies.Instance.UpdateLobbyAsync(hostLobby.Id, new UpdateLobbyOptions
            {
                HostId = joinedLobby.Players[1].Id
            });
        }catch(LobbyServiceException e)
        {
            Debug.LogError(e);
        }
    }

    private async void DeleteLobby()
    {
        try
        {

       await LobbyService.Instance.DeleteLobbyAsync(joinedLobby.Id);
        } catch(LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    public async void SetPlayerName()
    {
        playerName = username.text;
        usernameUI.text = playerName;
        InitializationOptions initializationOptions = new InitializationOptions();
        initializationOptions.SetProfile(playerName);
        await UnityServices.InitializeAsync(initializationOptions);

        AuthenticationService.Instance.SignedIn += () => {
            Debug.Log("Signed in " + AuthenticationService.Instance.PlayerId);
            
        };

        Debug.Log("Set Name as " + playerName);

        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    public async void StartGame()
    {
        if (IsLobbyHost())
        {
            try
            {
                Debug.Log("Game Started");

                string relayCode = await TestRelay.Instance.CreateRelay();

                Lobby lobby = await Lobbies.Instance.UpdateLobbyAsync(joinedLobby.Id, new UpdateLobbyOptions
                {
                    Data = new Dictionary<string, DataObject>
                    {
                        { KEY_START_GAME, new DataObject(DataObject.VisibilityOptions.Member, relayCode) }
                    }
                });
                GamaManager.Instance.StartGame();
                MainLobbyUI.SetActive(false);
                GameMap.SetActive(true);

            }

            catch (LobbyServiceException e)
            {
                Debug.Log(e);
            }
        }
    }
}
