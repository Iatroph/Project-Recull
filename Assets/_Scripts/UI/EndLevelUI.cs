using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EndLevelUI : MonoBehaviour
{
    [Header("Backgrounds")]
    public GameObject floor;
    public GameObject time;
    public GameObject kills;

    [Header("Text")]
    private TMP_Text floorText;
    private TMP_Text timeText;
    private TMP_Text killsText;


    private void Awake()
    {
        floorText = floor.GetComponentInChildren<TMP_Text>();
        timeText = time.GetComponentInChildren<TMP_Text>();
        killsText = kills.GetComponentInChildren<TMP_Text>();

        floor.SetActive(false);
        time.SetActive(false);
        kills.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance)
        {
            timeText.text = "Time: " + GameManager.instance.formattedTime;
            killsText.text = "Kills: " + GameManager.instance.enemyKills;

        }
    }

    public IEnumerator EndSequence()
    {
        yield return new WaitForSeconds(2f);
        floor.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        time.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        kills.SetActive(true);
    }
}
