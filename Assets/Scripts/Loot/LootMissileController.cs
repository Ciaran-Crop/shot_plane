using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootMissileController : LootController
{
    [SerializeField] int addCount = 1;
    protected override void PickUp(Collider2D collision)
    {
        if(collision.TryGetComponent<PlayerController>(out PlayerController playerController))
        {
            playerController.ChangeAddMissileCount(addCount);
        }
        base.PickUp(collision);
    }
}
