using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IntroSequence : MonoBehaviour
{
    public static IntroSequence instance;

    public GameObject CyroRoomLight;
    public GameObject playerLight;

    public GameObject ammoCounter;
    public GameObject recallTimer;

    private WeaponBase revolver;

    public PlayerWeaponManager PWM;

    public TMP_Text PressToRecall;

    bool ReboundPause = false;

    private float waitTime = 1f;
    private float timer;

    private void Awake()
    {
        instance = this;
        CyroRoomLight.SetActive(false);
        playerLight.SetActive(false);
        timer = waitTime;
    }

    // Start is called before the first frame update
    void Start()
    {
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
        CyroRoomLight.SetActive(true);
        playerLight.SetActive(true);

    }

    public IEnumerator TeachRebound()
    {
        ReboundPause = true;
        Time.timeScale = 0;
        PressToRecall.gameObject.SetActive(true);
        yield return waitForKeyPress(KeyCode.R);
        PressToRecall.gameObject.SetActive(false);
        PWM.ToggleAllowRecall();
        revolver.Recall();
        Time.timeScale = 1;
        yield return null;
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
