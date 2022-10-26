using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    [TextArea]
    public string text;
    bool isTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player") && !isTriggered)
        {
            isTriggered = true;
            TutorialManager.instance.ActivateTutorialMessage(text);
        }
    }
}
