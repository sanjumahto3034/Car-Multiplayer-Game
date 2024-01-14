using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;

public class NPC_AI : NetworkBehaviour
{
    //Animation and It's Key
    [SerializeField] private Animator animator;
    public const string KEY_WALK = "walk";

    NavMeshAgent agent;
    private Vector3 TargetPoint;

    private void Awake()
    {

    }
    private void Start()
    {

    }
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (!IsServer)
            return;
        InitilizeAI();
    }
    private void InitilizeAI()
    {
        agent = GetComponent<NavMeshAgent>();
        transform.position = GlobalEventSystem.GetNavMeshRandomPoints(GlobalEventSystem.CarTransform[0].position, 100);
        Debug.DrawLine(TargetPoint, TargetPoint + Vector3.up * 50, Color.red, 20);
        TargetPoint = GlobalEventSystem.GetNavMeshRandomPoints();
        Debug.Log($"[ NPC AI ] Spawn New NPC Name : {gameObject.name} at-> {TargetPoint}, {transform.position}");   
        agent.SetDestination(TargetPoint);
        animator.SetBool(KEY_WALK, true);
    }
    private void Update()
    {
        if (!IsServer)
            return;
        if (Vector3.Distance(transform.position, TargetPoint) < 1f)
        {
            TargetPoint = GlobalEventSystem.GetNavMeshRandomPoints();
            agent.SetDestination(TargetPoint);
        }

        foreach (Transform car_transform in GlobalEventSystem.CarTransform)
        {
            if (Vector3.Distance(car_transform.position, transform.position) > 20)
            {
                InitilizeAI();
            }
        }
    }
}
