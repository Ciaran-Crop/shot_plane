using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnergy : MonoBehaviour
{
    [SerializeField] StatSystem_HUD_Energy energyStatBar;
    [SerializeField] float overdriveTime = 0.1f;
    [SerializeField] int overdriveCost = 2;
    [SerializeField] Color defaultFrontImageColor;
    [SerializeField] Color defaultBackImageColor;
    [SerializeField] Color fullFrontImageColor;
    [SerializeField] Color fullBackImageColor;
    WaitForSeconds waitForOverdrive;
    public const int MAX_ENERGY = 100;
    public const int PERCENT = 1;

    int energy;
    bool canObtain = true;

    void Awake()
    {
        waitForOverdrive = new WaitForSeconds(overdriveTime);
    }

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
            energy -= value;
            energyStatBar.UpdateStat(energy, MAX_ENERGY);
        }

        if (canObtain)
        {
            NotFullState();
        }

        if (energy == 0 && !canObtain)
        {
            PlayerOverdrive.off.Invoke();
        }
    }

    void OnEnable()
    {
        PlayerOverdrive.on += OverdriveOn;
        PlayerOverdrive.off += OverdriveOff;
    }

    void OnDisable()
    {
        PlayerOverdrive.on -= OverdriveOn;
        PlayerOverdrive.off -= OverdriveOff;
    }

    public void Obtain(int value)
    {
        if (!canObtain) return;
        energy = Mathf.Clamp(energy + value, 0, MAX_ENERGY);
        energyStatBar.UpdateStat(energy, MAX_ENERGY);
        if (energy == MAX_ENERGY)
        {
            FullState();
        }
    }

    void FullState()
    {
        energyStatBar.setBarColor(fullFrontImageColor, fullBackImageColor);
    }

    void NotFullState()
    {
        energyStatBar.setBarColor(defaultFrontImageColor, defaultBackImageColor);
    }

    public bool isEnough(int useValue) => energy >= useValue;

    IEnumerator OverdriveEnergyCoroutine()
    {
        while (energy >= overdriveCost)
        {
            Use(overdriveCost);
            yield return waitForOverdrive;
        }
    }

    void OverdriveOn()
    {
        canObtain = false;
        StartCoroutine(nameof(OverdriveEnergyCoroutine));
    }

    void OverdriveOff()
    {
        canObtain = true;
        StopCoroutine(nameof(OverdriveEnergyCoroutine));
    }
}
