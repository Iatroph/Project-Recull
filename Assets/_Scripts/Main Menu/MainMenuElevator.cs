using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuElevator : MonoBehaviour
{
	// Amplitude of the shake. A larger value shakes the camera harder.
	public float shakeAmount = 0.01f;

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
