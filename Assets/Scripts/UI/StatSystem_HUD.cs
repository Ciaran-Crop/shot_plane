using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatSystem_HUD : StatSystem
{
    [SerializeField] Text textFillPercent;

    public void UpdateText(float targetFillAmount)
    {
        textFillPercent.text = Mathf.FloorToInt(targetFillAmount * 100f) + "%";
    }

    public override void Initialize(float curStat, float maxStat)
    {
        base.Initialize(curStat, maxStat);
        UpdateText(targetFillAmount);
    }

    protected override IEnumerator BufferedStatCoroutine(Image image)
    {
        UpdateText(targetFillAmount);
        return base.BufferedStatCoroutine(image);
    }
}
