using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public Vector2 moveVector;
    public bool jumpWasPressed, jumpIsHeld, jumpWasReleased;
    public bool runIsHeld;
    public bool dashWasPressed;
    public bool simpleAttackWasPressed;
    
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

    public void OnSimpleAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            simpleAttackWasPressed = true;
        }
        else if(context.canceled)
        {
            simpleAttackWasPressed = false;
        }
        StartCoroutine(SimpleAttackWaitFrame());
    }
    
    private IEnumerator SimpleAttackWaitFrame()
    {
        yield return new WaitForEndOfFrame();
        simpleAttackWasPressed = false;
    }
}
