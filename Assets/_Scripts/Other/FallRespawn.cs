using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallRespawn : MonoBehaviour
{
    public Transform respawnPosition;

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("Player"))
    //    {
    //        if (collision.gameObject.GetComponent<PlayerMovement>())
    //        {
    //            collision.gameObject.transform.position = respawnPosition.position;
    //            collision.gameObject.GetComponent<PlayerMovement>().StopAllVelocity();
    //        }
    //    }
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (other.gameObject.GetComponent<PlayerMovement>())
            {
                other.gameObject.transform.position = respawnPosition.position;
                other.gameObject.GetComponent<PlayerMovement>().StopAllVelocity();
            }
        }
    }
}
