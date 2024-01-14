using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanCarMove : MonoBehaviour
{
    public bool CanMove = true;
    string TAG_CAR = "AICars";
    string TAG_TRAFFIC_SIGNAL = "traffic_signal";
    TrafficSignalManager trafficSignalManager;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(TAG_CAR) && other.gameObject != transform.parent.gameObject)
        {
            CanMove = false;
        }
        if (other.gameObject.CompareTag(TAG_TRAFFIC_SIGNAL))
        {
            trafficSignalManager = other.gameObject.GetComponent<TrafficSignalManager>();
            bool signal_status = trafficSignalManager.signalStatus;
            if (!signal_status)
            {
                trafficSignalManager.OnSignalUpdate += OnSignalUpdate;
            }
            CanMove = signal_status;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(TAG_CAR))
        {
            CanMove = true;
        }
    }
    private void OnSignalUpdate(bool update)
    {
        if (!update)
            return;

        CanMove = update;
        Debug.Log($"Car Release -> {update}," + gameObject.name);
        if (trafficSignalManager != null)
        {
            trafficSignalManager.OnSignalUpdate -= OnSignalUpdate;
            trafficSignalManager = null;
        }
    }
}
