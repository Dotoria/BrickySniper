using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance { get; private set; }
    
    public List<EnemyScriptableObject> enemySO;
    private List<EnemyScriptableObject> _soList = new();

    private int _poolSize = 10;
    private ObjectPool _enemyPool;
    public GameObject enemyPrefab;

    private float time = 0f;
    private System.Random _random = new();
    private bool isSpawning = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        
        ObjectPool.CreatePool("enemy", enemyPrefab, _poolSize);
    }

    private void Update()
    {
        time += Time.deltaTime;
        
        foreach (var so in enemySO)
        {
            if (so.firstSpawnTime < time && !_soList.Contains(so))
            {
                _soList.Add(so);
            }
        }

        if (_soList.Count > 0 && !isSpawning) StartCoroutine(SetEnemy());
    }

    // destroyWallList 중 어느 한 군데에서 특정 enemy를 스폰, 발사하기
    IEnumerator SetEnemy()
    {
        isSpawning = true;
        
        var newEnemy = ObjectPool.Instance["enemy"].GetFromPool();
        
        Enemy enemy = newEnemy.GetComponent<Enemy>();
        enemy.enemySO = _soList[_random.Next(_soList.Count)];

        Vector3 pos = WallManager.destroyWallList[_random.Next(WallManager.destroyWallList.Count)].transform.position;
        Vector3 dir = new Vector3(_random.Next(-6, 6) * 1f, -7.75f) - pos;
        
        enemy.Shoot(pos, dir);
        
        yield return new WaitForSeconds(_random.Next(30, 80) / 10f);
        isSpawning = false;
    }

    // 풀로 돌려놓기
    public void DestroyEnemy(GameObject obj)
    {
        Destroy(obj.GetComponent<PolygonCollider2D>());
        ObjectPool.Instance["enemy"].ReturnToPool(obj);
    }
}