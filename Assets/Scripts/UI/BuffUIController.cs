using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuffUIController : MonoBehaviour
{
    [SerializeField] Text descriptionText;
    [SerializeField] Text valueText;

    void Awake()
    {
        GetComponent<Canvas>().worldCamera = Camera.main;
        Hide();
    }

    public virtual void UpdateText(string description, string value)
    {
        descriptionText.text = description;
        valueText.text = value;
    }

    public void Show() => gameObject.SetActive(true);

    public void Hide() => gameObject.SetActive(false);
}
