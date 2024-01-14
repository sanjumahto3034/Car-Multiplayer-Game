using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_TRAIN : MonoBehaviour
{
    public List<Transform> pathList = new();

    public void SetDestination(List<Transform> transformList)
    {
        pathList = transformList;
        transform.position = pathList[0].position;
        transform.LookAt(pathList[0].position);
    }
    public int counter = 1;

    private void Update()
    {
        if (counter > pathList.Count - 1)
        {
            OnDestinationReach();
            return;
        }
        transform.position = Vector3.MoveTowards(transform.position, pathList[counter].position, 0.01f);
        transform.LookAt(pathList[counter].position);
        Debug.DrawRay(transform.position, transform.up, Color.blue);
        Debug.DrawRay(pathList[counter].position, pathList[counter].up, Color.red);
        if (Vector3.Distance(transform.position, pathList[counter].position) < 1f)
        {
            counter++;
        }
    }
    private void OnDestinationReach()
    {
        NPC_Spawner.Instance.SpawnNPC_Train(1);
        Destroy(gameObject);
    }
}
