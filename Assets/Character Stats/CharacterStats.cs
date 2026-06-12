using System;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterMovementStats", menuName = "Scriptable Objects/CharacterMovementStats")]
public class CharacterStats : ScriptableObject
{
    [Header("Walk")] 
    [Range(1f, 100f)] public float maxWalkSpeed = 12f;
    [Range(0.2f, 50f)] public float groundAcceleration = 5f;
    [Range(.2f, 50f)] public float groundDeceleration = 20f;
    [Range(.2f, 50f)] public float airAcceleration = 5f;
    [Range(.2f, 50f)] public float airDeceleration = 5f;
    
    [Header("Run")]
    [Range(1f, 100f)] public float maxRunSpeed = 20f;
    public bool onlyRunning = true;

    [Header("Ground/Collision checks")] 
    public LayerMask groundLayer;
    public float groundDetectionRayLength = 0.02f, headDetectionRayLength = .02f;
    [Range(0f, 1f)] public float headWidth = 0.75f;
    public bool debugShowGroundedBox = true;

    [Header("Jump")]
    public float JumpHeight = 6.5f;
    [Range(1f, 1.1f)] public float JumpHeightCompensationFactor = 1.054f;
    public float TimeTillJumpApex = 0.35f;
    [Range(0.01f, 5f)] public float GravityOnReleaseMultiplier = 2f;
    public float MaxFallSpeed = 26f;
    [Range(1, 5)] public int NumberOfJumpsAllowed = 2;
    
    [Header(" Jump Cut")]
    [Range(0.02f, 0.3f)] public float TimeForUpwardsCancel = 0.027f;
    
    [Header (" Jump Apex")]
    [Range(0.5f, 1f)] public float ApexThreshold = 0.97f;
    [Range(0.01f, 1f)] public float ApexHangTime = 0.075f;
    
    [Header ("Jump Buffer")]
    [Range(0f, 1f)] public float JumpBufferTime = 0.125f;
    
    [Header(" Jump Coyote Time")]
    [Range(0, 1f)] public float JumpCoyoteTime = 0.1f;

    [Header("Dash")] 
    public float dashSpeed = 20;
    public float dashDuration = 0.2f;
    public float betweenDashInterval = .8f;
    [Range(20f, 200f)] public float dashAcceleration = 100;

    [Header("Health")] 
    public int maxHealth;
    
    [Header("Attack")] 
    public float specialAttackInputBuffer = 0.2f;
    [Range(0f, 1f)]public float attackInputBuffer = 0.5f;
    [Range(0.1f, 20f)]public float attackDecelerationMultiplier = 0.2f;

    [Header("Combat")] 
    public int attackOneDmg = 1;
    public int attackTwoDmg = 2, attackThreeDmg = 3;
    public int specialAttackDmg = 4;
    [Range(0.01f, 1)] public float takeHitStaggerTime = 0.2f;
    [Range(0.01f, 1)] public float invulnerabilityTimeAfterHit = 0.2f;
    
    
    [Header ("Debug" )]
    public bool DebugShowIsGroundedBox; public bool DebugShowHeadBumpBox;
    
    public float gravity { get; private set; }
    public float initialJumpVelocity { get; private set; }
    public float adjustedJumpHeight { get; private set; }
    
    private void OnValidate()
    {
        CalculateValues();
    }

    private void OnEnable()
    {
        CalculateValues();
    }

    private void CalculateValues()
    {
        adjustedJumpHeight = JumpHeight * JumpHeightCompensationFactor;
        gravity = -(2f * adjustedJumpHeight) / Mathf.Pow(TimeTillJumpApex, 2f);
        initialJumpVelocity = Mathf.Abs(gravity) * TimeTillJumpApex;
    }
}
