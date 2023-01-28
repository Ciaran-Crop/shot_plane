using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootEnergyController : LootController
{
    [SerializeField] int obtainEnergy = 20;
    [SerializeField] int fullEnergyScore = 200;
    [SerializeField] AudioData scoreData;
    protected override void PickUp(Collider2D collision)
    {
        if(collision.TryGetComponent<PlayerEnergy>(out PlayerEnergy playerEnergy))
        {
            if(playerEnergy.IsFull)
            {
                pickData = scoreData;
                lootMessage.text = string.Format("SCORE + {0}", fullEnergyScore);
                collision.GetComponent<PlayerScore>().UpdateScore(fullEnergyScore);
            }
            else
            {
                pickData = defaultData;
                lootMessage.text = string.Format("ENERGY + {0}", obtainEnergy);
                playerEnergy.Obtain(obtainEnergy);
            }
        }
        base.PickUp(collision);
    }
}
