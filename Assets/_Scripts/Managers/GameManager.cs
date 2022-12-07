using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private int currentLevelIndex;

    [HideInInspector]
    public bool levelComplete;
    public bool isPlayerDead = false;

    [Header("References")]
    public GameObject player;
    public GameObject playerHUD;
    public EndLevelUI endLevelUI;
    public GameObject deathCanvas;

    private PlayerStats pStats;
    private PlayerCamera pCamera;
    private PlayerMovement pMovement;
    private PlayerWeaponManager pWepManager;

    [Header("Level Statistics")]
    [SerializeField] private float clearTime;
    [SerializeField] public string formattedTime;
    [SerializeField] public int enemyKills = 0;

    private TimeSpan timeSpan;

    private void Awake()
    {
        currentLevelIndex = SceneManager.GetActiveScene().buildIndex;

        instance = this;
        deathCanvas.SetActive(false);

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

    public void DeathSequence()
    {
        pCamera.ToggleAllowInput(false);
        pMovement.ToggleUserInput(false);
        pMovement.CancelInputs();
        pWepManager.ToggleAllowInput(false);
        ToggleHUD(false);
        ToggleWeaponMesh();
        StartCoroutine(DeathCoroutine());
        isPlayerDead = true;
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
        pMovement.CancelMovement();
        pWepManager.ToggleAllowInput(false);

    }

    public void DisableInput()
    {
        pCamera.ToggleAllowInput(false);
        pMovement.ToggleUserInput(false);
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
        playerHUD.SetActive(toggle);
    }

    public void ToggleWeaponMesh()
    {
        pWepManager.ToggleCurrentWeaponMesh();
    }

    public void IncreaseKillCount()
    {
        enemyKills++;
    }

    public void LoadNextLevel()
    {
        if (currentLevelIndex + 1 < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(currentLevelIndex + 1);
            Time.timeScale = 1;
        }
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
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

        if (isPlayerDead)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(currentLevelIndex);
            }

        }

    }

    public IEnumerator DeathCoroutine()
    {
        yield return new WaitForSeconds(1.5f);
        deathCanvas.SetActive(true);
    }
}
