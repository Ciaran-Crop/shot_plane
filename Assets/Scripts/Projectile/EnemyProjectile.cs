using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : Projectile
{
    TrailRenderer trail;

    void Awake()
    {
        trail = GetComponentInChildren<TrailRenderer>();
    }

    void OnDisable()
    {
        trail.Clear();
    }
    protected override bool OnCollisionEnter2D(Collision2D collision)
    {
        bool collisionResult = base.OnCollisionEnter2D(collision);
        if (collisionResult)
        {
            AudioManager.Instance.PlayEnemyProjectileHit1();
        }
        return collisionResult;
    }
}
