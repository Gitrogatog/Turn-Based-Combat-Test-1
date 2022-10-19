using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManagerScript : MonoBehaviour
{
    public static PlayerInputManagerScript instance;
    public event System.Action<GameInputs> OnUpdateInputs;
    GameInputs currentInputs;

    [Header("Movement Settings")]
    public bool analogMovement;

    [Header("Mouse Cursor Settings")]
    //public bool cursorLocked = true;
    public bool cursorInputForLook = true;

    void Awake()
    {
        instance = this;
        currentInputs = new GameInputs();
    }

    void FixedUpdate(){
        if(OnUpdateInputs != null){
            OnUpdateInputs(currentInputs);
        }
        currentInputs.confirm = false;
        currentInputs.cancel = false;
    }
    public void OnMove(InputValue value)
    {
        MoveInput(value.Get<Vector2>());
    }

    public void OnLook(InputValue value)
    {
        if(cursorInputForLook)
        {
            LookInput(value.Get<Vector2>());
        }
    }

    public void OnSelect(InputValue value)
    {
        ConfirmInput(value.isPressed);
    }

    public void OnCancel(InputValue value)
    {
        CancelInput(value.isPressed);
    }


    public void MoveInput(Vector2 newMoveDirection)
    {
        currentInputs.move = newMoveDirection;
    } 

    public void LookInput(Vector2 newLookDirection)
    {
        currentInputs.look = newLookDirection;
    }

    public void ConfirmInput(bool newConfirmState)
    {
        currentInputs.confirm = newConfirmState;
    }

    public void CancelInput(bool newConfirmState)
    {
        currentInputs.confirm = newConfirmState;
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        //SetCursorState(cursorLocked);
    }

    private void SetCursorState(bool newState)
    {
        Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
    }
}

public struct GameInputs{
    public Vector2 move;
    public Vector2 look;
    public bool confirm;
    public bool cancel;
}