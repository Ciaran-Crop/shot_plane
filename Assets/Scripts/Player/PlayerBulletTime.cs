using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletTime : Singleton<PlayerBulletTime>
{
    float t;
    float defaultTimeScale;

    [SerializeField] bool openBulletTime = true;

    public void CloseBulletTime() => openBulletTime = false;
    public void OpenBulletTime() => openBulletTime = true;

    protected override void Awake()
    {
        base.Awake();
        defaultTimeScale = Time.fixedDeltaTime;
    }
    public void BulletTime(float bulletTime, float fadeOutTime)
    {
        if (openBulletTime)
        {
            StartCoroutine(StartBulletTimeCoroutine(bulletTime, fadeOutTime, -1f, -1f));
        }
    }

    public void BulletTime(float bulletTime, float fadeOutTime, float fadeInTime)
    {
        if (openBulletTime)
        {
            StartCoroutine(StartBulletTimeCoroutine(bulletTime, fadeOutTime, -1f, fadeInTime));
        }
    }

    public void BulletTime(float bulletTime, float fadeOutTime, float keepTime, float fadeInTime)
    {
        if (openBulletTime)
        {
            StartCoroutine(StartBulletTimeCoroutine(bulletTime, fadeOutTime, keepTime, fadeInTime));
        }
    }

    IEnumerator StartBulletTimeCoroutine(float bulletTime, float fadeOutTime, float keepTime, float fadeInTime)
    {
        // fade in
        if (fadeInTime > 0f)
        {
            t = 0f;
            while (t <= 1f)
            {
                t += Time.unscaledDeltaTime / fadeInTime;
                Time.timeScale = Mathf.Lerp(1f, bulletTime, t);
                Time.fixedDeltaTime = defaultTimeScale * Time.timeScale;
                yield return null;
            }
        }
        else
        {
            Time.timeScale = bulletTime;
            Time.fixedDeltaTime = defaultTimeScale * Time.timeScale;
        }

        // keep
        if (keepTime > 0f)
        {
            yield return new WaitForSecondsRealtime(keepTime);
        }

        // fade out
        if (fadeOutTime > 0f)
        {
            t = 0f;
            while (t <= 1f)
            {
                t += Time.unscaledDeltaTime / fadeOutTime;
                Time.timeScale = Mathf.Lerp(bulletTime, 1f, t);
                Time.fixedDeltaTime = defaultTimeScale * Time.timeScale;
                yield return null;
            }
        }
        else
        {
            Time.timeScale = 1f;
            Time.fixedDeltaTime = defaultTimeScale * Time.timeScale;
        }

    }
}
