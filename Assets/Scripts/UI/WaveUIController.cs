using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveUIController : MonoBehaviour
{
    [SerializeField] Text waveText;

    void Awake()
    {
        GetComponent<Canvas>().worldCamera = Camera.main;
    }

    public virtual void UpdateText(int value)
    {
        waveText.text = string.Format("-  Wave  {0}  - ", value);
    }
}
