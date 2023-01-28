using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BossController : MonoBehaviour
{
    public enum SkillEnum
    {
        ScatterProjectile,
        AimProjectile,
        RedAimProjectile,
        Laser,
        CombineProjectile,
    }
    int skillNum = 4;
    int highSkillNum = 2;
    int index = 0;
    [SerializeField] AudioData laserLaunch;
    [SerializeField] AudioData laserFire;
    [SerializeField] Transform muzzleUp;
    [SerializeField] Transform muzzleMiddle;
    [SerializeField] Transform muzzleBottom;
    [SerializeField] GameObject enemyProjectileNormal;
    [SerializeField] GameObject enemyProjectileAim;
    [SerializeField] GameObject enemyProjectileRedAim;
    [SerializeField] float moveSpeed = 15f;
    [SerializeField] float skillInterval = 2f;
    [SerializeField] float fireInterval = 0.5f;
    [SerializeField] float paddingX = 0.2f;
    [SerializeField] float paddingY = 0.2f;
    [SerializeField] float collisionDamage = 1f;
    [SerializeField] bool hasCollisionDamage = false;
    [SerializeField] float enemyProjectileUpAngle = 2f;
    [SerializeField] float laserColdTime = 15f;
    Quaternion enemyProjectileUpRotation;
    WaitForSeconds waitForLaserCold;
    bool laserReady;
    [SerializeField] float enemyProjectileBottomAngle = -2f;
    [SerializeField, Range(1, 30)] int scatterCount = 10;
    [SerializeField, Range(1, 30)] int aimCount = 10;
    [SerializeField] Animator laserAnimator;
    [SerializeField] ParticleSystem fireVFX;
    int laserAnimationId = Animator.StringToHash("launchBeam");
    Quaternion enemyProjectileBottomRotation;
    HealthSystem healthSystem;

    EnemyMoveState moveState = EnemyMoveState.Random;
    List<SkillEnum> skillList;
    Vector3 moveTo;
    WaitForSeconds waitForSkillInterval;
    WaitForSeconds waitForFireInterval;
    Coroutine skillCoroutine;
    bool skillState;
    bool laserState;

    public void ChangeMoveState(EnemyMoveState state) => moveState = state;
    public void ChangeSkillState(bool state) => skillState = state;

    IEnumerator RandomMoveCoroutine()
    {
        transform.position = ViewPort.Instance.RandomEnemyCreatePosition(paddingX, paddingY);
        moveTo = ViewPort.Instance.RandomEnemyMoveRightPosition(paddingX, paddingY);
        while (gameObject.activeSelf && !GameManager.IsGameOver)
        {
            if (moveState == EnemyMoveState.Y)
            {
                moveTo = ViewPort.Instance.RightPaddingPosition(EnemyManager.Instance.GetPlayer().transform.position.y);
            }
            if (Vector3.Distance(transform.position, moveTo) >= Time.deltaTime * moveSpeed)
            {
                transform.position = Vector3.MoveTowards(transform.position, moveTo, Time.deltaTime * moveSpeed);
            }
            else
            {
                moveTo = ViewPort.Instance.RandomEnemyMoveRightPosition(paddingX, paddingY);
            }
            if (GameManager.IsGameOver) yield break;

            yield return null;
        }
    }

    IEnumerator RandomSkillCoroutine()
    {
        while (!GameManager.IsGameOver)
        {
            yield return waitForSkillInterval;
            if (skillState || laserState) yield return null;
            SkillEnum skillEnum = RandomSkill();
            ChangeSkillState(true);
            switch (skillEnum)
            {
                case SkillEnum.ScatterProjectile:
                    yield return StartCoroutine(nameof(ScatterProjectile));
                    break;
                case SkillEnum.AimProjectile:
                    yield return StartCoroutine(nameof(AimProjectile));
                    break;
                case SkillEnum.Laser:
                    if (laserReady) yield return StartCoroutine(nameof(Laser));
                    break;
                case SkillEnum.CombineProjectile:
                    yield return StartCoroutine(nameof(CombineProjectile));
                    break;
                default:
                    yield return null;
                    break;
            }
            ChangeSkillState(false);
        }
    }

    SkillEnum RandomSkill()
    {
        index = 0;
        if (!healthSystem.LessHalfHealth)
        {
            index = Random.Range(0, skillNum - highSkillNum);
        }
        else
        {
            index = Random.Range(0, skillNum);
        }
        return skillList[index];
    }

    void InitProjectile(GameObject projectileObject)
    {
        projectileObject.GetComponent<Projectile>().setLauncher(gameObject);
        projectileObject.SetActive(true);
    }

    void FireProjectileUp(GameObject projectile) => InitProjectile(PoolManager.Release(projectile, muzzleUp.position, enemyProjectileUpRotation, false));

    void FireProjectileMiddle(GameObject projectile) => InitProjectile(PoolManager.Release(projectile, muzzleMiddle.position, Quaternion.identity, false));

    void FireProjectileBottom(GameObject projectile) => InitProjectile(PoolManager.Release(projectile, muzzleBottom.position, enemyProjectileBottomRotation, false));

    IEnumerator ScatterProjectile()
    {
        fireVFX.Play();
        for (int i = 0; i < scatterCount; i++)
        {
            FireProjectileUp(enemyProjectileNormal);
            FireProjectileMiddle(enemyProjectileNormal);
            FireProjectileBottom(enemyProjectileNormal);
            AudioManager.Instance.PlayRandomEnemyLaunch();
            yield return waitForFireInterval;
        }
        fireVFX.Stop();
    }

    IEnumerator AimProjectile()
    {
        fireVFX.Play();
        for (int i = 0; i < aimCount; i++)
        {
            FireProjectileMiddle(enemyProjectileAim);
            AudioManager.Instance.PlayRandomEnemyLaunch();
            yield return waitForFireInterval;
        }
        fireVFX.Stop();
    }

    IEnumerator Laser()
    {
        ChangeMoveState(EnemyMoveState.Y);
        laserState = true;
        laserReady = false;
        AudioManager.Instance.PlayAudio(laserLaunch);
        laserAnimator.SetTrigger(laserAnimationId);
        yield return null;
    }

    public void CloseLaser()
    {
        laserState = false;
        ChangeMoveState(EnemyMoveState.Random);
        StartCoroutine(nameof(LaserColdDownCoroutine));
    }

    public void LaserAudio()
    {
        AudioManager.Instance.PlayAudio(laserFire);
    }

    IEnumerator RedAimProjectile()
    {
        fireVFX.Play();
        for (int i = 0; i < aimCount; i++)
        {
            FireProjectileMiddle(enemyProjectileRedAim);
            AudioManager.Instance.PlayRandomEnemyLaunch();
            yield return waitForFireInterval;
        }
        fireVFX.Stop();
    }

    IEnumerator CombineProjectile()
    {
        yield return AimProjectile();
        yield return ScatterProjectile();
    }

    IEnumerator LaserColdDownCoroutine()
    {
        yield return waitForLaserCold;
        laserReady = true;
    }


    protected virtual void AddSkill()
    {
        skillList.Add(SkillEnum.ScatterProjectile);
        skillList.Add(SkillEnum.AimProjectile);
        skillList.Add(SkillEnum.RedAimProjectile);
        skillList.Add(SkillEnum.Laser);
        skillList.Add(SkillEnum.CombineProjectile);
    }

    protected virtual void Awake()
    {
        skillList = new List<SkillEnum>();
        enemyProjectileBottomRotation = Quaternion.AngleAxis(enemyProjectileBottomAngle, transform.forward);
        enemyProjectileUpRotation = Quaternion.AngleAxis(enemyProjectileUpAngle, transform.forward);
        AddSkill();
        waitForSkillInterval = new WaitForSeconds(skillInterval);
        waitForFireInterval = new WaitForSeconds(fireInterval);
        healthSystem = GetComponent<HealthSystem>();
        var size = transform.GetChild(0).GetComponent<Renderer>().bounds.size;
        paddingX = size.x;
        paddingY = size.y;
        skillState = false;
        laserState = false;
        laserReady = false;
        waitForLaserCold = new WaitForSeconds(laserColdTime);
    }

    public void StartSkill()
    {
        StartCoroutine(nameof(RandomMoveCoroutine));
        StartCoroutine(nameof(RandomSkillCoroutine));
        StartCoroutine(nameof(LaserColdDownCoroutine));
    }

    void OnEnable()
    {
        fireVFX.Stop();
    }

    void OnDisable()
    {
        StopAllCoroutines();
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
