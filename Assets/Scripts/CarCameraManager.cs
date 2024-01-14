using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CarCameraManager : MonoBehaviour
{


    private Transform TopViewTarget;
    private Transform InsideViewTarget;


    [SerializeField] private CinemachineVirtualCamera virtualCamera;


    [TextArea(1, 5)]
    [SerializeField] private string DEBUG;
    public enum LookType
    {
        INSIDE,
        TOP_100,
        TOP_50
    }
    public LookType lookType;
    private void Awake()
    {
        Switch100();

    }
    private void Start()
    {
        GlobalInputSystem.SwitchCameraEvent += SwitchCamera;
    }
    public void SetCameraTargetPoint(Transform topView, Transform insideView)
    {
        TopViewTarget = topView;
        InsideViewTarget = insideView;

        virtualCamera.m_Follow = TopViewTarget;
        virtualCamera.m_LookAt = TopViewTarget;

        lookType = LookType.TOP_100;
        Switch100();
    }
    private void Update()
    {

        if (TopViewTarget == null || InsideViewTarget == null)
            return;
    }
    void SwitchCamera()
    {
        if (lookType == LookType.TOP_100)
        {
            lookType = LookType.TOP_50;
            Switch50();
            return;
        }
        if (lookType == LookType.TOP_50)
        {
            lookType = LookType.INSIDE;
            SwtichInside();
            return;
        }
        if (lookType == LookType.INSIDE)
        {
            lookType = LookType.TOP_100;
            Switch100();
            return;
        }

    }

    void Switch100()
    {
        virtualCamera.m_Follow = TopViewTarget;
        virtualCamera.m_LookAt = TopViewTarget;
        if (virtualCamera != null)
        {
            // Get the CinemachineFramingTransposer component from the virtual camera
            CinemachineFramingTransposer transposer = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();

            if (transposer != null)
            {
                // Change the camera's distance
                transposer.m_CameraDistance = 1f;
                SetCameraDamping(1);
            }
        }
        //TopView50Camera.enabled = false;
        //TopView50Camera.transform.gameObject.GetComponent<AudioListener>().enabled = false;

        //TopView100Camera.enabled = true;
        //TopView100Camera.transform.gameObject.GetComponent<AudioListener>().enabled = true;

        //InsideCamera.enabled = false;
        //InsideCamera.transform.gameObject.GetComponent<AudioListener>().enabled = false;
    }
    void Switch50()
    {
        virtualCamera.m_Follow = TopViewTarget;
        virtualCamera.m_LookAt = TopViewTarget;
        if (virtualCamera != null)
        {
            // Get the CinemachineFramingTransposer component from the virtual camera
            CinemachineFramingTransposer transposer = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();

            if (transposer != null)
            {
                // Change the camera's distance
                transposer.m_CameraDistance = 0.5f;
                SetCameraDamping(1);
            }
        }
        //TopView50Camera.enabled = true;
        //TopView50Camera.transform.gameObject.GetComponent<AudioListener>().enabled = true;

        //TopView100Camera.enabled = false;
        //TopView100Camera.transform.gameObject.GetComponent<AudioListener>().enabled = false;

        //InsideCamera.enabled = false;
        //InsideCamera.transform.gameObject.GetComponent<AudioListener>().enabled = false;

    }
    void SwtichInside()
    {
        virtualCamera.m_Follow = InsideViewTarget;
        virtualCamera.m_LookAt = InsideViewTarget;
        if (virtualCamera != null)
        {
            // Get the CinemachineFramingTransposer component from the virtual camera
            CinemachineFramingTransposer transposer = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();

            if (transposer != null)
            {
                // Change the camera's distance
                transposer.m_CameraDistance = 0f;
                SetCameraDamping(0);
            }
        }
        //TopView50Camera.enabled = false;
        //TopView50Camera.transform.gameObject.GetComponent<AudioListener>().enabled = false;

        //TopView100Camera.enabled = false;
        //TopView100Camera.transform.gameObject.GetComponent<AudioListener>().enabled = false;

        //InsideCamera.enabled = true;
        //InsideCamera.transform.gameObject.GetComponent<AudioListener>().enabled = true;

    }
    void SetCameraDamping(float value)
    {
        if (virtualCamera != null)
        {
            // Get the CinemachineFramingTransposer component from the virtual camera
            CinemachineFramingTransposer transposer = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();

            if (transposer != null)
            {
                // Change the camera's distance
                transposer.m_XDamping = value;
                transposer.m_YDamping = value;
                transposer.m_ZDamping = value;
            }
        }
    }

}
