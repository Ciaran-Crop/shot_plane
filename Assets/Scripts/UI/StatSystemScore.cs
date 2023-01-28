using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatSystemScore : MonoBehaviour
{
    [SerializeField] Text curScoreText;
    [SerializeField] float delayTime = 1f;
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
        t = 0f;
        while (t <= 1f)
        {
            t += Time.deltaTime / delayTime;
            curScore = Mathf.FloorToInt(Mathf.Clamp(Mathf.Lerp(curScore, score, t), 0f, score));
            UpdateText(curScore);
            yield return null;
        }
        if(curScore != score) curScore = score;
    }

    public void UpdateStat(float score)
    {
        addScore = score;
        StartCoroutine(AddScoreCoroutine(addScore));
    }

}
