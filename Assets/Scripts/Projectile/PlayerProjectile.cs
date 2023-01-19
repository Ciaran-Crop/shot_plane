using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : Projectile
{
    protected override bool OnCollisionEnter2D(Collision2D collision)
    {
        bool collisionResult =  base.OnCollisionEnter2D(collision);
        if(collisionResult)
        {
            AudioManager.Instance.PlayRandomPlayerProjectileHit();
        }
        return collisionResult;
    }
}
