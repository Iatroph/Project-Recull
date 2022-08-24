using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Revolver : WeaponBase
{
    Animator anim;

    [Header("Revolver Specific References")]
    public GameObject muzzleFlashParticle;
    public Transform muzzleFlashSpawn;
    public GameObject bulletImpactParticle;


    new void Awake()
    {
        base.Awake();
        anim = GetComponent<Animator>();
    }

    new void Update()
    {
        base.Update();

        if (PlayerMovement.instance.isGrounded && PlayerMovement.instance.moveDir.magnitude != 0 && !PlayerMovement.instance.isSliding && !PlayerMovement.instance.isDashing)
        {
            anim.SetBool("IsMoving", true);
        }
        else
        {
            anim.SetBool("IsMoving", false);
        }
    }

    public override void Shoot()
    {
        canShoot = false;
        shootTimer = fireRate;

        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);
        float z = Random.Range(-spread, spread);


        if (!infiniteAmmo)
        {
            currentAmmo--;
        }

        Vector3 shootDir = playerCam.transform.forward + new Vector3(x, y, z);
        RaycastHit hit;

        if (Physics.Raycast(playerCam.transform.position, shootDir, out hit, float.MaxValue, ~ignore))
        {
            if (hit.point != null)
            {
                projectileSpawnPoint = hit.point;
            }
            GameObject bullet = Instantiate(projectile, projectileSpawnPoint, Quaternion.identity);
            firedProjectiles.Add(bullet);
            bullet.GetComponent<BulletBounce>().AddForceOfNormal(hit.normal);

            if (hit.transform.gameObject.GetComponent<Hurtbox>() != null)
            {
                hit.transform.gameObject.GetComponent<Hurtbox>().AdjustDamage(damage);
            }

            GameObject tracer = Instantiate(bulletTracer, projectileSpawnPoint, Quaternion.identity);
            SetTracerLine(tracer.GetComponent<LineRenderer>(), tracerSpawn.position, hit.point);

            GameObject impact = Instantiate(bulletImpactParticle, projectileSpawnPoint, Quaternion.LookRotation(hit.normal));

        }
        else
        {
            GameObject trail = Instantiate(bulletTracer, projectileSpawnPoint, Quaternion.identity);
            SetTracerLine(trail.GetComponent<LineRenderer>(), tracerSpawn.position, playerCam.transform.position + (playerCam.transform.forward * 100));

            projectileSpawnPoint = playerCam.transform.position + (playerCam.transform.forward * 100);
            GameObject bullet = Instantiate(projectile, projectileSpawnPoint, Quaternion.identity);
            firedProjectiles.Add(bullet);
        }
    }

    public override void ShootFromInput()
    {
        if ((currentAmmo > 0 || infiniteAmmo) && canShoot)
        {
            Shoot();
            GameObject MuzzleFlash = Instantiate(muzzleFlashParticle, muzzleFlashSpawn.position, Quaternion.identity);
            anim.SetTrigger("Shoot");
        }
    }
}
