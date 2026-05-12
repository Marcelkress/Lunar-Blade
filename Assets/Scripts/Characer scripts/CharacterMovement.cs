using System;
using System.Collections;
using System.Timers;
using UnityEngine;
using UnityEngine.TextCore.LowLevel;

public class CharacterMovement : MonoBehaviour
{
    [Header("References")] 
    public CharacterMovementStats moveStats;
    [SerializeField] private Collider2D feetColl, bodyColl;

    private Rigidbody2D rb;
    private InputManager inputManager;
    
    // Movement
    [HideInInspector] public float horizontalVelocity;
    private bool isFacingRight;
    
    //Collision check
    private RaycastHit2D groundHit, headHit;
    [HideInInspector] public bool isGrounded, bumpedHead;

    // jump vars
    public float VerticalVelocity { get; private set; } 
    private bool isJumping; 
    private bool _isFastFalling; 
    private bool _isFalling; 
    private float fastFallTime;
    private float _fastFallReleaseSpeed; 
    private int numberOfJumpsUsed;
    
    //apex vars
    private float apexPoint;
    private float _timePastApexThreshold; private bool _isPastApexThreshold;
    
    //jump buffer vars
    private float jumpBufferTimer;
    private bool jumpReleasedDuringBuffer;
    
    //coyote time vars
    private float coyoteTimer;
    
    // dash vars
    public bool canDash;
    private bool isDashing;
    private bool isAirDashing;
    private float dashTimer, dashOnGroundTimer;
    private int numberOfDashesUsed;
    private Vector2 dashDirection;
    private bool isDashFastFalling;
    private float dashFastFallTime, dashFastFallingReleaseSpeed;
    
    private void Awake()
    {
        isFacingRight = true;

        inputManager = GetComponent<InputManager>();
        rb = GetComponent<Rigidbody2D>();
    }
    
    private void Update()
    {
        CountTimers();
        JumpChecks();
        LandCheck();
        DashCheck();
    }
    
    private void FixedUpdate()
    {
        CollisionCheck();
        Jump();
        Fall();
        Dash();
        
        if (isGrounded)
        {
            Move(moveStats.groundAcceleration, moveStats.groundDeceleration, inputManager.moveVector);
        }
        else
        {
            Move(moveStats.airAcceleration, moveStats.airDeceleration, inputManager.moveVector);
        }
        
        ApplyVelocity();
    }

    private void ApplyVelocity()
    {
        // clamp fall speed;
        VerticalVelocity = Mathf.Clamp(VerticalVelocity, -moveStats.MaxFallSpeed, 50f);
        rb.linearVelocity = new Vector2(horizontalVelocity, VerticalVelocity);
    }
    
    #region  Move
    
    private void Move(float acceleration, float deceleration, Vector2 moveInput)
    {
        if (Mathf.Abs(moveInput.x) >= moveStats.moveThreshold) 
        {
            TurnCheck(moveInput);
            
            float targetVelocity = 0;

            if (inputManager.runIsHeld || moveStats.onlyRunning)
            {
                targetVelocity = moveInput.x * moveStats.maxRunSpeed;
            }
            else
            {
                targetVelocity = moveInput.x * moveStats.maxWalkSpeed;
            }

            horizontalVelocity = Mathf.Lerp(horizontalVelocity, targetVelocity, acceleration * Time.fixedDeltaTime);
        }
        else if(Mathf.Abs(moveInput.x) < moveStats.moveThreshold)
        {
            horizontalVelocity = Mathf.Lerp(horizontalVelocity, 0, deceleration * Time.fixedDeltaTime);
        }
    }

    private void TurnCheck(Vector2 moveInput)
    {
        if (isFacingRight && moveInput.x < 0)
        {
            Turn(false);
        }
        else if (!isFacingRight && moveInput.x > 0)
        {
            Turn(true);
        }
    }

    private void Turn(bool turnRight)
    {
        if (turnRight)
        {
            isFacingRight = true;
            transform.Rotate(0, 180f, 0);
        }
        else
        {
            isFacingRight = false;
            transform.Rotate(0, -180f, 0);
        }
    }

    #endregion

    #region CollisionChecks
    
    private void IsGrounded()
    {
        Vector2 boxCastOrigin = new Vector2(feetColl.bounds.center.x, feetColl.bounds.min.y);
        Vector2 boxCastSize = new Vector2(feetColl.bounds.size.x, moveStats.groundDetectionRayLength);

        groundHit = Physics2D.BoxCast(boxCastOrigin, boxCastSize, 0f, Vector2.down, moveStats.groundDetectionRayLength,
            moveStats.groundLayer);

        if (groundHit.collider != null)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }

        if (moveStats.debugShowGroundedBox)
        {
            Color rayColor;
            if (isGrounded)
            {
                rayColor = Color.green;
            }
            else
            {
                rayColor = Color.red;
            }
            
            Debug.DrawRay(new Vector2(boxCastOrigin.x - boxCastSize.x / 2, boxCastOrigin.y), Vector2.down * moveStats.groundDetectionRayLength, rayColor);
            Debug.DrawRay(new Vector2(boxCastOrigin.x + boxCastSize.x / 2, boxCastOrigin.y), Vector2.down * moveStats.groundDetectionRayLength, rayColor);
            Debug.DrawRay(new Vector2(boxCastOrigin.x - boxCastSize.x / 2, boxCastOrigin.y - moveStats.groundDetectionRayLength), Vector2.right * boxCastSize.x, rayColor);
        }
    }

    private void CollisionCheck()
    {
        IsGrounded();
    }

    private void BumpedHead()
    {
        Vector2 boxCastOrigin = new Vector2(feetColl.bounds.center.x, bodyColl.bounds.max.y);
        Vector2 boxCastSize =
            new Vector2(feetColl.bounds.size.x * moveStats.headWidth, moveStats.headDetectionRayLength);

        headHit = Physics2D.BoxCast(boxCastOrigin, boxCastSize, 0f, Vector2.up,
            moveStats.headDetectionRayLength, moveStats.groundLayer);

        if (headHit.collider != null)
        {
            bumpedHead = true;
        }
        else
        {
            bumpedHead = false;
        }

    }
    
    #endregion

    #region  JUMP

    private void ResetJumpValues()
    {
        isJumping = false;
        _isFalling = false;
        _isFastFalling = false;
        fastFallTime = 0;
        _isPastApexThreshold = false;
    }
    
    private void JumpChecks()
    {
        if (inputManager.jumpWasPressed)
        {
            jumpBufferTimer = moveStats.JumpBufferTime;
            jumpReleasedDuringBuffer = false;
        }
        
        // When we release jump button
        if (inputManager.jumpWasReleased)
        {
            if (jumpBufferTimer > 0)
            {
                jumpReleasedDuringBuffer = true;
            }

            if (isJumping && VerticalVelocity > 0f)
            {
                if (_isPastApexThreshold)
                {
                    _isPastApexThreshold = false;
                    _isFastFalling = true;
                    fastFallTime = moveStats.TimeForUpwardsCancel;
                    VerticalVelocity = 0;
                }
                else
                {
                    _isFastFalling = true;
                    _fastFallReleaseSpeed = VerticalVelocity;
                }
            }
        }
        
        // initiate jump
        if (jumpBufferTimer > 0 && !isJumping && (isGrounded || coyoteTimer > 0))
        {
            Debug.Log("Normal jump");
            InitiateJump(1);

            if (jumpReleasedDuringBuffer)
            {
                _isFastFalling = true;
                _fastFallReleaseSpeed = VerticalVelocity;
            }
        }
        
        // Double jump
        else if (jumpBufferTimer > 0 && isJumping && numberOfJumpsUsed < moveStats.NumberOfJumpsAllowed)
        {
            Debug.Log("Double jump");
            _isFastFalling = false;
            InitiateJump(1);
        }
        
        // Initiate jump after coyote time
        else if (jumpBufferTimer > 0 && _isFalling && numberOfJumpsUsed < moveStats.NumberOfJumpsAllowed - 1)
        {
            Debug.Log("Coyote jump");
            InitiateJump(2);
            _isFastFalling = false;
        }
    }

    private void InitiateJump(int numberOfJumpsUsed)
    {
        if (!isJumping)
        {
            isJumping = true;
        }

        jumpBufferTimer = 0;
        this.numberOfJumpsUsed += numberOfJumpsUsed;
        VerticalVelocity = moveStats.initialJumpVelocity;
    }

    private void Jump()
    {
        // Apply gravity while jumping
        if (isJumping)
        {
            if (bumpedHead)
            {
                _isFastFalling = true;
            }
            
            // gravity on ascending
            if (VerticalVelocity >= 0)
            {
                apexPoint = Mathf.InverseLerp(moveStats.initialJumpVelocity, 0f, VerticalVelocity);

                if (apexPoint > moveStats.ApexThreshold)
                {
                    if (!_isPastApexThreshold)
                    {
                        _isPastApexThreshold = true;
                        _timePastApexThreshold = 0f;
                    }

                    if (_isPastApexThreshold)
                    {
                        _timePastApexThreshold += Time.fixedDeltaTime;
                        if (_timePastApexThreshold < moveStats.ApexHangTime)
                        {
                            VerticalVelocity = 0f;
                        }
                        else
                        {
                            VerticalVelocity = -.01f;
                        }
                    }
                }

                // Gravity on ascending not past apex threshold
                else if(!_isFastFalling)
                {
                    VerticalVelocity += moveStats.gravity * Time.fixedDeltaTime;
                    
                    if (_isPastApexThreshold)
                    {
                        _isPastApexThreshold = false;
                    }
                }
            }
            
            // Gravity on descending
            else if (!_isFastFalling)
            {
                VerticalVelocity += moveStats.gravity * moveStats.GravityOnReleaseMultiplier * Time.fixedDeltaTime;
            }
            
            else if (VerticalVelocity < 0f)
            {
                if (!_isFalling)
                {
                    _isFalling = true;
                }
            }
        }
        
        // Jump cut
        if (_isFastFalling)
        {
            if (fastFallTime >= moveStats.TimeForUpwardsCancel)
            {
                VerticalVelocity += moveStats.gravity * moveStats.GravityOnReleaseMultiplier * Time.fixedDeltaTime;
            }
            else if (fastFallTime < moveStats.TimeForUpwardsCancel)
            {
                VerticalVelocity = Mathf.Lerp(_fastFallReleaseSpeed, 0f, (fastFallTime / moveStats.TimeForUpwardsCancel));
            }

            fastFallTime += Time.fixedDeltaTime;
        }
    }
    
    #endregion
    
    #region Land/Fall

    private void LandCheck()
    {
        // Landed
        if ((isJumping || _isFalling) && isGrounded && VerticalVelocity <= 0)
        {
            isJumping = false;
            _isFalling = false;
            _isFastFalling = false;
            _isPastApexThreshold = false;
            fastFallTime = 0;
            VerticalVelocity = Physics2D.gravity.y;
            
            numberOfJumpsUsed = 0;
            //numberOfDashesUsed = 0;
        }
    }

    private void Fall()
    {
        // Normal gravity while falling
        if (!isGrounded && !isJumping)
        {
            if (!_isFalling)
            {
                _isFalling = true;
            }

            VerticalVelocity += moveStats.gravity * Time.fixedDeltaTime;
        }
    }
    
    #endregion
    
    #region Timers
    private void CountTimers()
    {
        jumpBufferTimer -= Time.deltaTime;

        if (!isGrounded)
        {
            coyoteTimer -= Time.deltaTime;
        }
        else
        {
            coyoteTimer = moveStats.JumpCoyoteTime;
        }
        
        if (isGrounded)
        {
            dashOnGroundTimer -= Time.deltaTime;
        }
    }
    #endregion
    
    # region Dash
    private void DashCheck()
    {
        if (inputManager.dashWasPressed)
        {
            // ground dash
            if (isGrounded && !isDashing)
            {
                InitiateDash();
            }
            
            // air dash
            else if (!isGrounded && !isDashing && numberOfDashesUsed < moveStats.numberOfDashes)
            {
                isAirDashing = true;
                InitiateDash();
                
                // Wall jump stuff from here tutorial, but we dont have wall slide/jump 
            }

            //canDash = false;
        }
    }

    private void InitiateDash()
    {
        dashDirection = inputManager.moveVector;

        Vector2 closestDirection = Vector2.zero;

        float minDistance = Vector2.Distance(dashDirection, moveStats.dashDirections[0]);

        for (int i = 0; i < moveStats.dashDirections.Length; i++)
        {
            // skip comparisons if we hit direction bang on
            if (dashDirection == moveStats.dashDirections[i])
            {
                closestDirection = dashDirection;
                break;
            }

            float distance = Vector2.Distance(dashDirection, moveStats.dashDirections[i]);
            
            // Check if this is a diagonal direction and apply a bias
            bool isDiagonal = (Mathf.Abs(moveStats.dashDirections[i].x) == 1 &&
                               Mathf.Abs(moveStats.dashDirections[i].y) == 1);
            if (isDiagonal)
            {
                distance -= moveStats.dashDiagonalBias;
            }
            else if (distance < minDistance)
            {
                minDistance = distance;
                closestDirection = moveStats.dashDirections[i];
            }
        }
        
        // With 0 direction input
        if (closestDirection == Vector2.zero)
        {
            if (isFacingRight)
            {
                closestDirection = Vector2.right;
            }
            else
            {
                closestDirection = Vector2.left;
            }
        }

        dashDirection = closestDirection;
        numberOfDashesUsed++;
        isDashing = true;
        dashTimer = 0;
        dashOnGroundTimer = moveStats.timeBetweenDashOnGround;
        
        ResetJumpValues();
    }
    
    private void Dash()
    {
        if (isDashing)
        {
            // Stop the dash after the timer
            dashTimer += Time.fixedDeltaTime;
            if (dashTimer >= moveStats.dashTime)
            {
                if (isGrounded)
                {
                    ResetDashes();
                }

                isAirDashing = false;
                isDashing = false;

                if (!isJumping)
                {
                    dashFastFallTime = 0f;
                    dashFastFallingReleaseSpeed = VerticalVelocity;

                    if (!isGrounded)
                    {
                        isDashFastFalling = true;
                    }
                }

                return;
            }

            horizontalVelocity = moveStats.dashSpeed * dashDirection.x;

            if (dashDirection.y != 0f || isAirDashing)
            {
                VerticalVelocity = moveStats.dashSpeed * dashDirection.y;
            }
        }
        
        // Handle dash cut time
        else if (isDashFastFalling)
        {
            if (VerticalVelocity > 0f)
            {
                if (dashFastFallTime < moveStats.dashTimeForUpwardsCancel)
                {
                    VerticalVelocity = Mathf.Lerp(dashFastFallingReleaseSpeed, 0f,
                        (dashFastFallTime / moveStats.dashTimeForUpwardsCancel));
                }
                else if (dashFastFallTime >= moveStats.dashTimeForUpwardsCancel)
                {
                    VerticalVelocity += moveStats.gravity * moveStats.dashGravityOnReleaseMultiplier *
                                        Time.fixedDeltaTime;
                }
                
                dashFastFallTime += Time.fixedDeltaTime;
            }
            else
            {
                VerticalVelocity += moveStats.gravity * moveStats.dashGravityOnReleaseMultiplier * Time.fixedDeltaTime;
            }

            if (isGrounded)
            {
                isDashFastFalling = false;
            }
        }
    }

    private void ResetDashes()
    {
        numberOfDashesUsed = 0;
    }
    
    #endregion
}
