using UnityEngine;
using System.Collections.Generic;

public class ProjectileSpawner : MonoBehaviour
{
    public static ProjectileSpawner instance;
    public GameObject projectilePrefab;
    public Transform spawnPointTop;
    public Transform spawnPointBottom;
    public float initialFireRate = 0.5f;
    public float fireRate = 0.5f;
    private float nextFire = 0f;

    public int poolSize = 30; // Number of projectiles to pre-instantiate
    private List<GameObject> projectilePool;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        spawnPointTop = gameObject.transform.GetChild(0);
        spawnPointBottom = gameObject.transform.GetChild(1);
        if(projectilePrefab == null)
        {
            projectilePrefab = Resources.Load<GameObject>("Projectile");
        }

        // Initialize the pool
        projectilePool = new List<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            GameObject proj = Instantiate(projectilePrefab, transform.GetChild(2));
            proj.SetActive(false); // Deactivate it for pooling
            projectilePool.Add(proj);
        }
    }

    void Update()
    {
        if(GameManager.instance.currentGameState == GameManager.GameState.Playing)
        {
            fireRate = Mathf.Max(0.1f, 0.5f - (Player.instance.GetScore() * 0.01f));
            if (Time.time > nextFire)
            {
                nextFire = Time.time + fireRate;
                Shoot();
            }
        }
    }

    private void Shoot()
    {
        GameObject projectile = GetPooledProjectile();
        if (projectile != null)
        {
            // Position the projectile at a random spawn point between top and bottom
            Vector3 spawnPoint = new Vector3(
                spawnPointTop.position.x, 
                Random.Range(spawnPointBottom.position.y, spawnPointTop.position.y), 
                spawnPointTop.position.z
            );

            projectile.transform.position = spawnPoint;
            projectile.SetActive(true);
        }
    }

    public void ResetProjectiles()
    {
        foreach (GameObject projectile in projectilePool)
        {
            projectile.SetActive(false);
        }
    }

    public void StopProjectiles()
    {
        foreach (GameObject projectile in projectilePool)
        {
            projectile.GetComponent<Projectile>().StopMovement();
        }
    }

    public void ResetSpeeds()
    {
        fireRate = initialFireRate;
        foreach (GameObject projectile in projectilePool)
        {
            projectile.GetComponent<Projectile>().ResetSpeed();
        }
    }

    GameObject GetPooledProjectile()
    {
        foreach (GameObject projectile in projectilePool)
        {
            if (!projectile.activeInHierarchy)
            {
                return projectile;
            }
        }

        // If no inactive projectiles, expand the pool
        GameObject newProjectile = Instantiate(projectilePrefab, transform.GetChild(2));
        newProjectile.SetActive(false);
        projectilePool.Add(newProjectile);
        return newProjectile;
    }
}
