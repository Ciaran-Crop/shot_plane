using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonEventTrigger :
        MonoBehaviour,
        IPointerEnterHandler,
        IPointerDownHandler,
        ISelectHandler,
        ISubmitHandler
{
    [SerializeField] AudioData selectData;
    [SerializeField] AudioData submitData;
    public void OnSelect(BaseEventData eventData)
    {
        AudioManager.Instance.PlayRandomPitch(selectData);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        AudioManager.Instance.PlayRandomPitch(submitData);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        AudioManager.Instance.PlayRandomPitch(selectData);
    }

    public void OnSubmit(BaseEventData eventData)
    {
        AudioManager.Instance.PlayRandomPitch(submitData);
    }
}
