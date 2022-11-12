using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PauseManager : MonoBehaviour
{
    public GameObject pauseCanvas;
    bool isGamePaused = false;
    // Start is called before the first frame update
    void Start()
    {
        pauseCanvas.SetActive(false);   
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isGamePaused)
            {
                PauseGame();
            }
            else if (isGamePaused)
            {
                UnPauseGame();
            }
        }
    }

    public void PauseGame()
    {
        isGamePaused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        pauseCanvas.SetActive(true);
        Time.timeScale = 0;

        if (GameManager.instance)
        {
            GameManager.instance.DisableInput();
        }
    }

    public void UnPauseGame()
    {
        isGamePaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        pauseCanvas.SetActive(false);
        Time.timeScale = 1;

        if (GameManager.instance)
        {
            GameManager.instance.EnablePlayerInput();
        }
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;

    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;

    }
}
