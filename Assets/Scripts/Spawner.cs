// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.Serialization;
//
// public class SpawnerInfo
// {
//     public string ObjectName;
//     public GameObject Prefab;
//     public List<ScriptableObject> ScriptableObjects = new();
//     public ObjectPool Pool { get; set; }
// }
//
// public class Spawner : MonoBehaviour
// {
//     public static Spawner Instance;
//     public SpawnerInfo[] SpawnerInfos;
//     
//     private ScriptableObjectBase _so;
//     private Vector3 _spawnPos;
//
//     private System.Random _random = new();
//     private bool _isSpawning = true;
//
//     private void Awake()
//     {
//         _spawnPos = transform.position;
//         if (!Instance)
//         {
//             Instance = this;
//         }
//         else
//         {
//             Destroy(gameObject);
//         }
//         
//         foreach (SpawnerInfo info in SpawnerInfos)
//         {
//             info.Pool = new ObjectPool(info.Prefab, 5);
//         }
//     }
//
//     void Start()
//     {
//         StartCoroutine(SpawnRepeatedly("Supply"));
//         StartCoroutine(SpawnRepeatedly("Enemy"));
//     }
//
//     public SpawnerInfo FindSpawnerInfo(string objectName)
//     {
//         foreach (SpawnerInfo info in SpawnerInfos)
//         {
//             if (info.ObjectName == objectName)
//             {
//                 return info;
//             }
//         }
//
//         return null;
//     }
//     
//     IEnumerator SpawnRepeatedly(string spawnObject)
//     {
//         SpawnerInfo info = FindSpawnerInfo(spawnObject);
//         
//         while (_isSpawning)
//         {
//             // 랜덤 시간 설정
//             float spawnTime = _random.Next(50, 100) / 10f;
//             yield return SupplySpawn(info);
//             Debug.Log($"Spawn {Spawner.Instance.SpawnerInfos} with " + spawnTime);
//             yield return new WaitForSeconds(spawnTime);
//         }
//     }
//
//     IEnumerator SpawnEmergingSuppliesRepeatedly()
//     {
//         while (_isSpawning)
//         {
//             float spawnTime = _random.Next(50, 100) / 10f;
//             yield return SupplySpawn(emergingSupplyList);
//             yield return new WaitForSeconds(spawnTime);
//         }
//     }
//     
//     IEnumerator SupplySpawn(SpawnerInfo info)
//     {
//         // 랜덤으로 선택
//         int nth = _random.Next(info.ScriptableObjects.Count);
//         supplySO = info.ScriptableObjects[nth];
//         
//         // 풀에서 가져오기
//         GameObject newSupply = info.Pool.GetFromPool();
//         newSupply.transform.position = _spawnPos;
//         newSupply.transform.SetParent(GameObject.Find("Supplies").transform);
//         
//         // 정보 설정
//         newSupply.name = supplySO.prefabName;
//         newSupply.GetComponent<SpriteRenderer>().sprite = supplySO.prefabSprite;
//         
//         // 속성 설정
//         Supply newSupplyInfo = newSupply.GetComponent<Supply>();
//         _so.ApplyTo(newSupplyInfo);
//         
//         yield return null;
//     }
//     
//     public ObjectPool GetPool()
// }