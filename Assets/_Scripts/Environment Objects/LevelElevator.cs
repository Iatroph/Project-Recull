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
    public bool startsClosed;
    public float shakeAmount = 0.01f;

    Vector3 originalPos;

    private bool shakeElevator = false;

    [Header("Positions")]
    public Transform door1Open;
    public Transform door1Closed;
    public Transform door2Open;
    public Transform door2Closed;
    public Transform playerPosition;

    private void Awake()
    {
        originalPos = transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (startsClosed)
        {
            door1.transform.position = door1Closed.position;
            door2.transform.position = door2Closed.position;
            StartCoroutine(StartLevelSequence());
        }
        else
        {
            door1.transform.position = door1Open.position;
            door2.transform.position = door2Open.position;
        }
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

        if (shakeElevator)
        {
            transform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;
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
        if (other.gameObject.CompareTag("Player") && isExit)
        {
            Debug.Log("Player In Elevator");
            if (GameManager.instance)
            {
                //GameManager.instance.LevelEndToggleOff();
                GameManager.instance.EndLevel();
            }
            //other.gameObject.GetComponent<PlayerMovement>().ToggleUserInput(false);            
            //other.gameObject.GetComponent<PlayerWeaponManager>().ToggleAllowInput(false);
            //other.gameObject.GetComponent<PlayerCamera>().ToggleAllowInput(false);

            other.gameObject.GetComponent<PlayerMovement>().CancelInputs();
            other.gameObject.GetComponent<PlayerSlide>().StopSlide();
            other.gameObject.GetComponent<PlayerCamera>().TweenCameraRotation(playerPosition.rotation.eulerAngles);

            other.gameObject.transform.DOMove(playerPosition.position, 0.5f);
            //other.gameObject.transform.DORotate(new Vector3(0, playerPosition.rotation.eulerAngles.y, 0), 0.3f);
            StartCoroutine(EndLevelSequence());

        }
    }

    public IEnumerator EndLevelSequence()
    {
        yield return new WaitForSeconds(1);
        CloseDoors();
        yield return new WaitForSeconds(1.1f);
        shakeElevator = true;
    }

    public IEnumerator StartLevelSequence()
    {
        yield return new WaitForSeconds(2);
        OpenDoors();
    }
}
