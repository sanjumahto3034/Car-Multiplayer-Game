using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

public class NPC_Spawner : MonoBehaviour
{
    [Header("@Prefab")]
    [SerializeField] private GameObject NPC_Object;
    [SerializeField] private GameObject[] NPC_Object_Car;
    [SerializeField] private GameObject[] NPC_Object_Train;

    public static NPC_Spawner Instance;

    [Serializable]
    private class MovePointTransformList
    {
        public string NameOfList = "Points";
        /// <summary>
        /// Point list
        /// </summary>
        public List<Transform> PointList
        {
            get
            {
                List<Transform> list = new();
                foreach (Transform item in parentObject)
                {
                    list.Add(item);
                }
                return list;
            }
        }
        public Transform parentObject;

    }
    [SerializeField] private List<MovePointTransformList> CarMovePoint = new();
    [SerializeField] private List<MovePointTransformList> TrainMovePoint = new();
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        StartCoroutine(SpawnNPC_Car(50));
        StartCoroutine(SpawnNPC_Train(2));
    }
    public void SpawnNPC(int count)
    {
        while (count > 0)
        {
            GameObject npc = Instantiate(NPC_Object);
            npc.GetComponent<NetworkObject>().Spawn(true);
            count--;
        }
    }
    public IEnumerator SpawnNPC_Car(int count)
    {
        while (count > 0)
        {
            Transform[] transformList = CarMovePoint[UnityEngine.Random.Range(0, CarMovePoint.Count)].PointList.ToArray();
            GameObject prefab = NPC_Object_Car[UnityEngine.Random.Range(0, NPC_Object_Car.Length)];
            GameObject npc_car = Instantiate(prefab, transformList[0].position, Quaternion.identity);
            npc_car.name = RandomName(prefab);
            npc_car.GetComponent<NPC_CAR>().SetDestination(transformList.ToList());
            count--;
            yield return new WaitForSeconds(1f);
        }
    }
    private string RandomName(GameObject @object)
    {
        return @object.name + "_" + UnityEngine.Random.Range(100, 999);
    }
    public IEnumerator SpawnNPC_Train(int count)
    {
        while (count > 0)
        {
            Transform[] transformList = TrainMovePoint[UnityEngine.Random.Range(0, TrainMovePoint.Count)].PointList.ToArray();
            GameObject npc = Instantiate(NPC_Object_Train[UnityEngine.Random.Range(0, NPC_Object_Train.Length)], transformList[0].position, Quaternion.identity);
            npc.GetComponent<NPC_TRAIN>().SetDestination(transformList.ToList());
            count--;
            yield return new WaitForSeconds(UnityEngine.Random.Range(20, 30));
        }
    }
    private void SpawnCarFromRandomPoints(int count)
    {
        while (count > 0)
        {
            GameObject npc = Instantiate(NPC_Object_Car[UnityEngine.Random.Range(0, NPC_Object_Car.Length)]);
            npc.GetComponent<NetworkObject>().Spawn(true);
            count--;
        }
    }
}
