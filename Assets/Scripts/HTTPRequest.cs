using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using UnityEngine;
using UnityEngine.Networking;

public class HTTPRequest : MonoBehaviour
{
    /// <summary>
    /// Instance of HTTPRequest
    /// </summary>
    public static HTTPRequest Instance;


    const string BASE_URL = "http://localhost:35527";
    const string LOGIN_URL = BASE_URL + "/login";
    const string REGISTER_URL = BASE_URL + "/register";
    private void Awake()
    {
        Instance = this;
    }
    public void RequestToRegister(Action<string> OnRegistrationSuccess, Action<string> OnRegistrationFailed, RegistrationDetails details)
    {
        StartCoroutine(api_put(JsonUtility.ToJson(details), REGISTER_URL, OnRegistrationSuccess, OnRegistrationFailed));
    }









    public void RequestToLogin(Action<string> OnLoginSuccess, string email, string password)
    {
        string credential = new LoginJsonData().GetString(email, password);
        StartCoroutine(api_post(OnLoginSuccess, credential));
    }
    private IEnumerator api_post(Action<string> OnRequestSuccess, string data)
    {
        var uwr = new UnityWebRequest(LOGIN_URL, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(data);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");

        //Send the request then wait here until it returns
        yield return uwr.SendWebRequest();

        if (uwr.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
            OnRequestSuccess("FAILED");
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);
            OnRequestSuccess(new LoginJsonData().ReturnToken(uwr.downloadHandler.text));
        }
    }
    private IEnumerator api_put(string data, string url, Action<string> OnRequestSuccess, Action<string> OnRequestFailed)
    {
        var uwr = new UnityWebRequest(url, "PUT");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(data);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");

        //Send the request then wait here until it returns
        yield return uwr.SendWebRequest();

        if (uwr.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
            OnRequestFailed(uwr.error);
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);
            OnRequestSuccess(uwr.downloadHandler.text);

        }
    }

}

[Serializable]
public class LoginJsonData
{
    public string email;
    public string password;


    [Serializable]
    public class ResponseObject
    {
        public int status;
        public string message;
        public string token;
    }
    public string GetString(string _email, string _password)
    {

        return JsonUtility.ToJson(new LoginJsonData()
        {
            email = _email,
            password = _password
        });
    }
    public string GetStringData()
    {
        return JsonUtility.ToJson(new LoginJsonData()
        {
            email = email,
            password = password
        });
    }
    public string GetStringData(LoginJsonData loginObject)
    {
        return JsonUtility.ToJson(loginObject);
    }
    public string ReturnToken(string data)
    {
        ResponseObject response = JsonUtility.FromJson<ResponseObject>(data);
        return response.token;
    }
}

[Serializable]
public class RegistrationDetails
{
    public string name;
    public string email;
    public string password;
    public int profileIconIndex = 0;
}





