using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    public static Dictionary<string, ObjectPool> Instance { get; private set; } = new Dictionary<string, ObjectPool>();
    
    private List<GameObject> poolList;
    private GameObject prefab;

    private ObjectPool(GameObject prefab, int initialSize)
    {
        this.prefab = prefab;
        poolList = new List<GameObject>(initialSize);
        
        for (int i = 0; i < initialSize; i++)
        {
            GameObject obj = Object.Instantiate(prefab);
            obj.SetActive(false);
            poolList.Add(obj);
        }
    }
    
    public static void CreatePool(string key, GameObject prefab, int initialSize)
    {
        Instance[key] = new ObjectPool(prefab, initialSize);
    }

    public GameObject GetFromPool()
    {
        poolList.RemoveAll(obj => !obj);
        foreach (GameObject obj in poolList)
        {
            if (!obj.activeInHierarchy)
            {
                obj.SetActive(true);
                return obj;
            }
        }

        GameObject newObj = Object.Instantiate(prefab);
        newObj.SetActive(true);
        poolList.Add(newObj);
        return newObj;
    }

    public void ReturnToPool(GameObject obj)
    {
        obj.SetActive(false);
    }

    public void ReturnToPool()
    {
        poolList.RemoveAll(obj => obj == null);
    }
}