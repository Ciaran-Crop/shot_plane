using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] GameObject hitVFX;
    [SerializeField] float damage = 1f;
    [SerializeField] protected float moveSpeed = 10f;
    [SerializeField] protected Vector2 direction;

    protected virtual void OnEnable()
    {
        StartCoroutine(nameof(ProjectileMoveCoroutine));
    }

    void OnDisable() 
    {
        StopCoroutine(nameof(ProjectileMoveCoroutine));
    }
    
    IEnumerator ProjectileMoveCoroutine()
    {
        while(gameObject.activeSelf)
        {
            transform.Translate(direction * moveSpeed * Time.deltaTime);
            yield return null;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) 
    {
        if(collision.gameObject.TryGetComponent<HealthSystem>(out HealthSystem healthSystem))
        {
            healthSystem.TakeDamage(damage);
            // 释放命中特效
            var contactPoint = collision.GetContact(0);
            PoolManager.Release(hitVFX, contactPoint.point, Quaternion.LookRotation(contactPoint.normal));
            gameObject.SetActive(false);
        }
    }
}
