using System.Collections.Generic;
using UnityEngine;

public class Pool<T> where T : MonoBehaviour
{
    private List<T> poolList;

    public static Pool<T> Create(int size = 10, T prefab = null)
    {
        var pool = new Pool<T>(size);
        if (prefab != null)
        {
            for (int i = 0; i < size; i++)
            {
                T newObj = Object.Instantiate(prefab);
                newObj.gameObject.SetActive(false);
                pool.ReturnToPool(newObj);
            }
        }
        return pool;
    }

    public Pool(int size = 10)
    {
        poolList = new List<T>(size);
    }

    public T GetFromPool()
    {
        if (poolList.Count > 0)
        {
            T obj = poolList[0];
            poolList.RemoveAt(0);
            obj.gameObject.SetActive(true);
            return obj;
        }
        return null;
    }

    public void ReturnToPool(T obj)
    {
        obj.gameObject.SetActive(false);
        poolList.Add(obj);
    }
}