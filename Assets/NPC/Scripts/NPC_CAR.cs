using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC_CAR : MonoBehaviour
{
    public List<Transform> pathList = new();
    [SerializeField] private CanCarMove CanCarMove;

    public void SetDestination(List<Transform> transformList)
    {
        pathList = transformList;
        transform.position = pathList[0].position;
        transform.LookAt(pathList[0].position);
    }
    public int counter = 1;

    private void Update()
    {
        if (!CanCarMove.CanMove)
        {
            return;
        }


        if (counter > pathList.Count - 1)
        {
            OnDestinationReach();
            return;
        }
        transform.position = Vector3.MoveTowards(transform.position, pathList[counter].position, 0.02f);
        Quaternion targetRotation = Quaternion.LookRotation(pathList[counter].position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);
        Debug.DrawRay(transform.position, transform.up, Color.blue);
        Debug.DrawRay(pathList[counter].position, pathList[counter].up, Color.red);
        if (Vector3.Distance(transform.position, pathList[counter].position) < 0.1f)
        {
            counter++;
        }
    }
    private void OnDestinationReach()
    {
        NPC_Spawner.Instance.SpawnNPC_Car(1);
        Destroy(gameObject);
    }
}
