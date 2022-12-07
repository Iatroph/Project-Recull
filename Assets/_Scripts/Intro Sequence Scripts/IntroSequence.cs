using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class IntroSequence : MonoBehaviour
{
    public static IntroSequence instance;

    public GameObject playerHUD;
    public GameObject CyroRoomLight;
    public GameObject[] lights;
    public GameObject playerLight;
    public Transform cryoChamberGlass;

    public GameObject ammoCounter;
    public GameObject recallTimer;

    private WeaponBase revolver;

    public PlayerWeaponManager PWM;

    public TMP_Text PressToRecall;
    public GameObject introCanvas;

    bool ReboundPause = false;

    private float waitTime = 1.5f;
    private float timer;

    public SoundFX lightOn;

    private void Awake()
    {
        introCanvas.SetActive(false);
        instance = this;
        CyroRoomLight.SetActive(false);
        foreach(GameObject g in lights)
        {
            g.SetActive(false);
        }
        playerLight.SetActive(false);
        timer = waitTime;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.instance)
        {
            GameManager.instance.DisablePlayerInput();
        }
        StartCoroutine(TurnOnLight());
    }

    // Update is called once per frame
    void Update()
    {
        if(revolver != null)
        {
            if(revolver.currentAmmo == 0 && ReboundPause == false)
            {
                timer -= Time.deltaTime;
                if(timer <= 0)
                {
                    StartCoroutine(TeachRebound());
                }

            }
        }
    }

    public void EnableAmmoCounter()
    {
        ammoCounter.SetActive(true);
        revolver = PWM.GetCurrentWeapon();
        PWM.ToggleAllowRecall();
    }

    public IEnumerator TurnOnLight()
    {
        yield return new WaitForSeconds(2);
        if (GameManager.instance)
        {
            GameManager.instance.EnablePlayerInput();
        }
        //CyroRoomLight.SetActive(true);
        foreach (GameObject g in lights)
        {
            g.SetActive(true);
        }
        MyAudioManager.instance.PlaySoundAtPoint(lightOn, transform.position);
        playerLight.SetActive(true);
        playerHUD.SetActive(true);
        yield return new WaitForSeconds(2);
        cryoChamberGlass.DOLocalMoveZ(cryoChamberGlass.localPosition.z - 7f, 2);
        //cryoChamberGlass.DOMoveY(cryoChamberGlass.localPosition.y + 7f, 2);


    }

    public IEnumerator TeachRebound()
    {
        ReboundPause = true;
        Time.timeScale = 0;
        PressToRecall.gameObject.SetActive(true);
        introCanvas.SetActive(true);
        //StartCoroutine(RecallTextFlicker());
        yield return waitForKeyPress(KeyCode.R);
        PressToRecall.gameObject.SetActive(false);
        introCanvas.SetActive(false);
        PWM.ToggleAllowRecall();
        revolver.Recall();
        recallTimer.SetActive(true);
        Time.timeScale = 1;
        yield return null;
    }

    public IEnumerator RecallTextFlicker()
    {
        while (ReboundPause)
        {
            yield return new WaitForSecondsRealtime(0.5f);
            PressToRecall.text = "PRESS \"R\" TO <color=\"red\">RECALL!";
            yield return new WaitForSecondsRealtime(0.8f);
            PressToRecall.text = "PRESS \"R\" TO <color=\"red\">RELOAD?";

        }
    }

    private IEnumerator waitForKeyPress(KeyCode key)
    {
        bool done = false;
        while (!done) // essentially a "while true", but with a bool to break out naturally
        {
            if (Input.GetKeyDown(key))
            {
                done = true; // breaks the loop
            }
            yield return null; // wait until next frame, then continue execution from here (loop continues)
        }

        // now this function returns
    }
}
