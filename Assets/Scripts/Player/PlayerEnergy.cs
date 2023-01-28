using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnergy : MonoBehaviour
{
    [SerializeField] StatSystem_HUD_Energy energyStatBar;

    [SerializeField] Color defaultFrontImageColor;
    [SerializeField] Color defaultBackImageColor;
    [SerializeField] Color fullFrontImageColor;
    [SerializeField] Color fullBackImageColor;
    public const int MAX_ENERGY = 100;
    public const int PERCENT = 1;
    int energy;
    bool canObtain = true;
    public bool IsFull => energy == MAX_ENERGY;

    // Start is called before the first frame update
    void Start()
    {
        energy = 0;
        energyStatBar.Initialize(energy, MAX_ENERGY);
    }

    public void Use(int value)
    {
        if (isEnough(value))
        {
            if(canObtain)
            {
                NotFullState();
            }
            energy -= value;
            energyStatBar.UpdateStat(energy, MAX_ENERGY);
        }
    }

    public void Obtain(int value)
    {
        if (!canObtain) return;
        energy = Mathf.Clamp(energy + value, 0, MAX_ENERGY);
        energyStatBar.UpdateStat(energy, MAX_ENERGY);
        if (IsFull)
        {
            FullState();
        }
    }

    public void FullState() => energyStatBar.setBarColor(fullFrontImageColor, fullBackImageColor);

    public void NotFullState() => energyStatBar.setBarColor(defaultFrontImageColor, defaultBackImageColor);

    public bool isEnough(int useValue) => energy >= useValue;
    
    public void SetUseObtain(bool state) => canObtain = state;
}
