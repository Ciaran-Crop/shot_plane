using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMissile : PlayerOverdriveProjectile
{
    [SerializeField] float lowSpeed = 8f;
    [SerializeField] float maxSpeed = 30f;

    [SerializeField] float speedChangeTime = 0.5f;
    [SerializeField] float explosionDamage = 5f;
    [SerializeField] float explosionRadius = 10f;
    [SerializeField] float explosionAttenuationFactor = 1f;
    [SerializeField] AudioData explosionAudioData;
    [SerializeField] AudioData targetAcquired;
    [SerializeField] LayerMask layerMask = default;
    WaitForSeconds waitForSpeedChange;
    Vector3 distanceInVector3;

    float GetDistanceExplosionDamage(float distance)
    {
        return Mathf.Clamp(explosionDamage - explosionAttenuationFactor * distance, 0f, explosionDamage);
    }

    IEnumerator SpeedChangeCoroutine()
    {
        moveSpeed = lowSpeed;
        yield return waitForSpeedChange;
        moveSpeed = maxSpeed;
    }

    protected override void Awake()
    {
        base.Awake();
        waitForSpeedChange = new WaitForSeconds(speedChangeTime);
    }

    protected override void OnEnable()
    {
        StartCoroutine(nameof(SpeedChangeCoroutine));
        base.OnEnable();
        if (target != null)
        {
            AudioManager.Instance.PlayRandomPitch(targetAcquired);
        }
    }

    // Explosion

    // # if UNITY_EDITOR
    // void OnDrawGizmosSelected()
    // {
    //     Gizmos.color = Color.yellow;
    //     Gizmos.DrawSphere(transform.position, explosionRadius);
    // }
    // # endif
    protected override bool OnCollisionEnter2D(Collision2D collision)
    {
        bool collisionResult = base.OnCollisionEnter2D(collision);
        if (collisionResult)
        {
            AudioManager.Instance.PlayRandomPitch(explosionAudioData);
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius, layerMask);
            foreach (var collider in colliders)
            {
                if (collider.gameObject.activeSelf && collider.TryGetComponent<HealthSystem>(out HealthSystem healthSystem))
                {
                    float distance = Vector3.Distance(collider.transform.position, transform.position);
                    healthSystem.TakeDamage(GetDistanceExplosionDamage(distance));
                }
            }
        }
        return collisionResult;
    }
}
