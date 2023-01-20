using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShootUIController : MonoBehaviour
{
    [SerializeField] GamePlayInput gamePlayInput;
    [SerializeField] Canvas gameCanvas;
    [SerializeField] Canvas pauseMenuCanvas;
    [SerializeField] Canvas waveUICanvas;
    [SerializeField] Button backGameButton;
    [SerializeField] Button optionsButton;
    [SerializeField] Button mainMenuButton;

    [SerializeField] AudioData backData;
    [SerializeField] AudioData pauseData;

    int PRESSED_ID = Animator.StringToHash("Pressed");

    void OnEnable()
    {
        gamePlayInput.onPause += Pause;
        gamePlayInput.onUnpause += Unpause;
        OnPressedBehaviour.UIActionDict.Add(backGameButton.gameObject, OnBackGameButtonClick);
        OnPressedBehaviour.UIActionDict.Add(optionsButton.gameObject, OnOptionsButtonClick);
        OnPressedBehaviour.UIActionDict.Add(mainMenuButton.gameObject, OnMainMenuButtonClick);
    }

    void OnDisable()
    {
        gamePlayInput.onPause -= Pause;
        gamePlayInput.onUnpause -= Unpause;
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
        ShootUIInput.Instance.SelectUI(backGameButton);
        AudioManager.Instance.PlayRandomPitch(pauseData);
    }

    void Unpause()
    {
        backGameButton.Select();
        backGameButton.animator.SetTrigger(PRESSED_ID);
        AudioManager.Instance.PlayRandomPitch(backData);
    }

    void OnMainMenuButtonClick()
    {
        ScenesLoadManager.Instance.LoadMainMenu();
    }

    void OnOptionsButtonClick()
    {
        OnBackGameButtonClick();
    }

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
}
