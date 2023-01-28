using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BuffManager : Singleton<BuffManager>
{
    [SerializeField] float waitForBuffUITime = 1f;
    [SerializeField] BuffUIController buffUIController;
    [SerializeField] BuffItem[] buffItems;
    WaitForSeconds waitForBuffUI;
    PlayerController playerController;
    BuffItem buffItem;

    protected override void Awake()
    {
        base.Awake();
        waitForBuffUI = new WaitForSeconds(waitForBuffUITime);
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    public IEnumerator RandomOneBuff()
    {
        buffUIController.Hide();
        buffItem = buffItems[Random.Range(0, buffItems.Length)];
        string valueString = Change(buffItem);
        buffUIController.UpdateText(buffItem.Title, valueString);
        buffUIController.Show();
        yield return waitForBuffUI;
    }

    string GetFactorString(float value)
    {
        return string.Format("{0}%", Mathf.RoundToInt(value * 100f));
    }

    string GetIntString(float value)
    {
        return value < 0 ? string.Format("{0}", Mathf.RoundToInt(value)) : string.Format("+{0}", Mathf.RoundToInt(value));
    }

    public string Change(BuffItem buffItem)
    {
        float factor = Random.Range(buffItem.MinRange, buffItem.MaxRange);
        switch (buffItem.TitleEn)
        {
            case "ChangeMoveSpeed":
                playerController.ChangeMoveSpeed(factor);
                return GetFactorString(factor);
            case "ChangeAccelerationTime":
                playerController.ChangeAccelerationTime(factor);
                return GetFactorString(factor);
            case "ChangeDecelerationTime":
                playerController.ChangeDecelerationTime(factor);
                return GetFactorString(factor);
            case "ChangeFireInterval":
                playerController.ChangeFireInterval(factor);
                return GetFactorString(factor);
            case "ChangeAddMissileCount":
                playerController.ChangeAddMissileCount(Mathf.RoundToInt(factor));
                return GetIntString(factor);
            case "ChangeMissileColdDownTime":
                playerController.ChangeMissileColdDownTime(factor);
                return GetFactorString(factor);
            case "ChangeDodgeCost":
                playerController.ChangeDodgeCost(Mathf.RoundToInt(factor));
                return GetIntString(factor);
            case "ChangeWaitForStraightSub":
                playerController.ChangeWaitForStraightSub(factor);
                return GetFactorString(factor);
            case "ChangeOverdriveCost":
                playerController.ChangeOverdriveCost(Mathf.RoundToInt(factor));
                return GetIntString(factor);
            case "ChangeHealthRegenerationWaitTime":
                playerController.ChangeHealthRegenerationWaitTime(factor);
                return GetFactorString(factor);
            default:
                return "";
        }
    }
}
