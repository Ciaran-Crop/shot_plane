using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEP : MonoBehaviour
{
    PlayerController playerController;
    [SerializeField] StatSystem_HUB_EP epStatBar;
    float needExperience;
    public const int PERCENT = 1;
    float maxExperience = Mathf.Pow(2f, 15);
    float minExperience = Mathf.Pow(2f, 5);
    float ep;
    int level;
    public int levelUpAddMissileCount = 3;

    float GetNeedExperience(int level)
    {
        return Mathf.Clamp(Mathf.Pow(2f, level + 4), minExperience, maxExperience);
    }

    void Awake()
    {
        level = 1;
        ep = 0;
        needExperience = GetNeedExperience(level);
        epStatBar.Initialize(ep, needExperience);
        epStatBar.UpdateText(level);
    }

    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponent<PlayerController>();
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
        playerController.ChangeDoTState(false);
        playerController.RestoreAll();
        playerController.ChangeAddMissileCount(levelUpAddMissileCount);
        switch (level)
        {
            case 1:
                break;
            case 2:
                playerController.ChangeCanDodge(true);
                break;
            case 3:
                playerController.UpdatePowerLevel();
                break;
            case 4:
                playerController.ChangeHealthRegenerationWaitTime(0.8f);
                break;
            case 5:
                playerController.ChangeWaitForStraightSub(0.8f);
                break;
            case 6:
                playerController.ChangeOverdriveCost(-1);
                playerController.UpdatePowerLevel();
                break;
            case 7:
                playerController.ChangeAddMissileCount(1);
                playerController.ChangeOverdriveCost(-1);
                break;
            case 8:
                playerController.ChangeWaitForStraightSub(0.8f);
                playerController.ChangeHealthRegenerationWaitTime(0.8f);
                break;
            case 9:
                playerController.ChangeMaxHealth(200);
                playerController.ChangeHealthRegenerationWaitTime(0.8f);
                break;
            case 10:
                playerController.ChangeAddMissileCount(1);
                playerController.ChangeOverdriveCost(-1);
                break;
            default:
                playerController.ChangeAddMissileCount(1);
                playerController.ChangeHealthRegenerationWaitTime(0.8f);
                break;
        }
    }

}
