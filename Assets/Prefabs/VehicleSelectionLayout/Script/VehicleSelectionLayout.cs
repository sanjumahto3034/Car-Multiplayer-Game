using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class VehicleSelectionLayout : MonoBehaviour
{
    [Serializable]
    public class VehicleList
    {
        public GameObject vehicle;
        public string vehicleName;
        public Sprite vehicleImage;
    }
    [SerializeField] private List<VehicleList> vehicleLists = new List<VehicleList>();

    [SerializeField] private GameObject ButtonObject;
    [SerializeField] private Transform parentOfSpawner;
    [SerializeField] private Canvas canvas;
    public Action OnSpawnInitialize;
    private void Awake()
    {
        InitializeVehicleList();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (MultiplayerUtils.IsRunning)
                canvas.enabled = !canvas.enabled;
        }
    }
    void InitializeVehicleList()
    {
        foreach (VehicleList list in vehicleLists)
        {
            GameObject button_object = Instantiate(ButtonObject, Vector3.zero, Quaternion.identity, parentOfSpawner);
            button_object.GetComponent<VehicleListInstance>().SetContent(list.vehicleImage, list.vehicle, list.vehicleName, SpawnVehcle);
        }
    }
    void SpawnVehcle(GameObject vehicleObject)
    {
        SpawnVehicle_ClientRpc(vehicleObject);
    }
    [ClientRpc]
    void SpawnVehicle_ClientRpc(GameObject vehicleObject)
    {
        GameObject vehicle = Instantiate(vehicleObject);
        vehicle.GetComponent<NetworkObject>().Spawn();
        OnSpawnInitialize?.Invoke();
    }
}
