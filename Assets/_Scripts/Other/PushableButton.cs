using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class PushableButton : MonoBehaviour, IInteractable
{
    public GameObject slidingDoor;
    public GameObject overHeadLight;
    public UnityEvent buttonEvent;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Interact()
    {
        Debug.Log("Button Pressed");
        overHeadLight.GetComponent<Light>().DOIntensity(0, 0.5f);
        //overHeadLight.SetActive(false);
        buttonEvent.Invoke();
        //slidingDoor.transform.DOMoveY(slidingDoor.transform.position.y + 4, 1);
        transform.DOMoveY(transform.position.y - 0.5f, 1f);
    }
}
