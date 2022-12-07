using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SlidingDoor : MonoBehaviour
{
    private Renderer render;
    public bool isLocked;
    public bool isAutomaticDoor;

    public GameObject door;
    private float upPosition;

    [Header("Slide Parameters")]
    public float upLength;
    public float slideDuration;

    [Header("Sound Effects")]
    public SoundFX openSound;
    public SoundFX closeSound;

    [Header("Material References")]
    public Material unlockedMat;
    public Material lockedMat;

    private void Awake()
    {
        render = door.GetComponent<Renderer>();
        upPosition = door.transform.position.y + upLength;
    }


    private void Update()
    {
        if (isLocked)
        {
            render.material = lockedMat;
        }
        else
        {
            render.material = unlockedMat;
        }
    }

    public void SlideUp()
    {
        door.transform.DOMoveY(upPosition, slideDuration);
    }

    public void SlideDown()
    {
        door.transform.DOMoveY(transform.position.y, slideDuration);
    }

    public void ToggleLocked(bool b)
    {
        isLocked = b;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(isAutomaticDoor && other.gameObject.CompareTag("Player") && !isLocked)
        {
            SlideUp();
            MyAudioManager.instance.PlaySoundAtPoint(openSound, transform.position);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (isAutomaticDoor && other.gameObject.CompareTag("Player") && !isLocked)
        {
            SlideDown();
            MyAudioManager.instance.PlaySoundAtPoint(closeSound, transform.position);

        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (isAutomaticDoor && other.gameObject.CompareTag("Player") && !isLocked)
        {
            SlideUp();
        }
    }

}
