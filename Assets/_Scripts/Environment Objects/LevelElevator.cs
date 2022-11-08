using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class LevelElevator : MonoBehaviour
{
    public GameObject door1;
    public GameObject door2;

    public bool isExit;

    [Header("Positions")]
    public Transform door1Open;
    public Transform door1Closed;
    public Transform door2Open;
    public Transform door2Closed;
    public Transform playerPosition;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            OpenDoors();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            CloseDoors();
        }
    }

    public void CloseDoors()
    {
        door1.transform.DOMove(door1Closed.position, 1);
        door2.transform.DOMove(door2Closed.position, 1);
    }

    public void OpenDoors()
    {
        door1.transform.DOMove(door1Open.position, 1);
        door2.transform.DOMove(door2Open.position, 1);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player In Elevator");
            other.gameObject.GetComponent<PlayerMovement>().ToggleUserInput(false);
            other.gameObject.GetComponent<PlayerMovement>().CancelInputs();
            other.gameObject.GetComponent<PlayerSlide>().StopSlide();
            other.gameObject.GetComponent<PlayerCamera>().ToggleAllowInput(false);
            other.gameObject.GetComponent<PlayerCamera>().ResetCamera();
            other.gameObject.GetComponent<PlayerWeaponManager>().ToggleAllowInput(false);
            //other.gameObject.transform.position = playerPosition.position;
            other.gameObject.transform.DOMove(playerPosition.position, 0.5f);
            other.gameObject.transform.DORotate(new Vector3(0, playerPosition.rotation.eulerAngles.y, 0), 0.3f);
            StartCoroutine(EndLevelSequence());

        }
    }

    public IEnumerator EndLevelSequence()
    {
        yield return new WaitForSeconds(1);
        CloseDoors();
    }
}
