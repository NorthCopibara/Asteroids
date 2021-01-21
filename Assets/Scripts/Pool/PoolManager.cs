using System;
using System.Collections.Generic;

using UnityEngine;

public static class PoolManager
{
    public static bool logStatus = true;

    private static Transform _root;

    private static Dictionary<GameObject, ObjectPool<GameObject>> _pools = new Dictionary<GameObject, ObjectPool<GameObject>>();
    private static Dictionary<GameObject, ObjectPool<GameObject>> _instanceLookup = new Dictionary<GameObject, ObjectPool<GameObject>>();

    private static GameObject InstantiatePrefab(GameObject prefab)
    {
        var go = UnityEngine.Object.Instantiate(prefab);
        if (_root != null) go.transform.parent = _root;
        go.SetActive(false);
        return go;
    }

    #region Static API

    public static int GetCountObjectInPool(GameObject prefab)
    {
        int count = 0;

        if (_pools.ContainsKey(prefab))
        {
            count = _pools[prefab].CountInQueue;
        }
        else
        {
            Debug.LogWarning("No pool contains the object: " + prefab.name);
        }

        return count;
    }

    public static void SetRoot(Transform Root)
    {
        _root = Root;
    }

    public static void PrintStatus()
    {
        foreach (KeyValuePair<GameObject, ObjectPool<GameObject>> keyVal in _pools)
        {
            Debug.Log(string.Format("Object Pool for Prefab: {0} in queue {1}", keyVal.Key.name, keyVal.Value.CountInQueue));
        }
    }

    public static void WarmPool(GameObject prefab, int size)
    {
        if (_pools.ContainsKey(prefab))
        {
            throw new Exception("Pool for prefab " + prefab.name + " has already been created");
        }

        var pool = new ObjectPool<GameObject>(() => { return InstantiatePrefab(prefab); }, size);
        _pools[prefab] = pool;

        if (logStatus)
        {
            PrintStatus();
        }
    }

    public static GameObject SpawnObjectOriginalTransform(GameObject prefab)
    {
        if (!_pools.ContainsKey(prefab))
        {
            WarmPool(prefab, 1);
        }

        var pool = _pools[prefab];

        var clone = pool.GetItem();
        clone.SetActive(true);

        _instanceLookup.Add(clone, pool);

        if (logStatus)
        {
            PrintStatus();
        }

        return clone;
    }

    public static GameObject SpawnObject(GameObject prefab)
    {
        return SpawnObject(prefab, Vector3.zero, Quaternion.identity);
    }

    public static GameObject SpawnObject(GameObject prefab, Vector3 position)
    {
        return SpawnObject(prefab, position, Quaternion.identity);
    }

    public static GameObject SpawnObject(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        if (!_pools.ContainsKey(prefab))
        {
            WarmPool(prefab, 1);
        }

        var pool = _pools[prefab];

        var clone = pool.GetItem();
        clone.transform.position = position;
        clone.transform.rotation = rotation;
        clone.SetActive(true);

        _instanceLookup.Add(clone, pool);

        if (logStatus)
        {
            PrintStatus();
        }

        return clone;
    }

    public static void ReleaseObject(GameObject clone)
    {
        DespawnView(clone);
        Despawn(clone);
    }

    private static void DespawnView(GameObject clone) 
    {
        var despawn = clone.GetComponent<IPoolable>();
        if (despawn != null)
        {
            despawn.OnDespawn();
        }
    }

    private static void Despawn(GameObject clone) 
    {
        clone.SetActive(false);

        if (_instanceLookup.ContainsKey(clone))
        {
            _instanceLookup[clone].ReleaseItem(clone);
            _instanceLookup.Remove(clone);

            if (logStatus)
            {
                PrintStatus();
            }
        }
        else
        {
            Debug.LogWarning("No pool contains the object: " + clone.name);
        }
    }

    public static void ReleaseAllObject() 
    {
        List<GameObject> clones = new List<GameObject>();

        foreach (var clone in _instanceLookup.Keys)
        {
            clones.Add(clone);
            
        }

        foreach (var clone in clones)
        {
            Despawn(clone);
        }
    }
    #endregion
}


