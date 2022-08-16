using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBase : MonoBehaviour //CREATE BASE CLASSES FOR ALL BULLETS
{
    protected BulletBounce bulletBounce;

    private GameObject Player;
    protected Rigidbody rb;

    protected bool isReturning = false;
    protected bool abilityTriggered = false;
    private float distance;

    [Header("Info")]
    public string projName;
    public int ID;

    [Header("Parameters")]
    public float returnSpeed;

    public void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        if(GetComponent<BulletBounce>() != null)
        {
            bulletBounce = GetComponent<BulletBounce>();
        }
        rb = GetComponent<Rigidbody>();
    }

    public void Start()
    {
        Physics.IgnoreCollision(GetComponent<Collider>(), Player.GetComponent<Collider>());
    }

    public void Update()
    {
        if (isReturning)
        {
            transform.position = Vector3.MoveTowards(transform.position, Player.transform.position, returnSpeed * Time.deltaTime);
        }
    }

    public void ReturnToPlayer()
    {
        isReturning = true;
    }

    public virtual void ActivateRecallAbility() 
    {

    }

    public void OnTriggerEnter(Collider other) //Add ammo collection
    {
        if (other.gameObject.CompareTag("Player") && isReturning)
        {
            Destroy(gameObject);
        }
    }
    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && isReturning)
        {
            Destroy(gameObject);
        }
    }

}
