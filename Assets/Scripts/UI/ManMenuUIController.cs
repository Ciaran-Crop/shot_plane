using System.Collections;
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

    [SerializeField] Vector3 settingPosition;
    [SerializeField] Vector3 operatePosition;
    [SerializeField] Vector3 mainMiddlePosition;
    [SerializeField] Vector3 subMiddlePosition;
    [SerializeField] Vector3 rankListPosition;
    [SerializeField] float middlePositionZ = 100f;
    [SerializeField] float menuMoveTime = 1f;
    [SerializeField] float rankListTime = 0.5f;
    [SerializeField] float operateTime = 0.5f;
    [SerializeField] Button cancelRankListButton;
    [SerializeField] Button confirmOperateButton;
    [SerializeField] Button cancelSettingButton;
    [SerializeField] Button confirmSettingButton;
    float menuT;
    RectTransform menuTransform;
    Coroutine menuCoroutine;
    Vector3 middlePosition;

    int menuState;

    #endregion

    #region RankList

    #endregion

    #region BaseFunc
    void Awake()
    {
        tempColor.a = 1f;
        Time.timeScale = 1f;
        middlePosition = new Vector3(0f, 0f, middlePositionZ);
        GameManager.GameState = GameState.GameMenu;
    }

    void Start()
    {
        StartCoroutine(nameof(TitleCoroutine));
        StartCoroutine(nameof(ProjectileCoroutine));
    }

    void OnEnable()
    {
        menuTransform = menu.GetComponent<RectTransform>();
        OnPressedBehaviour.UIActionDict.Add(startGameButton.gameObject.name, OnStartGameButtonOn);
        OnPressedBehaviour.UIActionDict.Add(settingButton.gameObject.name, OnSettingButtonOn);
        OnPressedBehaviour.UIActionDict.Add(operateButton.gameObject.name, OnOperateButtonOn);
        OnPressedBehaviour.UIActionDict.Add(exitButton.gameObject.name, OnExitButtonOn);
        OnPressedBehaviour.UIActionDict.Add(surviveGameButton.gameObject.name, OnSurviveGameButtonOn);
        OnPressedBehaviour.UIActionDict.Add(endlessGameButton.gameObject.name, OnEndlessGameButtonOn);
        // OnPressedBehaviour.UIActionDict.Add(rankListButton.gameObject.name, OnRankListButtonOn);
        OnPressedBehaviour.UIActionDict.Add(BuffGameButton.gameObject.name, OnBuffGameButtonOn);
        OnPressedBehaviour.UIActionDict.Add(backMainButton.gameObject.name, OnBackMainButtonOn);
        OnPressedBehaviour.UIActionDict.Add(cancelRankListButton.gameObject.name, OnCancelRankListButtonOn);
        OnPressedBehaviour.UIActionDict.Add(confirmOperateButton.gameObject.name, OnConfirmOperateButtonOn);
        OnPressedBehaviour.UIActionDict.Add(cancelSettingButton.gameObject.name, OnCancelSettingButtonOn);
        OnPressedBehaviour.UIActionDict.Add(confirmSettingButton.gameObject.name, OnConfirmSettingButtonOn);
        menuState = 1;
        SelectTheFirst();
    }

    void OnDisable()
    {
        OnPressedBehaviour.UIActionDict.Clear();
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
    IEnumerator MenuMoveCoroutine(Vector3 startPos, Vector3 endPos, float time)
    {
        menuT = 0f;
        middlePosition.x = (startPos.x + endPos.x) / 2;
        while (menuT <= 1f)
        {
            menuT += Time.deltaTime / time;
            menuTransform.anchoredPosition = BezierCurve.Instance.QuadraticBezierCurve(startPos, middlePosition, endPos, menuT);
            yield return null;
        }
        SelectTheFirst();
    }

    void MoveTo(Vector3 nowPos, Vector3 targetPos, float transformTime, int menuStateChange)
    {
        if (menuCoroutine != null)
        {
            StopCoroutine(menuCoroutine);
        }
        menuState = menuStateChange;
        menuCoroutine = StartCoroutine(MenuMoveCoroutine(nowPos, targetPos, transformTime));
    }

    void SelectTheFirst()
    {
        switch (menuState)
        {
            case 1:
                UIInput.Instance.SelectUI(startGameButton);
                break;
            case 2:
                UIInput.Instance.SelectUI(surviveGameButton);
                break;
            case 3:
                UIInput.Instance.SelectUI(cancelRankListButton);
                break;
            case 4:
                UIInput.Instance.SelectUI(confirmOperateButton);
                break;
            case 5:
                UIInput.Instance.SelectUI(cancelSettingButton);
                break;
            default:
                break;
        }
    }

    Vector3 GetNowPosition() => menuTransform.anchoredPosition3D;

    void ChangeToSubMenu() => MoveTo(GetNowPosition(), subMiddlePosition, menuMoveTime, 2);

    void ChangeToMainMenu() => MoveTo(GetNowPosition(), mainMiddlePosition, menuMoveTime, 1);

    void ChangeToSetting() => MoveTo(GetNowPosition(), settingPosition, menuMoveTime, 5);

    void CloseSetting(bool isSave)
    {
        // TODO
        ChangeToMainMenu();
    }

    void ChangeToRankList() => MoveTo(GetNowPosition(), rankListPosition, rankListTime, 3);

    void CloseRankList() => ChangeToSubMenu();

    void ChangeToOperate() => MoveTo(GetNowPosition(), operatePosition, operateTime, 4);

    void ConfirmOperate() => ChangeToMainMenu();
    void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    void OnStartGameButtonOn() => ChangeToSubMenu();

    void OnSettingButtonOn() => ChangeToSetting();

    void OnOperateButtonOn() => ChangeToOperate();

    void OnExitButtonOn() => ExitGame();

    void OnSurviveGameButtonOn() => ScenesLoadManager.Instance.LoadShootShoot(GameMode.Survive);

    void OnEndlessGameButtonOn() => ScenesLoadManager.Instance.LoadShootShoot(GameMode.Endless);

    void OnBuffGameButtonOn() => ScenesLoadManager.Instance.LoadShootShoot(GameMode.Buff);

    void OnRankListButtonOn() => ChangeToRankList();

    void OnCancelRankListButtonOn() => CloseRankList();

    void OnBackMainButtonOn() => ChangeToMainMenu();

    void OnConfirmOperateButtonOn() => ConfirmOperate();

    void OnCancelSettingButtonOn() => CloseSetting(false);

    void OnConfirmSettingButtonOn() => CloseSetting(true);

    #endregion

}
