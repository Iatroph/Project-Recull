using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement instance;

    //Horizontal Input (A & D)
    float horizontalInput;
    //Vertical Input (W & S)
    float verticalInput;

    [HideInInspector] public Vector3 moveDir;

    Rigidbody rb;

    RaycastHit slopeHit;
    RaycastHit wallHit;

    float storedGravity;

    [HideInInspector]
    public bool isGrounded;
    bool canDoubleJump;
    [HideInInspector] public bool exitingSlope;
    bool hasUsedCoyoteTime;
    [HideInInspector] public bool isDashing;
    [HideInInspector] public bool isSliding;
    bool isHittingWall;
    bool canChangeState;

    private float coyoteTimer;

    [Header("References")]
    public Camera playerCam;
    public GameObject playerObj;
    public CapsuleCollider playerCollider;
    public Transform orientation;
    public PhysicMaterial frictionMaterial;

    [Header("Movement Parameters")]
    public float currentMoveSpeed;
    public float maxMoveSpeed;

    public float groundSpeed;
    public float groundDrag;

    public float airSpeed;
    public float airMultiplier;

    public float dashSpeed;
    public float dashSpeedChangeFactor;

    public float slideSpeed;
    public float slideSpeedChangeFactor;

    [Header("Jump Parameters")]
    public float jumpForce;
    public float doubleJumpForce;
    public float jumpCoolDown;
    public float coyoteTime;
    public float wallJumpUpForce;
    public float wallJumpSideForce;
    public int maxWallJumps;
    [HideInInspector]
    public int wallJumpCount;

    [Header("Friction Parameters")]
    public float groundFriction;
    public float airFriction;

    [Header("Slope Parameters")]
    public float maxSlopeAngle;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode slideKey = KeyCode.LeftControl;
    public KeyCode dashKey = KeyCode.LeftShift;

    [Header("Other Parameters")]
    public float gravity;
    public float playerHeight = 2;
    public LayerMask whatIsNotGround;

    public enum MoveState
    {
        walking,
        airborne,
        sliding,
        airborneslide,
        dashing,
    }
    [HideInInspector]
    public MoveState movestate;
    [HideInInspector]
    public float desiredMoveSpeed;
    private float lastDesiredMoveSpeed;
    private MoveState lastState;
    private bool keepMomentum;

    private void UpdateMoveState()
    {
        if (canChangeState)
        {
            if (isSliding)
            {
                //desiredMoveSpeed = slideSpeed;
                movestate = MoveState.sliding;
                //speedChangeFactor = slideSpeedChangeFactor;
            }
            else if (isDashing)
            {
                //desiredMoveSpeed = dashSpeed;
                movestate = MoveState.dashing;
                //speedChangeFactor = dashSpeedChangeFactor;
            }
            else if (!isGrounded)
            {
                //desiredMoveSpeed = airSpeed;
                movestate = MoveState.airborne;
            }
            else if (isGrounded)
            {
                //desiredMoveSpeed = groundSpeed;
                movestate = MoveState.walking;
            }
        }

        if(movestate == MoveState.sliding)
        {
            desiredMoveSpeed = slideSpeed;
        }

        if (movestate == MoveState.walking)
        {
            desiredMoveSpeed = groundSpeed;
        }

        if (movestate == MoveState.dashing)
        {
            desiredMoveSpeed = dashSpeed;
        }

        if (movestate == MoveState.airborne)
        {
            desiredMoveSpeed = airSpeed;
        }

        currentMoveSpeed = desiredMoveSpeed;

        //bool desiredMoveSpeedHasChanged = desiredMoveSpeed != lastDesiredMoveSpeed;

        //if (lastState == MoveState.dashing)
        //{
        //    keepMomentum = true;
        //}

        //if (lastState == MoveState.sliding)
        //{
        //    keepMomentum = true;
        //}

        //if (desiredMoveSpeedHasChanged)
        //{
        //    if (keepMomentum)
        //    {
        //        if (moveSpeedLerp != null)
        //        {
        //            StopCoroutine(moveSpeedLerp);
        //        }
        //        moveSpeedLerp = StartCoroutine(SmoothlyLerpMoveSpeed());
        //    }
        //    else
        //    {
        //        if (moveSpeedLerp != null)
        //        {
        //            StopCoroutine(moveSpeedLerp);
        //        }
        //        currentMoveSpeed = desiredMoveSpeed;
        //    }
        //}

        //lastDesiredMoveSpeed = desiredMoveSpeed;
        //lastState = movestate;
    }

    public void directlyChangeMovementState(MoveState newState)
    {
        canChangeState = false;
        movestate = newState;
        Invoke(nameof(reenableMoveState), 0.1f);
    }

    public void reenableMoveState()
    {
        canChangeState = true;
    }

    private float speedChangeFactor;
    private Coroutine moveSpeedLerp;
    private  IEnumerator SmoothlyLerpMoveSpeed()
    {
        float time = 0;
        float difference = Mathf.Abs(desiredMoveSpeed - currentMoveSpeed);
        float startvalue = currentMoveSpeed;

        float boostFactor = speedChangeFactor;

        while(time < difference)
        {
            currentMoveSpeed = Mathf.Lerp(startvalue, desiredMoveSpeed, time / difference);

            time += Time.deltaTime * boostFactor;

            yield return null;
        }

        currentMoveSpeed = desiredMoveSpeed;
        speedChangeFactor = 1f;
        keepMomentum = false;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        rb.useGravity = false;
        playerCollider = GetComponent<CapsuleCollider>();
        coyoteTimer = coyoteTime;
        storedGravity = gravity;
        canChangeState = true;
        wallJumpCount = maxWallJumps;

        instance = this;
    }

    private void Update() //ALL INPUTS IN UPDATE
    {
        UserInput();
        SpeedControl();
        UpdateMoveState();
        UpdateFriction();
        CoyoteTime();
        WallCheck();

        //isGrounded = Physics.CheckSphere(transform.position - new Vector3(0, .63f, 0), 0.4f, ~whatIsNotGround);
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 2 * 0.5f + 0.3f, ~whatIsNotGround);


        //Debug.Log(movestate);

        if (/*movestate == MoveState.walking || movestate == MoveState.sliding*/ isGrounded && !isDashing)
        {
            rb.drag = groundDrag;
            canDoubleJump = true;
        }
        else
        {
            rb.drag = 0;
        }

        if (movestate == MoveState.walking)
        {
            wallJumpCount = maxWallJumps;
        }


        if (movestate == MoveState.walking && moveDir.magnitude == 0 && OnSlope() && !exitingSlope)
        {
            rb.velocity = new Vector3(0, 0, 0);
        }
        else if (movestate == MoveState.walking && moveDir.magnitude == 0 && !isSliding && !isDashing)
        {
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
        }
    }
    private void FixedUpdate() //ALL FORCES IN FIXEDUPDATE EXCEPT IMPULSE FORCES
    {
        ApplyMovement();
    }

    private void UserInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (isSliding)
        {
            moveDir = orientation.forward * 0 + orientation.right * horizontalInput;
        }
        else
        {
            moveDir = orientation.forward * verticalInput + orientation.right * horizontalInput;
        }

        if (isHittingWall == true && !isGrounded && Input.GetKeyDown(jumpKey) && wallJumpCount > 0 && !isDashing)
        {
            WallJump();
        }
        else if (Input.GetKeyDown(jumpKey) && isGrounded && !isDashing || (Input.GetKeyDown(jumpKey) && coyoteTimer >= 0 && !hasUsedCoyoteTime && !isDashing))
        {
            Jump();
        }
        else if(Input.GetKeyDown(jumpKey) && !isGrounded && canDoubleJump && !isDashing)
        {
            DoubleJump();
        }

        if(isSliding && Input.GetKeyDown(jumpKey))
        {
            directlyChangeMovementState(MoveState.airborne);
        }
    }

    private void ApplyMovement()
    {
        if(OnSlope() && !exitingSlope && isSliding)
        {
            rb.AddForce(moveDir * currentMoveSpeed * 10f * 0.4f, ForceMode.Acceleration);

            if (rb.velocity.y > 0)
            {
                rb.AddForce(Vector3.down * 150f, ForceMode.Force);
            }
        }
        else if (OnSlope() && !exitingSlope)
        {
            rb.AddForce(GetSlopeMoveDirection(moveDir) * currentMoveSpeed * 20f, ForceMode.Force);

            if (rb.velocity.y > 0)
            {
                rb.AddForce(Vector3.down * 50f, ForceMode.Force);
            }
        }
        else if(isGrounded && isSliding)
        {
            rb.AddForce(moveDir.normalized * currentMoveSpeed * 10 * 0.4f, ForceMode.Acceleration);
        }
        else if (isGrounded)
        {
            rb.AddForce(moveDir.normalized * currentMoveSpeed * 10, ForceMode.Force);
        }
        else if (!isGrounded)
        {
            rb.AddForce(moveDir.normalized * currentMoveSpeed * 10 * airMultiplier, ForceMode.Force);
        }

        if (!OnSlope() || !isGrounded)
        {
            rb.AddForce(Vector3.down * gravity, ForceMode.Force);
        }

    }

    private void WallCheck()
    {
        if (Physics.Raycast(transform.position, orientation.right, out wallHit, 0.7f, ~whatIsNotGround))
        {
            isHittingWall = true;
        }
        else if (Physics.Raycast(transform.position, -orientation.right, out wallHit, 0.7f, ~whatIsNotGround))
        {
            isHittingWall = true;
        }
        else if (Physics.Raycast(transform.position, orientation.forward, out wallHit, 0.7f, ~whatIsNotGround))
        {
            isHittingWall = true;
        }
        else if (Physics.Raycast(transform.position, -orientation.forward, out wallHit, 0.7f, ~whatIsNotGround))
        {
            isHittingWall = true;
        }
        else
        {
            isHittingWall = false;
        }
    }

    private void WallJump()
    {
        wallJumpCount--;
        Vector3 forceToApply = transform.up * wallJumpUpForce + wallHit.normal * wallJumpSideForce;
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);

        rb.AddForce(forceToApply, ForceMode.Impulse);
    }

    private void Jump()
    {
        exitingSlope = true;
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        hasUsedCoyoteTime = true;
        isGrounded = false;
        directlyChangeMovementState(MoveState.airborne);
        Invoke(nameof(ResetJump), jumpCoolDown);
    }
    private void DoubleJump()
    {
        exitingSlope = true;
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        rb.AddForce(Vector3.up * doubleJumpForce, ForceMode.Impulse);
        Invoke(nameof(ResetJump), jumpCoolDown);
        canDoubleJump = false;
    }

    private void ResetJump()
    {
        exitingSlope = false;
    }

    private void UpdateFriction()
    {
        if (isGrounded)
        {
            frictionMaterial.staticFriction = groundFriction;
            frictionMaterial.dynamicFriction = groundFriction;
        }
        else if (!isGrounded)
        {
            frictionMaterial.staticFriction = airFriction;
            frictionMaterial.dynamicFriction = airFriction;
        }
    }

    private void CoyoteTime()
    {
        if (isGrounded)
        {
            hasUsedCoyoteTime = false;
            coyoteTimer = coyoteTime;
        }

        if (!isGrounded)
        {
            coyoteTimer -= Time.deltaTime;
        }
    }

    private void SpeedControl()
    {
        if (OnSlope() && !exitingSlope)
        {
            if (rb.velocity.magnitude > currentMoveSpeed)
            {
                rb.velocity = rb.velocity.normalized * currentMoveSpeed;
            }
        }
        else
        {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z); //The current velocity

            if (flatVel.magnitude > currentMoveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * currentMoveSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }

    }

    public Vector2 FindVelRelativeToLook()
    {
        float lookAngle = orientation.transform.eulerAngles.y;
        float moveAngle = Mathf.Atan2(rb.velocity.x, rb.velocity.z) * Mathf.Rad2Deg;

        float u = Mathf.DeltaAngle(lookAngle, moveAngle);
        float v = 90 - u;

        float magnitue = rb.velocity.magnitude;
        float yMag = magnitue * Mathf.Cos(u * Mathf.Deg2Rad);
        float xMag = magnitue * Mathf.Cos(v * Mathf.Deg2Rad);

        return new Vector2(xMag, yMag);
    }

    public bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.4f, ~whatIsNotGround))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    public Vector3 GetSlopeMoveDirection(Vector3 direction)
    {
        return Vector3.ProjectOnPlane(direction, slopeHit.normal).normalized;
    }

    public Vector3 GetDirection(Transform moveInput)
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3();

        direction = moveInput.forward * verticalInput + moveInput.right * horizontalInput;

        if(verticalInput == 0 && horizontalInput == 0)
        {
            direction = moveInput.forward;
        }

        return direction.normalized;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position - new Vector3(0, 0.63f, 0), 0.4f);
    }

    public void StopAllVelocity()
    {
        rb.velocity = Vector3.zero;
    }

    public void DisableGravity()
    {
        gravity = 0;
    }

    public void EnableGravity()
    {
        gravity = storedGravity;
    }
}
