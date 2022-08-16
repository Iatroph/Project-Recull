using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class PlayerDash : MonoBehaviour
{
    //Horizontal Input (A & D)
    float horizontalInput;
    //Vertical Input (W & S)
    float verticalInput;

    Vector3 dashDir;

    private float dashCount;

    [Header("References")]
    public Transform orientation;
    public PlayerCamera playerCam;
    private Rigidbody rb;
    private PlayerMovement pm;

    [Header("Dash Parameters")]
    public float dashForce;
    public float dashUpwardForce;
    public float dashDuration;
    public float groundedDashDuration;
    public float maxDashes;

    [Header("Dash Recharge")]
    public float dashRecharge;
    private float dashRechargeTimer;

    [Header("Keybinds")]
    public KeyCode dashKey = KeyCode.LeftShift;

    [Header("Other")]
    public float dashForwardFov;
    public float dashBackwardFov;
    public float dashFov;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovement>();
        dashCount = maxDashes;
    }

    private void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        //if (Input.GetKeyDown(dashKey) && dashRechargeTimer <= 0 && !pm.isSliding)
        //{
        //    Dash();
        //}

        //if (Input.GetKeyDown(dashKey) && dashRechargeTimer <= 0 && dashCount > 0 &&!pm.isSliding)
        //{
        //    Dash();
        //}

        if (Input.GetKeyDown(dashKey) && dashCount > 0 && !pm.isSliding)
        {
            Dash();
        }

        //if (dashRechargeTimer > 0)
        //{
        //    dashRechargeTimer -= Time.deltaTime;
        //}

        if(dashCount < maxDashes)
        {
            dashRechargeTimer -= Time.deltaTime;

            if(dashRechargeTimer <= 0)
            {
                dashCount++;
                dashRechargeTimer = dashRecharge;
            }
        }

        //if (pm.isGrounded)
        //{
        //    dashCount = maxDashes;
        //}

    }

    private void Dash()
    {
        dashCount--;
        dashRechargeTimer = dashRecharge;

        dashDir = pm.moveDir;
        pm.StopAllVelocity();
        pm.DisableGravity();
        pm.isDashing = true;

        if (dashDir.magnitude == 0)
        {
            dashDir = orientation.transform.forward;
            playerCam.DoFov(dashForwardFov);
        }

        if(verticalInput == 1)
        {
            playerCam.DoFov(dashForwardFov);
        }
        else if( verticalInput == -1)
        {
            playerCam.DoFov(dashBackwardFov);

        }

        Vector3 forceToApply = dashDir * dashForce + orientation.up * dashUpwardForce;

        delayedForceToApply = forceToApply;

        Invoke(nameof(DelayedDashForce), 0.025f);

        //Invoke(nameof(ResetDash), dashDuration);

        if (pm.isGrounded)
        {
            Invoke(nameof(ResetDash), groundedDashDuration);
        }
        else
        {
            Invoke(nameof(ResetDash), dashDuration);

        }
    }

    private Vector3 delayedForceToApply;

    private void DelayedDashForce()
    {
        rb.AddForce(delayedForceToApply, ForceMode.Impulse);
    }

    private void ResetDash()
    {
        pm.EnableGravity();
        playerCam.DoFov(playerCam.FOV);
        pm.isDashing = false;
    }
}
