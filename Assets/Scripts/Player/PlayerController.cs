using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] GamePlayInput input;
    new Rigidbody2D rigidbody2D;
    new Collider2D collider2D;
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float accelerationTime = 5f;
    [SerializeField] float decelerationTime = 5f;
    [SerializeField] float moveRotationAngle = 50;
    [SerializeField] GameObject projectile;
    [SerializeField] float fireInterval = 0.2f;
    [SerializeField] Transform muzzleUp;
    [SerializeField] Transform muzzleMiddle;
    [SerializeField] Transform muzzleBottom;
    [SerializeField] float projectileUpAngle = 0.05f;
    [SerializeField] Text weaponLevelText;
    Quaternion projectileUpRotation;
    [SerializeField] float projectileBottomAngle = -0.05f;
    Quaternion projectileBottomRotation;

    public int PowerLevel => powerLevel;
    [SerializeField, Range(1, 3)] int powerLevel = 1;
    Coroutine coroutine;
    WaitForSeconds fireWaitForSeconds;
    Vector2 velocity;
    Quaternion rotation;
    WaitForFixedUpdate waitForFixedUpdate;

    bool isDodging = false;
    [SerializeField] int dodgeCost = 25;
    [SerializeField] float maxRoll = 720f;
    [SerializeField] float rollSpeed = 360f;
    [SerializeField] Vector3 dodgeScale;
    float curRoll;
    float dodgeDuration;
    float t;
    float moveT;

    PlayerEnergy playerEnergy;

    WaitForSeconds waitForStraight;
    [SerializeField] float waitForStraightTime = 0.5f;

    bool isOverdrive = false;

    int dodgeCostFactor = 2;

    float moveSpeedFactor = 1.2f;
    float shootSpeedFactor = 1.2f;
    WaitForSeconds waitForShootOverdrive;

    [SerializeField] GameObject playerProjectileOverdrive;

    float paddingX;
    float paddingY;

    [SerializeField] public float bulletTime = 0.5f;
    [SerializeField] public float fadeInTime = 0.5f;
    [SerializeField] public float keepTime = 1f;
    [SerializeField] public float fadeOutTime = 0.5f;

    void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        collider2D = GetComponent<Collider2D>();
        playerEnergy = GetComponent<PlayerEnergy>();
        var size = transform.GetChild(0).GetComponent<Renderer>().bounds.size;
        paddingX = size.x;
        paddingY = size.y;
        curRoll = 0;
        dodgeDuration = maxRoll / rollSpeed;
        waitForStraight = new WaitForSeconds(waitForStraightTime);
        waitForFixedUpdate = new WaitForFixedUpdate();
        waitForShootOverdrive = new WaitForSeconds(shootSpeedFactor * fireInterval);
        ChangePowerLevel(powerLevel);
    }

    public void SetWaitForStraightSub(float percent)
    {
        waitForStraight = new WaitForSeconds(Mathf.Max(waitForStraightTime * (1 - percent), 0.1f));
    }


    void OnEnable()
    {
        input.onMove += Move;
        input.onStopMove += StopMove;
        input.onFire += Fire;
        input.onStopFire += StopFire;
        input.onDodge += Dodge;
        input.onOverdrive += Overdrive;
        PlayerOverdrive.on += OverdriveOn;
        PlayerOverdrive.off += OverdriveOff;
    }

    void OnDisable()
    {
        input.onMove -= Move;
        input.onStopMove -= StopMove;
        input.onFire -= Fire;
        input.onStopFire -= StopFire;
        input.onDodge -= Dodge;
        input.onOverdrive -= Overdrive;
        PlayerOverdrive.on -= OverdriveOn;
        PlayerOverdrive.off -= OverdriveOff;
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
        transform.position = ViewPort.Instance.PlayerEnablePosition(
            transform.position,
            paddingX,
            paddingY
        );
    }

    # region Move

    void Move(Vector2 moveInput)
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        float moveSpeedX = isOverdrive ? moveSpeed * moveSpeedFactor : moveSpeed;
        Quaternion rotationTarget = Quaternion.AngleAxis(moveRotationAngle * moveInput.y, transform.right);
        coroutine = StartCoroutine(StartMoveCoroutine(accelerationTime, moveInput.normalized * moveSpeedX, rotationTarget));
    }

    void StopMove()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        coroutine = StartCoroutine(StartMoveCoroutine(decelerationTime, Vector2.zero, Quaternion.identity));
    }

    IEnumerator StartMoveCoroutine(float time, Vector2 moveVelocity, Quaternion rotationTarget)
    {
        moveT = 0f;
        velocity = rigidbody2D.velocity;
        rotation = transform.rotation;
        while (moveT < 1f)
        {
            moveT += Time.fixedDeltaTime / time;
            rigidbody2D.velocity = Vector2.Lerp(velocity, moveVelocity, moveT);
            transform.rotation = Quaternion.Lerp(rotation, rotationTarget, moveT);
            yield return waitForFixedUpdate;
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

    GameObject GetProjectile()
    {
        return isOverdrive ? playerProjectileOverdrive : projectile;
    }

    IEnumerator StartFire()
    {
        while (true)
        {
            {
                switch (powerLevel)
                {
                    case 1:
                        GameObject p1 = PoolManager.Release(GetProjectile(), muzzleMiddle.position, Quaternion.identity, false);
                        p1.GetComponent<Projectile>().setLauncher(gameObject);
                        p1.SetActive(true);
                        break;
                    case 2:
                        GameObject p2 = PoolManager.Release(GetProjectile(), muzzleUp.position, projectileUpRotation, false);
                        GameObject p3 = PoolManager.Release(GetProjectile(), muzzleBottom.position, projectileBottomRotation, false);
                        p2.GetComponent<Projectile>().setLauncher(gameObject);
                        p2.SetActive(true);
                        p3.GetComponent<Projectile>().setLauncher(gameObject);
                        p3.SetActive(true);
                        break;
                    case 3:
                        GameObject p4 = PoolManager.Release(GetProjectile(), muzzleUp.position, projectileUpRotation, false);
                        GameObject p5 = PoolManager.Release(GetProjectile(), muzzleMiddle.position, Quaternion.identity, false);
                        GameObject p6 = PoolManager.Release(GetProjectile(), muzzleBottom.position, projectileBottomRotation, false);
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
                AudioManager.Instance.PlayPlayerProjectileLaunch(true);
                yield return isOverdrive ? waitForShootOverdrive : fireWaitForSeconds;
            }
        }
    }
    #endregion

    #region Dodge

    void Dodge()
    {
        int useEnergy = isOverdrive ? dodgeCost * dodgeCostFactor : dodgeCost;
        if (isDodging || !playerEnergy.isEnough(useEnergy)) return;
        StartCoroutine(StartDodgeCoroutine(useEnergy));
    }

    IEnumerator StartDodgeCoroutine(int useEnergy)
    {
        PlayerBulletTime.Instance.BulletTime(bulletTime, fadeOutTime, fadeInTime);
        AudioManager.Instance.PlayPlayerDodge();
        curRoll = 0f;
        t = 0f;
        isDodging = true;
        collider2D.isTrigger = true;
        playerEnergy.Use(useEnergy);
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

    #endregion

    public void ChangePowerLevel(int value)
    {
        powerLevel = value;
        weaponLevelText.text = "" + powerLevel;
    }
    public void UpdatePowerLevel()
    {
        ChangePowerLevel(Mathf.Clamp(powerLevel + 1, 1, 3));
    }

    #region Overdrive

    void Overdrive()
    {
        if (!playerEnergy.isEnough(PlayerEnergy.MAX_ENERGY) || isOverdrive) return;
        PlayerOverdrive.on.Invoke();
    }

    void OverdriveOn()
    {
        isOverdrive = true;
        PlayerBulletTime.Instance.BulletTime(bulletTime, fadeOutTime, keepTime, fadeInTime);
    }

    void OverdriveOff()
    {
        isOverdrive = false;
    }

    #endregion
}
