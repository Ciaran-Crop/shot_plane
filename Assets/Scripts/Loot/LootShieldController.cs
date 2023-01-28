using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootShieldController : LootController
{
    [SerializeField] float restoreHealth = 20f;
    [SerializeField] int fullHealthScore = 200;
    [SerializeField] AudioData scoreData;
    protected override void PickUp(Collider2D collision)
    {
        if(collision.TryGetComponent<PlayerHealth>(out PlayerHealth playerHealth))
        {
            if(playerHealth.IsFullHealth)
            {
                pickData = scoreData;
                lootMessage.text = string.Format("SCORE + {0}", fullHealthScore);
                collision.GetComponent<PlayerScore>().UpdateScore(fullHealthScore);
            }
            else
            {
                pickData = defaultData;
                lootMessage.text = string.Format("SHIELD + {0}", restoreHealth);
                playerHealth.RestoreHealth(restoreHealth);
            }
        }
        base.PickUp(collision);
    }
}
