using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuElevator : MonoBehaviour
{

	// How long the object should shake for.
	public float shakeDuration = 0f;

	// Amplitude of the shake. A larger value shakes the camera harder.
	public float shakeAmount = 0.7f;
	public float decreaseFactor = 1.0f;

	Vector3 originalPos;

    private void Awake()
    {
		originalPos = transform.position;
    }

    void Update()
	{
		transform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;
    }
}
