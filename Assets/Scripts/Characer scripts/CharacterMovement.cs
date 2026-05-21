using System;
using System.Collections;
using System.Timers;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [Header("References")] 
    public CharacterMovementStats moveStats;
    [SerializeField] private Collider2D feetColl, bodyColl;

    private Rigidbody2D rb;
    private InputManager inputManager;
    
    // Movement
    [HideInInspector] public Vector2 moveVelocity;
    private bool isFacingRight;
    public bool canMove;
    
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
    private bool isDashing;
    private bool canDash;
    private float canDashTimer, isDashingTimer;
    private bool isDashHanging;
    private bool groundDashCheck;
    
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
        DashCheck();
    }
    
    private void FixedUpdate()
    {
        CollisionCheck();
        Jump();
        Dash(moveStats.dashAcceleration);

        if (isGrounded)
        {
            Move(moveStats.groundAcceleration, moveStats.groundDeceleration, inputManager.moveVector);
        }
        else
        {
            Move(moveStats.airAcceleration, moveStats.airDeceleration, inputManager.moveVector);
        }
    }

    public void LockMove(bool val)
    {
        canMove = val;
    }
    
    public void LockMove(float lockTime)
    {
        canMove = false;
        StartCoroutine(UnlockMove(lockTime));
    }

    private IEnumerator UnlockMove(float time)
    {
        yield return new WaitForSeconds(time);
        canMove = true;
    }
    
    #region  Move
    
    private void Move(float acceleration, float deceleration, Vector2 moveInput)
    {
        if (!canMove)
        {
            moveVelocity = Vector2.Lerp(moveVelocity, Vector2.zero, deceleration * Time.fixedDeltaTime);

            rb.linearVelocity = new Vector2(moveVelocity.x, rb.linearVelocity.y);
        }
        else if (moveInput != Vector2.zero && !isDashing)
        {
            TurnCheck(moveInput);
            
            Vector2 targetVelocity = Vector2.zero;

            if (inputManager.runIsHeld || moveStats.onlyRunning)
            {
                targetVelocity = new Vector2(moveInput.x, 0f) * moveStats.maxRunSpeed;
            }
            else
            {
                targetVelocity = new Vector2(moveInput.x, 0f) * moveStats.maxWalkSpeed;
            }

            moveVelocity = Vector2.Lerp(moveVelocity, targetVelocity, acceleration * Time.fixedDeltaTime);

            rb.linearVelocity = new Vector2(moveVelocity.x, rb.linearVelocity.y);
        }
        else if(moveInput == Vector2.zero)
        {
            moveVelocity = Vector2.Lerp(moveVelocity, Vector2.zero, deceleration * Time.fixedDeltaTime);

            rb.linearVelocity = new Vector2(moveVelocity.x, rb.linearVelocity.y);
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
            groundDashCheck = true;
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
        if (isDashing)
        {
            Debug.Log("Dashing");
            return;
        }
        
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
                else
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
        
        // Normal gravity while falling
        if (!isGrounded && !isJumping)
        {
            if (!_isFalling)
            {
                _isFalling = true;
            }

            VerticalVelocity += moveStats.gravity * Time.fixedDeltaTime;
        }
        
        // clamp fall speed;
        VerticalVelocity = Mathf.Clamp(VerticalVelocity, -moveStats.MaxFallSpeed, 50f);

        rb.linearVelocity = new Vector2(rb.angularVelocity, VerticalVelocity);
    }
    
    
    
    #endregion
    
    #region Timers
    private void CountTimers()
    {
        jumpBufferTimer -= Time.deltaTime;

        isDashingTimer += Time.deltaTime;
        canDashTimer += Time.deltaTime;
        if (canDashTimer > moveStats.dashInterval)
        {
            canDash = true;
        }
        else
        {
            canDash = false;
        }

        if (!isGrounded)
        {
            coyoteTimer -= Time.deltaTime;
        }
        else
        {
            coyoteTimer = moveStats.JumpCoyoteTime;
        }
    }
    #endregion
    
    # region Dash
    private void DashCheck()
    {
        if (inputManager.dashWasPressed && canDash && groundDashCheck)
        {
            isDashingTimer = 0;
        }
    }
    
    private void Dash(float acceleration)
    {
        if (isDashingTimer < moveStats.dashDuration)
        {
            groundDashCheck = false;
            canDashTimer = 0;
            isGrounded = false;
            isDashing = true;
            
            int direction = isFacingRight ? 1 : -1;
            
            Vector2 targetVelocity = Vector2.zero;
            
            targetVelocity = new Vector2(direction, 0f) * moveStats.dashSpeed;
            
            moveVelocity = Vector2.Lerp(moveVelocity, targetVelocity, acceleration * Time.fixedDeltaTime);

            rb.linearVelocity = new Vector2(moveVelocity.x, 0f);
        }
        else
        {
            isDashing = false;
        }
    }
    
    #endregion
}
