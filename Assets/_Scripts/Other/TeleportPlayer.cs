using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPlayer : MonoBehaviour
{
    public GameObject player;
    public GameObject elevator;
    public Transform teleportPos;
    [SerializeField] private Vector3 distance;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //float distance = Vector3.Distance(player.transform.position, teleportPos.position);
        distance = elevator.transform.position - player.transform.position;
        //if (Input.GetKeyDown(KeyCode.T))
        //{
        //    player.transform.position = teleportPos.position - distance;
        //    elevator.transform.position = teleportPos.position;
        //}
    }
}
