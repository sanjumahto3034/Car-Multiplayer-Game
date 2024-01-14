using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedMeterManager : MonoBehaviour
{
    [SerializeField] private GameObject SpeedMeterObject;
    [SerializeField] private RectTransform Analog;
    private static SpeedMeterManager Instance;
    private void Awake()
    {
        Instance = this;
        SpeedMeterObject.SetActive(false);
    }
    public static void SetSpeed(float speed)
    {
        Instance.SpeedMeterObject.SetActive(speed > 0);
        Instance.Analog.localRotation = Quaternion.Euler(new Vector3(0, 0, -speed));
    }
}
