using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserDamage : MonoBehaviour
{
    [SerializeField] float damage;
    [SerializeField] GameObject hitVFX;
    [SerializeField] AudioData hitSFX;

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<HealthSystem>(out HealthSystem healthSystem))
        {
            if (collision.gameObject.activeSelf)
            {
                healthSystem.TakeDamage(damage);
                var contactPoint = collision.GetContact(0);
                PoolManager.Release(hitVFX, contactPoint.point, Quaternion.LookRotation(contactPoint.normal)).SetActive(true);
                AudioManager.Instance.PlayAudio(hitSFX);
            }
        }
    }
}
