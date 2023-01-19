using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOverdriveProjectile : Projectile
{
    [SerializeField] ProjectileGuidanceSystem projectileGuidanceSystem;
    protected override void OnEnable()
    {
        SetTarget(EnemyManager.Instance.RandomEnemyTarget());
        transform.rotation = Quaternion.identity;
        if (target == null)
        {
            base.OnEnable();
        }
        else
        {
            StartCoroutine(projectileGuidanceSystem.HomingCoroutine(target));
        }
    }

    protected override bool OnCollisionEnter2D(Collision2D collision)
    {
        bool collisionResult = base.OnCollisionEnter2D(collision);
        if (collisionResult)
        {
            AudioManager.Instance.PlayRandomPitch(AudioManager.Instance.PlayerProjectileHit3Data);
        }
        return collisionResult;
    }
}
