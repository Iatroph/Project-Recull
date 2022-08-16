using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatTracker : MonoBehaviour
{
    public TMP_Text rbSpeed;
    float speed;
    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        speed = rb.velocity.magnitude;
        rbSpeed.text = "Speed: " + Mathf.Round(speed * 10.0f) * 0.1f;

    }
}
