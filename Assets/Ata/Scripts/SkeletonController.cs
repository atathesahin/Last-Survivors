using UnityEngine;

public class SkeletonController : MonoBehaviour
{
    private Player player;
    private float attackRange;
    private int damage;
    private float attackCooldown = 1.5f;
    private float lastAttackTime;
    private Transform targetEnemy;
    public float moveSpeed = 3f;
    public Animator animator;
    public float stoppingDistance = 2f; // Distance to stop before colliding with enemy

    public void Initialize(Player player, float attackRange, int damage)
    {
        this.player = player;
        this.attackRange = attackRange;
        this.damage = damage;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        FindEnemyByTag();
        RotateTowardsEnemy();
        MoveTowardsEnemy();
        AttackEnemiesInRange();
    }

    private void FindEnemyByTag()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;

        foreach (var enemyObj in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemyObj.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemyObj.transform;
            }
        }

        targetEnemy = closestEnemy;
    }

    private void RotateTowardsEnemy()
    {
        if (targetEnemy != null)
        {
            Vector3 direction = (targetEnemy.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
    }

    private void MoveTowardsEnemy()
    {
        if (targetEnemy != null)
        {
            float distanceToTarget = Vector3.Distance(transform.position, targetEnemy.position);
            if (distanceToTarget > stoppingDistance)
            {
                Vector3 direction = (targetEnemy.position - transform.position).normalized;
                transform.position += direction * moveSpeed * Time.deltaTime;
                animator.SetBool("isWalking", true);
            }
            else
            {
                animator.SetBool("isWalking", false);
            }
        }
        else
        {
            animator.SetBool("isWalking", false);
        }
    }

    private void AttackEnemiesInRange()
    {
        if (targetEnemy == null) return;

        float distanceToTarget = Vector3.Distance(transform.position, targetEnemy.position);
        if (distanceToTarget <= attackRange && Time.time > lastAttackTime + attackCooldown)
        {
            Enemy enemy = targetEnemy.GetComponent<Enemy>();
            if (enemy != null)
            {
                animator.SetTrigger("attack");
                enemy.TakeDamage(damage);
                lastAttackTime = Time.time;
                
            }
        }
    }
}