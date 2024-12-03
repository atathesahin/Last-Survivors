using UnityEngine;

public class ProjectileShooter : MonoBehaviour
{
    public GameObject magicProjectilePrefab;
    public Transform shootPoint;
    public float projectileSpeed = 20f;
    public int attackDamage = 20;
    public float attackCooldown = 1.5f;
    public float attackRange = 15f;

    public void ShootProjectile(Transform target)
    {
        if (target == null)
        {
            Debug.LogWarning("Hedef bulunamadı!");
            return;
        }

        float distanceToTarget = Vector3.Distance(shootPoint.position, target.position);
        if (distanceToTarget <= attackRange)
        {
            if (magicProjectilePrefab != null && shootPoint != null)
            {
         
                GameObject projectile = Instantiate(magicProjectilePrefab, shootPoint.position, Quaternion.identity);
                Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();

                if (projectileRb != null)
                {
                    Vector3 direction = (target.position - shootPoint.position).normalized;
                    projectileRb.velocity = direction * projectileSpeed;

        
                    Projectile projectileComponent = projectile.GetComponent<Projectile>();
                    if (projectileComponent != null)
                    {
                        projectileComponent.Initialize(attackDamage);
                    }

            
                    Destroy(projectile, 5f);
                }
            }
        }
    }
}