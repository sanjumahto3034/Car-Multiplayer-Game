using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HostMenuManager : MonoBehaviour
{
    private MainMenuManager mainMenuManager;
    [SerializeField] private Button CloseButton;
    [SerializeField] private GameObject hostMenu;

    [Header("Menu Content")]
    [SerializeField] private TMP_InputField lobbyName;
    [SerializeField] private Toggle noOfPlayerToggle_2;
    [SerializeField] private Toggle noOfPlayerToggle_3;
    [SerializeField] private Toggle noOfPlayerToggle_4;
    [SerializeField] private Toggle IsLobbyPrivateToggle;
    [SerializeField] private Button createLobbyButton;
    public Action OnClose;
    [SerializeField] private Button CloseButtonHeader;
    private void Awake()
    {
        mainMenuManager = GetComponent<MainMenuManager>();
        CloseButton.onClick.AddListener(() =>
        {
            Disable();
        });

        UpdateToggleEvent();
        Disable();
        createLobbyButton.onClick.AddListener(Create);
        CloseButtonHeader.onClick.AddListener(() =>
        {
            Disable();
        });
    }
    public void Enable()
    {
        hostMenu.SetActive(true);
    }
    public void Disable()
    {
        hostMenu.SetActive(false);
        lobbyName.text = "";
        noOfPlayerToggle_2.isOn = true;
        noOfPlayerToggle_3.isOn = false;
        noOfPlayerToggle_4.isOn = false;
        IsLobbyPrivateToggle.isOn = false;
        OnClose?.Invoke();
    }

    void UpdateToggleEvent()
    {
        noOfPlayerToggle_2.onValueChanged.AddListener((b) =>
        {
            noOfPlayerToggle_3.isOn = false;
            noOfPlayerToggle_4.isOn = false;
        });
        noOfPlayerToggle_3.onValueChanged.AddListener((b) =>
        {
            noOfPlayerToggle_2.isOn = false;
            noOfPlayerToggle_4.isOn = false;
        });
        noOfPlayerToggle_4.onValueChanged.AddListener((b) =>
        {
            noOfPlayerToggle_2.isOn = false;
            noOfPlayerToggle_3.isOn = false;
        });
    }

    void Create()
    {
        if (string.IsNullOrEmpty(lobbyName.text))
        {
            GlobalEventSystem.DispalyPopup("No Room Name Found","Ops! It's look like you don't give any room name. \nPlease Give Room Name To Continue");
            return;
        }
        int maxPlayer = 2;
        if (noOfPlayerToggle_2.isOn)
            maxPlayer = 2;
        if (noOfPlayerToggle_3.isOn)
            maxPlayer = 3;
        if (noOfPlayerToggle_4.isOn)
            maxPlayer = 4;

        MultiplayerUtils.Instance.CreateLobby((l) => { Disable(); mainMenuManager.EnableOnJoinLobbyMenu(); }, lobbyName.text, maxPlayer, IsLobbyPrivateToggle.isOn);
        mainMenuManager.DisableMenuMananger();
    }

}
