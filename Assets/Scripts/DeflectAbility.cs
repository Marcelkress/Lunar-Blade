using System;
using UnityEngine;
using UnityEngine.Events;

public class DeflectAbility : MonoBehaviour
{
    private PlayerHealth health;
    private InputManager inputManager;
    private bool deflectPressed, canDeflect;
    private CharacterStats stats;
    private CharacterMovement movement;
    public bool currentlyDeflecting;
    
    private float deflectTime = 0.2f;
    private float cooldownTime = 0.8f;

    private float deflectionTimer;
    private float cooldownTimer;

    public UnityEvent deflectEvent;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        movement = GetComponentInParent<CharacterMovement>();
        stats = movement.moveStats;
        inputManager = GetComponentInParent<InputManager>();
        canDeflect = true;
        deflectTime = stats.deflectTime;
        cooldownTime = stats.deflectCoolDownTime;   
    }
    
    void Update()
    {
        deflectPressed = inputManager.deflectPressed;

        if (canDeflect && !currentlyDeflecting && deflectPressed && movement.isGrounded)
        {
            deflectEvent?.Invoke();
            deflectionTimer = deflectTime;
            cooldownTimer = cooldownTime + deflectTime;
        }
    }

    private void FixedUpdate()
    {
        deflectionTimer -= Time.fixedDeltaTime;
        cooldownTimer -= Time.fixedDeltaTime;
        
        if (deflectionTimer <= 0)
        {
            currentlyDeflecting = false;
        }
        else
        {
            currentlyDeflecting = true;
        }

        if (cooldownTimer <= 0)
        {
            canDeflect = true;
        }
        else
        {
            canDeflect = false;
        }
    }

    public bool IsDeflecting()
    {
        return currentlyDeflecting;
    }
}
