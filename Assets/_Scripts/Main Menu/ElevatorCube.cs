using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ElevatorCube : MonoBehaviour
{
    private float timer;
    public float time = 1;
    Vector3 startPos;
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        timer = time;
        MoveDown();
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            transform.position = startPos;
            timer = time;
            MoveDown();
        }
    }

    public void MoveDown()
    {
        transform.DOMoveY(transform.position.y - 10, time - 0.1f);
    }
}
