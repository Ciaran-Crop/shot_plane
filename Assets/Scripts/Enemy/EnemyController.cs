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
    [SerializeField, Range(1, 3)] float powerLevel = 1f;

    [SerializeField] Transform muzzleUp;
    [SerializeField] Transform muzzleMiddle;
    [SerializeField] Transform muzzleBottom;

    [SerializeField] float enemyProjectileUpAngle = 2f;
    Quaternion enemyProjectileUpRotation;
    [SerializeField] float enemyProjectileBottomAngle = -2f;
    Quaternion enemyProjectileBottomRotation;
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

            yield return null;
        }
    }

    IEnumerator RandomFireCoroutine()
    {
        while (gameObject.activeSelf)
        {
            yield return new WaitForSeconds(Random.Range(minFireInterval, maxFireInterval));
            switch (powerLevel)
            {
                case 1:
                    PoolManager.Release(enemyProjectile, muzzleMiddle.position, Quaternion.identity);
                    break;
                case 2:
                    PoolManager.Release(enemyProjectile, muzzleUp.position, enemyProjectileUpRotation);
                    PoolManager.Release(enemyProjectile, muzzleBottom.position, enemyProjectileBottomRotation);
                    break;
                case 3:
                    PoolManager.Release(enemyProjectile, muzzleUp.position, enemyProjectileUpRotation);
                    PoolManager.Release(enemyProjectile, muzzleMiddle.position, Quaternion.identity);
                    PoolManager.Release(enemyProjectile, muzzleBottom.position, enemyProjectileBottomRotation);
                    break;
                default:
                    break;
            }
        }
    }

    void Start()
    {
        transform.position = ViewPort.Instance.RandomEnemyCreatePosition(paddingX, paddingY);
        enemyProjectileBottomRotation = Quaternion.AngleAxis(enemyProjectileBottomAngle, transform.forward);
        enemyProjectileUpRotation = Quaternion.AngleAxis(enemyProjectileUpAngle, transform.forward);
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }

    void OnEnable()
    {
        StartCoroutine(nameof(RandomMoveCoroutine));
        StartCoroutine(nameof(RandomFireCoroutine));
    }

    void Awake()
    {
    }

}
