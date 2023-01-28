using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUIController : MonoBehaviour
{
    [SerializeField] Text modeText;
    [SerializeField] Text scoreText;
    [SerializeField] Button playAgainButton;
    [SerializeField] Button backMainButton;
    [SerializeField] Button shareButton;

    float gameScore;
    GameMode gameMode;

    #region ButtonActions

    void OnPlayAgainButtonClick()
    {
        ScenesLoadManager.Instance.LoadShootShoot(GameManager.GameMode);
    }

    void OnBackMainButtonClick()
    {
        ScenesLoadManager.Instance.LoadMainMenu();
    }

    void OnShareButtonClick()
    {
        Application.OpenURL("https://app3774.acapp.acwing.com.cn/");
        ScenesLoadManager.Instance.LoadMainMenu();
    }

    #endregion

    void Awake()
    {
        Time.timeScale = 1f;
        GameManager.GameState = GameState.GameScore;
        gameScore = GameManager.LastGameScore;
        gameMode = GameManager.GameMode;
    }

    void UpdateState()
    {
        modeText.text = modeText.text + gameMode.ToString();
        scoreText.text = scoreText.text + gameScore.ToString();
    }

    void OnEnable()
    {
        OnPressedBehaviour.UIActionDict.Add(playAgainButton.gameObject.name, OnPlayAgainButtonClick);
        OnPressedBehaviour.UIActionDict.Add(backMainButton.gameObject.name, OnBackMainButtonClick);
        OnPressedBehaviour.UIActionDict.Add(shareButton.gameObject.name, OnShareButtonClick);
        UIInput.Instance.SelectUI(playAgainButton);
        UpdateState();
    }

    void OnDisable()
    {
        OnPressedBehaviour.UIActionDict.Clear();
    }
}
