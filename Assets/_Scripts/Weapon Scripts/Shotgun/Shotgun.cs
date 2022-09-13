using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : WeaponBase
{
    Animator anim;
    private int buckShotCount;

    [Header("Shotgun Parameters")]
    public int buckShot;


    [Header("Shotgun Specific References")]
    public GameObject muzzleFlashParticle;
    public Transform muzzleFlashSpawn;
    public GameObject bulletImpactParticle;
    public GameObject impactSparkParticle;
    public GameObject impactCritParticle;

    new void Awake()
    {
        base.Awake();
        anim = GetComponent<Animator>();
    }

    new void Update()
    {
        base.Update();
    }

    public override void Shoot()
    {
        canShoot = false;
        shootTimer = fireRate;

        if (!infiniteAmmo)
        {
            currentAmmo--;
        }

        for (int i = 0; i < buckShot; i++)
        {
            float x = Random.Range(-spread, spread);
            float y = Random.Range(-spread, spread);
            float z = Random.Range(-spread, spread);

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
                    Hurtbox hb = hit.transform.gameObject.GetComponent<Hurtbox>();
                    hb.AdjustDamage(damage);

                    if (hb.isWeakPoint)
                    {
                        GameObject critSpark = Instantiate(impactCritParticle, projectileSpawnPoint, Quaternion.identity);

                    }
                    else
                    {
                        GameObject spark = Instantiate(impactSparkParticle, projectileSpawnPoint, Quaternion.identity);

                    }
                }

                GameObject impact = Instantiate(bulletImpactParticle, projectileSpawnPoint, Quaternion.LookRotation(hit.normal));


                GameObject tracer = Instantiate(bulletTracer, projectileSpawnPoint, Quaternion.identity);
                SetTracerLine(tracer.GetComponent<LineRenderer>(), tracerSpawn.position, hit.point);

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

    }

    public override void ShootFromInput()
    {
        if ((currentAmmo > 0 || infiniteAmmo) && canShoot && !isSwitchingWeapons)
        {
            Shoot();
            GameObject MuzzleFlash = Instantiate(muzzleFlashParticle, muzzleFlashSpawn.position, Quaternion.identity);
            if(anim != null)
            {
                anim.SetTrigger("Shoot");
            }
        }
    }

    public override void PlaySwitchAnimation()
    {
        if (anim != null)
        {
            anim.SetTrigger("Switching");
        }

    }

    public override void AddAmmo()
    {
        buckShotCount++;
        if(buckShotCount == buckShot && currentAmmo < magCapacity)
        {
            currentAmmo++;
            buckShotCount = 0;
        }
    }
}
