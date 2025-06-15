using System.Collections;
using System.Collections.Generic;
using Common;
using Data;
using UnityEngine;

namespace Game
{
    public class EnemyManager : MonoBehaviour
    {
        private List<EnemyScriptableObject> enemySO;
        private List<EnemyScriptableObject> _soList = new();

        private int _poolSize = 10;
        private GameObject enemyPrefab;

        private float time = 0f;
        private System.Random _random = new();
        private bool isSpawning = false;

        private void Start()
        {
            enemyPrefab = Resources.Load<GameObject>("Enemy");
            enemySO = DataManager.Instance.EnemiesData["INF"];

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

        private void OnDestroy()
        {
            ObjectPool.Instance["enemy"].ReturnToPool();
        }

        // destroyWallList 중 어느 한 군데에서 특정 enemy를 스폰, 발사하기
        IEnumerator SetEnemy()
        {
            isSpawning = true;

            ObjectPool.Instance["enemy"].GetFromPool().TryGetComponent(out Enemy enemy);
            enemy.enemySO = _soList[_random.Next(_soList.Count)];

            // Vector3 pos = WallManager.DestroyWallList[_random.Next(WallManager.DestroyWallList.Count)].transform.position;
            Vector3 pos = new Vector3(_random.Next(-60, 60) * 0.1f, 9f);
            Vector3 dir = new Vector3(_random.Next(-6, 6) * 1f, -6.75f) - pos;

            enemy.Shoot(pos, dir);

            yield return new WaitForSeconds(_random.Next(30, 80) / 10f);
            isSpawning = false;
        }
    }
}