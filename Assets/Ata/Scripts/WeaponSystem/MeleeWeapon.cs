using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    public int attackDamage = 25; 
    public float attackRange = 2f; 
    public float attackCooldown = 1f; 
    public Animator animator;

    private float lastAttackTime;

    void Update()
    {
        AutoAttackClosestEnemy();
    }


    public void AutoAttackClosestEnemy()
    {

        if (Time.time >= lastAttackTime + attackCooldown)
        {
            Enemy closestEnemy = GetClosestEnemy();
            if (closestEnemy != null)
            {

                PlayAttackAnimation();
                closestEnemy.TakeDamage(attackDamage);
                lastAttackTime = Time.time;
            }
        }
    }

    private Enemy GetClosestEnemy()
    {
        float closestDistance = Mathf.Infinity;
        Enemy closestEnemy = null;
        
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange);

        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy"))
            {
                float distance = Vector3.Distance(transform.position, hitCollider.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = hitCollider.GetComponent<Enemy>();
                }
            }
        }

        return closestEnemy;
    }

    private void PlayAttackAnimation()
    {
        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }
    }
}