using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatSystem : MonoBehaviour
{
    protected float targetFillAmount;
    protected float curFillAmount;
    [SerializeField] protected Image frontStatImage;
    [SerializeField] protected Image backStatImage;

    [SerializeField] protected bool fillDelay = true;
    [SerializeField] protected float delayFillTime = 0.2f;
    [SerializeField] protected float fillSpeed = 0.1f;
    protected float t;
    protected WaitForSeconds waitForDelayFill;

    Coroutine bufferedStatCoroutine;

    Canvas canvas;

    void Awake()
    {
        waitForDelayFill = new WaitForSeconds(delayFillTime);
        canvas = GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;
    }

    void Start()
    {

    }

    void OnEnable()
    {

    }

    public virtual void Initialize(float curStat, float maxStat)
    {
        targetFillAmount = curStat / maxStat;
        curFillAmount = targetFillAmount;
        backStatImage.fillAmount = targetFillAmount;
        frontStatImage.fillAmount = targetFillAmount;
    }

    public void UpdateStat(float targetStat, float maxStat)
    {
        if (bufferedStatCoroutine != null)
        {
            StopCoroutine(bufferedStatCoroutine);
        }

        targetFillAmount = targetStat / maxStat;

        if (targetFillAmount < curFillAmount)
        {
            frontStatImage.fillAmount = targetFillAmount;
            StartCoroutine(BufferedStatCoroutine(backStatImage));
        }
        else if (targetFillAmount > curFillAmount)
        {
            backStatImage.fillAmount = targetFillAmount;
            StartCoroutine(BufferedStatCoroutine(frontStatImage));
        }
    }

    protected virtual IEnumerator BufferedStatCoroutine(Image image)
    {
        if (fillDelay)
        {
            yield return waitForDelayFill;
        }
        t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * fillSpeed;
            curFillAmount = Mathf.Lerp(curFillAmount, targetFillAmount, t);
            image.fillAmount = curFillAmount;
            yield return null;
        }
    }

}
