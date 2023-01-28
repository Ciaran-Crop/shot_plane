using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ScoreElementController : MonoBehaviour
{
    [SerializeField] Text rankText;
    [SerializeField] Image photoImage;
    [SerializeField] Text nameText;
    [SerializeField] Text scoreText;
    RectTransform rectTransform;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void Set(string rank, string name, string score)
    {
        rankText.text = rank;
        nameText.text = name;
        scoreText.text = score;
        // ResetRectTransform();
    }

    #region  Useless
    void ResetRectTransform()
    {
        rectTransform.anchoredPosition3D.Set(rectTransform.anchoredPosition3D.x, rectTransform.anchoredPosition3D.y, 0f);
        rectTransform.localScale = Vector3.one;
    }
    #endregion

    #region  BUG
    // ! import Bug
    public void Set(string rank, string image, string name, string score)
    {
        rankText.text = rank;
        nameText.text = name;
        scoreText.text = score;
        GetImage(image);
    }

    void GetImage(string url)
    {
        StartCoroutine(GetImageCoroutine(url));
    }


    IEnumerator GetImageCoroutine(string url)
    {
        UnityWebRequest unityWebRequest = new UnityWebRequest(url);
        DownloadHandlerTexture downloadHandlerTexture = new DownloadHandlerTexture(true);
        unityWebRequest.downloadHandler = downloadHandlerTexture;

        yield return unityWebRequest.SendWebRequest();
        while (!unityWebRequest.isDone) yield return null;

        if (unityWebRequest.result == UnityWebRequest.Result.Success)
        {
            SetImage(downloadHandlerTexture.texture);
        }
        else
        {
            Debug.Log("Error in :" + unityWebRequest.error);
        }
    }


    void SetImage(Texture2D texture)
    {
        Sprite temp = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0));
        photoImage.sprite = temp;
        // ResetRectTransform();
    }

    #endregion

}
