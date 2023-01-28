using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShootUIController : MonoBehaviour
{
    [SerializeField] GamePlayInput gamePlayInput;
    [SerializeField] GameOverController gameOverController;
    [SerializeField] Canvas gameCanvas;
    [SerializeField] Canvas pauseMenuCanvas;
    [SerializeField] Canvas waveUICanvas;
    [SerializeField] Canvas gameOverCanvas;
    [SerializeField] Button backGameButton;
    [SerializeField] Button optionsButton;
    [SerializeField] Button mainMenuButton;

    [SerializeField] AudioData backData;
    [SerializeField] AudioData pauseData;
    [SerializeField] AudioData confirmGameOverData;
    [SerializeField] Button cancelSettingButton;
    [SerializeField] Button confirmSettingButton;
    [SerializeField] Vector3 settingPosition;
    [SerializeField] Vector3 mainMiddlePosition;

    [SerializeField] float middlePositionZ = 100f;
    [SerializeField] float menuMoveTime = 1f;
    [SerializeField] GameObject menu;
    Animator gameOverAnimator;
    int PRESSED_ID = Animator.StringToHash("Pressed");
    int GameOverExit_ID = Animator.StringToHash("GameOverScreenExit");

    int menuState;
    float menuT;
    RectTransform menuTransform;
    Coroutine menuCoroutine;
    Vector3 middlePosition;

    void Awake()
    {
        Time.timeScale = 1f;
        GameManager.GameState = GameState.GamePlaying;
        middlePosition = new Vector3(0f, 0f, middlePositionZ);
    }

    void Start()
    {
        gameOverAnimator = gameOverController.Animator;
        gameOverAnimator.enabled = false;
        gameOverCanvas.enabled = false;
    }

    void OnEnable()
    {
        menuTransform = menu.GetComponent<RectTransform>();
        gamePlayInput.SwitchToGamePlayInput();
        gamePlayInput.onPause += Pause;
        gamePlayInput.onUnpause += Unpause;
        gamePlayInput.onGameOverConfirm += ConfirmGameOver;
        GameManager.onGameOver += OnGameOver;
        OnPressedBehaviour.UIActionDict.Add(backGameButton.gameObject.name, OnBackGameButtonClick);
        OnPressedBehaviour.UIActionDict.Add(optionsButton.gameObject.name, OnOptionsButtonClick);
        OnPressedBehaviour.UIActionDict.Add(mainMenuButton.gameObject.name, OnMainMenuButtonClick);
        OnPressedBehaviour.UIActionDict.Add(cancelSettingButton.gameObject.name, OnCancelSettingButtonClick);
        OnPressedBehaviour.UIActionDict.Add(confirmSettingButton.gameObject.name, OnConfirmSettingButtonClick);
    }

    void OnDisable()
    {
        gamePlayInput.onPause -= Pause;
        gamePlayInput.onUnpause -= Unpause;
        gamePlayInput.onGameOverConfirm += ConfirmGameOver;
        GameManager.onGameOver -= OnGameOver;
        OnPressedBehaviour.UIActionDict.Clear();
    }

    void Pause()
    {
        GameManager.SetGamePause();
        PlayerBulletTime.Instance.SetPause();
        gameCanvas.enabled = false;
        pauseMenuCanvas.enabled = true;
        waveUICanvas.enabled = false;
        gamePlayInput.SwitchToPauseInput();
        gamePlayInput.ChangeUpdateModeToDynamic();
        menuState = 1;
        SelectTheFirst();
        AudioManager.Instance.PlayRandomPitch(pauseData);
    }

    void Unpause()
    {
        backGameButton.Select();
        backGameButton.animator.SetTrigger(PRESSED_ID);
        AudioManager.Instance.PlayRandomPitch(backData);
    }

    void OnMainMenuButtonClick() => ScenesLoadManager.Instance.LoadMainMenu();
    void OnOptionsButtonClick() => ChangeToSetting();

    void OnBackGameButtonClick()
    {
        GameManager.CancelGamePause();
        PlayerBulletTime.Instance.CancelPause();
        gameCanvas.enabled = true;
        pauseMenuCanvas.enabled = false;
        waveUICanvas.enabled = true;
        gamePlayInput.SwitchToGamePlayInput();
        gamePlayInput.ChangeUpdateModeToFixed();
    }

    public void ChangeUIState(bool state)
    {
        if (gameCanvas.enabled == state) return;
        gameCanvas.enabled = state;
    }

    void OnGameOver()
    {
        gamePlayInput.DisableAllInput();
        gameOverCanvas.enabled = true;
        gameOverAnimator.enabled = true;
        GameManager.SetLastGameScore();
    }

    public void SwitchToGameOverInput() => gamePlayInput.SwitchToGameOverInput();

    public void ConfirmGameOver()
    {
        AudioManager.Instance.PlayRandomPitch(confirmGameOverData);
        gamePlayInput.DisableAllInput();
        gameOverAnimator.Play(GameOverExit_ID);
        ScenesLoadManager.Instance.LoadScore(); // Temp
    }

    void SelectTheFirst()
    {
        switch (menuState)
        {
            case 1:
                UIInput.Instance.SelectUI(backGameButton);
                break;
            case 2:
                UIInput.Instance.SelectUI(cancelSettingButton);
                break;
            default:
                break;
        }
    }

    IEnumerator MenuMoveCoroutine(Vector3 startPos, Vector3 endPos, float time)
    {
        menuT = 0f;
        middlePosition.x = (startPos.x + endPos.x) / 2;
        while (menuT <= 1f)
        {
            menuT += Time.unscaledDeltaTime / time;
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

    Vector3 GetNowPosition() => menuTransform.anchoredPosition3D;

    void ChangeToMainMenu() => MoveTo(GetNowPosition(), mainMiddlePosition, menuMoveTime, 1);

    void ChangeToSetting() => MoveTo(GetNowPosition(), settingPosition, menuMoveTime, 2);

    void CloseSetting(bool isSave)
    {
        // TODO
        ChangeToMainMenu();
    }

    void OnCancelSettingButtonClick() => CloseSetting(false);

    void OnConfirmSettingButtonClick() => CloseSetting(true);
}
