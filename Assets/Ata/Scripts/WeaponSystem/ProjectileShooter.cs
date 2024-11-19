using UnityEngine;

public class ProjectileShooter : MonoBehaviour
{
    public GameObject magicProjectilePrefab;
    public Transform shootPoint;
    public float projectileSpeed = 20f;
    public int attackDamage = 20;
    public float attackCooldown = 1.5f; 

    public void ShootProjectile(Transform target)
    {
        if (magicProjectilePrefab != null)
        {
            if (shootPoint != null)
            {
                GameObject projectile = Instantiate(magicProjectilePrefab, shootPoint.position, Quaternion.identity);
                Vector3 direction = (target.position - shootPoint.position).normalized;
                projectile.GetComponent<Rigidbody>().velocity = direction * projectileSpeed;
                projectile.GetComponent<Projectile>().Initialize(attackDamage); 
                Destroy(projectile, 5f); 
                Debug.Log("Mermi fırlatıldı: " + projectile.name);
            }
            else
            {
                Debug.LogWarning("Shoot point atanmadı!");
            }
        }
        else
        {
            Debug.LogWarning("magicProjectilePrefab atanmamış!");
        }
    }
}