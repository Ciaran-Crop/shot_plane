using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{

    # region Base

    [Header("-- Base --")]
    [SerializeField] GamePlayInput input;
    [SerializeField] SkillBarSystem skillBarSystem;
    [SerializeField] ShootUIController shootUIController;
    PlayerEnergy playerEnergy;
    PlayerHealth playerHealth;
    PlayerEffect playerEffect;
    PlayerEP playerEP;
    new Rigidbody2D rigidbody2D;
    new Collider2D collider2D;

    bool inUI = false;

    #endregion

    # region Move

    [Header("-- Move --")]
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float accelerationTime = 5f;
    [SerializeField] float decelerationTime = 5f;
    [SerializeField] float moveRotationAngle = 50;
    WaitForFixedUpdate waitForFixedUpdate;
    Vector2 velocity;
    Coroutine coroutine;
    Quaternion rotation;
    float moveT;
    float paddingX;
    float paddingY;

    #endregion

    # region Fire
    // need to move to SkillBarSystem

    [Header("-- Fire --")]
    [SerializeField] GameObject projectile;
    [SerializeField] GameObject missile;
    [SerializeField] float fireInterval = 0.2f;
    [SerializeField] Transform muzzleUp;
    [SerializeField] Transform muzzleMiddle;
    [SerializeField] Transform muzzleBottom;
    [SerializeField] float projectileUpAngle = 0.05f;
    [SerializeField] float projectileBottomAngle = -0.05f;
    [SerializeField, Range(1, 3)] int powerLevel = 1;
    [SerializeField] int missileCount = 3;
    [SerializeField] float missileColdDownTime = 3f;
    bool missileUseAvailable = true;
    Quaternion projectileUpRotation;
    Quaternion projectileBottomRotation;
    WaitForSeconds fireWaitForSeconds;
    public int PowerLevel => powerLevel;
    GameObject GetProjectile => isOverdrive ? playerProjectileOverdrive : projectile;

    #endregion

    # region Dodge

    [Header("-- Dodge --")]
    [SerializeField] int dodgeCost = 25;
    [SerializeField] float maxRoll = 720f;
    [SerializeField] float rollSpeed = 360f;
    [SerializeField] Vector3 dodgeScale = Vector3.one;
    [SerializeField] float waitForStraightTime = 0.5f;
    [SerializeField] float minWaitForStraightTime = 0.1f;
    float curRoll;
    float dodgeDuration;
    bool isDodging = false;
    public bool CanDodge { get; }
    bool canDodge = false;
    float t;
    WaitForSeconds waitForStraight;

    #endregion

    # region Overdrive

    [Header("-- Overdrive --")]
    [SerializeField] GameObject playerProjectileOverdrive;
    [SerializeField] float overdriveTime = 0.1f;
    [SerializeField] int overdriveCost = 4;
    bool isOverdrive = false;
    float shootSpeedFactor = 1.2f;
    float moveSpeedFactor = 1.2f;
    int dodgeCostFactor = 2;
    WaitForSeconds waitForShootOverdrive;
    WaitForSeconds waitForOverdrive;

    #endregion

    # region Bullet

    [SerializeField] public float bulletTime = 0.5f;
    [SerializeField] public float fadeInTime = 0.5f;
    [SerializeField] public float keepTime = 1f;
    [SerializeField] public float fadeOutTime = 0.5f;

    #endregion

    # region BaseFunc

    void Awake()
    {
        curRoll = 0;
        dodgeDuration = maxRoll / rollSpeed;
        waitForStraight = new WaitForSeconds(waitForStraightTime);
        waitForFixedUpdate = new WaitForFixedUpdate();
        waitForShootOverdrive = new WaitForSeconds(shootSpeedFactor * fireInterval);
        fireWaitForSeconds = new WaitForSeconds(fireInterval);
        projectileUpRotation = Quaternion.AngleAxis(projectileUpAngle, transform.forward);
        projectileBottomRotation = Quaternion.AngleAxis(projectileBottomAngle, transform.forward);
        waitForOverdrive = new WaitForSeconds(overdriveTime);
    }

    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        rigidbody2D.gravityScale = 0f;
        collider2D = GetComponent<Collider2D>();
        playerEnergy = GetComponent<PlayerEnergy>();
        playerHealth = GetComponent<PlayerHealth>();
        playerEffect = GetComponent<PlayerEffect>();
        playerEP = GetComponent<PlayerEP>();
        var size = transform.GetChild(0).GetComponent<Renderer>().bounds.size;
        paddingX = size.x;
        paddingY = size.y;
        input.SwitchToGamePlayInput();
        ChangePowerLevel(powerLevel);
    }

    void OnEnable()
    {
        input.onMove += Move;
        input.onStopMove += StopMove;
        input.onFire += Fire;
        input.onStopFire += StopFire;
        input.onDodge += Dodge;
        input.onOverdrive += Overdrive;
        input.onMissile += LaunchMissile;
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
        input.onMissile -= LaunchMissile;
        PlayerOverdrive.on -= OverdriveOn;
        PlayerOverdrive.off -= OverdriveOff;
    }

    void Update() => transform.position = ViewPort.Instance.PlayerEnablePosition(
            transform.position,
            paddingX,
            paddingY
        );

    #endregion

    # region DodgeFunc

    #endregion

    # region MoveFunc

    IEnumerator StartMoveCoroutine(float time, Vector2 moveVelocity, Quaternion rotationTarget)
    {
        moveT = 0f;
        velocity = rigidbody2D.velocity;
        rotation = transform.rotation;
        while (moveT <= 1f)
        {
            moveT += Time.fixedDeltaTime / time;
            rigidbody2D.velocity = Vector2.Lerp(velocity, moveVelocity, moveT);
            transform.rotation = Quaternion.Lerp(rotation, rotationTarget, moveT);
            yield return waitForFixedUpdate;
        }
    }

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

    #endregion

    # region FireFunc

    void InitProjectile(GameObject projectileObject)
    {
        projectileObject.GetComponent<Projectile>().setLauncher(gameObject);
        projectileObject.SetActive(true);
    }

    void FireProjectileUp() => InitProjectile(PoolManager.Release(GetProjectile, muzzleUp.position, projectileUpRotation, false));
    void FireProjectileMiddle() => InitProjectile(PoolManager.Release(GetProjectile, muzzleMiddle.position, Quaternion.identity, false));

    void FireProjectileBottom() => InitProjectile(PoolManager.Release(GetProjectile, muzzleBottom.position, projectileBottomRotation, false));
    void FireMissile() => InitProjectile(PoolManager.Release(missile, muzzleMiddle.position, Quaternion.identity, false));

    WaitForSeconds IsOverdriveTime() => isOverdrive ? waitForShootOverdrive : fireWaitForSeconds;

    void Fire()
    {
        StartCoroutine(nameof(StartFire));
    }

    void StopFire()
    {
        StopCoroutine(nameof(StartFire));
    }

    public void ChangeMissileUseAvailable(bool state) => missileUseAvailable = state;

    void LaunchMissile()
    {
        if (missileCount > 0 && missileUseAvailable)
        {
            FireMissile();
            missileCount--;
            missileUseAvailable = false;
            skillBarSystem.UseOneMissile(missileCount, missileColdDownTime);
        }
    }

    IEnumerator StartFire()
    {
        while (true)
        {
            {
                switch (powerLevel)
                {
                    case 1:
                        FireProjectileMiddle();
                        break;
                    case 2:
                        FireProjectileUp();
                        FireProjectileBottom();
                        break;
                    case 3:
                        FireProjectileUp();
                        FireProjectileMiddle();
                        FireProjectileBottom();
                        break;
                    default:
                        break;
                }
                AudioManager.Instance.PlayPlayerProjectileLaunch(true);
                yield return IsOverdriveTime();
            }
        }
    }

    #endregion

    # region Dodge

    void Dodge()
    {
        int useEnergy = isOverdrive ? dodgeCost * dodgeCostFactor : dodgeCost;
        if (isDodging || !playerEnergy.isEnough(useEnergy) || !canDodge) return;
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
        collider2D.isTrigger = false;
        yield return waitForStraight;
        isDodging = false;
    }

    #endregion

    # region ChangeFunc

    public void ChangePowerLevel(int value)
    {
        powerLevel = value;
        skillBarSystem.UpdateWeaponText(powerLevel);
    }
    public void UpdatePowerLevel() => ChangePowerLevel(Mathf.Clamp(powerLevel + 1, 1, 3));
    public void ChangeMoveSpeed(float factor) => moveSpeed *= factor;
    public void ChangeAccelerationTime(float factor) => accelerationTime *= factor;
    public void ChangeDecelerationTime(float factor) => decelerationTime *= factor;
    public void ChangeFireInterval(float factor)
    {
        fireInterval *= factor;
        fireWaitForSeconds = new WaitForSeconds(fireInterval);
    }
    public void ChangeAddMissileCount(int value) => missileCount = Mathf.Clamp(missileCount + value, 0, 10);
    public void ChangeMissileColdDownTime(float factor) => missileColdDownTime *= factor;
    public void ChangeDodgeCost(int value) => dodgeCost = Mathf.Clamp(dodgeCost + value, 1, 100);
    public void ChangeWaitForStraightSub(float factor) => waitForStraight = new WaitForSeconds(Mathf.Max(waitForStraightTime * factor, minWaitForStraightTime));
    public void ChangeOverdriveCost(int value) => overdriveCost = Mathf.Clamp(overdriveCost + value, 1, 100);
    public void ChangeCanDodge(bool state)
    {
        canDodge = state;
        skillBarSystem.ChangeDodgeState(canDodge);
    }
    public void ChangeHealthRegenerationWaitTime(float factor) => playerHealth.SetHealthRegenerationWaitTime(factor);
    public void RestoreHealth(int value) => playerHealth.RestoreHealth(value);
    public void ChangeDoTState(bool state)
    {
        if (state)
        {
            playerEffect.SetDoT();
        }
        else
        {
            playerEffect.CancelDoT();
        }
    }
    public void ChangeLevelUpMissile(int value) => playerEP.levelUpAddMissileCount += value;
    public void RestoreAll() => playerHealth.RestoreHealth(playerHealth.MaxHealth);
    public void ChangeMaxHealth(float value) => playerHealth.setMaxHealth(value);

    public void ChangeUIShow(bool state)
    {
        shootUIController.ChangeUIState(state);
        ChangeUIState(state);
    }

    public void ChangeUIState(bool state) => inUI = state;
    public void SetUIBefore() => ChangeUIShow(inUI);

    #endregion

    #region OverdriveFunc

    IEnumerator OverdriveEnergyCoroutine()
    {
        while (playerEnergy.isEnough(overdriveCost))
        {
            playerEnergy.Use(overdriveCost);
            yield return waitForOverdrive;
        }
        PlayerOverdrive.off.Invoke();
    }
    void Overdrive()
    {
        if (!playerEnergy.isEnough(PlayerEnergy.MAX_ENERGY) || isOverdrive) return;
        PlayerOverdrive.on.Invoke();
    }

    void OverdriveOn()
    {
        isOverdrive = true;
        PlayerBulletTime.Instance.BulletTime(bulletTime, fadeOutTime, keepTime, fadeInTime);
        playerEnergy.SetUseObtain(false);
        StartCoroutine(nameof(OverdriveEnergyCoroutine));
    }

    void OverdriveOff()
    {
        isOverdrive = false;
        playerEnergy.SetUseObtain(true);
        playerEnergy.NotFullState();
        StopCoroutine(nameof(OverdriveEnergyCoroutine));
    }

    #endregion
}
