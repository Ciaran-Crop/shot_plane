using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : HealthSystem
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Die()
    {
        base.Die();
        var enemyManager = EnemyManager.Instance;
        enemyManager.GetComponent<EnemyManager>().RemoveOneEnemy(gameObject);
        AudioManager.Instance.PlayRandomEnemyExplosion();
        GetComponent<LootManager>().RandomLoot();
    }
}
