using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class DamageEffect : MonoBehaviour
{
    public Image redPanel;
    public Image greenPanel;
    private Color redColor;
    private Color greenColor;
    float redAlpha;
    float greenAlpha;
    float newAlpha;

    private void Awake()
    {
        redColor = redPanel.color;
        redColor = new Color(redColor.r, redColor.g, redColor.b, 0.5f);

        greenColor = greenPanel.color;
        greenColor = new Color(greenColor.r, greenColor.g, greenColor.b, 0.5f);
    }

    private void Update()
    {
        redAlpha = Mathf.Lerp(redAlpha, 0, 7 * Time.deltaTime);
        greenAlpha = Mathf.Lerp(greenAlpha, 0, 7 * Time.deltaTime);

        redPanel.color = new Color(redPanel.color.r, redPanel.color.g, redPanel.color.b, redAlpha);
        greenPanel.color = new Color(greenPanel.color.r, greenPanel.color.g, greenPanel.color.b, greenAlpha);

    }

    public void RedFlash()
    {
        redPanel.color = redColor;
        redAlpha = 0.5f;
    }

    public void GreenFlash()
    {
        greenPanel.color = greenColor;
        greenAlpha = 0.5f;
    }

}
