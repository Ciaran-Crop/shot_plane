using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScenesLoadManager : PersistentSingleton<ScenesLoadManager>
{
    const string SHOOTSHOOT = "ShootShoot";
    const string MAIN_MENU = "MainMenu";
    [SerializeField] Image fadeBackImage;
    [SerializeField] float fadeTime = 2f;
    Color color;
    void Load(string loadScene)
    {
        // SceneManager.LoadScene(loadScene);
        StopAllCoroutines();
        StartCoroutine(LoadSceneFadeInOut(loadScene));
    }

    IEnumerator LoadSceneFadeInOut(string sceneName)
    {
        fadeBackImage.gameObject.SetActive(true);
        var loadAsyncOperation = SceneManager.LoadSceneAsync(sceneName);
        loadAsyncOperation.allowSceneActivation = false;

        while (color.a < 1f)
        {
            color.a = Mathf.Clamp01(color.a + Time.unscaledDeltaTime / fadeTime);
            fadeBackImage.color = color;
            yield return null;
        }

        yield return new WaitUntil(() => loadAsyncOperation.progress >= 0.9f);

        loadAsyncOperation.allowSceneActivation = true;
        while (color.a > 0f)
        {
            color.a = Mathf.Clamp01(color.a - Time.unscaledDeltaTime / fadeTime);
            fadeBackImage.color = color;
            yield return null;
        }

        fadeBackImage.gameObject.SetActive(false);
    }

    public void LoadShootShoot()
    {
        Load(SHOOTSHOOT);
    }

    public void LoadShootShoot(GameMode gameMode)
    {
        GameManager.GameMode = gameMode;
        Load(SHOOTSHOOT);
    }

    public void LoadMainMenu()
    {
        Load(MAIN_MENU);
    }
}
