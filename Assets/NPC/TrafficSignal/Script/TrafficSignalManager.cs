using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficSignalManager : MonoBehaviour
{
    public bool signalStatus { get; private set; }
    public Action<bool> OnSignalUpdate;
    public enum SignalStatus { RED, YELLOW, GREEN }
    public SignalStatus Status;

    [Header("Signal Status")]
    [SerializeField] private Material DeActiveMaterial;
    [Header("Red Signal")]
    [SerializeField] private GameObject RedSignal;
    [SerializeField] private Material RedSignalActive;
    [Header("Yellow Signal")]
    [SerializeField] private GameObject YellowSignal;
    [SerializeField] private Material YellowSignalActive;
    [Header("Green Signal")]
    [SerializeField] private GameObject GreenSignal;
    [SerializeField] private Material GreenSignalActive;



    private Renderer RedSingalRenderer;
    private Renderer YellowSingalRenderer;
    private Renderer GreenSingalRenderer;
    private bool currentStatus;
    private void Awake()
    {
        if (RedSignal.TryGetComponent<Renderer>(out Renderer red_renderer)) { RedSingalRenderer = red_renderer; };
        if (YellowSignal.TryGetComponent<Renderer>(out Renderer yellow_renderer)) { YellowSingalRenderer = yellow_renderer; };
        if (GreenSignal.TryGetComponent<Renderer>(out Renderer green_renderer)) { GreenSingalRenderer = green_renderer; };
        YellowSingalRenderer.material = DeActiveMaterial;
    }
    public void SetActive(bool status)
    {
        currentStatus = status;
        signalStatus = status;
        OnSignalUpdate?.Invoke(status);

        RedSingalRenderer.material = !status ? RedSignalActive : DeActiveMaterial;
        //YellowSingalRenderer.material = status ? YellowSignalActive : DeActiveMaterial; 
        GreenSingalRenderer.material = status ? GreenSignalActive : DeActiveMaterial;
        //if (carMove != null && status)
        //{
        //    carMove.CanMove = status;
        //    carMove = null;
        //    Debug.Log("Giving Way to " + carMove.gameObject.name);
        //}
    }
    CanCarMove carMove;
    string TAG_CAR = "AICars";
    private void OnTriggerEnter(Collider other)
    {
        //if (other.gameObject.CompareTag(TAG_CAR) && !currentStatus)
        //{
        //    carMove = other.gameObject.GetComponentInChildren<CanCarMove>();
        //    Debug.Log("Trigger Way to " + carMove.gameObject.name);
        //}
    }
}
