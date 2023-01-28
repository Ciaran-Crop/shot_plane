using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    [SerializeField] StatSystemScore statSystemScore;

    float score;


    void Start()
    {
        statSystemScore.Initialize(score);
    }

    public void UpdateScore(float value)
    {
        score += value;
        statSystemScore.UpdateStat(score);
        GameManager.GameScore = score;
    }
}
