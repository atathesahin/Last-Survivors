using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 50;
    public int damage = 10;
    public float moveSpeed = 3f;
    private Transform playerTransform;

    public float damageCooldown = 1.5f;
    private float lastDamageTime;

    public float stoppingDistance = 1.5f; // Oyuncuya olan minimum mesafe
    [SerializeField] private ObjectPool damagePopupPool;

    void Start()
    {
        playerTransform = Player.Instance.transform;
    }

    void Update()
    {
        MoveTowardsPlayer();
    }

    public void SetDamagePopupPool(ObjectPool pool)
    {
        damagePopupPool = pool;
    }

    private void MoveTowardsPlayer()
    {
        if (playerTransform != null)
        {
            Vector3 direction = (playerTransform.position - transform.position).normalized;

            
            if (Vector3.Distance(transform.position, playerTransform.position) > stoppingDistance)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * moveSpeed);

                transform.position += direction * moveSpeed * Time.deltaTime;
            }

            // Eğer oyuncuya yeterince yakınsa saldırmayı dene
            if (Vector3.Distance(transform.position, playerTransform.position) <= stoppingDistance)
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
        Debug.Log($"[Enemy] Alınan hasar: {damageAmount}"); 
        ShowDamage(damageAmount);

        health -= damageAmount;
        if (health <= 0)
        {
            Die();
        }
    }

    private void ShowDamage(int damageAmount)
    {
        if (damagePopupPool == null)
        {
            Debug.LogError("DamagePopupPool referansı atanmadı!");
            return;
        }

        GameObject popup = damagePopupPool.GetFromPool();
        if (popup != null)
        {
            popup.transform.position = transform.position + Vector3.up * 2f;

            DamagePopup damagePopup = popup.GetComponent<DamagePopup>();
            if (damagePopup != null)
            {
                damagePopup.ShowDamage(damageAmount); // Doğrudan ShowDamage çağırılıyor
            }
            else
            {
                Debug.LogError("DamagePopup scripti prefab'de eksik!");
            }
        }
        else
        {
            Debug.LogError("DamagePopup prefab'ı Object Pool'dan alınamadı!");
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