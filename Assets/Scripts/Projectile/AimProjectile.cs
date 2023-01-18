using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimProjectile : Projectile
{
    [SerializeField] string targetName;
    GameObject target;
    // Start is called before the first frame update
    void Start()
    {

    }

    void Awake()
    {
        target = GameObject.FindGameObjectWithTag(targetName);
    }

    IEnumerator TrackTargetCoroutine()
    {
        yield return null;
        if (gameObject.activeSelf)
        {
            direction = (target.transform.position - transform.position).normalized;
        }
    }

    protected override void OnEnable()
    {
        StartCoroutine(nameof(TrackTargetCoroutine));
        base.OnEnable();
    }

    // Update is called once per frame
    void Update()
    {

    }
    protected override bool OnCollisionEnter2D(Collision2D collision)
    {
        bool collisionResult = base.OnCollisionEnter2D(collision);
        if (collisionResult)
        {
            AudioManager.Instance.PlayEnemyProjectileHit2();
        }
        return collisionResult;
    }
}
