using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSlide : MonoBehaviour
{
    Vector3 slideDir;
    bool continueSlide;

    [Header("References")]
    private PlayerMovement pm;
    public Camera playerCam;
    private Rigidbody rb;
    private CapsuleCollider capsuleCollider;
    public Transform orientation;

    [Header("Slide Parameters")]
    public float slideForce;

    [Header("Keybinds")]
    public KeyCode slideKey = KeyCode.LeftControl;

    [Header("Camera Parameters")]
    public Vector3 slidingHeight;
    private Vector3 normalHeight;
    public float heightChangeTime;

    [Header("Capsule Collider Parameters")]
    public Vector3 slidingCenter;
    private Vector3 normalSlidingCenter;
    public float colliderSlidingHeight;
    private float normalColliderSlidingHeight;

    private void Awake()
    {
        pm = GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();

        normalSlidingCenter = capsuleCollider.center;
        normalColliderSlidingHeight = capsuleCollider.height;
        normalHeight = playerCam.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(slideKey) && !pm.isDashing && !pm.isSliding && pm.isGrounded)
        //{
        //    StartSlide();
        //}

        if (pm.isSliding && Input.GetKey(slideKey))
        {
            continueSlide = true;
            //Sliding();
        }
        else
        {
            continueSlide = false;
        }

        //if (Input.GetKeyUp(slideKey) || Input.GetKeyDown(pm.jumpKey))
        //{
        //    StopSlide();
        //}
    }

    public void StartSlideFromInput()
    {
        if (!pm.isDashing && !pm.isSliding && pm.isGrounded)
        {
            StartSlide();
        }
    }


    private void FixedUpdate()
    {
        if (continueSlide)
        {
            Sliding();
        }
    }

    private void StartSlide()
    {
        capsuleCollider.center = slidingCenter;
        capsuleCollider.height = colliderSlidingHeight;
        StopAllCoroutines();
        StartCoroutine(MoveCamera(playerCam, slidingHeight));

        pm.isSliding = true;
        slideDir = pm.moveDir;
        if (slideDir.magnitude == 0)
        {
            slideDir = orientation.transform.forward;
        }

        rb.AddForce(slideDir.normalized * slideForce, ForceMode.Force);
    }

    private void Sliding()
    {
        if (!pm.OnSlope() || rb.velocity.y > -0.1f)
        {
            rb.AddForce(slideDir.normalized * slideForce, ForceMode.Acceleration);
        }
        else
        {
            rb.AddForce(pm.GetSlopeMoveDirection(slideDir) * slideForce, ForceMode.Acceleration);
        }
    }

    public void StopSlide()
    {
        StopAllCoroutines();
        StartCoroutine(MoveCamera(playerCam.GetComponent<Camera>(), normalHeight));
        pm.isSliding = false;
        capsuleCollider.center = normalSlidingCenter;
        capsuleCollider.height = normalColliderSlidingHeight;
    }

    public void CancelSlide()
    {
        StopAllCoroutines();
        StartCoroutine(MoveCamera(playerCam.GetComponent<Camera>(), normalHeight));
        pm.isSliding = false;
        capsuleCollider.center = normalSlidingCenter;
        capsuleCollider.height = normalColliderSlidingHeight;
    }

    IEnumerator MoveCamera(Camera camera, Vector3 target)
    {
        float startTime = Time.time;
        Vector3 origPos = camera.transform.localPosition;

        while (Time.time < startTime + heightChangeTime)
        {
            camera.transform.localPosition = Vector3.Lerp(origPos, target, (Time.time - startTime) / heightChangeTime);
            yield return null;
        }

        camera.transform.localPosition = target;
    }


}
