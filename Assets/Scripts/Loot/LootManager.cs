using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootManager : MonoBehaviour
{
    [SerializeField] LootItem[] loots;

    public void RandomLoot()
    {
        Vector2 newPosition2D = new Vector2(transform.position.x, transform.position.y);
        for(int i = 0;i < loots.Length;i++)
        {
            loots[i].Spawn(newPosition2D + Random.insideUnitCircle);
        }
    }
}
