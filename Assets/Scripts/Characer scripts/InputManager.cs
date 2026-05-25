using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private CharacterMovement movement;
    public Vector2 moveVector;
    public bool jumpWasPressed, jumpIsHeld, jumpWasReleased;
    public bool runIsHeld;
    public bool dashWasPressed;
    public bool attackOneWasPressed, attackTwoWasPressed,
        attackThreeWasPressed;
    public bool specialAttackPressed;
    
    private bool specialOneWasPressed, specialTwoWasPressed;
    private float timer;
    private bool countTimer;

    private void Start()
    {
        movement = GetComponent<CharacterMovement>();
    }

    private void Update()
    {
       SpecialAttackBuffer();
    }

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
        StartCoroutine(ResetNextFrame(() => attackOneWasPressed = false));
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
        StartCoroutine(ResetNextFrame(() => attackTwoWasPressed = false));
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
        StartCoroutine(ResetNextFrame(() => attackThreeWasPressed = false));
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

        //StartCoroutine(ResetNextFrame(() => specialOneWasPressed = false));
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
        
        //StartCoroutine(ResetNextFrame(() => specialTwoWasPressed = false));
    }

    private void SpecialAttackBuffer()
    {
        if (specialOneWasPressed || specialTwoWasPressed)
        {
            countTimer = true;
        }

        if (countTimer)
        {
            timer += Time.deltaTime;
            Debug.Log("Within buffer");
            
            if (specialOneWasPressed && specialTwoWasPressed && timer < movement.moveStats.specialAttackInputBuffer)
            {
                specialAttackPressed = true;
                //Debug.Log("Special Attack!");
                StartCoroutine(ResetNextFrame(() => specialAttackPressed = false));
                //StartCoroutine(ResetNextFrame(() => specialOneWasPressed = false));
                //StartCoroutine(ResetNextFrame(() => specialTwoWasPressed = false));
            }
            else if (timer > movement.moveStats.specialAttackInputBuffer)
            {
                countTimer = false;
                timer = 0;
            }
        }
    }
    
    #endregion
    
    // Routine that resets bool after a frame
    private IEnumerator ResetNextFrame(params Action[] resets)
    {
        yield return new WaitForEndOfFrame();
        foreach (var r in resets) r();
    }
}
