using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatSystemScore : MonoBehaviour
{
    [SerializeField] Text curScoreText;
    float t;

    float curScore;
    float addScore;

    void Awake()
    {
        if (TryGetComponent<Canvas>(out Canvas canvas))
        {
            canvas = GetComponent<Canvas>();
            canvas.worldCamera = Camera.main;
        }
    }

    public void Initialize(float score)
    {
        curScore = score;
        addScore = curScore;
        UpdateText(curScore);
    }

    void UpdateText(float score)
    {
        curScoreText.text = score.ToString();
    }

    IEnumerator AddScoreCoroutine(float score)
    {
        while (curScore < score)
        {
            curScore = Mathf.Clamp(curScore + 2, 0f, score);
            UpdateText(curScore);
            yield return null;
        }
    }

    public void UpdateStat(float score)
    {
        addScore = score;
        StartCoroutine(AddScoreCoroutine(addScore));
    }

}
