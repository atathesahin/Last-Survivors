using UnityEngine;

public class Axe : MonoBehaviour
{
    private int damage; 

    public void Initialize(int damage)
    {
        this.damage = damage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                Debug.Log($"Enemy hit! Damage: {damage}");
            }
        }

        Destroy(gameObject,0.5f);
    }
}