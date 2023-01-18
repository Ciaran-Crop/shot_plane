using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] public int deathEnergyRewards = 1;
    [SerializeField] GameObject deathVFX;
    [SerializeField] protected float maxHealth;
    protected float health;

    [SerializeField] StatSystem onHeadStatBar;
    [SerializeField] bool showOnHeadStatBar = true;

    void Start()
    {

    }

    public void ShowOnHeadStatBar()
    {
        onHeadStatBar.gameObject.SetActive(true);
        onHeadStatBar.Initialize(health, maxHealth);
    }

    public void HideOnHeadStatBar()
    {
        onHeadStatBar.gameObject.SetActive(false);
    }

    protected virtual void OnEnable()
    {
        health = maxHealth;

        if (showOnHeadStatBar)
        {
            ShowOnHeadStatBar();
        }
        else
        {
            HideOnHeadStatBar();
        }
    }

    protected virtual void OnDisable()
    {
        health = 0f;
        StopAllCoroutines();
    }

    public virtual bool TakeDamage(float damage)
    {
        health = Mathf.Clamp(health - damage, 0f, maxHealth);

        if (showOnHeadStatBar)
        {
            onHeadStatBar.UpdateStat(health, maxHealth);
        }

        if (health <= 0f)
        {
            Die();
            return true;
        }
        return false;
    }

    public virtual void Die()
    {
        health = 0f;
        if (deathVFX)
        {
            PoolManager.Release(deathVFX, transform.position);
        }
        gameObject.SetActive(false);
    }

    public virtual void RestoreHealth(float restoreHealth)
    {
        health = Mathf.Clamp(health + restoreHealth, 0f, maxHealth);
        if (showOnHeadStatBar)
        {
            onHeadStatBar.UpdateStat(health, maxHealth);
        }
    }

    protected IEnumerator LifeRegenerationCoroutine(WaitForSeconds waitTime, float percent)
    {
        while (health < maxHealth)
        {
            yield return waitTime;
            RestoreHealth(maxHealth * percent);
        }
    }

    protected IEnumerator SustainedDamageCoroutine(WaitForSeconds waitTime, float percent)
    {
        while (health > 0f)
        {
            yield return waitTime;
            TakeDamage(maxHealth * percent);
        }
    }

    internal void setDer(int der)
    {
        deathEnergyRewards = der;
    }

    internal void setMaxHealth(int maxHealth)
    {
        this.maxHealth = maxHealth;
    }
}
