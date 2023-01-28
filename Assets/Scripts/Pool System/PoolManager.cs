using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [SerializeField] Pool[] playerProjectilePools;
    [SerializeField] Pool[] enemyProjectilePools;
    [SerializeField] Pool[] vFXPools;
    [SerializeField] Pool[] enemyPools;
    [SerializeField] Pool[] lootPools;

    static Dictionary<GameObject, Pool> dictionary;


#if UNITY_EDITOR
    void OnDestroy()
    {
        CheckRuntimeSize(playerProjectilePools);
        CheckRuntimeSize(enemyProjectilePools);
        CheckRuntimeSize(vFXPools);
        CheckRuntimeSize(enemyPools);
        CheckRuntimeSize(lootPools);
    }

#endif

    void CheckRuntimeSize(Pool[] pools)
    {
        foreach (var pool in pools)
        {
            if (pool.Size < pool.RuntimeSize)
            {
                Debug.LogWarning(
                    string.Format(
                        "Pool : {0}: Size : {1}, but the Runtime Queue Size is {2}",
                        pool.Prefab.name,
                        pool.Size,
                        pool.RuntimeSize
                    )
                );
            }
        }
    }

    void Awake()
    {
        dictionary = new Dictionary<GameObject, Pool>();
        Initialize(playerProjectilePools);
        Initialize(enemyProjectilePools);
        Initialize(vFXPools);
        Initialize(enemyPools);
        Initialize(lootPools);
    }

    void Initialize(Pool[] pools)
    {
        foreach (var pool in pools)
        {
#if UNITY_EDITOR
            if (dictionary.ContainsKey(pool.Prefab))
            {
                Debug.LogError("Same dictionary key in pool manager, prefab key: " + pool.Prefab.name);

                continue;
            }
#endif
            dictionary.Add(pool.Prefab, pool);
            Transform poolParent = new GameObject("Pool: " + pool.Prefab.name).transform;
            poolParent.parent = transform;
            pool.Initialize(poolParent);
        }
    }
    /// <summary>
    ///     return PoolManger GameObject according to <paramref name="prefab"></paramref>
    /// </summary>
    /// <param name="prefab">prefab</param>
    /// <returns>
    ///     A GameObject
    /// </returns>
    public static GameObject Release(GameObject prefab)
    {
#if UNITY_EDITOR
        if (!dictionary.ContainsKey(prefab))
        {
            Debug.LogError("Pool Manager has not this key: " + prefab.name);
            return null;
        }
#endif
        return dictionary[prefab].Get();
    }

    public static GameObject Release(GameObject prefab, Vector2 position)
    {
#if UNITY_EDITOR
        if (!dictionary.ContainsKey(prefab))
        {
            Debug.LogError("Pool Manager has not this key: " + prefab.name);
            return null;
        }
#endif
        return dictionary[prefab].Get(position);
    }

    public static GameObject Release(GameObject prefab, Vector2 position, Quaternion rotation)
    {
#if UNITY_EDITOR
        if (!dictionary.ContainsKey(prefab))
        {
            Debug.LogError("Pool Manager has not this key: " + prefab.name);
            return null;
        }
#endif
        return dictionary[prefab].Get(position, rotation);
    }


    public static GameObject Release(GameObject prefab, Vector2 position, Quaternion rotation, bool setActive)
    {
#if UNITY_EDITOR
        if (!dictionary.ContainsKey(prefab))
        {
            Debug.LogError("Pool Manager has not this key: " + prefab.name);
            return null;
        }
#endif
        return dictionary[prefab].Get(position, rotation, setActive);
    }

    public static GameObject Release(GameObject prefab, Vector2 position, Quaternion rotation, Vector3 localScale)
    {
#if UNITY_EDITOR
        if (!dictionary.ContainsKey(prefab))
        {
            Debug.LogError("Pool Manager has not this key: " + prefab.name);
            return null;
        }
#endif
        return dictionary[prefab].Get(position, rotation, localScale);
    }
}
