using System;
using UnityEngine;

public class DeflectAbility : MonoBehaviour
{
    private PlayerHealth health;
    private InputManager inputManager;
    private bool deflectPressed, canDeflect;
    public bool currentlyDeflecting;
    
    [Header("Deflection settings")] 
    public float deflectTime = 0.2f;
    public float cooldownTime = 0.8f;

    private float deflectionTimer;
    private float cooldownTimer;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inputManager = GetComponentInParent<InputManager>();
        canDeflect = true;
    }
    
    void Update()
    {
        deflectPressed = inputManager.deflectPressed;

        if (canDeflect && !currentlyDeflecting && deflectPressed)
        {
            Debug.Log("Deflect");
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
