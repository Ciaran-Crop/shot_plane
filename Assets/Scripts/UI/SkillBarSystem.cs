using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillBarSystem : MonoBehaviour
{
    [SerializeField] Text weaponLevelText;
    void Awake()
    {
        if (TryGetComponent<Canvas>(out Canvas canvas))
        {
            canvas = GetComponent<Canvas>();
            canvas.worldCamera = Camera.main;
        }
    }

    public void UpdateWeaponText(int value)
    {
        weaponLevelText.text = "" + value;
    }
}
