using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] GamePlayInput input;
    new Rigidbody2D rigidbody2D;
    new Collider2D collider2D;
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

    bool isDodging = false;
    [SerializeField] int dodgeCost = 25;
    [SerializeField] float maxRoll = 720f;
    [SerializeField] float rollSpeed = 360f;
    [SerializeField] Vector3 dodgeScale;
    float curRoll;
    float dodgeDuration;
    float t;

    PlayerEnergy playerEnergy;

    WaitForSeconds waitForStraight;
    [SerializeField] float waitForStraightTime = 0.5f;

    void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        collider2D = GetComponent<Collider2D>();
        playerEnergy = GetComponent<PlayerEnergy>();
        curRoll = 0;
        dodgeDuration = maxRoll / rollSpeed;
        waitForStraight = new WaitForSeconds(waitForStraightTime);
    }

    void OnEnable()
    {
        input.onMove += Move;
        input.onStopMove += StopMove;
        input.onFire += Fire;
        input.onStopFire += StopFire;
        input.onDodge += Dodge;
    }

    void OnDisable()
    {
        input.onMove -= Move;
        input.onStopMove -= StopMove;
        input.onFire -= Fire;
        input.onStopFire -= StopFire;
        input.onDodge -= Dodge;
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
        StartCoroutine(nameof(MovePositionLimitCoroutine));
    }

    void StopMove()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        coroutine = StartCoroutine(StartMoveCoroutine(decelerationTime, Vector2.zero, Quaternion.identity));
        StopCoroutine(nameof(MovePositionLimitCoroutine));
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
                    GameObject p1 = PoolManager.Release(projectile, muzzleMiddle.position, Quaternion.identity);
                    p1.GetComponent<Projectile>().setLauncher(gameObject);
                    break;
                case 2:
                    GameObject p2 = PoolManager.Release(projectile, muzzleUp.position, projectileUpRotation);
                    GameObject p3 = PoolManager.Release(projectile, muzzleBottom.position, projectileBottomRotation);
                    p2.GetComponent<Projectile>().setLauncher(gameObject);
                    p3.GetComponent<Projectile>().setLauncher(gameObject);
                    break;
                case 3:
                    GameObject p4 = PoolManager.Release(projectile, muzzleUp.position, projectileUpRotation);
                    GameObject p5 = PoolManager.Release(projectile, muzzleMiddle.position, Quaternion.identity);
                    GameObject p6 = PoolManager.Release(projectile, muzzleBottom.position, projectileBottomRotation);
                    p4.GetComponent<Projectile>().setLauncher(gameObject);
                    p5.GetComponent<Projectile>().setLauncher(gameObject);
                    p6.GetComponent<Projectile>().setLauncher(gameObject);
                    break;
                default:
                    break;
            }
            AudioManager.Instance.PlayPlayerProjectileLaunch(true);
            yield return fireWaitForSeconds;
        }
    }
    # endregion

    # region Dodge

    void Dodge()
    {
        if (isDodging || !playerEnergy.isEnough(dodgeCost)) return;
        StartCoroutine(nameof(StartDodgeCoroutine));
    }

    IEnumerator StartDodgeCoroutine()
    {
        AudioManager.Instance.PlayPlayerDodge();
        curRoll = 0f;
        t = 0f;
        isDodging = true;
        collider2D.isTrigger = true;
        playerEnergy.Use(dodgeCost);
        while (curRoll < maxRoll)
        {
            curRoll += Time.deltaTime * rollSpeed;
            transform.rotation = Quaternion.AngleAxis(curRoll, transform.right);
            t += Time.deltaTime / dodgeDuration;
            transform.localScale = BezierCurve.Instance.QuadraticBezierCurve(Vector3.one, dodgeScale, Vector3.one, t);
            yield return null;
        }
        yield return waitForStraight;
        collider2D.isTrigger = false;
        isDodging = false;
    }

    # endregion
}
