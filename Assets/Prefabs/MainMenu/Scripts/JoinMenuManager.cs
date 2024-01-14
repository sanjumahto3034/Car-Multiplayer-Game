using IngameDebugConsole;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

public class JoinMenuManager : MonoBehaviour
{
    private MainMenuManager mainMenuManager;
    [SerializeField] private GameObject joinMenu;
    [SerializeField] private GameObject LobbyContent;
    [SerializeField] private Transform parent;
    private static JoinMenuManager Instance;
    [SerializeField] private Button closeButton;
    public Action OnClose;
    [SerializeField] private Button CloseButtonHeader;

    private void Awake()
    {
        Instance = this;
        mainMenuManager = GetComponent<MainMenuManager>();
        closeButton.onClick.AddListener(Disable);
        CloseButtonHeader.onClick.AddListener(Disable);
    }
    public void Enable()
    {
        joinMenu.SetActive(true);
        MultiplayerUtils.RequestLobbyList(OnGetLobbyList);
    }
    [ConsoleMethod("testjoin", "SDfkljsdf")]
    public static void TestCode()
    {
        MultiplayerUtils.RequestLobbyList(Instance.OnGetLobbyList);
    }
    public void OnGetLobbyList(List<Lobby> lobbyList)
    {
        foreach (Transform item in parent)
        {
            Destroy(item.gameObject);
        }
        foreach (Lobby lobby in lobbyList)
        {
            GameObject lobby_object = Instantiate(LobbyContent, Vector3.zero, Quaternion.identity, parent);
            lobby_object.transform.GetChild(0).GetComponent<TMP_Text>().text = lobby.Name;
            lobby_object.transform.GetChild(1).GetComponent<TMP_Text>().text = lobby.AvailableSlots + "/" + lobby.MaxPlayers;
            lobby_object.GetComponent<Button>().onClick.AddListener(() =>
            {
                Debug.Log("Click on buttonsfdjasdlf " + lobby.Id);
                MultiplayerUtils.JoinLobby(() =>
                {
                    Disable();
                    mainMenuManager.EnableOnJoinLobbyMenu();
                }, lobby.Id);
            });
        }
    }
    public void Disable()
    {
        OnClose?.Invoke();
        joinMenu.SetActive(false);
    }

    private void ResetData()
    {
        foreach (Transform item in parent)
        {
            Destroy(item);
        }
    }
    void Start()
    {

    }

    void Update()
    {

    }
}
