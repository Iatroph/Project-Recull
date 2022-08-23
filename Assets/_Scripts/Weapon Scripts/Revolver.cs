using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Revolver : WeaponBase
{
    Animator anim;

    new void Awake()
    {
        base.Awake();
        anim = GetComponent<Animator>();
    }

    new void Update()
    {
        base.Update();

        //if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)) //GET REFERENCE FROM PLAYERMOVEMENT SCRIPT INSTEAD
        //{
        //    anim.SetBool("IsMoving", true);
        //}
        //else
        //{
        //    anim.SetBool("IsMoving", false);
        //}

        if (PlayerMovement.instance.isGrounded && PlayerMovement.instance.moveDir.magnitude != 0 && !PlayerMovement.instance.isSliding && !PlayerMovement.instance.isDashing)
        {
            anim.SetBool("IsMoving", true);
        }
        else
        {
            anim.SetBool("IsMoving", false);
        }
    }

    public override void ShootFromInput()
    {
        if ((currentAmmo > 0 || infiniteAmmo) && canShoot)
        {
            Shoot();
            anim.SetTrigger("Shoot");
        }
    }
}
