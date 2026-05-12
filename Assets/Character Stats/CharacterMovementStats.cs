using System;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterMovementStats", menuName = "Scriptable Objects/CharacterMovementStats")]
public class CharacterMovementStats : ScriptableObject
{
    [Header("Walk")] 
    [Range(0f, 1f)] public float moveThreshold = 0.25f;
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
    [Range(1, 200f)]public float dashSpeed = 20;
    [Range(0, 1)]public float dashTime = 0.2f;
    [Range(0, 1)]public float timeBetweenDashOnGround = 0.2f;
    public bool resetOnWallSlide = true;
    [Range(0, 4)]public int numberOfDashes = 1;
    [Range(0, .5f)]public float dashDiagonalBias = 0.2f;
    [Range(0.02f, .5f)]public float dashDiagonallyBias = 0.4f;
    
    [Header("Dash Cancel Time")]
    [Range(0.01f, 5)] public float dashGravityOnReleaseMultiplier = 1f;
    [Range(0.02f, 0.4f)] public float dashTimeForUpwardsCancel = 0.027f;
    
    [Header("Simple Attack")] 
    public int damage;
    
    [Header ("Debug" )]
    public bool DebugShowIsGroundedBox; public bool DebugShowHeadBumpBox;
    
    public float gravity { get; private set; }
    public float initialJumpVelocity { get; private set; }
    public float adjustedJumpHeight { get; private set; }

    public readonly Vector2[] dashDirections = new Vector2[]
    {
        new Vector2(0, 0), // nothing
        new Vector2(1, 0), // right
        new Vector2(1, 1), // top right
        new Vector2(0, 1), // up
        new Vector2(-1, 1), // top left
        new Vector2(-1, 0), // left
        new Vector2(-1, -1), // bottom left
        new Vector2(0, -1), // down
        new Vector2(1, -1), // bottom right
    };
    
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
