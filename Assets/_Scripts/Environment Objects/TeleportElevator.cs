using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TeleportElevator : MonoBehaviour
{
    public GameObject door1;
    public GameObject door2;

    public GameObject invisibleWall;

    public float timeInElevator;

    private float timer;

    private bool elevatorTriggered;

    public GameObject player;
    public Transform teleportPos;
    private Vector3 distance;

    [Header("Positions")]
    public Transform door1Open;
    public Transform door1Closed;
    public Transform door2Open;
    public Transform door2Closed;
    // Start is called before the first frame update
    void Start()
    {
        timer = timeInElevator;

    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            distance = transform.position - player.transform.position;

        }

        if (timer <= 0 && !elevatorTriggered)
        {
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
        door1.transform.DOMove(door1Closed.position, 1);
        door2.transform.DOMove(door2Closed.position, 1);
        yield return new WaitForSeconds(4);
        player.transform.position = teleportPos.position - distance;
        transform.position = teleportPos.position;
        yield return new WaitForSeconds(0.1f);
        door1.transform.DOMove(door1Open.position, 1);
        door2.transform.DOMove(door2Open.position, 1);

        invisibleWall.SetActive(false);

        yield return null;
    }
}
