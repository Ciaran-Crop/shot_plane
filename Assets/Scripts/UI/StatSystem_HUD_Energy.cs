using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatSystem_HUD_Energy : StatSystem_HUD
{
    [SerializeField] Text titleText;
    public void setBarColor(Color frontColor, Color backColor)
    {
        frontStatImage.color = frontColor;
        backStatImage.color = backColor;
        textFillPercent.color = frontColor;
        titleText.color = frontColor;
    }
}
