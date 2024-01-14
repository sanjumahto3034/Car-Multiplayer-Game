using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FirstTimeNameInput : MonoBehaviour
{
    [SerializeField] private TMP_InputField InputBox;
    [SerializeField] private CustomButtonEvent buttonEvent;
    private void Awake()
    {
        MainMenuManager.Instance.DisableItems();
        InputBox.text = PlayerPrefs.GetString(StaticString.PLAYER_NAME, "");
        buttonEvent.OnClick += () =>
        {
            if (string.IsNullOrEmpty(InputBox.text))
            {
                GlobalEventSystem.DispalyPopup("Error!", "Name can't empty");
                return;
            }
            PlayerPrefs.SetString(StaticString.PLAYER_NAME, InputBox.text);
            MainMenuManager.Instance.EnableItems();
            Destroy(gameObject);

        };
    }
}
