using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPS_LocalGameManager : MonoBehaviour
{
    public static FPS_LocalGameManager Instance;
    [SerializeField] private Camera m_camera;
    public static Camera PlayerCamera { get { return Instance.m_camera; } }
    private void Awake()
    {
        Instance = this;
    }

    public static void GetHitPoint(Action<RaycastHit> callback_RayCastHit, Action<Vector3> callback_hitPoint)
    {
        Ray ray = PlayerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        Vector3 hitPosition = ray.GetPoint(75f);
        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            callback_RayCastHit(hitInfo);
        }
        callback_hitPoint(hitPosition);

    }

}
