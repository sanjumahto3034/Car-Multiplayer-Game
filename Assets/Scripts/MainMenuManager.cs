using IngameDebugConsole;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private JoinMenuManager joinMenuManager;
    [SerializeField] private LobbyMenuManager lobbyMenuManager;
    [SerializeField] private HostMenuManager hostMenuManager;
    [SerializeField] private GameObject MenuObject;


    [Header("Buttons Of Main Menu")]
    [SerializeField] private Button HostButton;
    [SerializeField] private Button JoinButton;
    [SerializeField] private Button QuitGameButton;


    public static MainMenuManager Instance;

    [Header("@prefab Name Add Prefab")]
    [SerializeField] private GameObject AddNamePrefab;

    private void Awake()
    {
        Instance = this;
        HostButton.onClick.AddListener(EnableHostMenu);
        JoinButton.onClick.AddListener(EnableLobbySearchMenu);
        QuitGameButton.onClick.AddListener(QuitGame);

    }
    IEnumerator TestEverySecond()
    {
        while (true)
        {
            NavMeshHit hit;
            Vector3 randomPosition = Vector3.zero;

            // Attempt to find a random point on the NavMesh
            if (NavMesh.SamplePosition(Random.insideUnitSphere * 1000f, out hit, 1000f, NavMesh.AllAreas))
            {
                randomPosition = hit.position;
            }
            Debug.DrawLine(hit.position, hit.position + Vector3.up * 20, Color.blue, 10f);
            yield return new WaitForSeconds(0.01f);
        }
    }
    private void Start()
    {
        hostMenuManager.OnClose += EnableMainMenu;
        joinMenuManager.OnClose += EnableMainMenu;
        hostMenuManager.Disable();
        lobbyMenuManager.Disable();
        joinMenuManager.Disable();

        if (string.IsNullOrEmpty(PlayerPrefs.GetString(StaticString.PLAYER_NAME, "")))
        {
            Instantiate(AddNamePrefab);
        }
    }
    public void EnableHostMenu()
    {
        hostMenuManager.Enable();
        DisableMainMenu();
    }
    public void EnableLobbySearchMenu()
    {
        joinMenuManager.Enable();
        DisableMainMenu();
    }
    public void EnableOnJoinLobbyMenu()
    {
        lobbyMenuManager.Enable();
        DisableMainMenu();
    }

    public void EnableMainMenu()
    {
        MenuObject.SetActive(true);
    }
    public void DisableMainMenu()
    {
        MenuObject.SetActive(false);
    }
    [ConsoleMethod("RestartScene", "Reload Current Sceen")]
    public static void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    public void DisableMenuMananger()
    {
        hostMenuManager.Disable();
        lobbyMenuManager.Disable();
        joinMenuManager.Disable();
        MenuObject.SetActive(false);
    }

    public void DisableItems()
    {
        hostMenuManager.gameObject.SetActive(false);
        lobbyMenuManager.gameObject.SetActive(false);
        joinMenuManager.gameObject.SetActive(false);
        MenuObject.SetActive(false);
    }
    public void EnableItems()
    {
        hostMenuManager.gameObject.SetActive(true);
        lobbyMenuManager.gameObject.SetActive(true);
        joinMenuManager.gameObject.SetActive(true);
        MenuObject.SetActive(true);
    }
}
