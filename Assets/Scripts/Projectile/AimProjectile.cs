using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimProjectile : Projectile
{
    [SerializeField] string targetName;
    [SerializeField, Range(0, 1)] float doTProbability;
    [SerializeField] int minDoTCount = 1;
    [SerializeField] int maxDoTCount = 3;   
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
            if(Random.Range(0f, 1f) <= doTProbability)
            {
                if (collision.gameObject.TryGetComponent<HealthSystem>(out HealthSystem healthSystem))
                {
                    healthSystem.TakeDoT(Random.Range(minDoTCount, maxDoTCount + 1));
                }
            }
        }
        return collisionResult;
    }
}
