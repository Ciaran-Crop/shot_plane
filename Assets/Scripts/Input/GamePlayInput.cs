using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "GamePlay")]
public class GamePlayInput : ScriptableObject, InputActions.IGamePlayActions, InputActions.IPauseMenuActions
{
    InputActions inputActions;
    public event UnityAction<Vector2> onMove = delegate { };
    public event UnityAction onStopMove = delegate { };
    public event UnityAction onFire = delegate { };
    public event UnityAction onStopFire = delegate { };
    public event UnityAction onDodge = delegate { };

    public event UnityAction onOverdrive = delegate { };
    public event UnityAction onPause = delegate { };

    public event UnityAction onUnpause = delegate { };


    void OnEnable()
    {
        inputActions = new InputActions();
        inputActions.GamePlay.SetCallbacks(this);
        inputActions.PauseMenu.SetCallbacks(this);
    }

    void SwitchActionsMapTo(InputActionMap actionMap, bool isDisableCursor)
    {
        DisableAllInput();
        actionMap.Enable();
        if (isDisableCursor)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    void OnDisable() => DisableAllInput();
    public void SwitchToGamePlayInput() => SwitchActionsMapTo(inputActions.GamePlay, true);
    public void SwitchToPauseInput() => SwitchActionsMapTo(inputActions.PauseMenu, false);
    public void DisableAllInput() => inputActions.Disable();
    public void ChangeUpdateModeToFixed() => InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInFixedUpdate;
    public void ChangeUpdateModeToDynamic() => InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInDynamicUpdate;

    # region Event
    // Start is called before the first frame update
    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            onMove.Invoke(context.ReadValue<Vector2>());
        }

        if (context.phase == InputActionPhase.Canceled)
        {
            onStopMove.Invoke();
        }
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            onFire.Invoke();
        }

        if (context.phase == InputActionPhase.Canceled)
        {
            onStopFire.Invoke();
        }
    }

    public void OnDodge(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            onDodge.Invoke();
        }
    }

    public void OnOverdrive(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            onOverdrive.Invoke();
        }
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            onPause.Invoke();
        }
    }

    public void OnUnPause(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            onUnpause.Invoke();
        }
    }
    # endregion

}
