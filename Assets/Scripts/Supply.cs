using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;

public class Supply : MonoBehaviour
{
    NavMeshAgent agent;
    private Player player;
    private RaycastHit2D hit;
    private SupplySpawner _spawner;

    public int healthPoint;
    public int movePoint;
    public SupplyLogic supplyLogic;
    public Target supplyTarget;
    public MoveLogic moveLogic;

    private bool canShoot;
    public List<GameObject> supplyList = new();
    private System.Random _random = new();

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        _spawner = GetComponent<SupplySpawner>();
        
        // 맵 중앙에서 스폰
        if (moveLogic == MoveLogic.Spawn)
        {
            float randomPosX = _random.Next(-20, 20) / 10f;
            float randomPosY = _random.Next(-20, 40) / 10f;
            Vector3 position = new Vector3(randomPosX, randomPosY);
            transform.position = position;
        }
    }
    
    void Update()
    {
        // 위에서 떨어짐
        if (moveLogic == MoveLogic.ToPlayer)
        {
            float randomPosX = _random.Next(-20, 20) / 10f;
            agent.SetDestination(new Vector3(randomPosX, player.transform.position.y));
            agent.speed = movePoint * 0.5f;
        }
        
        if (hit.collider)
        {
            if (hit.collider.CompareTag("Player"))
            {
                // player에 보급품 장착
                supplyList.Add(gameObject);
            }

            if (hit.collider.CompareTag("Ball"))
            {
                // Logic에 따라 다르게 행동
                switch (supplyLogic)
                {
                    case SupplyLogic.Split:
                        
                        break;
                    case SupplyLogic.Default:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        if (healthPoint == 0)
        {
            _spawner.supplyPool.ReturnToPool(gameObject);
        }
    }
}