using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillBarSystem : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] Text weaponLevelText;
    [SerializeField] Image missileColdDownImage;
    [SerializeField] Text missileCountText;
    bool showDodge;

    void Awake()
    {
        if (TryGetComponent<Canvas>(out Canvas canvas))
        {
            canvas = GetComponent<Canvas>();
            canvas.worldCamera = Camera.main;
        }
    }

    # region Weapon

    public void UpdateWeaponText(int value)
    {
        weaponLevelText.text = "" + value;
    }

    #endregion

    #region Missile

    IEnumerator StartColdDown(float coldDownTime)
    {
        float t = 0f;
        missileColdDownImage.fillAmount = 1f;
        while (t < 1f)
        {
            t += Time.deltaTime / coldDownTime;
            missileColdDownImage.fillAmount = Mathf.Lerp(1f, 0f, t);
            yield return null;
        }
        playerController.ChangeMissileUseAvailable(true);
    }

    public void UpdateMissileText(int value)
    {
        missileCountText.text = value.ToString();
    }

    public void UseOneMissile(int remainCount, float coldDownTime)
    {
        UpdateMissileText(remainCount);
        StartCoroutine(StartColdDown(coldDownTime));
    }

    # endregion

    # region Dodge

    public void ChangeDodgeState(bool canDodge)
    {
        // TODO
    }

    #endregion
}
