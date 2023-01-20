using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEP : MonoBehaviour
{
    [SerializeField] StatSystem_HUB_EP epStatBar;
    float needExperience;
    public const int PERCENT = 1;
    PlayerEffect playerEffect;
    PlayerController playerController;
    PlayerHealth playerHealth;
    PlayerEnergy playerEnergy;

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
        playerController = GetComponent<PlayerController>();
        playerHealth = GetComponent<PlayerHealth>();
        playerEnergy = GetComponent<PlayerEnergy>();
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
        playerHealth.RestoreHealth(100);
        switch (level)
        {
            case 1:
                break;
            case 2:
                break;
            case 3:
                playerController.UpdatePowerLevel();
                break;
            case 4:
                playerHealth.SetHealthRegenerationWaitTime(0.2f);
                break;
            case 5:
                playerController.SetWaitForStraightSub(0.2f);
                break;
            case 6:
                playerController.UpdatePowerLevel();
                break;
            case 7:
                playerEnergy.SetOverdriveCost(2);
                break;
            case 8:
                playerController.SetWaitForStraightSub(0.2f);
                playerHealth.SetHealthRegenerationWaitTime(0.2f);
                break;
            case 9:
                playerController.SetWaitForStraightSub(0.1f);
                playerHealth.SetHealthRegenerationWaitTime(0.1f);
                break;
            case 10:
                playerEnergy.SetOverdriveCost(1);
                break;
            default:
                playerController.SetWaitForStraightSub(0.1f);
                playerHealth.SetHealthRegenerationWaitTime(0.1f);
                break;
        }
        // level2 nothing
        // level3  武器升级
        // level4 生命自动恢复时间减少 20%
        // level5 翻滚硬直减少
        // level6 武器升级
        // level6 所有技能时间减少 20%
        // level7 减少爆发使用能量  所有技能时间减少 10% 生命自动恢复时间减少 20%
        // level8 生命恢复技能时间减少 20%
        // level9 增加爆发时间 生命自动恢复时间减少 20%
        // level10 生命自动恢复时间减少 20% 翻滚技能时间减少 20%
        // level11 所有技能时间减少 10%
        // level12 所有技能时间减少 10%
    }

}
