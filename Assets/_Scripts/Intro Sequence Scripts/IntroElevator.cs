using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class IntroElevator : MonoBehaviour
{
    public GameObject door1;
    public GameObject door2;
    public GameObject invisibleWall;

    public float timeInElevator;

    private float timer;

    private bool elevatorTriggered;

    public GameObject player;
    public GameObject elevator;
    public Transform teleportPos;
    private Vector3 distance;
    // Start is called before the first frame update
    void Start()
    {
        timer = timeInElevator;
    }

    // Update is called once per frame
    void Update()
    {
        if(player != null)
        {
            distance = elevator.transform.position - player.transform.position;

        }

        if (timer <= 0 && !elevatorTriggered)
        {
            //invisibleWall.SetActive(true);
            //elevatorTriggered = true;
            //door1.transform.DOMoveX(door1.transform.position.x + 1.5f, 1);
            //door2.transform.DOMoveX(door2.transform.position.x - 1.5f, 1);
            StartCoroutine(ElevatorEvent());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player = other.gameObject;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player in elevator");
            timer -= Time.deltaTime;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            timer = timeInElevator;
        }
    }

    public IEnumerator ElevatorEvent()
    {
        elevatorTriggered = true;
        invisibleWall.SetActive(true);
        door1.transform.DOMoveX(door1.transform.position.x + 1.5f, 1);
        door2.transform.DOMoveX(door2.transform.position.x - 1.5f, 1);
        yield return new WaitForSeconds(4);
        player.transform.position = teleportPos.position - distance;
        elevator.transform.position = teleportPos.position;
        yield return new WaitForSeconds(0.1f);
        door1.transform.DOMoveX(door1.transform.position.x - 1.5f, 1);
        door2.transform.DOMoveX(door2.transform.position.x + 1.5f, 1);

        invisibleWall.SetActive(false);

        yield return null;
    }
}
