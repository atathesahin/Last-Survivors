using UnityEngine;

public class Projectile : MonoBehaviour
{
    private int damage;

    public void Initialize(int attackDamage)
    {
        damage = attackDamage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                Destroy(gameObject); // Çarpışmadan sonra mermi yok edilir
            }
        }
    }
}