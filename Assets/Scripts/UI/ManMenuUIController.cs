using System;
using System.Runtime.Serialization.Formatters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManMenuUIController : MonoBehaviour
{
    #region Title
    [Header("--- Title ---")]
    [SerializeField] Text UpTitleText;
    [SerializeField] Text BottomTitleText;
    [SerializeField] float flashingTime;
    [SerializeField] Color upBaseTitleColor;
    [SerializeField] Color upAfterTitleColor;
    [SerializeField] Color bottomBaseTitleColor;
    [SerializeField] Color bottomAfterTitleColor;
    [SerializeField] GameObject Projectile;
    [SerializeField] float moveSpeed = 50f;
    [SerializeField] Vector3 direction;
    WaitForSeconds waitForShoot = new WaitForSeconds(1f);
    Color tempColor;
    float t;
    float moveT;
    #endregion

    #region Menu

    [SerializeField] GameObject menu;
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject subMenu;
    [SerializeField] Button startGameButton;
    [SerializeField] Button settingButton;
    [SerializeField] Button operateButton;
    [SerializeField] Button exitButton;
    [SerializeField] Button surviveGameButton;
    [SerializeField] Button endlessGameButton;
    [SerializeField] Button BuffGameButton;
    [SerializeField] Button rankListButton;
    [SerializeField] Button backMainButton;

    [SerializeField] Vector3 mainPosition;
    [SerializeField] Vector3 subPosition;
    [SerializeField] Vector3 middlePosition;
    [SerializeField] float menuMoveTime = 1f;
    float menuT;
    RectTransform menuTransform;
    Coroutine menuCoroutine;

    bool isMainMenu;

    #endregion

    #region RankList

    #endregion

    #region BaseFunc
    void Awake()
    {
        tempColor.a = 1f;
        Time.timeScale = 1f;
        GameManager.GameState = GameState.GamePlaying;
    }

    void Start()
    {
        StartCoroutine(nameof(TitleCoroutine));
        StartCoroutine(nameof(ProjectileCoroutine));
    }

    void OnEnable()
    {
        menuTransform = menu.GetComponent<RectTransform>();
        OnPressedBehaviour.UIActionDict.Add(startGameButton.gameObject, OnStartGameButtonOn);
        OnPressedBehaviour.UIActionDict.Add(settingButton.gameObject, OnSettingButtonOn);
        OnPressedBehaviour.UIActionDict.Add(operateButton.gameObject, OnOperateButtonOn);
        OnPressedBehaviour.UIActionDict.Add(exitButton.gameObject, OnExitButtonOn);
        OnPressedBehaviour.UIActionDict.Add(surviveGameButton.gameObject, OnSurviveGameButtonOn);
        OnPressedBehaviour.UIActionDict.Add(endlessGameButton.gameObject, OnEndlessGameButtonOn);
        OnPressedBehaviour.UIActionDict.Add(rankListButton.gameObject, OnRankListButtonOn);
        OnPressedBehaviour.UIActionDict.Add(BuffGameButton.gameObject, OnBuffGameButtonOn);
        OnPressedBehaviour.UIActionDict.Add(backMainButton.gameObject, OnBackMainButtonOn);
        isMainMenu = true;
        SelectTheFirst();
    }

    void OnDisable()
    {
        startGameButton.onClick.RemoveAllListeners();
        StopAllCoroutines();
    }
    #endregion

    #region TitleFunc
    IEnumerator TitleCoroutine()
    {
        while (true)
        {
            t = 0f;
            while (t <= 1f)
            {
                t += Time.deltaTime / flashingTime;
                tempColor.r = Mathf.Lerp(upBaseTitleColor.r, upAfterTitleColor.r, t);
                tempColor.g = Mathf.Lerp(upBaseTitleColor.g, upAfterTitleColor.g, t);
                tempColor.b = Mathf.Lerp(upBaseTitleColor.b, upAfterTitleColor.b, t);
                UpTitleText.color = tempColor;
                tempColor.r = Mathf.Lerp(bottomBaseTitleColor.r, bottomAfterTitleColor.r, t);
                tempColor.g = Mathf.Lerp(bottomBaseTitleColor.g, bottomAfterTitleColor.g, t);
                tempColor.b = Mathf.Lerp(bottomBaseTitleColor.b, bottomAfterTitleColor.b, t);
                BottomTitleText.color = tempColor;
                yield return null;
            }
            t = 0f;

            while (t <= 1f)
            {
                t += Time.deltaTime / flashingTime;
                tempColor.r = Mathf.Lerp(upAfterTitleColor.r, upBaseTitleColor.r, t);
                tempColor.g = Mathf.Lerp(upAfterTitleColor.g, upBaseTitleColor.g, t);
                tempColor.b = Mathf.Lerp(upAfterTitleColor.b, upBaseTitleColor.b, t);
                UpTitleText.color = tempColor;
                tempColor.r = Mathf.Lerp(bottomAfterTitleColor.r, bottomBaseTitleColor.r, t);
                tempColor.g = Mathf.Lerp(bottomAfterTitleColor.g, bottomBaseTitleColor.g, t);
                tempColor.b = Mathf.Lerp(bottomAfterTitleColor.b, bottomBaseTitleColor.b, t);
                BottomTitleText.color = tempColor;
                yield return null;
            }
        }
    }
    IEnumerator ProjectileCoroutine()
    {
        while (true)
        {
            moveT = 0f;
            while (moveT <= 1f)
            {
                moveT += Time.deltaTime;
                Projectile.transform.Translate(direction * moveSpeed * Time.deltaTime);
                yield return null;
            }
            yield return waitForShoot;
            moveT = 0f;
            while (moveT <= 1f)
            {
                moveT += Time.deltaTime;
                Projectile.transform.Translate(direction * -1 * moveSpeed * Time.deltaTime);
                yield return null;
            }
            yield return waitForShoot;
        }
    }
    #endregion

    #region MenuFunc

    IEnumerator MenuMoveCoroutine(Vector3 startPos, Vector3 midPos, Vector3 endPos)
    {
        menuT = 0f;
        while (menuT <= 1f)
        {
            menuT += Time.deltaTime / menuMoveTime;
            menuTransform.anchoredPosition = BezierCurve.Instance.QuadraticBezierCurve(startPos, midPos, endPos, menuT);
            yield return null;
        }
        SelectTheFirst();
    }

    void ChangeToSubMenu()
    {
        if (menuCoroutine != null)
        {
            StopCoroutine(menuCoroutine);
        }
        isMainMenu = false;
        menuCoroutine = StartCoroutine(MenuMoveCoroutine(mainPosition, middlePosition, subPosition));
    }

    void ChangeToMainMenu()
    {
        if (menuCoroutine != null)
        {
            StopCoroutine(menuCoroutine);
        }
        isMainMenu = true;
        menuCoroutine = StartCoroutine(MenuMoveCoroutine(subPosition, middlePosition, mainPosition));
    }

    void SelectTheFirst()
    {
        if (isMainMenu)
        {
            MainMenuUIInput.Instance.SelectUI(startGameButton);
        }
        else
        {
            MainMenuUIInput.Instance.SelectUI(surviveGameButton);
        }
    }

    void OpenSetting()
    {
        Debug.Log("Click Setting");
    }

    void OpenRankList()
    {
        Debug.Log("Click RankList");
    }

    void OpenOperate()
    {
        Debug.Log("Click Operate");
    }

    void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    void OnStartGameButtonOn() => ChangeToSubMenu();

    void OnSettingButtonOn() => OpenSetting();

    void OnOperateButtonOn() => OpenOperate();

    void OnExitButtonOn() => ExitGame();

    void OnSurviveGameButtonOn() => ScenesLoadManager.Instance.LoadShootShoot(GameMode.Survive);

    void OnEndlessGameButtonOn() => ScenesLoadManager.Instance.LoadShootShoot(GameMode.Endless);

    void OnBuffGameButtonOn() => ScenesLoadManager.Instance.LoadShootShoot(GameMode.Buff);

    void OnRankListButtonOn() => OpenRankList();

    void OnBackMainButtonOn() => ChangeToMainMenu();

    #endregion

}
