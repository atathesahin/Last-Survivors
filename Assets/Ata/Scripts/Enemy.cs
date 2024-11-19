using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 50;
    public int damage = 10;
    public float moveSpeed = 3f;
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

          
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * moveSpeed);

          
            transform.position += direction * moveSpeed * Time.deltaTime;

     
            if (Vector3.Distance(transform.position, playerTransform.position) < 1.5f)
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
        Player.Instance.GainGold(10); 
        UIManager.Instance.UpdateGoldUI(Player.Instance.gold);
        WaveManager.Instance.EnemyDefeated(gameObject); 
        Destroy(gameObject); 
    }
}
