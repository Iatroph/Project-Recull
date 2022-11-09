using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class MainMenuCameraShake : MonoBehaviour
{

	public float magnitude;
	public float roughness;
	public float fadeInTime;
	public float fadeOutTime;

	public float shakeInterval;
	private float shakeTimer;

	void Awake()
	{
		shakeTimer = shakeInterval;
		//CameraShaker.Instance.StartShake(0.7f, 0.7f, 1);
		CameraShaker.Instance.ShakeOnce(magnitude, roughness, fadeInTime, fadeOutTime);

	}

	void Update()
	{
		shakeTimer -= Time.deltaTime;
		if(shakeTimer <= 0)
        {
			CameraShaker.Instance.ShakeOnce(magnitude, roughness, fadeInTime, fadeOutTime);
			shakeTimer = shakeInterval;
        }

	}
}
