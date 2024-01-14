using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopupManager : MonoBehaviour
{
    [SerializeField] private CustomButtonEvent BackButtonEvent;
    [SerializeField] private CustomButtonEvent YesButtonEvent;
    [SerializeField] private CustomButtonEvent OkButtonEvent;
    [SerializeField] private CustomButtonEvent NoButtonEvent;
    [SerializeField] private TMP_Text HeaderText;
    [SerializeField] private TMP_Text DescriptionText;
    public Action OnClickYes;
    public Action OnClickNo;
    public Action OnClickOk;


    private void Awake()
    {
        BackButtonEvent.OnClick += () =>
        {
            OnClickOk?.Invoke();
            Destroy(gameObject);
        };
        YesButtonEvent.OnClick += () =>
        {
            OnClickYes?.Invoke();
            Destroy(gameObject);
        };
        OkButtonEvent.OnClick += () =>
        {
            OnClickOk?.Invoke();
            Destroy(gameObject);
        };
        NoButtonEvent.OnClick += () =>
        {
            OnClickNo?.Invoke();
            Destroy(gameObject);
        };
    }
    public void SetContent(string headerText,string descriptionText)
    {
        HeaderText.text = headerText;
        DescriptionText.text = descriptionText;
    }
}
