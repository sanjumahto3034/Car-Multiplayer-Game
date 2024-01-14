using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficSignalAreaManager : MonoBehaviour
{
    [SerializeField] private List<TrafficSignalManager> trafficManagers;

    private float remainingTimeToSwapSignal = 0;

    private void Update()
    {
        if (remainingTimeToSwapSignal <= 0)
        {
            NextSignal();
            remainingTimeToSwapSignal = 10;
        }
        remainingTimeToSwapSignal -= Time.deltaTime;
    }
    int counter = 0;
    private void NextSignal()
    {
        for (int i = 0; i < trafficManagers.Count; i++)
        {
            trafficManagers[i].SetActive(i == counter);
            Transform signal = trafficManagers[i].gameObject.transform;
            Debug.DrawRay(signal.position, signal.right * 50f, i == counter ? Color.green : Color.red, 10f);
        }
        counter++;
        if (counter > trafficManagers.Count - 1)
        {
            counter = 0;
        }

    }
}
