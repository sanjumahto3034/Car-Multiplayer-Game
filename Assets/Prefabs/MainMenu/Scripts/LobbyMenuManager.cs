using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

public class LobbyMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject lobbyObject;
    [SerializeField] private TMP_Text LobbyNameLabel;
    [SerializeField] private TMP_Text LobbyCodeLabel;
    [SerializeField] private TMP_Text NoOfPlayerLabel;
    [SerializeField] private Transform parent;
    [SerializeField] private GameObject playerDataContent;
    private MainMenuManager menuManager;
    [SerializeField] private Button CloseButtonHeader;
    [SerializeField] private Button StartGameButton;
    private void Awake()
    {
        menuManager = GetComponent<MainMenuManager>();
        CloseButtonHeader.onClick.AddListener(() =>
        {
            Disable();
            MultiplayerUtils.LeaveLobby();
        });
        StartGameButton.onClick.AddListener(() =>
        {
            MultiplayerUtils.StartGame();
            menuManager.DisableMenuMananger();
        });
    }
    public void Enable()
    {
        lobbyObject.SetActive(true);
        MultiplayerUtils.GetJoinLobby((Lobby lobby) =>
        {
            OnGetLobbyData(lobby);
            MultiplayerUtils.OnLobbyUpdate(OnLobbyUpdate);
        });
    }
    void OnLobbyUpdate(LobbyEventCallbacks callbacks)
    {
        callbacks.PlayerJoined += (player) =>
        {
            MultiplayerUtils.GetJoinLobby(OnGetLobbyData);
            Debug.Log("[LobbyEvent] New Player Join");
        };
        callbacks.KickedFromLobby += () =>
        {
            Disable();
            Debug.Log("[LobbyEvent] You have been kicked from the lobby");
        };
        callbacks.PlayerLeft += (player) =>
        {
            MultiplayerUtils.GetJoinLobby(OnGetLobbyData);
            Debug.Log("[LobbyEvent] Player Left the game");
        };
        callbacks.LobbyDeleted += () =>
        {
            Disable();
            Debug.Log("[LobbyEvent] Lobby Has been deleted");
        };
        callbacks.DataAdded += (data) =>
        {

            MultiplayerUtils.GetJoinLobby((lobby) =>
            {
                string code = lobby.Data[MultiplayerUtils.KEY_GAME_CODE].Value;
                Debug.Log("Game Started " + code);

                if (!string.IsNullOrEmpty(code) && MultiplayerUtils.IsClient)
                {
                    MultiplayerUtils.JoinUDP(code);
                    menuManager.DisableMenuMananger();
                }
            });

        };
        callbacks.DataChanged += (data) =>
        {
            Debug.Log("On Data Change");
        };
        callbacks.DataRemoved += (data) =>
        {
            Debug.Log("On Data Remove");
        };
    }

    void OnGetLobbyData(Lobby lobbyData)
    {

        foreach (Transform item in parent)
        {
            Destroy(item.gameObject);
        }

        LobbyNameLabel.text = lobbyData.Name;
        LobbyCodeLabel.text = lobbyData.LobbyCode;
        NoOfPlayerLabel.text = lobbyData.Players.Count + "/" + lobbyData.MaxPlayers;

        StartGameButton.gameObject.SetActive(lobbyData.HostId == MultiplayerUtils.AuthenticationId);

        foreach (Player player in lobbyData.Players)
        {
            GameObject player_object = Instantiate(playerDataContent, Vector3.zero, Quaternion.identity, parent);

            TMP_Text nameLable = player_object.GetComponentInChildren<TMP_Text>();
            if (nameLable != null)
                nameLable.text = player.Data[MultiplayerUtils.KEY_PLAYER_NAME].Value;


            Debug.Log($"1:{MultiplayerUtils.AuthenticationId},{lobbyData.HostId}");

            if (MultiplayerUtils.AuthenticationId == lobbyData.HostId && MultiplayerUtils.IsHost)
            {
                GameObject buttonObject = player_object.GetComponentInChildren<Button>().transform.gameObject;
                buttonObject.SetActive(true);
                buttonObject.GetComponent<Button>().onClick.AddListener(() =>
                {
                    MultiplayerUtils.KickPlayer(player);
                });
            }
            else
            {
                GameObject buttonObject = player_object.GetComponentInChildren<Button>().transform.gameObject;
                buttonObject.SetActive(false);
            }

        }
    }

    public void Disable()
    {
        menuManager.EnableMainMenu();
        lobbyObject.SetActive(false);
    }
}
