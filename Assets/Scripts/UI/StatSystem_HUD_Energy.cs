using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatSystem_HUD_Energy : StatSystem_HUD
{
    public void setBarColor(Color frontColor, Color backColor)
    {
        frontStatImage.color = frontColor;
        backStatImage.color = backColor;
    }
}
