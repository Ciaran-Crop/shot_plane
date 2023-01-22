using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class MainMenuUIInput : Singleton<MainMenuUIInput>
{
    InputSystemUIInputModule inputSystemUIInputModule;
    protected override void Awake()
    {
        base.Awake();
        inputSystemUIInputModule = GetComponent<InputSystemUIInputModule>();
        inputSystemUIInputModule.enabled = false;
    }

    public void SelectUI(Selectable UIObject)
    {
        UIObject.Select();
        UIObject.OnSelect(null);
        inputSystemUIInputModule.enabled = true;
    }
}
