using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth : EnemyHealth
{
    [SerializeField] int lootCount = 3;

    public override void Die()
    {
        base.Die();
        LootManager.Instance.RandomMoreLoot(transform.position, lootCount);
    }
}
