using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class LocalGameManager : MonoBehaviour
{
    public static LocalGameManager Instance;
    void Awake()
    {
        Instance = this;
    }

    [SerializeField] private Transform vehicleSpawnTransform;
    public Vector3 spawnPosition
    {
        get
        {
            return vehicleSpawnTransform.position;
        }
    }
    [SerializeField] private CarCameraManager m_carCameraManager;
    public CarCameraManager carCameraManager { get { return m_carCameraManager; } }
    public void SetLookTargetToCar(Transform topView, Transform insideView) => carCameraManager.SetCameraTargetPoint(topView, insideView);
    [HideInInspector] public GameObject CurrentVehicleObject;

}
