using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Stat_HUD : StatSystem_HUD
{
    public override void UpdateText(float targetFillAmount)
    {
        textFillPercent.text = (targetFillAmount * 100f).ToString("F2") + "%";
    }
}
