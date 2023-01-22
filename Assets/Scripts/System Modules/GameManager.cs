using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : PersistentSingleton<GameManager>
{
    public static GameState GameState { get => Instance.gameState; set => Instance.gameState = value; }
    GameState gameState = GameState.GamePlaying;
    GameState lastGameState = GameState.GamePlaying;

    public static GameMode GameMode { get => Instance.gameMode; set => Instance.gameMode = value; }
    GameMode gameMode = GameMode.Survive;

    public static bool IsGameOver => Instance.gameState == GameState.GameOver;
    public static bool IsGamePause => Instance.gameState == GameState.GamePause;
    public static bool IsGamePlaying => Instance.gameState == GameState.GamePlaying;

    public static void SetGameState(GameState state)
    {
        Instance.lastGameState = Instance.gameState;
        Instance.gameState = state;
    }

    public static void SetGamePause()
    {
        SetGameState(GameState.GamePause);
    }

    public static void CancelGamePause()
    {
        Instance.gameState = Instance.lastGameState;
        Instance.lastGameState = GameState.GamePause;
    }

}

public enum GameState
{
    GamePlaying,
    GamePause,
    GameOver,
}

public enum GameMode
{
    Survive,
    Endless,
    Buff,
}
