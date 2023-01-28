using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootScoreController : LootController
{
    [SerializeField] int score;
    protected override void PickUp(Collider2D collision)
    {
        if(collision.TryGetComponent<PlayerScore>(out PlayerScore playerScore))
        {
            playerScore.UpdateScore(score);
        }
        base.PickUp(collision);
    }
}
