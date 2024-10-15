using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SupplySpawner : MonoBehaviour
{
    private SupplyScriptableObject supplySO;
    public ObjectPool supplyPool;

    public List<SupplyScriptableObject> fallingSupplyList = new();
    public List<SupplyScriptableObject> emergingSupplyList = new();
    private Vector3 _spawnPos;

    public GameObject supplyPrefab;

    private System.Random _random = new();
    private bool _isSpawning = true;

    private void Awake()
    {
        _spawnPos = transform.position;
        supplyPool = new ObjectPool(supplyPrefab, 10);
    }

    void Start()
    {
        StartCoroutine(SpawnFallingSuppliesRepeatedly());
    }
    
    IEnumerator SpawnFallingSuppliesRepeatedly()
    {
        while (_isSpawning)
        {
            // 랜덤 시간 설정
            float spawnTime = _random.Next(50, 100) / 10f;
            yield return SupplySpawn(fallingSupplyList);
            Debug.Log("Spawn Supply with " + spawnTime);
            yield return new WaitForSeconds(spawnTime);
        }
    }

    IEnumerator SpawnEmergingSuppliesRepeatedly()
    {
        while (_isSpawning)
        {
            float spawnTime = _random.Next(50, 100) / 10f;
            yield return SupplySpawn(emergingSupplyList);
            yield return new WaitForSeconds(spawnTime);
        }
    }
    
    IEnumerator SupplySpawn(List<SupplyScriptableObject> supplyList)
    {
        // 랜덤으로 선택
        int nth = _random.Next(supplyList.Count);
        supplySO = supplyList[nth];
        
        // 풀에서 가져오기
        GameObject newSupply = supplyPool.GetFromPool();
        newSupply.transform.position = _spawnPos;
        newSupply.transform.SetParent(GameObject.Find("Supplies").transform);
        
        // 정보 설정
        newSupply.name = supplySO.prefabName;
        newSupply.GetComponent<SpriteRenderer>().sprite = supplySO.prefabSprite;
        
        // 속성 설정
        Supply newSupplyInfo = newSupply.GetComponent<Supply>();
        newSupplyInfo.healthPoint = supplySO.healthPoint;
        newSupplyInfo.movePoint = supplySO.movePoint;
        
        newSupplyInfo.supplyLogic = supplySO.supplyLogic;
        newSupplyInfo.supplyTarget = supplySO.supplyTarget;
        newSupplyInfo.moveLogic = supplySO.moveLogic;
        
        yield return null;
    }
}