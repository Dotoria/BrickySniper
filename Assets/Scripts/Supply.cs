using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;

public class Supply : MonoBehaviour
{
    private Player player;
    private RaycastHit2D hit;
    private SupplySpawner _spawner;

    public int healthPoint;
    public SupplyLogic supplyLogic;
    public Target supplyTarget;

    private bool canShoot;
    public List<GameObject> supplyList = new();

    void Awake()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        _spawner = GetComponent<SupplySpawner>();
    }
    void Update()
    {
        Vector3 position = transform.position;
        position.x += Mathf.Sin(Time.time);
        position.y -= Time.deltaTime;
        transform.position = position;
        
        if (hit.collider)
        {
            if (hit.collider.CompareTag("Player"))
            {
                // player에 보급품 장착
                supplyList.Add(gameObject);
            }
        }

        if (healthPoint == 0)
        {
            _spawner.supplyPool.ReturnToPool(gameObject);
        }
    }
}