using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]public class LootItem
{
    [SerializeField] GameObject prefab;
    [SerializeField, Range(0f, 100f)] float spawnProbability;
    public void Spawn(Vector3 location)
    {
        if(Random.Range(0f, 100f) <= spawnProbability)
        {
            PoolManager.Release(prefab, location).SetActive(true);
        }
    }
}