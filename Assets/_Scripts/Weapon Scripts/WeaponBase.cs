using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBase : MonoBehaviour
{
    protected bool canShoot;
    [HideInInspector]
    public bool canRecall;
    protected float shootTimer;
    [HideInInspector]
    public float currentAmmo;
    protected Vector3 projectileSpawnPoint;
    [HideInInspector]
    protected bool isSwitchingWeapons = false;
    [HideInInspector]
    public float recallCooldownTimer;

    [Header("Weapon Info")]
    public string weaponName;
    public int weaponID;
    public bool isActiveWeapon;

    [Header("References")]
    public Camera playerCam;
    public GameObject projectile;
    public Transform tracerSpawn;
    public GameObject bulletTracer;

    [Header("Mesh List")]
    public List<MeshRenderer> Meshes = new List<MeshRenderer>();

    [Header("Weapon Statistics")]
    public float damage;
    public float fireRate;
    public float spread;
    public float magCapacity;

    [Header("Base Recall Parameters")]
    public float recallCooldown;

    [Header("Modifiers")]
    public bool infiniteAmmo;

    [Header("Layermask")]
    public LayerMask ignore;

    [Header("Spent Projectiles List")]
    public List<GameObject> firedProjectiles = new List<GameObject>();

    protected void Awake()
    {
        currentAmmo = magCapacity;
        recallCooldownTimer = 0;
    }

    protected void Update()
    {
        FireRateTimer();
        RecallTimer();
    }

    public virtual void AddAmmo()
    {
        if(currentAmmo < magCapacity)
        {
            currentAmmo++;
        }
    }

    public virtual void ShootFromInput()
    {
        if((currentAmmo > 0 || infiniteAmmo) && canShoot && !isSwitchingWeapons)
        {
            Shoot();
        }
    }

    public virtual void Shoot()
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

    public virtual void Recall()
    {
        canRecall = false;
        recallCooldownTimer = recallCooldown;
        for (int i = firedProjectiles.Count - 1; i > -1; i--)
        {
            if (firedProjectiles[i] != null)
            {
                firedProjectiles[i].GetComponent<ProjectileBase>().ActivateRecallAbility();

            }
            else
            {
                firedProjectiles[i] = firedProjectiles[firedProjectiles.Count - 1];
                firedProjectiles.RemoveAt(firedProjectiles.Count - 1);
            }

        }
    }

    public virtual void SwitchingWeapons()
    {
        isSwitchingWeapons = !isSwitchingWeapons;
    }

    public virtual void PlaySwitchAnimation(){}

    public virtual void FireRateTimer()
    {
        shootTimer -= Time.deltaTime;
        if (shootTimer <= 0)
        {
            shootTimer = 0;
            canShoot = true;
        }
    }

    public virtual void RecallTimer()
    {
        recallCooldownTimer -= Time.deltaTime;
        if(recallCooldownTimer <= 0)
        {
            recallCooldownTimer = 0;
            canRecall = true;
        }
    }

    public virtual void ToggleMesh()
    {
        foreach(MeshRenderer mr in Meshes)
        {
            mr.enabled = !mr.enabled;
        }
    }

    public void SetTracerLine(LineRenderer lr, Vector3 start, Vector3 end)
    {
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
    }
}
