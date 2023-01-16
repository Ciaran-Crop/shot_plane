using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "GamePlay")]
public class GamePlayInput : ScriptableObject, InputActions.IGamePlayActions
{
    InputActions inputActions;
    public event UnityAction<Vector2> onMove = delegate {};
    public event UnityAction onStopMove = delegate {};
    public event UnityAction onFire = delegate {};
    public event UnityAction onStopFire = delegate {};

    void OnEnable()
    {
        inputActions = new InputActions();
        inputActions.GamePlay.SetCallbacks(this);
    }

    void OnDisable() {
        DisableGamePlayInput();
    }

    public void EnableGamePlayInput()
    {
        inputActions.GamePlay.Enable();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void DisableGamePlayInput()
    {
        inputActions.GamePlay.Disable();
    }

    // Start is called before the first frame update
    public void OnMove(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Performed)
        {
            onMove.Invoke(context.ReadValue<Vector2>());
        }

        if(context.phase == InputActionPhase.Canceled)
        {
            onStopMove.Invoke();
        }
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Performed)
        {
            onFire.Invoke();
        }

        if(context.phase == InputActionPhase.Canceled)
        {
            onStopFire.Invoke();
        }
    }
}
