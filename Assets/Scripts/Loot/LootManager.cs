using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootManager : Singleton<LootManager>
{
    [SerializeField] GameObject[] loots;

    int len => loots.Length;

    public void RandomOneLoot(Vector3 location)
    {
        PoolManager.Release(loots[Random.Range(0, len)], location).SetActive(true);
    }

    public void RandomMoreLoot(Vector3 location, int count)
    {
        for(int i = 0;i < count; i++)
        {
            RandomOneLoot(location);
        }
    }
}
