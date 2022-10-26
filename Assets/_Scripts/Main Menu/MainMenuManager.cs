using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class MainMenuManager : MonoBehaviour
{
    public GameObject buttonContainer;
    public Image panel;
    public Image logo;

    public void PlayButton()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    public void StartPlayCoroutine()
    {
        Cursor.visible = false;
        StartCoroutine(PlayCoroutine());
    }

    public IEnumerator PlayCoroutine()
    {
        buttonContainer.SetActive(false);
        panel.DOFade(1, 2);
        yield return new WaitForSeconds(2f);
        logo.DOFade(0, 1);
        yield return new WaitForSeconds(1f);
        PlayButton();
    }
}
