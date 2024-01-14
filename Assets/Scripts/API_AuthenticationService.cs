using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class API_AuthenticationService : MonoBehaviour
{

    /// <summary>
    /// Login Class
    /// </summary>
    [Serializable]
    private class LoginClass
    {
        [SerializeField] private GameObject @object;
        [SerializeField] private TMP_InputField emailInputBox;
        [SerializeField] private TMP_InputField passwordInputBox;
        [SerializeField] private CustomButtonEvent submitButton;
        public Action OnLoginSucces;
        public void Initialize()
        {
            submitButton.OnClick += () => HTTPRequest.Instance.RequestToLogin((string token) =>
            {
                Debug.Log($"Login Status + {token}");
            }, emailInputBox.text, passwordInputBox.text);
        }
        public void Reset()
        {
            emailInputBox.text = string.Empty;
            passwordInputBox.text = string.Empty;
        }
        public void Enable()
        {
            Reset();
            @object.SetActive(true);
        }
        public void Disable()
        {
            Reset();
            @object.SetActive(false);
        }
    }

    /// <summary>
    /// Register Class
    /// </summary>
    [Serializable]
    private class RegisterClass
    {
        [Header("Registration Details")]
        [SerializeField] private GameObject @object;
        [SerializeField] private TMP_InputField nameInputBox;
        [SerializeField] private TMP_InputField emailInputBox;
        [SerializeField] private GameObject emailErrorBox;


        [SerializeField] private TMP_InputField password_1InputBox;
        [SerializeField] private TMP_InputField password_2InputBox;
        [SerializeField] private GameObject password_error_1;
        [SerializeField] private GameObject password_error_2;

        [SerializeField] private CustomButtonEvent registerDetailsSubmiButton;



        [Header("Aavtar Picture and Detail View")]
        [SerializeField] private GameObject prefabOfIcon;
        [SerializeField] private Sprite[] profileIcons;
        [SerializeField] private Transform parentOfItem;
        [SerializeField] private TMP_Text profileName;
        [SerializeField] private TMP_Text profileEmail;
        [SerializeField] private Image profilePicture;
        [SerializeField] private CustomButtonEvent finalButton;

        public Action OnLoginSucces;
        public void Initialize()
        {

            nameInputBox.text = "Sanju Mahto";
            emailInputBox.text = "sanjumahto302@gmail.com";
            password_1InputBox.text = "123";
            password_2InputBox.text = "123";

            registerDetailsSubmiButton.OnClick += InitilizeUserDetails;
        }
        private void InitilizeUserDetails()
        {
            string user_name = nameInputBox.text;
            string user_email = emailInputBox.text;
            string user_password_1 = password_1InputBox.text;
            string user_password_2 = password_2InputBox.text;


            if (user_password_1 != user_password_2 || user_password_1.Length == 0 || user_password_2.Length == 0)
                return;

            RegistrationDetails details = new RegistrationDetails()
            {
                name = user_name,
                email = user_email,
                password = user_password_1,
            };

            HTTPRequest.Instance.RequestToRegister(OnRegistrationSuccess, OnRegistrationFailed, details);
        }

        void OnRegistrationSuccess(string data)
        {
            Debug.Log("[API Response] " + data);
        }
        void OnRegistrationFailed(string error)
        {
            Debug.LogError(error);
        }


        private void InitilizeAavtars()
        {
            profilePicture.sprite = profileIcons[0];
            Color non_select_background = prefabOfIcon.GetComponent<Image>().color;
            Color select_background = Color.black;
            Image select_image = null;

            for (int i = 0; i < profileIcons.Length; i++)
            {
                Sprite profile_image = profileIcons[i];
                CustomButtonEvent button = Instantiate(prefabOfIcon, Vector3.zero, Quaternion.identity, parentOfItem).GetComponent<CustomButtonEvent>();
                button.transform.GetChild(0).GetComponent<Image>().sprite = profile_image;
                Action action = () =>
                {
                    if (select_image != null)
                    {
                        select_image.color = non_select_background;
                    }

                    select_image = button.GetComponent<Image>();
                    select_image.color = select_background;
                    profilePicture.sprite = profile_image;
                };
                button.OnClick += action;
                if (i == 0) action();

            }

        }
        public void Reset()
        {
            nameInputBox.text = string.Empty;
            emailInputBox.text = string.Empty;
            password_1InputBox.text = string.Empty;
            password_2InputBox.text = string.Empty;
        }
        public void Enable()
        {
            Reset();
            @object.SetActive(true);
        }
        public void Disable()
        {
            Reset();
            @object.SetActive(false);
        }
    }
    [Header("@Login Object")]
    [SerializeField] private LoginClass loginObject;
    [Header("@Register Object")]
    [SerializeField] private RegisterClass registerObject;


    private void Awake()
    {
        //EnableLogin();
        EnableRegister();
    }
    private void Start()
    {
        //Intialize Form
        loginObject.Initialize();
        registerObject.Initialize();

        //Succcess Callback on register success
        loginObject.OnLoginSucces += OnLoginSuccess;
        registerObject.OnLoginSucces += OnLoginSuccess;

    }
    void EnableLogin()
    {
        loginObject.Enable();
        registerObject.Disable();
    }
    void EnableRegister()
    {
        registerObject.Enable();
        loginObject.Disable();
    }

    void ResetAllEntries()
    {
        loginObject.Reset();
        registerObject.Reset();
    }
    void OnLoginSuccess()
    {

    }
}
