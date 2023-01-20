using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatSystemEffect : MonoBehaviour
{
    [SerializeField] Image restoreImage;
    [SerializeField] Image doTImage;

    void Awake()
    {
        restoreImage.gameObject.SetActive(false);
        doTImage.gameObject.SetActive(false);
        if (TryGetComponent<Canvas>(out Canvas canvas))
        {
            canvas = GetComponent<Canvas>();
            canvas.worldCamera = Camera.main;
        }
    }

    public void SetRestoreImage(bool isShow)
    {
        restoreImage.gameObject.SetActive(isShow);
    }

    public void SetDoTImage(bool isShow)
    {
        doTImage.gameObject.SetActive(isShow);
    }
}
