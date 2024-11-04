using UnityEngine;

public class StraightProjectile : MonoBehaviour
{
    private float speed;
    private int damage;
    private float lifetime;

    public void Initialize(int damageAmount, float projectileSpeed, float projectileLifetime)
    {
        damage = damageAmount;
        speed = projectileSpeed;
        lifetime = projectileLifetime;

        Invoke("Deactivate", lifetime);
    }

    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<Enemy>().TakeDamage(damage);
            gameObject.SetActive(false);
        }
    }
}