using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class DamageEffect : MonoBehaviour
{
    public Image redPanel;
    private Color color;

    private void Awake()
    {
        redPanel = GetComponent<Image>();
        color = redPanel.color;
        color = new Color(color.r, color.g, color.b, 0.5f);
    }

    public void RedFlash()
    {
        redPanel.color = color;
        redPanel.DOFade(0, 0.3f);
    }

}
