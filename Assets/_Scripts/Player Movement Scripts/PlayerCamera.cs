using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using EZCameraShake;


public class PlayerCamera : MonoBehaviour
{
    //Horizontal Input (A & D)
    float horizontalInput;
    //Vertical Input (W & S)
    float verticalInput;

    //Rotation along the X-Axis (Up & Down)
    float xRotation;
    //Rotation along the Y-Axis (Right & Left)
    float yRotation;

    private float tilt;
    float startOrientation;
    bool allowInput = true;

    [Header("References")]
    public Camera playerCam;
    public Transform orientation;
    public Transform playerObj;

    [Header("Settings")]
    public float sensX;
    public float sensY;
    public float FOV;

    [Header("Camera Tilt Parameters")]
    public float camTilt;
    public float camTiltTime;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Start()
    {
        FOV = playerCam.fieldOfView;
        //Locks cursor to center of screen and sets its visibility to false
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }

    public void ToggleAllowInput(bool toggle)
    {
        allowInput = toggle;
    }

    public void ResetCamera()
    {
        //playerCam.transform.rotation = Quaternion.Euler(Vector3.zero);
        playerCam.transform.DORotate(Vector3.zero, 0.3f);
    }

    public void TweenCameraRotation(Vector3 rot)
    {
        playerCam.transform.DORotate(rot, 0.3f);

    }

    private void Update()
    {
        CamTilt();
        if (allowInput)
        {
            horizontalInput = Input.GetAxisRaw("Horizontal");
            verticalInput = Input.GetAxisRaw("Vertical");

            float mouseX = Input.GetAxisRaw("Mouse X") * sensX; //Left & Right
            float mouseY = Input.GetAxisRaw("Mouse Y") * sensY; //Up & Down

            //Sets rotation of the Y-Axis to the input from mouseX
            yRotation += mouseX;

            //Sets rotation of the X-Axis to the input from mouseY
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            playerCam.transform.localRotation = Quaternion.Euler(xRotation, yRotation, tilt);
            //orientation.rotation = Quaternion.Euler(0, yRotation, 0);
            orientation.localRotation = Quaternion.Euler(0, yRotation, 0);

            //orientation.rotation = Quaternion.Euler(orientation.rotation.x, yRotation, orientation.rotation.z);
            //orientation.rotation = Quaternion.Euler(0, yRotation + startOrientation, 0);


            playerObj.localRotation = Quaternion.Euler(0, yRotation, 0);
        }

    }

    private void CamTilt() //Tilts camera slightly when moving left or right
    {
        if (horizontalInput == -1)
        {
            tilt = Mathf.Lerp(tilt, camTilt, camTiltTime * Time.deltaTime);
        }
        else if (horizontalInput == 1)
        {
            tilt = Mathf.Lerp(tilt, -camTilt, camTiltTime * Time.deltaTime);
        }
        else
        {
            tilt = Mathf.Lerp(tilt, 0, camTiltTime * Time.deltaTime);
        }
    }

    public void DoFov(float endValue)
    {
        playerCam.DOFieldOfView(endValue, 0.15f);
    }

}
