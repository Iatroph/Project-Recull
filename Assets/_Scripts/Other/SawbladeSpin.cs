using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawbladeSpin : MonoBehaviour
{
    public float spinSpeed;
    public bool spinOtherWay;

    private void Update()
    {
        if (spinOtherWay)
        {
            transform.Rotate(0, -6 * spinSpeed * Time.deltaTime, 0);

        }
        else
        {
            transform.Rotate(0, 6 * spinSpeed * Time.deltaTime, 0);

        }
    }
}
