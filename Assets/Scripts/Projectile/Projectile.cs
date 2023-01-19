using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] GameObject hitVFX;
    [SerializeField] float damage = 1f;
    [SerializeField] protected float moveSpeed = 10f;
    [SerializeField] protected Vector2 direction;
    GameObject launcher;

    protected virtual void OnEnable()
    {
        StartCoroutine(nameof(ProjectileMoveCoroutine));
    }

    void OnDisable()
    {
        launcher = null;
        StopCoroutine(nameof(ProjectileMoveCoroutine));
    }

    public void setLauncher(GameObject newLauncher)
    {
        if (launcher == null)
        {
            launcher = newLauncher;
        }
    }

    IEnumerator ProjectileMoveCoroutine()
    {
        while (gameObject.activeSelf)
        {
            transform.Translate(direction * moveSpeed * Time.deltaTime);
            yield return null;
        }
    }

    protected virtual bool OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<HealthSystem>(out HealthSystem healthSystem))
        {
            if (collision.gameObject.activeSelf)
            {
                bool isDead = healthSystem.TakeDamage(damage);
                if (launcher.gameObject.tag == "Player")
                {
                    PlayerEnergy playerEnergy = launcher.GetComponent<PlayerEnergy>();
                    PlayerEP playerEP = launcher.GetComponent<PlayerEP>();
                    playerEnergy.Obtain(PlayerEnergy.PERCENT);
                    playerEP.Obtain(PlayerEP.PERCENT);
                    if (isDead)
                    {
                        playerEnergy.Obtain(healthSystem.deathEnergyRewards);
                        playerEP.Obtain(healthSystem.deathEnergyRewards);
                    }
                }
                // 释放命中特效
                var contactPoint = collision.GetContact(0);
                PoolManager.Release(hitVFX, contactPoint.point, Quaternion.LookRotation(contactPoint.normal));
                gameObject.SetActive(false);
                return true;
            }
        }
        return false;
    }
}
