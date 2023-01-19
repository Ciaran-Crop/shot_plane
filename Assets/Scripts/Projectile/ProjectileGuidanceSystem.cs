using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileGuidanceSystem : MonoBehaviour
{
    Vector3 toTargetVector;
    [SerializeField] Projectile projectile;
    [SerializeField] float minEulerAngle = -50f;
    [SerializeField] float maxEulerAngle = 50f;
    float randomEulerAngle;
    public virtual IEnumerator HomingCoroutine(GameObject target)
    {
        randomEulerAngle = Random.Range(minEulerAngle, maxEulerAngle);
        while (gameObject.activeSelf)
        {
            if (target.activeSelf)
            {
                toTargetVector= target.transform.position - transform.position;
                transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(toTargetVector.y, toTargetVector.x) * Mathf.Rad2Deg, transform.forward);
                transform.rotation *= Quaternion.Euler(0f, 0f, randomEulerAngle);
            }
            projectile.Move();
            yield return null;
        }
    }
}
