using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 50;
    public int damage = 10;
    public float moveSpeed = 3f;
    private Transform playerTransform;

    // Hasar bekleme süresi ve son hasar zamanı
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

            // Oyuncuya bakacak şekilde düşmanı döndür
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * moveSpeed);

            // Oyuncuya doğru hareket et
            transform.position += direction * moveSpeed * Time.deltaTime;

            // Eğer düşman oyuncuya çok yakınsa, saldırmaya çalış
            if (Vector3.Distance(transform.position, playerTransform.position) < 1.5f)
            {
                TryAttackPlayer();
            }
        }
    }

    private void TryAttackPlayer()
    {
        // Hasar bekleme süresi dolduysa hasar ver
        if (Time.time >= lastDamageTime + damageCooldown)
        {
            Player.Instance.TakeDamage(damage);
            lastDamageTime = Time.time; // Son hasar zamanını güncelle
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
        Player.Instance.GainGold(10); // Düşman öldüğünde altın kazandır
        UIManager.Instance.UpdateGoldUI(Player.Instance.gold); // Altın bilgisini güncelle
        WaveManager.Instance.EnemyDefeated(gameObject); // Düşman öldüğünde bildir
        Destroy(gameObject); // Düşmanı devre dışı bırak
    }
}
