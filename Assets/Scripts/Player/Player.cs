using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [SerializeField] GamePlayInput input;
    new Rigidbody2D rigidbody2D;
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float paddingX = 0.1f;
    [SerializeField] float paddingY = 0.2f;
    [SerializeField] float accelerationTime = 5f;
    [SerializeField] float decelerationTime = 5f;
    [SerializeField] float moveRotationAngle = 50;
    [SerializeField] GameObject projectile;
    [SerializeField] float fireInterval = 0.2f;
    [SerializeField] Transform muzzleUp;
    [SerializeField] Transform muzzleMiddle;
    [SerializeField] Transform muzzleBottom;
    [SerializeField] float projectileUpAngle = 0.05f;
    Quaternion projectileUpRotation;
    [SerializeField] float projectileBottomAngle = -0.05f;
    Quaternion projectileBottomRotation;
    [SerializeField, Range(1, 3)] int powerLevel = 1;
    Coroutine coroutine;
    WaitForSeconds fireWaitForSeconds;

    void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        input.onMove += Move;
        input.onStopMove += StopMove;
        input.onFire += Fire;
        input.onStopFire += StopFire;
    }

    void OnDisable()
    {
        input.onMove -= Move;
        input.onStopMove -= StopMove;
        input.onFire -= Fire;
        input.onStopFire -= StopFire;
    }

    void Start()
    {
        rigidbody2D.gravityScale = 0f;
        fireWaitForSeconds = new WaitForSeconds(fireInterval);
        projectileUpRotation = Quaternion.AngleAxis(projectileUpAngle, transform.forward);
        projectileBottomRotation = Quaternion.AngleAxis(projectileBottomAngle, transform.forward);
        input.EnableGamePlayInput();
    }

    // Update is called once per frame
    void Update()
    {

    }

    # region Move

    void Move(Vector2 moveInput)
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        Quaternion rotationTarget = Quaternion.AngleAxis(moveRotationAngle * moveInput.y, transform.right);
        coroutine = StartCoroutine(StartMoveCoroutine(accelerationTime, moveInput.normalized * moveSpeed, rotationTarget));
        StartCoroutine(MovePositionLimitCoroutine());
    }

    void StopMove()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        coroutine = StartCoroutine(StartMoveCoroutine(decelerationTime, Vector2.zero, Quaternion.identity));
        StopCoroutine(MovePositionLimitCoroutine());
    }

    IEnumerator StartMoveCoroutine(float time, Vector2 moveVelocity, Quaternion rotationTarget)
    {
        float t = 0f;
        while (t < 1f)
        {
            t += Time.fixedDeltaTime / time;
            rigidbody2D.velocity = Vector2.Lerp(rigidbody2D.velocity, moveVelocity, t);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotationTarget, t);
            yield return null;
        }
    }

    IEnumerator MovePositionLimitCoroutine()
    {
        while (true)
        {
            transform.position = ViewPort.Instance.PlayerEnablePosition(
                transform.position,
                paddingX,
                paddingY
            );
            yield return null;
        }
    }

    # endregion


    # region Fire
    void Fire()
    {
        StartCoroutine(nameof(StartFire));
    }

    void StopFire()
    {
        StopCoroutine(nameof(StartFire));
    }

    IEnumerator StartFire()
    {
        while (true)
        {
            switch (powerLevel)
            {
                case 1:
                    PoolManager.Release(projectile, muzzleMiddle.position, Quaternion.identity);
                    break;
                case 2:
                    PoolManager.Release(projectile, muzzleUp.position, projectileUpRotation);
                    PoolManager.Release(projectile, muzzleBottom.position, projectileBottomRotation);
                    break;
                case 3:
                    PoolManager.Release(projectile, muzzleUp.position, projectileUpRotation);
                    PoolManager.Release(projectile, muzzleMiddle.position, Quaternion.identity);
                    PoolManager.Release(projectile, muzzleBottom.position, projectileBottomRotation);
                    break;
                default:
                    break;
            }
            yield return fireWaitForSeconds;
        }
    }
    # endregion
}
