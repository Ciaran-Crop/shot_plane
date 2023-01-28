using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] GameObject enemyProjectile;
    [SerializeField] float minFireInterval = 0f;
    [SerializeField] float maxFireInterval = 1f;
    [SerializeField] float moveSpeed = 15f;

    [SerializeField] float moveRotationAngle = 25f;
    [SerializeField] float paddingX = 0.2f;
    [SerializeField] float paddingY = 0.2f;
    [SerializeField, Range(1, 3)] int powerLevel = 1;

    [SerializeField] Transform muzzleUp;
    [SerializeField] Transform muzzleMiddle;
    [SerializeField] Transform muzzleBottom;
    [SerializeField] float collisionDamage = 1f;
    [SerializeField] bool hasCollisionDamage = false;

    [SerializeField] float enemyProjectileUpAngle = 2f;
    Quaternion enemyProjectileUpRotation;
    [SerializeField] float enemyProjectileBottomAngle = -2f;
    [SerializeField] ParticleSystem fireVFX;
    Quaternion enemyProjectileBottomRotation;
    HealthSystem healthSystem;

    EnemyMoveState moveState = EnemyMoveState.Random;

    public void ChangeMoveState(EnemyMoveState state) => moveState = state;

    IEnumerator RandomMoveCoroutine()
    {
        transform.position = ViewPort.Instance.RandomEnemyCreatePosition(paddingX, paddingY);
        Vector3 moveTo = ViewPort.Instance.RandomEnemyMoveRightPosition(paddingX, paddingY);
        while (gameObject.activeSelf)
        {
            // if (gameObject.name == "Enemy01")
            // {
            //     Debug.Log(string.Format("{0} move to {1} but now : {2}, distance: {3}"
            // , gameObject.tag, moveTo, transform.position, Vector3.Distance(transform.position, moveTo)));
            // }
            if (moveState != EnemyMoveState.Random) yield return null;
            if (Vector3.Distance(transform.position, moveTo) >= Time.deltaTime * moveSpeed)
            {
                transform.position = Vector3.MoveTowards(transform.position, moveTo, Time.deltaTime * moveSpeed);
                transform.rotation = Quaternion.AngleAxis(
                    (moveTo - transform.position).normalized.y * moveRotationAngle,
                    transform.right
                );
            }
            else
            {
                moveTo = ViewPort.Instance.RandomEnemyMoveRightPosition(paddingX, paddingY);
            }

            if (GameManager.IsGameOver) yield break;

            yield return null;
        }
    }

    IEnumerator RandomFireCoroutine()
    {
        while (gameObject.activeSelf)
        {
            yield return new WaitForSeconds(Random.Range(minFireInterval, maxFireInterval));
            if (GameManager.IsGameOver) yield break;
            fireVFX.Play();
            switch (powerLevel)
            {
                case 1:
                    GameObject p1 = PoolManager.Release(enemyProjectile, muzzleMiddle.position, Quaternion.identity, false);
                    p1.GetComponent<Projectile>().setLauncher(gameObject);
                    p1.SetActive(true);
                    break;
                case 2:
                    GameObject p2 = PoolManager.Release(enemyProjectile, muzzleUp.position, enemyProjectileUpRotation, false);
                    GameObject p3 = PoolManager.Release(enemyProjectile, muzzleBottom.position, enemyProjectileBottomRotation, false);
                    p2.GetComponent<Projectile>().setLauncher(gameObject);
                    p2.SetActive(true);
                    p3.GetComponent<Projectile>().setLauncher(gameObject);
                    p3.SetActive(true);
                    break;
                case 3:
                    GameObject p4 = PoolManager.Release(enemyProjectile, muzzleUp.position, enemyProjectileUpRotation, false);
                    GameObject p5 = PoolManager.Release(enemyProjectile, muzzleMiddle.position, Quaternion.identity, false);
                    GameObject p6 = PoolManager.Release(enemyProjectile, muzzleBottom.position, enemyProjectileBottomRotation, false);
                    p4.GetComponent<Projectile>().setLauncher(gameObject);
                    p4.SetActive(true);
                    p5.GetComponent<Projectile>().setLauncher(gameObject);
                    p5.SetActive(true);
                    p6.GetComponent<Projectile>().setLauncher(gameObject);
                    p6.SetActive(true);
                    break;
                default:
                    break;
            }
            fireVFX.Stop();
            AudioManager.Instance.PlayRandomEnemyLaunch();
        }
    }

    void Start()
    {
        transform.position = ViewPort.Instance.RandomEnemyCreatePosition(paddingX, paddingY);
        enemyProjectileBottomRotation = Quaternion.AngleAxis(enemyProjectileBottomAngle, transform.forward);
        enemyProjectileUpRotation = Quaternion.AngleAxis(enemyProjectileUpAngle, transform.forward);
        healthSystem = GetComponent<HealthSystem>();
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }

    void OnEnable()
    {
        fireVFX.Stop();
        StartCoroutine(nameof(RandomMoveCoroutine));
        StartCoroutine(nameof(RandomFireCoroutine));
    }

    void Awake()
    {
    }

    internal void setPowerLevel(int power)
    {
        powerLevel = power;
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (hasCollisionDamage)
        {
            if (collision.gameObject.TryGetComponent<HealthSystem>(out HealthSystem collisionHealthSystem))
            {
                if (collision.gameObject.activeSelf)
                {
                    collisionHealthSystem.TakeDamage(collisionDamage);
                    healthSystem.TakeDamage(collisionDamage);
                    AudioManager.Instance.PlayEnemyProjectileHit1();
                }
            }
        }
    }

    public void OpenCollisionDamage() => hasCollisionDamage = true;
    public void CloseCollisionDamage() => hasCollisionDamage = false;

}

public enum EnemyMoveState
{
    Random,
    Y,
    X,
}