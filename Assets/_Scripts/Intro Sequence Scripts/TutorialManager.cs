using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    public GameObject tutorialPanel;
    public TMP_Text tutorialText;

    public float messageLength;

    public static TutorialManager instance;
    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        tutorialPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ActivateTutorialMessage(string s)
    {
        StopAllCoroutines();
        StartCoroutine(TutorialPopUp(s));
    }

    public IEnumerator TutorialPopUp(string s)
    {
        tutorialPanel.SetActive(true);
        DisplayTutorialMessage(s);
        yield return new WaitForSecondsRealtime(messageLength);
        tutorialPanel.SetActive(false);

    }

    public void DisplayTutorialMessage(string s)
    {
        tutorialText.text = s;
    }
}
