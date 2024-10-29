using System;
using System.Collections.Generic;
using UnityEngine;

public class WallManager : MonoBehaviour
{
    public static WallManager Instance { get; private set; }
    
    public GameObject[] wallPrefabs;

    private readonly System.Random _random = new();
    public List<GameObject> wallList = new();
    public static List<GameObject> destroyWallList = new();

    private void Awake()
    {
        for (int i = 0; i < 16; i++)
        {
            GameObject newWall = Instantiate(wallPrefabs[_random.Next(wallPrefabs.Length)]);
            wallList.Add(newWall);
        }
        
        SetWallPos();

        int destroy = _random.Next(6, 10);
        destroyWallList.Add(wallList[destroy]);
        wallList[destroy].SetActive(false);
    }

    void SetWallPos()
    {
        float x = -6.5f;
        float y = -5.5f;
        
        // right
        for (int i = 0; i < 6; i++)
        {
            wallList[i].transform.position = new Vector2(x, y);
            wallList[i].transform.SetParent(transform);
            y += 3f;
        }

        x += 2f;
        y -= 2f;
        
        // up
        for (int i = 6; i < 10; i++)
        {
            wallList[i].transform.position = new Vector2(x, y);
            if (wallList[i].name.Contains("WeakWall"))
            {
                wallList[i].transform.position = new Vector2(x, y + 0.25f);
            }
            wallList[i].transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90f));
            wallList[i].transform.SetParent(transform);
            x += 3f;
        }

        x -= 1f;
        y -= 1f;
        
        // left
        for (int i = 10; i < 16; i++)
        {
            wallList[i].transform.position = new Vector2(x, y);
            wallList[i].transform.SetParent(transform);
            y -= 3f;
        }
    }
}