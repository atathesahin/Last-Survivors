using UnityEngine;

public class Boss : MonoBehaviour
{
    public int health = 500;
    public int damage = 25;
    public float moveSpeed = 2f;
    public int goldReward = 50;  
    private Transform playerTransform;
    public float damageCooldown = 1.5f;
    private float lastDamageTime;
    void Start()
    {
        playerTransform = Player.Instance.transform;
    }

    void Update()
    {
        MoveTowardsPlayer();
    }

    private void MoveTowardsPlayer()
    {
        if (playerTransform != null)
        {
            Vector3 direction = (playerTransform.position - transform.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;

            if (Vector3.Distance(transform.position, playerTransform.position) < 2f)
            {
                TryAttackPlayer();
            }
        }
    }

    private void TryAttackPlayer()
    {
        
        if (Time.time >= lastDamageTime + damageCooldown)
        {
            Player.Instance.TakeDamage(damage);
            lastDamageTime = Time.time; 
        }
    }

    public void TakeDamage(int damageAmount)
    {
        health -= damageAmount;
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Player.Instance.GainGold(goldReward);  
        Destroy(gameObject);         
    }
}