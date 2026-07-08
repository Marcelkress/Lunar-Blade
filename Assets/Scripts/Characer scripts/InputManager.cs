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
    public bool attackOneWasReleased;
    public bool specialAttackPressed;

    private bool specialOneWasPressed, specialTwoWasPressed;
    private float specialOneTimer, specialTwoTimer;

    public static int playerCount = 0;
    public int layerMask;
    public int playerID;
    
    private AbilityChargeManager abilityChargeManager; 

    // God vars
    public bool canReceiveInput { get; private set; }

    private void Awake()
    {
        playerCount++;
        canReceiveInput = true;
        
        if (playerCount == 1)
        {
            playerID = 1;
            int layer = LayerMask.NameToLayer("Player 1");
            layerMask = layer;
            gameObject.layer = layer;
        }
        else if (playerCount == 2)
        {
            playerID = 2;
            int layer = LayerMask.NameToLayer("Player 2");
            layerMask = layer;
            gameObject.layer = layer;
        }
    }

    private void Start()
    {
        movement = GetComponent<CharacterMovement>();
        abilityChargeManager = GetComponentInChildren<AbilityChargeManager>();
    }

    private void Update()
    {
       SpecialAttackBuffer();
    }

    public void LockInput(bool lockInput)
    {
        canReceiveInput = !lockInput;
    }

    #region Movement
    
    public void OnMove(InputAction.CallbackContext context)
    {
        moveVector = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if(!canReceiveInput)
            return;
        if (context.started || context.performed)
        {
            //Debug.Log("Recognizing jump");
            //Debug.Log(Time.frameCount);
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
        if(!canReceiveInput)
            return;
        
        //Debug.Log("Dash");
        
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
    {if(!canReceiveInput)
            return;
        runIsHeld = context.performed;
    }

    

    #endregion

    #region Attack
    
    public void OnAttackOne(InputAction.CallbackContext context)
    {if(!canReceiveInput)
            return;
        if (context.performed)
        {
            attackOneWasPressed = true;
        }
        else if(context.canceled)
        {
            attackOneWasPressed = false;
            attackOneWasReleased = true;
        }
        StartCoroutine(ResetNextFrame(() => attackOneWasPressed = false));
        StartCoroutine(ResetNextFrame(() => attackOneWasReleased = false));
    }
    
    public void OnAttackTwo(InputAction.CallbackContext context)
    {if(!canReceiveInput)
            return;
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
    {if(!canReceiveInput)
            return;
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
    {if(!canReceiveInput)
            return;
        if (context.performed)
        {
            specialOneWasPressed = true;
            specialOneTimer = 0;
        }
    }

    public void OnSpecialAttackBindingTwo(InputAction.CallbackContext context)
    {if(!canReceiveInput)
            return;
        if (context.performed)
        {
            specialTwoWasPressed = true;
            specialTwoTimer = 0; 
        }
    }
    private void SpecialAttackBuffer()
    {
        if (specialOneWasPressed)
        {
            specialOneTimer += Time.deltaTime;
            if (specialOneTimer > movement.moveStats.specialAttackInputBuffer)
            {
                specialOneWasPressed = false;
            }
        }

        if (specialTwoWasPressed)
        {
            specialTwoTimer += Time.deltaTime;
            if (specialTwoTimer > movement.moveStats.specialAttackInputBuffer)
            {
                specialTwoWasPressed = false;
            }
        }

        // We can only perform ability if we are able to consume a charge

        if (specialOneWasPressed && specialTwoWasPressed)
        {
            if (abilityChargeManager.hasCharge)
            {
                specialAttackPressed = true;
                StartCoroutine(ResetNextFrame(() => specialAttackPressed = false));
                specialOneWasPressed = false;
                specialTwoWasPressed = false;
            }
        }
    }
    
    #endregion
    
    /// <summary>
    /// Routine that executes action after one frame
    /// </summary>
    /// <param name="resets"></param>
    /// <returns></returns>
    private IEnumerator ResetNextFrame(params Action[] resets)
    {
        yield return new WaitForEndOfFrame();
        foreach (var r in resets) r();
    }
}
