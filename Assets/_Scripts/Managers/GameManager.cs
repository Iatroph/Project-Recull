using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private bool levelComplete;

    [Header("References")]
    public GameObject player;
    public GameObject playerHUD;
    public EndLevelUI endLevelUI;

    private PlayerStats pStats;
    private PlayerCamera pCamera;
    private PlayerMovement pMovement;
    private PlayerWeaponManager pWepManager;

    [SerializeField] private float clearTime;
    [SerializeField] public string formattedTime;
    [SerializeField] public int enemyKills = 0;

    private TimeSpan timeSpan;

    private void Awake()
    {
        instance = this;

        if (player)
        {
            pStats = player.GetComponent<PlayerStats>();
            pCamera = player.GetComponent<PlayerCamera>();
            pMovement = player.GetComponent<PlayerMovement>();
            pWepManager = player.GetComponent<PlayerWeaponManager>();
        }
    }

    public void EndLevel()
    {
        endLevelUI.gameObject.SetActive(true);
        LevelEndToggleOff();
        StartCoroutine(endLevelUI.EndSequence());
    }

    public void LevelEndToggleOff()
    {
        levelComplete = true;
        DisablePlayerInput();
        ToggleHUD(false);
        ToggleWeaponMesh();
        pWepManager.RecallAll();
    }

    public void DisablePlayerInput()
    {
        pCamera.ToggleAllowInput(false);
        pMovement.ToggleUserInput(false);
        pMovement.CancelInputs();
        pWepManager.ToggleAllowInput(false);

    }

    public void EnablePlayerInput()
    {
        pCamera.ToggleAllowInput(true);
        pMovement.ToggleUserInput(true);
        pWepManager.ToggleAllowInput(true);
    }

    public void ToggleHUD(bool toggle)
    {
        playerHUD.SetActive(false);
    }

    public void ToggleWeaponMesh()
    {
        pWepManager.ToggleCurrentWeaponMesh();
    }

    public void IncreaseKillCount()
    {
        enemyKills++;
    }


    // Update is called once per frame
    void Update()
    {
        if (!levelComplete)
        {
            clearTime += Time.unscaledDeltaTime;
            timeSpan = TimeSpan.FromSeconds(clearTime);
            formattedTime = string.Format("{0:00}:{1:00}", timeSpan.Minutes, timeSpan.Seconds);
        }

    }
}
