using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatTracker : MonoBehaviour
{
    public TMP_Text rbSpeed;
    public TMP_Text wallJumps;
    public TMP_Text dashes;
    public TMP_Text moveState;

    float speed;
    Rigidbody rb;
    PlayerMovement pm;
    PlayerDash pd;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovement>();
        pd = GetComponent<PlayerDash>();
    }

    // Update is called once per frame
    void Update()
    {
        speed = rb.velocity.magnitude;
        rbSpeed.text = "Speed: " + Mathf.Round(speed * 10.0f) * 0.1f;
        wallJumps.text = "Wall Jumps: " + pm.wallJumpCount;
        dashes.text = "Dashes: " + pd.dashCount;
        moveState.text = "Move State: " + pm.movestate;

    }
}
