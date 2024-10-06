using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupplySpawner : MonoBehaviour
{
    private SupplyScriptableObject supplySO;
    public ObjectPool supplyPool;

    public List<SupplyScriptableObject> supplyTypeList = new();
    private Vector3 _spawnPos;

    public GameObject supplyPrefab;

    private System.Random _random = new();
    private bool isSpawning = false;

    private void Awake()
    {
        _spawnPos = transform.position;
        supplyPool = new ObjectPool(supplyPrefab, 10);
    }

    void Start()
    {
        StartCoroutine(SpawnSuppliesRepeatedly());
    }
    
    IEnumerator SpawnSuppliesRepeatedly()
    {
        // 랜덤 시간 설정
        int spawnTime = _random.Next(50, 100);
        
        while (true)
        {
            yield return SupplySpawn();
            yield return new WaitForSeconds(spawnTime / 10f);
        }
    }
    
    IEnumerator SupplySpawn()
    {
        // 랜덤으로 선택
        int nth = _random.Next(supplyTypeList.Count);
        supplySO = supplyTypeList[nth];
        
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
        newSupplyInfo.supplyLogic = supplySO.supplyLogic;
        newSupplyInfo.supplyTarget = supplySO.supplyTarget;
        
        yield return null;
    }
}