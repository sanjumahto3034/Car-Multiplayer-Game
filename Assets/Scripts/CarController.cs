using System;
using System.Collections;
using System.Collections.Generic;
using IngameDebugConsole;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(ClientNetworkTransform))]
[RequireComponent(typeof(NetworkObject))]
[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(NetworkRigidbody))]
public class CarController : NetworkBehaviour
{
    [Header("Debug Field")]
    [TextArea(1, 5)]
    [SerializeField] private string CAR_DEBUG;


    private float horizontalInput, verticalInput;
    private float currentSteerAngle, currentbreakForce;
    private bool isBreaking;

    // Settings
    [SerializeField] private float motorForce, breakForce, maxSteerAngle;

    // Wheel Colliders
    [SerializeField] private WheelCollider frontLeftWheelCollider, frontRightWheelCollider;
    [SerializeField] private WheelCollider rearLeftWheelCollider, rearRightWheelCollider;

    // Wheels
    [SerializeField] private Transform frontLeftWheelTransform, frontRightWheelTransform;
    [SerializeField] private Transform rearLeftWheelTransform, rearRightWheelTransform;

    [Header("Car Audio Volume")]
    [SerializeField] private AudioSource carAudioSource;


    [Header("Camera Look Point")]
    [SerializeField] private Transform topViewCameraLookPoint;
    [SerializeField] private Transform insideViewCameraLookPoint;

    [SerializeField] private Rigidbody rb;
    private static CarController controller;


    [SerializeField] private Transform StearingRotation;

    [Header("Mirror Manager")]
    [SerializeField] private GameObject LeftMirror;
    [SerializeField] private GameObject RightMirror;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (IsOwner && IsHost)
        {
            GlobalEventSystem.CarTransform.Add(transform);
        }
        if (!IsOwner)
        {
            NotOwner();
            return;
        }
        controller = this;
        LocalGameManager.Instance.carCameraManager.SetCameraTargetPoint(topViewCameraLookPoint, insideViewCameraLookPoint);
        ResetPosition();
        if (LocalGameManager.Instance.CurrentVehicleObject != null)
        {
            transform.position = LocalGameManager.Instance.CurrentVehicleObject.transform.position + Vector3.up * 0.1f;
            LocalGameManager.Instance.CurrentVehicleObject.GetComponent<NetworkObject>().Despawn();
        }
        LocalGameManager.Instance.CurrentVehicleObject = gameObject;


        GlobalInputSystem.EnableNativeInputUI();
        GlobalInputSystem.ResetRotationEvent += ResetRotation;
        GlobalInputSystem.ResetPositionEvent += ResetPosition;
        GlobalInputSystem.EnableUI();
        if (IsHost)
        {
            NPC_Spawner.Instance.SpawnNPC_Car(10);
        }

    }
    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();
        if (IsOwner && IsHost)
        {
            GlobalEventSystem.CarTransform.Remove(transform);
        }
    }
    private void NotOwner()
    {
        LeftMirror?.SetActive(false);
        RightMirror?.SetActive(false);
    }
    [ConsoleMethod("ResetVehiclePosition", "Reset Vehicle Position")]
    public static void Console_ResetPosition()
    {
        controller?.ResetPosition();
    }
    [ConsoleMethod("ResetVehicleRotation", "Reset Vehicle Rotation")]
    public static void Console_ResetRotation()
    {
        controller?.ResetRotation();
    }

    void ResetRotation()
    {
        if (!IsOwner)
            return;
        transform.eulerAngles = new Vector3(0, 0, 0);
    }
    void ResetPosition()
    {
        if (!IsOwner)
            return;
        float diff = 1.2f;
        float dif_X = UnityEngine.Random.Range(-diff, diff);
        float dif_Y = UnityEngine.Random.Range(-diff, diff);
        transform.position = LocalGameManager.Instance.spawnPosition + new Vector3(dif_X, 1, dif_Y);
    }
    private void Awake()
    {
        carAudioSource.loop = true;
        if (!IsOwner)
            return;
    }
    private void FixedUpdate()
    {
        if (!IsOwner)
            return;
        GetInput();
        HandleMotor();
        HandleSteering();
        UpdateWheels();
    }
    private void Update()
    {

        string debug_ = "Velocity  : " + rb.velocity + "\n";
        CAR_DEBUG = debug_;
        carAudioSource.pitch = Mathf.Clamp(rb.velocity.magnitude / 2, 1f, 3);
        SpeedMeterManager.SetSpeed(rb.velocity.magnitude * 10f);

    }
    private void GetInput()
    {
        // Steering Input
        horizontalInput = GlobalInputSystem.Move.x;

        // Acceleration Input
        verticalInput = GlobalInputSystem.Move.y;
        StearingRotation.localEulerAngles = new Vector3(0, horizontalInput * 60, 0);
        //Debug.Log("Eular ANgel Is" + StearingRotation.localEulerAngles);
        // Breaking Input
        isBreaking = GlobalInputSystem.IsHandBreakApplied;
    }

    private void HandleMotor()
    {
        frontLeftWheelCollider.motorTorque = verticalInput * motorForce;
        frontRightWheelCollider.motorTorque = verticalInput * motorForce;
        currentbreakForce = isBreaking ? breakForce : 0f;
        ApplyBreaking();
    }

    private void ApplyBreaking()
    {
        frontRightWheelCollider.brakeTorque = currentbreakForce;
        frontLeftWheelCollider.brakeTorque = currentbreakForce;
        rearLeftWheelCollider.brakeTorque = currentbreakForce;
        rearRightWheelCollider.brakeTorque = currentbreakForce;
    }

    private void HandleSteering()
    {
        currentSteerAngle = maxSteerAngle * horizontalInput;
        frontLeftWheelCollider.steerAngle = currentSteerAngle;
        frontRightWheelCollider.steerAngle = currentSteerAngle;
    }

    private void UpdateWheels()
    {
        UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateSingleWheel(frontRightWheelCollider, frontRightWheelTransform);
        UpdateSingleWheel(rearRightWheelCollider, rearRightWheelTransform);
        UpdateSingleWheel(rearLeftWheelCollider, rearLeftWheelTransform);
    }

    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.rotation = rot;
        wheelTransform.position = pos;
    }
}