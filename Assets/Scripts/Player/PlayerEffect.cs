using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffect : MonoBehaviour
{
    [SerializeField] StatSystemEffect statSystemEffect;
    public int status;
    public const int NORMAL = 0;
    public const int DOT = 1;
    public const int RESTORE = 2;

    void Awake() => status = NORMAL;
    public void SetRestore()
    {
        statSystemEffect.SetRestoreImage(true);
        if (status == NORMAL)
        {
            status = RESTORE;
        }
    }

    public void CancelRestore()
    {
        statSystemEffect.SetRestoreImage(false);
        if (status == RESTORE)
        {
            status = NORMAL;
        }
    }

    public void SetDoT()
    {
        statSystemEffect.SetDoTImage(true);
        status = DOT;
    }

    public void CancelDoT()
    {
        statSystemEffect.SetDoTImage(false);
        if (status == DOT)
        {
            status = NORMAL;
        }
    }
}
