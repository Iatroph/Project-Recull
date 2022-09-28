using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class DamageEffect : MonoBehaviour
{
    public Image redPanel;
    private Color color;
    float newAlpha;

    private void Awake()
    {
        redPanel = GetComponent<Image>();
        color = redPanel.color;
        color = new Color(color.r, color.g, color.b, 0.5f);
    }

    private void Update()
    {
        newAlpha = Mathf.Lerp(newAlpha, 0, 7 * Time.deltaTime);
        redPanel.color = new Color(redPanel.color.r, redPanel.color.g, redPanel.color.b, newAlpha);
    }

    public void RedFlash()
    {
        //redPanel.DOKill();
        redPanel.color = color;
        newAlpha = 0.5f;
        //redPanel.DOFade(0, 0.3f);
    }

}
