using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerOverdrive : MonoBehaviour
{
    public static UnityAction on = delegate { };
    public static UnityAction off = delegate { };

    [SerializeField] GameObject triggerVFX;
    [SerializeField] GameObject engineVFXNormal;
    [SerializeField] GameObject engineVFXOverdrive;
    void Awake()
    {
        on += OverdriveOn;
        off += OverdriveOff;
    }

    void OnDestroy()
    {
        on -= OverdriveOn;
        off -= OverdriveOff;
    }

    void OverdriveOn()
    {
        triggerVFX.SetActive(true);
        engineVFXNormal.SetActive(false);
        engineVFXOverdrive.SetActive(true);
        AudioManager.Instance.PlayerOverdriveOn();
    }

    void OverdriveOff()
    {
        engineVFXOverdrive.SetActive(false);
        engineVFXNormal.SetActive(true);
        AudioManager.Instance.PlayerOverdriveOff();
    }
}
