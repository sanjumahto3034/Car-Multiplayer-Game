using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class GlobalEventSystem : MonoBehaviour
{
    private static GlobalEventSystem Instance;
    private void Awake()
    {
        Instance = this;
    }
    public static List<Transform> CarTransform = new List<Transform>();


    /// <summary>
    /// Popup
    /// </summary>
    [Header("@MessagePopup")]
    private GameObject popup_instance;
    [SerializeField] private GameObject PopupPrefab;

    public static void MessagePopup(string headerText, string descriptionText) => Instance.Utils_CreatePopup(headerText, descriptionText);
    public static void DispalyPopup(string headerText, string descriptionText) => Instance.Utils_CreatePopup(headerText, descriptionText);
    private void Utils_CreatePopup(string headerText, string descriptionText)
    {
        if (popup_instance != null) { return; }


        popup_instance = Instantiate(PopupPrefab);
        PopupManager popupManager = popup_instance.GetComponent<PopupManager>();
        popupManager.SetContent(headerText, descriptionText);
    }



    /// <summary>
    /// Loader
    /// </summary>
    [Header("@LoaderPopup")]
    private GameObject loader_instance;
    [SerializeField] private GameObject LoaderPrefab;

    public static void ShowLoader(string headerText, string descriptionText) => Instance.Utils_CreateLoader(headerText, descriptionText);
    public static void RemoveLoader() => Instance.Utils_RemoveLoader();
    public static void HideLoader() => Instance.Utils_RemoveLoader();
    public static Vector3 GetNavMeshRandomPoints(Vector3 currentPosition, float area) { return Instance.Utils_RandomNavMeshPoints(currentPosition, area); }
    public static Vector3 GetNavMeshRandomPoints() { return Instance.Utils_RandomNavMeshPoints(); }
    private void Utils_CreateLoader(string headerText, string descriptionText)
    {
        if (loader_instance != null) { return; }
        loader_instance = Instantiate(LoaderPrefab);
        LoaderManager loaderManager = loader_instance.GetComponent<LoaderManager>();
        loaderManager.SetContent(headerText, descriptionText);
    }
    private void Utils_RemoveLoader()
    {
        if (loader_instance == null) return;
        LoaderManager loaderManager = loader_instance.GetComponent<LoaderManager>();
        loaderManager.DestroyPopup();
    }
    private Vector3 Utils_RandomNavMeshPoints(Vector3 currentPosition, float area)
    {
        NavMeshHit hit;
        Vector3 randomPoints = Vector3.zero;

        while (randomPoints == Vector3.zero)
        {
            if (NavMesh.SamplePosition(currentPosition, out hit, area, NavMesh.AllAreas))
            {
                return hit.position;
            }
        }

        return Vector3.zero;
    }
    private Vector3 Utils_RandomNavMeshPoints()
    {
        NavMeshHit hit;

        if (NavMesh.SamplePosition(Random.insideUnitCircle, out hit, 1000, NavMesh.AllAreas))
        {
            Debug.Log($"NavMesh Random location {hit.position}");
            return hit.position;
        }
        Debug.Log($"@return default location {Vector3.zero}");
        return Vector3.zero;
    }


#if UNITY_EDITOR
    //========================================================================\\
    //========================================================================\\
    //========================================================================\\
    //========================================================================\\
    //===================== [For Unity Editor Use Start] =====================\\
    //========================================================================\\
    //========================================================================\\
    //========================================================================\\
    [UnityEditor.MenuItem("MultiplayerUtils/Hack/DetectBottom")]
    public static void DetectBottom()
    {
        foreach (GameObject item in Selection.gameObjects)
        {
            if (Physics.Raycast(item.transform.position, item.transform.up * -1, out RaycastHit hitInfo))
            {
                item.transform.position = hitInfo.point;
            }
        }
    }
    [UnityEditor.MenuItem("MultiplayerUtils/Hack/AddIndexOnEndName")]
    public static void AddIndexingOnGameObjectEnd()
    {
        int counter = 0;
        while (Selection.gameObjects.Length > counter)
        {
            Selection.gameObjects[counter].name += $"_{counter + 1}";
            counter++;
        }
    }
    [UnityEditor.MenuItem("MultiplayerUtils/Hack/CustomEvent")]
    public static void CustomEvent()
    {
        foreach (GameObject item in Selection.gameObjects)
        {
            item.transform.SetAsFirstSibling();
        }
    }
    //======================================================================\\
    //======================================================================\\
    //======================================================================\\
    //======================================================================\\
    //===================== [For Unity Editor Use End] =====================\\
    //======================================================================\\
    //======================================================================\\
    //======================================================================\\
#endif
}
