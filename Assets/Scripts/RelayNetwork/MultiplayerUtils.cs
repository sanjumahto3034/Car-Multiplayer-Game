using System.Collections;
using System.Collections.Generic;
using IngameDebugConsole;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using Unity.Services.Lobbies.Models;
using Unity.Services.Lobbies;
using System;
using System.Net.NetworkInformation;
using System.Data.Common;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MultiplayerUtils : MonoBehaviour
{
    private string m_relay_code;
    public static MultiplayerUtils Instance;

    private string m_lobby_code
    {
        get
        {
            return m_lobby.LobbyCode;
        }
    }
    private string m_lobby_id
    {
        get
        {
            return m_lobby.Id;
        }
    }
    public static string LobbyCode { get { return Instance.m_lobby_code; } }

    public const string KEY_PLAYER_NAME = "player_name";
    public const string KEY_AUTHENTICATION_ID = "player_authentication_id";
    public const string KEY_START_GAME = "start_game";
    public const string KEY_GAME_CODE = "game_code";
    public const string KEY_WAITING_FOR_START_GAME = "0";
    public const string KEY_GAME_HAS_BEEN_STARTED = "1";
    private string m_authenticationId;
    public static string AuthenticationId { get { return Instance.m_authenticationId; } }

    private Lobby m_lobby;
    public static bool IsHost { get { return Instance.m_is_host; } }
    public static bool IsClient { get { return Instance.m_is_client; } }
    public static bool IsRunning
    {
        get
        {
            return NetworkManager.Singleton.IsListening;
        }
    }
    private bool m_is_host
    {
        get
        {
            return Instance.m_lobby.HostId == AuthenticationId;
        }
    }
    private bool m_is_client
    {

        get
        {
            return Instance.m_lobby.HostId != AuthenticationId;
        }

    }
    void Awake()
    {
        Instance = this;
    }

    async void Start()
    {
        await UnityServices.InitializeAsync();
        AuthenticationService.Instance.SignedIn += () =>
        {
            m_authenticationId = AuthenticationService.Instance.PlayerId;
            Debug.Log($"Authentication Success : {AuthenticationId}");
        };
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }
    private float remainingTimeToSendHeartbeat = 15;
    private void Update()
    {
        if (m_lobby == null)
            return;

        if (remainingTimeToSendHeartbeat <= 0)
        {
            SendHeartBeatToServer();
            remainingTimeToSendHeartbeat = 15;
        }
        remainingTimeToSendHeartbeat -= Time.deltaTime;
    }
    private void SendHeartBeatToServer()
    {
        LobbyService.Instance.SendHeartbeatPingAsync(m_lobby_id);
    }

#if UNITY_EDITOR
    [MenuItem("MultiplayerUtils/Create Lobby")]
    public static void MenuItem_CreateLobby() => Instance.Utils_CreateLobby(null, "defaultLobby", 4, false);

#endif
    public void CreateLobby(string lobbyName, int maxPlayer, bool isPrivate)
    {
        Utils_CreateLobby(null, lobbyName, maxPlayer, isPrivate);
    }
    public void CreateLobby(Action<Lobby> OnLobbyCreateSuccess, string lobbyName, int maxPlayer, bool isPrivate)
    {
        Utils_CreateLobby(OnLobbyCreateSuccess, lobbyName, maxPlayer, isPrivate);
    }
    private async void Utils_CreateLobby(Action<Lobby> OnLobbyCreateSuccess, string lobbyName, int maxPlayer, bool isPrivate)
    {

        try
        {
            CreateLobbyOptions options = new CreateLobbyOptions()
            {
                IsPrivate = isPrivate,
                Player = GetPlayer(),
            };

            Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayer, options);
            m_lobby = lobby;
            OnLobbyCreateSuccess?.Invoke(lobby);
        }
        catch (LobbyServiceException e)
        {
            Debug.LogError(e);
        }
    }



    public static void JoinLobby(Action OnJoinSuccess, string lobbyId)
    {
        Instance.Utils_JoinLobby(OnJoinSuccess, lobbyId);
    }
    private async void Utils_JoinLobby(Action OnJoinSuccess, string lobbyId)
    {
        try
        {
            JoinLobbyByIdOptions options = new JoinLobbyByIdOptions()
            {
                Player = GetPlayer(),
            };
            Lobby lobby = await LobbyService.Instance.JoinLobbyByIdAsync(lobbyId, options);
            m_lobby = lobby;
            OnJoinSuccess?.Invoke();

        }
        catch (LobbyServiceException e)
        {
            Debug.LogError(e);
            GlobalEventSystem.RemoveLoader();
            GlobalEventSystem.DispalyPopup("Failed To Join Game", e.ToString());
        }
    }







    public Player GetPlayer()
    {
        return new Player
        {
            Data = new Dictionary<string, PlayerDataObject>{
                {KEY_PLAYER_NAME,new PlayerDataObject(PlayerDataObject.VisibilityOptions.Public,PlayerPrefs.GetString(StaticString.PLAYER_NAME,"")) },
                {KEY_AUTHENTICATION_ID,new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member,AuthenticationId) }
            }
        };
    }

    public static void StartGame()
    {
        GlobalEventSystem.ShowLoader("Starting Game", "Wait While Connecting to Server");
        Instance.Util_StartGame();
    }
    private void Util_StartGame()
    {
        Debug.Log("Request To Start Game");
        try
        {
            TryToHostUDP((gameCode) =>
            {
                UpdateLobbyOptions options = new UpdateLobbyOptions()
                {
                    Data = new Dictionary<string, DataObject>
            {
                { KEY_GAME_CODE, new DataObject(DataObject.VisibilityOptions.Member, gameCode) }
            }
                };
                LobbyService.Instance.UpdateLobbyAsync(m_lobby_id, options);
                GlobalEventSystem.RemoveLoader();
            });
        }
        catch (LobbyServiceException e)
        {
            Debug.LogError(e);
            GlobalEventSystem.RemoveLoader();
            GlobalEventSystem.DispalyPopup("Failed To Join Game", e.ToString());
        }

    }

    public static void StartGameWithCode(string code)
    {
        Instance.Util_StartGameWithCode(code);
    }
    private void Util_StartGameWithCode(string code)
    {
        Debug.Log($"Try to join game with code -> {code}");
    }








    public static void LeaveLobby()
    {
        Instance.Util_LeaveLobby();
    }
    private void Util_LeaveLobby()
    {
        if (string.IsNullOrEmpty(AuthenticationId))
            return;
        try
        {
            LobbyService.Instance.RemovePlayerAsync(m_lobby_id, AuthenticationId);
        }
        catch (LobbyServiceException e)
        {
            Debug.LogError(e);
            GlobalEventSystem.RemoveLoader();
            GlobalEventSystem.DispalyPopup("Failed To Join Game", e.ToString());
        }
    }


    public static void KickPlayer(Player player)
    {
        Instance.Util_KickPlayer(player);
    }

    private void Util_KickPlayer(Player player)
    {
        LobbyService.Instance.RemovePlayerAsync(m_lobby_id, player.Id);
    }

    public static async void OnLobbyUpdate(Action<LobbyEventCallbacks> OnLobbyUpdateCallback)
    {
        try
        {
            LobbyEventCallbacks OnLobbyDataUpdate = new();
            await Lobbies.Instance.SubscribeToLobbyEventsAsync(Instance.m_lobby.Id, OnLobbyDataUpdate);
            OnLobbyUpdateCallback?.Invoke(OnLobbyDataUpdate);
        }
        catch (LobbyServiceException e)
        {
            Debug.LogError(e);
            GlobalEventSystem.RemoveLoader();
            GlobalEventSystem.DispalyPopup("Failed To Join Game", e.ToString());
        }
    }
    public static async void GetJoinLobby(Action<Lobby> OnGetSuccess)
    {
        try
        {
            Lobby lobby = await LobbyService.Instance.GetLobbyAsync(Instance.m_lobby.Id);
            OnGetSuccess?.Invoke(lobby);
        }
        catch (LobbyServiceException e)
        {
            Debug.LogError(e);
            GlobalEventSystem.RemoveLoader();
            GlobalEventSystem.DispalyPopup("Failed To Join Game", e.ToString());
        }
    }
    public static async void RequestLobbyList(Action<List<Lobby>> OnGetSuccess)
    {
        try
        {
            Debug.Log("Request To Load Lobby List");
            QueryLobbiesOptions options = new QueryLobbiesOptions()
            {
                Count = 5
            };
            QueryResponse response = await LobbyService.Instance.QueryLobbiesAsync(options);
            OnGetSuccess?.Invoke(response.Results);
            Debug.Log("Load Lobby Success");
        }

        catch (LobbyServiceException e)
        {
            Debug.LogError(e);
            GlobalEventSystem.RemoveLoader();
            GlobalEventSystem.DispalyPopup("Failed To Join Game", e.ToString());
        }
    }




    /// <summary>
    /// Create a Lobby In Relay Services
    /// </summary>
    [ConsoleMethod("HostUDP", "Create a Lobby In Relay Services")]
    public static void HostUDP()
    {
        Instance.TryToHostUDP(null);
    }



#if UNITY_EDITOR
    [MenuItem("Car Multiplayer/Create")]
    public static void MenuItem_HostUDP()
    {
        //Instance.TryToHostUDP(null);
    }

#endif
    private async void TryToHostUDP(Action<string> OnHostSuccess)
    {
        try
        {
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(2);
            string relayCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetHostRelayData(
                allocation.RelayServer.IpV4,
                (ushort)allocation.RelayServer.Port,
                allocation.AllocationIdBytes,
                allocation.Key,
                allocation.ConnectionData
            );

            NetworkManager.Singleton.StartHost();
            m_relay_code = relayCode;
            Debug.Log($"Create Lobby Successful Lobby Code Is : {relayCode}");
            Debug.Log(relayCode);

            OnHostSuccess?.Invoke(relayCode);
            //NetworkManager.Singleton.SceneManager.LoadScene("FPSScene",UnityEngine.SceneManagement.LoadSceneMode.Single);
        }
        catch (RelayServiceException e)
        {
            Debug.LogError($"Create Lobby Exception {e}");
            GlobalEventSystem.RemoveLoader();
            GlobalEventSystem.DispalyPopup("Failed To Join Game", e.ToString());
        }
    }



    /// <summary>
    /// Can be able to Join Lobby
    /// </summary>
    /// <param name="_lobbyCode"></param>
    [ConsoleMethod("JoinLobby", "Player can join lobby using lobby code", "Lobby Code")]
    public static void JoinUDP(string _lobbyCode)
    {
        Instance.TryToJoinUDP(_lobbyCode);
    }

    public async void TryToJoinUDP(string _lobbyCode)
    {
        try
        {
            JoinAllocation allocation = await RelayService.Instance.JoinAllocationAsync(_lobbyCode);
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetClientRelayData(
                allocation.RelayServer.IpV4,
                (ushort)allocation.RelayServer.Port,
                allocation.AllocationIdBytes,
                allocation.Key,
                allocation.ConnectionData,
                allocation.HostConnectionData
            );
            NetworkManager.Singleton.StartClient();
            Debug.Log($"Joining Lobby with code : {_lobbyCode}");
        }
        catch (RelayServiceException e)
        {
            Debug.LogError($"Unable to join Lobby Exception : {e}");
            GlobalEventSystem.RemoveLoader();
            GlobalEventSystem.DispalyPopup("Failed To Join Game", e.ToString());
        }
    }

    [ConsoleMethod("Reconnect", "Reconnect To Game", "code")]
    public static void Console_Reconnect(string code)
    {
        JoinUDP(code);
    }

}
