using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBounce : MonoBehaviour
{
    private Rigidbody rb;

    private Vector3 bounceRotation;
    private Vector3 bounceForce;
    private Vector3 bounceTorgue;

    [Header("Force Parameters")]
    public float xForce;
    public float lowerYForce;
    public float upperYForce;
    public float zForce;
    public float torgueForce;
    public float normalForce;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        RandomForce();
        RandomTorgue();
    }

    private void Start()
    {
        Bounce();
    }

    private void RandomTorgue()
    {
        if (Random.Range(0, 2) == 0)
        {
            bounceTorgue.x = torgueForce;
            bounceTorgue.z = torgueForce;
        }
        else
        {
            bounceTorgue.x = -torgueForce;
            bounceTorgue.z = -torgueForce;
        }

        int rand = Random.Range(0, 3);

        if (rand == 0)
        {
            bounceTorgue.x = 0;
        }
        else if(rand == 1)
        {
            bounceTorgue.z = 0;
        }
        else
        {

        }

    }

    private void RandomForce()
    {
        bounceForce.x = Random.Range(-xForce, xForce);
        bounceForce.y = Random.Range(lowerYForce, upperYForce);
        bounceForce.z = Random.Range(-zForce, zForce);
    }

    public void AddForceOfNormal(Vector3 normal)
    {
        rb.AddForce(normal * normalForce, ForceMode.Impulse);
    }

    public void Bounce()
    {
        rb.AddForce(bounceForce.x, bounceForce.y, bounceForce.z, ForceMode.Impulse);
        rb.AddTorque(bounceTorgue, ForceMode.VelocityChange);
    }

    public void Spin()
    {
        rb.AddTorque(bounceTorgue, ForceMode.VelocityChange);
    }

}

