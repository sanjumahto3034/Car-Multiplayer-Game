using System.Collections;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif
[RequireComponent(typeof(Camera))]
public class ScreenShotManager : MonoBehaviour
{
    Camera targetCamera; // Reference to the camera you want to capture
    private static ScreenShotManager instance;
    [SerializeField] private string Name;

    private void Awake()
    {
        targetCamera = GetComponent<Camera>();
        instance = this;
    }

    // Function to capture the screenshot
#if UNITY_EDITOR
    [MenuItem("Car Multiplayer/Capure")]
    public static void Caputure() => instance.CaptureScreenshot();
#endif
    public void CaptureScreenshot()
    {
        StartCoroutine(TakeScreenshotAndSave());
    }

    IEnumerator TakeScreenshotAndSave()
    {
        yield return new WaitForEndOfFrame();

        // Create a texture to store the screenshot
        RenderTexture renderTexture = new RenderTexture(Screen.width, Screen.height, 24);
        targetCamera.targetTexture = renderTexture;

        // Create a new Texture2D and read the camera render into it
        Texture2D screenshot = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        targetCamera.Render();
        RenderTexture.active = renderTexture;
        screenshot.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);

        // Reset the active camera texture and target texture
        targetCamera.targetTexture = null;
        RenderTexture.active = null;
        Destroy(renderTexture);

        // Convert texture to bytes and save as a PNG file
        byte[] bytes = screenshot.EncodeToPNG();
        string filename = string.IsNullOrEmpty(Name) ? "Screenshot_" + System.DateTime.Now.ToString("yyyy-MM-dd-HHmmss") + ".png" : Name + ".png";
        System.IO.File.WriteAllBytes(Application.persistentDataPath + "/" + filename, bytes);

        Debug.Log("Screenshot saved to: " + Application.persistentDataPath + "/" + filename);
    }
}
