using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManMenuUIController : MonoBehaviour
{
    [SerializeField] Button startGameButton;

    void Start()
    {
        Time.timeScale = 1f;
        GameManager.GameState = GameState.GamePlaying;
    }

    void OnEnable()
    {
        OnPressedBehaviour.UIActionDict.Add(startGameButton.gameObject, OnStartGameButtonOn);
    }

    void OnDisable()
    {
        startGameButton.onClick.RemoveAllListeners();
    }   

    void OnStartGameButtonOn()
    {
        ScenesLoadManager.Instance.LoadShootShoot();
    }
}
