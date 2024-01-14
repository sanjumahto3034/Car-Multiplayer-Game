using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VehicleListInstance : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Button button;
    [SerializeField] private TMP_Text text;
    public Action<GameObject> OnClick;
    private GameObject vehicleObject;
    private void Awake()
    {
        button.onClick.AddListener(() =>
        {
            OnClick?.Invoke(vehicleObject);
        });
    }

    public void SetContent(Sprite sprite, GameObject _vehicleObject, string _name, Action<GameObject> OnClickCallback)
    {
        image.sprite = sprite;
        vehicleObject = _vehicleObject;
        OnClick = OnClickCallback;
        text.text = _name;
    }

}
