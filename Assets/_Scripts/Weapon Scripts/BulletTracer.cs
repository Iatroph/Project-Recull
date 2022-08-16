using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BulletTracer : MonoBehaviour
{
    LineRenderer lr;
    public float startWidth;

    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
        lr.startWidth = startWidth;
    }

    private void Start()
    {
        if (lr != null)
        {
            DOTween.To(TweenLineRendererWidth, startWidth, 0, .1f);
        }
        Destroy(gameObject, .15f);
    }

    private void OnDestroy()
    {
        DOTween.KillAll();
    }

    private void TweenLineRendererWidth(float f)
    {
        lr.startWidth = f;
        lr.endWidth = f;
    }


}
