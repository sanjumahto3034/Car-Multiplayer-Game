using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GlobalInputSystem : MonoBehaviour
{
    public static Vector2 Move = Vector2.zero;
    public static bool IsHandBreakApplied = false;
    private static GlobalInputSystem Instance;
    public static Action SwitchCameraEvent;
    public static Action ResetRotationEvent;
    public static Action ResetPositionEvent;

    [SerializeField] private bool IsNative;
    [SerializeField] private CustomButtonEvent LeftMoveInput;
    [SerializeField] private CustomButtonEvent RightMoveInput;

    [SerializeField] private CustomButtonEvent gasInput;
    [SerializeField] private CustomButtonEvent breaknput;

    [SerializeField] private CustomButtonEvent handBreakInput;

    [SerializeField] private CustomButtonEvent cameraSwitchInput;
    [SerializeField] private CustomButtonEvent ResetRotatonInput;
    [SerializeField] private CustomButtonEvent ResetPositionInput;

    private void Awake()
    {
        Instance = this;
        UpdateTouchInput();
        if (IsNative)
        {
            DisableNativeInputUI();
        }
    }
    public static void EnableUI()
    {
        Instance.ResetRotatonInput.gameObject.SetActive(true);
        Instance.ResetPositionInput.gameObject.SetActive(true);
    }
    public static void EnableNativeInputUI()
    {
        if (Instance.IsNative)
        {
            Instance.LeftMoveInput.gameObject.SetActive(true);
            Instance.RightMoveInput.gameObject.SetActive(true);
            Instance.gasInput.gameObject.SetActive(true);
            Instance.breaknput.gameObject.SetActive(true);
            Instance.handBreakInput.gameObject.SetActive(true);
            Instance.cameraSwitchInput.gameObject.SetActive(true);
        }
    }
    public static void DisableNativeInputUI()
    {
        Instance.LeftMoveInput.gameObject.SetActive(false);
        Instance.RightMoveInput.gameObject.SetActive(false);
        Instance.gasInput.gameObject.SetActive(false);
        Instance.breaknput.gameObject.SetActive(false);
        Instance.handBreakInput.gameObject.SetActive(false);
        Instance.cameraSwitchInput.gameObject.SetActive(false);
    }
    private void UpdateTouchInput()
    {
        LeftMoveInput.OnClickEvent += (bool input) =>
        {
            Move.x = input ? -1 : 0;
        };
        RightMoveInput.OnClickEvent += (bool input) =>
        {
            Move.x = input ? 1 : 0;
        };


        gasInput.OnClickEvent += (bool input) =>
        {
            Move.y = input ? 1 : 0;
        };
        breaknput.OnClickEvent += (bool input) =>
        {
            Move.y = input ? -1 : 0;
        };

        handBreakInput.OnClickEvent += (bool input) =>
        {
            IsHandBreakApplied = input;
        };
        cameraSwitchInput.OnClick += OnCameraSwitch;
        ResetRotatonInput.OnClick += () => { ResetRotationEvent?.Invoke(); };
        ResetPositionInput.OnClick += () => { ResetPositionEvent?.Invoke(); };
    }

    //========================================================================== \\
    //========================================================================== \\
    //========================== Input From Input System ======================= \\
    //========================================================================== \\
    //========================================================================== \\
    private void OnMove(InputValue value)
    {
        Move = value.Get<Vector2>();
    }
    private void OnHandBreak(InputValue value)
    {
        Debug.Log("Handbreak - " + value.Get<float>());
        IsHandBreakApplied = value.Get<float>() > 0 ? true : false;
    }
    private void OnCameraSwitch()
    {
        SwitchCameraEvent?.Invoke();
    }

    //========================================================================== \\
    //========================================================================== \\
    //========================== Input From Input System ======================= \\
    //========================================================================== \\
    //========================================================================== \\

}
