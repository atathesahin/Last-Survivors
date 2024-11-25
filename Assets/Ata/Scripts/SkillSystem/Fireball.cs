using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float speed; 
    public int damage; 
    public bool applyBurn; 
    public int burnDuration; 
    public int burnDamagePerSecond; 
    
    public float lifetime = 5f;
    private void Update()
    {
        // Fireball'un ileriye doğru hareketi
        transform.Translate(Vector3.forward * (speed * Time.deltaTime));
        Destroy(gameObject,4);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                if (applyBurn)
                {
                    enemy.StartCoroutine(ApplyBurnEffect(enemy));
                }
            }

        
        }
    }

    private IEnumerator ApplyBurnEffect(Enemy enemy)
    {
        float elapsedTime = 0f;
        while (elapsedTime < burnDuration)
        {
            enemy.TakeDamage(burnDamagePerSecond);
            yield return new WaitForSeconds(1f);
            elapsedTime += 1f;
        }
    }
}
