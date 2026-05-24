using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public Vector2 moveVector;
    public bool jumpWasPressed, jumpIsHeld, jumpWasReleased;
    public bool runIsHeld;
    public bool dashWasPressed;
    public bool attackOneWasPressed, attackTwoWasPressed,
        attackThreeWasPressed;
    
    private bool specialOneWasPressed, specialTwoWasPressed;
    
    #region Movement
    public void OnMove(InputAction.CallbackContext context)
    {
        //Debug.Log("move");
        moveVector = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started || context.performed)
        {
            //Debug.Log("Recognizing jump");
            jumpWasPressed = true;
            jumpWasReleased = false;
        }
        else if (context.canceled)
        {
            jumpWasPressed = false;
            jumpWasReleased = true;
        }

        StartCoroutine(JumpWaitFrame());
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        //dashIsHeld = context.performed;
        
        if (context.performed)
        {
            dashWasPressed = true;
        }
        else if(context.canceled)
        {
            dashWasPressed = false;
        }
        StartCoroutine(DashWaitFrame());
    }
    
    private IEnumerator DashWaitFrame()
    {
        yield return new WaitForEndOfFrame();
        dashWasPressed = false;
    }

    private IEnumerator JumpWaitFrame()
    {
        yield return new WaitForEndOfFrame();
        jumpWasPressed = false;
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        runIsHeld = context.performed;
    }

    

    #endregion

    #region Attack
    
    public void OnAttackOne(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            attackOneWasPressed = true;
        }
        else if(context.canceled)
        {
            attackOneWasPressed = false;
        }
        StartCoroutine(SimpleAttackWaitFrame());
    }
    
    public void OnAttackTwo(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            attackTwoWasPressed = true;
        }
        else if(context.canceled)
        {
            attackTwoWasPressed = false;
        }
        StartCoroutine(SimpleAttackWaitFrame());
    }
    
    public void OnAttackThree(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            attackThreeWasPressed = true;
        }
        else if(context.canceled)
        {
            attackThreeWasPressed = false;
        }
        StartCoroutine(SimpleAttackWaitFrame());
    }

    
    private IEnumerator SimpleAttackWaitFrame()
    {
        yield return new WaitForEndOfFrame();
        attackOneWasPressed = attackTwoWasPressed = attackThreeWasPressed = false;
    }
    
    public void OnSpecialAttackBindingOne(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            specialOneWasPressed = true;
        }
        else if(context.canceled)
        {
            specialOneWasPressed = false;
        }
    }
    
    public void OnSpecialAttackBindingTwo(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            specialTwoWasPressed = true;
        }
        else if(context.canceled)
        {
            specialTwoWasPressed = false;
        }
    }

    #endregion
    
}
