using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour //CONVERT TO BASE CLASS
{
    bool canShoot;
    float shootTimer;
    Vector3 projectileSpawnPoint;

    [Header("References")]
    public Camera playerCam;
    public GameObject projectile;
    public Transform trailSpawn;
    public GameObject bulletTrail;

    [Header("Gun Parameters")]
    public float damage;
    public float fireRate;
    public float spread;
    public float ammoCapacity;
    private float currentAmmo;

    [Header("Extra Modifiers")]
    public bool infiniteAmmo;

    [Header("Spent Bullets List")]
    public List<GameObject> firedBullets = new List<GameObject>();

    private void Awake()
    {
        currentAmmo = ammoCapacity;
    }

    private void Update()
    {
        FireRateTimer();

        if (Input.GetMouseButton(0) && (currentAmmo > 0 || infiniteAmmo) && canShoot == true)
        {
            Shoot();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Recall();
        }
    }

    private void Recall()
    {
        for (int i = firedBullets.Count - 1; i > -1; i--)
        {
            if(firedBullets[i] != null)
            {
                //firedBullets[i].GetComponent<ProjectileBase>().ReturnToPlayer();
                firedBullets[i].GetComponent<ProjectileBase>().ActivateRecallAbility();

            }
            else
            {
                firedBullets[i] = firedBullets[firedBullets.Count - 1];
                firedBullets.RemoveAt(firedBullets.Count - 1);
            }

        }
    }

    public void SetTrail(LineRenderer lr, Vector3 start, Vector3 end)
    {
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
    }

    public void Shoot()
    {
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        canShoot = false;
        shootTimer = fireRate;
        if (!infiniteAmmo)
        {
            currentAmmo--;
        }

        Vector3 shootDir = playerCam.transform.forward + new Vector3(x, y, 0);
        RaycastHit hit;

        if (Physics.Raycast(playerCam.transform.position, shootDir, out hit, float.MaxValue))
        {
            if(hit.point != null)
            {
                projectileSpawnPoint = hit.point;
            }
            GameObject bullet = Instantiate(projectile, projectileSpawnPoint, Quaternion.identity);
            firedBullets.Add(bullet);
            bullet.GetComponent<BulletBounce>().AddForceOfNormal(hit.normal);

            //if (hit.transform.gameObject.GetComponentInParent<IDamageable>() != null)
            //{
            //    hit.transform.gameObject.GetComponentInParent<IDamageable>().TakeDamage(damage);
            //}

            if (hit.transform.gameObject.GetComponent<Hurtbox>() != null)
            {
                hit.transform.gameObject.GetComponent<Hurtbox>().AdjustDamage(damage);
            }

            GameObject trail = Instantiate(bulletTrail, projectileSpawnPoint, Quaternion.identity);
            SetTrail(trail.GetComponent<LineRenderer>(), trailSpawn.position, hit.point);

        }
        else
        {
            GameObject trail = Instantiate(bulletTrail, projectileSpawnPoint, Quaternion.identity);
            SetTrail(trail.GetComponent<LineRenderer>(), trailSpawn.position, playerCam.transform.position + (playerCam.transform.forward * 100));

            projectileSpawnPoint = playerCam.transform.position + (playerCam.transform.forward * 100);
            GameObject bullet = Instantiate(projectile, projectileSpawnPoint, Quaternion.identity);
            firedBullets.Add(bullet);
            Debug.Log("Hit Nothing");
        }

    }

    public void FireRateTimer()
    {
        shootTimer -= Time.deltaTime;
        if (shootTimer <= 0)
        {
            shootTimer = 0;
            canShoot = true;
        }
    }


}
