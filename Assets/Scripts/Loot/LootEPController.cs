using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootEPController : LootController
{
    [SerializeField] int obtainEP = 100;
    protected override void PickUp(Collider2D collision)
    {
        if(collision.TryGetComponent<PlayerEP>(out PlayerEP playerEP))
        {
            playerEP.Obtain(obtainEP);
        }
        base.PickUp(collision);
    }
}
