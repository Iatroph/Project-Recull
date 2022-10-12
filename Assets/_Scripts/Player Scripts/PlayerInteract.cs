using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [Header("References")]
    public Camera playerCam;

    [Header("Interact Parameters")]
    public float interactRange;

    [Header("Layermask")]
    public LayerMask ignore;

    private void Awake()
    {
        playerCam = Camera.main;
    }

    private void Update()
    {
        Vector3 shootDir = playerCam.transform.forward;
        RaycastHit hit;

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (Physics.Raycast(playerCam.transform.position, shootDir, out hit, interactRange, ~ignore))
            {
                //Debug.Log(hit.transform.name);
                Debug.DrawLine(playerCam.transform.position, hit.point, Color.green, 10);

                if (hit.transform.gameObject.GetComponent<IInteractable>() != null)
                {
                    hit.transform.gameObject.GetComponent<IInteractable>().Interact();
                }
            }
            else
            {
                Debug.DrawLine(playerCam.transform.position, playerCam.transform.position + playerCam.transform.forward * interactRange, Color.green, 10);
            }

        }

    }
}
