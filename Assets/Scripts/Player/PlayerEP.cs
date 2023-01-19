using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEP : MonoBehaviour
{
    [SerializeField] StatSystem_HUB_EP epStatBar;
    float needExperience;
    public const int PERCENT = 1;
    PlayerEffect playerEffect;

    float maxExperience = Mathf.Pow(2f, 15);
    float minExperience = Mathf.Pow(2f, 5);

    float ep;

    int level;

    float GetNeedExperience(int level)
    {
        return Mathf.Clamp(Mathf.Pow(2f, level + 4), minExperience, maxExperience);
    }

    // Start is called before the first frame update
    void Start()
    {
        playerEffect = GetComponent<PlayerEffect>();
        level = 1;
        ep = 0;
        needExperience = GetNeedExperience(level);
        epStatBar.Initialize(ep, needExperience);
        epStatBar.UpdateText(level);
    }

    public void Obtain(int value)
    {
        ep = ep + value;
        if (ep >= needExperience)
        {
            LevelUP();
        }
        epStatBar.UpdateStat(ep, needExperience);
    }

    void LevelUP()
    {
        ep -= needExperience;
        level++;
        needExperience = GetNeedExperience(level);
        epStatBar.AnimationLevelUp();
        epStatBar.UpdateText(level);
        UpdatePlayer();
    }

    void UpdatePlayer()
    {
        // 生命回满
        // 去除所有负面效果
        playerEffect.CancelDoT();
        gameObject.GetComponent<HealthSystem>().RestoreHealth(100);
        // level2 nothing
        // level3  武器升级
        if (level == 3)
        {
            gameObject.GetComponent<PlayerController>().UpdatePowerLevel();
            return;
        }
        // level4 生命自动恢复时间减少 20%
        if (level == 5)
        {
            gameObject.GetComponent<PlayerController>().UpdatePowerLevel();
            return;
        }
        // level5 武器升级
        // level6 所有技能时间减少 20%
        // level7 所有技能时间减少 10% 生命自动恢复时间减少 20%
        // level8 生命恢复技能时间减少 20%
        // level9 增加爆发时间 生命自动恢复时间减少 20%
        // level10 生命自动恢复时间减少 20% 翻滚技能时间减少 20%
        // level11 所有技能时间减少 10%
        // level12 所有技能时间减少 10%
    }

}
