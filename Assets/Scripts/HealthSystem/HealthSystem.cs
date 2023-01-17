using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] GameObject deathVFX;
    [SerializeField] protected float maxHealth;
    protected float health;

    void Start()
    {

    }

    protected virtual void OnEnable()
    {
        health = maxHealth;
    }

    public virtual void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0f)
        {
            Die();
        }
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
}
