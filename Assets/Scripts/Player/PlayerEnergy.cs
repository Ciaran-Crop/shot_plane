using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnergy : MonoBehaviour
{
    [SerializeField] StatSystem_HUD_Energy energyStatBar;
    const int MAX_ENERGY = 100;
    public const int PERCENT = 1;
    
    int energy;

    // Start is called before the first frame update
    void Start()
    {
        energy = 0;
        energyStatBar.Initialize(energy, MAX_ENERGY);
    }

    public void Use(int value)
    {
        if(isEnough(value))
        {
            energy -= value;
            energyStatBar.UpdateStat(energy, MAX_ENERGY);
        }
    }

    public void Obtain(int value)
    {
        energy = Mathf.Clamp(energy + value, 0, MAX_ENERGY);
        energyStatBar.UpdateStat(energy, MAX_ENERGY);
    }

    public bool isEnough(int useValue) => energy >= useValue;

    // Update is called once per frame
    void Update()
    {
        
    }
}
