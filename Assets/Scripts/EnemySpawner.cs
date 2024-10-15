using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private EnemyScriptableObject enemySO;
    public ObjectPool enemyPool;

    public List<EnemyScriptableObject> enemyTypeList = new();
    private Vector3 _spawnPos;

    public GameObject enemyPrefab;

    private System.Random _random = new();
    private bool _isSpawning = true;

    private void Awake()
    {
        _spawnPos = transform.position;
        enemyPool = new ObjectPool(enemyPrefab, 10);
    }

    void Start()
    {
        StartCoroutine(SpawnEnemiesRepeatedly());
    }

    IEnumerator SpawnEnemiesRepeatedly()
    {
        while (_isSpawning)
        {
            // 랜덤 시간 설정
            int spawnTime = _random.Next(30, 50);

            yield return EnemySpawn();
            Debug.Log("Spawn Enemy with " + spawnTime);
            yield return new WaitForSeconds(spawnTime / 10f);
        }
    }

    IEnumerator EnemySpawn()
    {
        // 랜덤으로 선택
        int nth = _random.Next(enemyTypeList.Count);
        enemySO = enemyTypeList[nth];
        
        // 풀에서 가져오기
        GameObject newEnemy = enemyPool.GetFromPool();
        newEnemy.transform.position = _spawnPos;
        newEnemy.transform.SetParent(GameObject.Find("Enemies").transform);
        
        // 정보 설정
        newEnemy.name = enemySO.prefabName;
        newEnemy.GetComponent<SpriteRenderer>().sprite = enemySO.prefabSprite;
        
        // 속성 설정
        Enemy newEnemyInfo = newEnemy.GetComponent<Enemy>();
        newEnemyInfo.healthPoint = enemySO.healthPoint;
        newEnemyInfo.attackPoint = enemySO.attackPoint;
        newEnemyInfo.movePoint = enemySO.movePoint;
        
        newEnemyInfo.attackLogic = enemySO.attackLogic;
        newEnemyInfo.attackTarget = enemySO.attackTarget;
        newEnemyInfo.moveLogic = enemySO.moveLogic;
        
        yield return null;
    }
}