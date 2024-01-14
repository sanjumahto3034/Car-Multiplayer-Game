using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateOnAxisRectTransform : MonoBehaviour
{
    [SerializeField] private Vector3 rotateAxis;
    [SerializeField] private float speed;
    private RectTransform rectTransform;
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    private void Update()
    {
        rectTransform.localEulerAngles += rotateAxis * speed * Time.deltaTime;
    }
}
