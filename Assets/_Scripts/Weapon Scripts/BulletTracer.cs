using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BulletTracer : MonoBehaviour
{
    LineRenderer lr;
    public float startWidth;
    public float shrinkTime;

    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
        lr.startWidth = startWidth;
    }

    private void Start()
    {
        if (lr != null)
        {
            DOTween.To(TweenLineRendererWidth, startWidth, 0, shrinkTime);
        }
        Destroy(gameObject, shrinkTime + .05f);
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
