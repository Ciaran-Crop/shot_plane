using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : HealthSystem
{
    [SerializeField] bool healthRegeneration = true;
    [SerializeField] float healthRegenerationPercent = 0.2f;
    [SerializeField] float healthRegenerationInterval = 12f;
    WaitForSeconds healthRegenerationWaitTime;
    Coroutine healthRegenerationCoroutine;

    [SerializeField] bool showHealthHUD = true;
    [SerializeField] StatSystem_HUD onHUDStatBar;

    void Awake()
    {
        healthRegenerationWaitTime = new WaitForSeconds(healthRegenerationInterval);
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);

        if (showHealthHUD)
        {
            onHUDStatBar.UpdateStat(health, maxHealth);
        }

        if (gameObject.activeSelf && healthRegeneration)
        {
            if (healthRegenerationCoroutine != null)
            {
                StopCoroutine(healthRegenerationCoroutine);
            }
            healthRegenerationCoroutine = StartCoroutine(LifeRegenerationCoroutine(healthRegenerationWaitTime, healthRegenerationPercent));
        }
    }

    public override void RestoreHealth(float restoreHealth)
    {
        base.RestoreHealth(restoreHealth);

        if (showHealthHUD)
        {
            onHUDStatBar.UpdateStat(health, maxHealth);
        }
    }

}
