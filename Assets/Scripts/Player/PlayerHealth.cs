using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : HealthSystem
{
    [SerializeField] bool healthRegeneration = true;
    [SerializeField] float healthRegenerationPercent = 0.2f;
    [SerializeField] float healthRegenerationInterval = 12f;
    [SerializeField] float sustainedDamageInterval = 2f;
    [SerializeField] float sustainedDamagePercent = 0.1f;
    [SerializeField] float RestoreIntervalTime = 2f;
    WaitForSeconds waitForRestoreInterval;
    WaitForSeconds healthRegenerationWaitTime;
    WaitForSeconds waitForSustainedDamageTime;
    Coroutine healthRegenerationCoroutine;
    Coroutine sustainedDamageCoroutine;

    [SerializeField] bool showHealthHUD = true;
    [SerializeField] StatSystem_HUD onHUDStatBar;

    PlayerEffect playerEffect;

    void Awake()
    {
        playerEffect = GetComponent<PlayerEffect>();
        healthRegenerationWaitTime = new WaitForSeconds(healthRegenerationInterval);
        waitForSustainedDamageTime = new WaitForSeconds(sustainedDamageInterval);
        waitForRestoreInterval = new WaitForSeconds(RestoreIntervalTime);
    }

    override protected void OnEnable()
    {
        base.OnEnable();
        if (showHealthHUD)
        {
            onHUDStatBar.Initialize(health, maxHealth);
            onHUDStatBar.gameObject.SetActive(true);
        }
        else
        {
            onHUDStatBar.gameObject.SetActive(false);
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        onHUDStatBar.UpdateStat(health, maxHealth);
    }

    public override bool TakeDamage(float damage)
    {
        bool isDead = base.TakeDamage(damage);

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
            healthRegenerationCoroutine = StartCoroutine(LifeRegenerationCoroutine(healthRegenerationWaitTime, waitForRestoreInterval, healthRegenerationPercent));
        }
        return isDead;
    }

    public override void TakeDoT(int count)
    {
        if (gameObject.activeSelf)
        {
            if (sustainedDamageCoroutine != null)
            {
                StopCoroutine(sustainedDamageCoroutine);
            }
            sustainedDamageCoroutine = StartCoroutine(SustainedDamageCoroutine(waitForSustainedDamageTime, count, sustainedDamagePercent));
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

    public override void Die()
    {
        base.Die();
        AudioManager.Instance.PlayPlayerExplosion();
        playerEffect.CancelRestore();
        playerEffect.CancelDoT();
    }

}
