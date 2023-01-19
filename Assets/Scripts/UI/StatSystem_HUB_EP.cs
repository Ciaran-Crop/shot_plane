using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatSystem_HUB_EP : StatSystem
{
    [SerializeField] Text levelUpText;
    [SerializeField] Text levelText;
    [SerializeField] float levelUpTimeDuration = 2f;
    [SerializeField] float waitForStayTime = 1f;
    float animationT;
    WaitForSeconds waitForStay;

    Vector3 levelUpVector;

    protected override void Awake()
    {
        base.Awake();
        levelUpText.gameObject.SetActive(false);
        levelUpVector = Vector3.one * 1.77f;
        waitForStay = new WaitForSeconds(waitForStayTime);
    }

    public virtual void UpdateText(int level)
    {
        levelText.text = string.Format(" LEVEL   {0}", level);
    }

    public override void Initialize(float curStat, float maxStat)
    {
        base.Initialize(curStat, maxStat);
    }

    public void AnimationLevelUp()
    {
        StartCoroutine(nameof(LevelUpAnimation));
    }

    IEnumerator LevelUpAnimation()
    {
        levelUpText.gameObject.transform.localScale = levelUpVector;
        levelUpText.gameObject.SetActive(true);
        animationT = 0f;
        while(animationT <= 1f)
        {
            animationT += Time.deltaTime / levelUpTimeDuration;
            levelUpText.gameObject.transform.localScale = Vector3.Lerp(levelUpVector, Vector3.one, animationT);
            yield return null;
        }
        yield return waitForStay;
        levelUpText.gameObject.SetActive(false);
    }

}
