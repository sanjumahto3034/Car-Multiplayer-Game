using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;

[RequireComponent(typeof(Button))]
[RequireComponent(typeof(EventTrigger))]
public class CustomButtonEvent : MonoBehaviour
{
    public Action OnClick;
    public Action<bool> OnEnter;
    public Action<bool> OnExit;
    public Action<bool> OnClickEvent;


    public Image image
    {
        get
        {
            return GetComponent<Image>();
        }
    }


    private EventTrigger trigger;
    public bool IsHolding { get; private set; }
    RectTransform rect;
    Vector3 currentScale;

    [SerializeField] private bool HasClickEffect;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        currentScale = rect.localScale;
        trigger = GetComponent<EventTrigger>();
        CreateTrigger_PointerDown();
        CreateTrigger_PointerUp();
    }
    private void CreateTrigger_PointerDown()
    {
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerDown;
        entry.callback.AddListener((data) => { Trigger_OnPointerDown(); });
        trigger.triggers.Add(entry);
    }
    private void CreateTrigger_PointerUp()
    {
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerUp;
        entry.callback.AddListener((data) => { Trigger_OnPointerUp(); });
        trigger.triggers.Add(entry);
    }
    void Trigger_OnPointerDown()
    {
        IsHolding = true;
        OnEnter?.Invoke(IsHolding);
        OnClickEvent?.Invoke(IsHolding);
        OnClick?.Invoke();
        if (HasClickEffect)
            rect.localScale = currentScale * 0.9f;
    }
    void Trigger_OnPointerUp()
    {
        IsHolding = false;
        OnExit?.Invoke(IsHolding);
        OnClickEvent?.Invoke(IsHolding);
        if (HasClickEffect)
            rect.localScale = currentScale;
    }
}
