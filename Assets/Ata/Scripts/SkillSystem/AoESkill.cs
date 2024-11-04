using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAoESkill", menuName = "Skills/Active/AoESkill")]
public class AoESkill : Skill
{
    public float baseRadius = 3f; // AOE etkisinin yarıçapı
    public int damage = 10; // Verilecek hasar
    public float damageInterval = 1f; // Her ne kadar sıklıkla hasar verilecek

    public override void ActivateSkill(Player player)
    {
        player.StartCoroutine(ApplyAoE(player));
        Debug.Log($"{skillName} activated! AOE damage area created.");
    }

    private IEnumerator ApplyAoE(Player player)
    {
        while (true)
        {
            Collider[] enemies = Physics.OverlapSphere(player.transform.position, baseRadius * currentLevel);
            foreach (Collider enemy in enemies)
            {
                if (enemy.CompareTag("Enemy"))
                {
                    Enemy enemyComponent = enemy.GetComponent<Enemy>();
                    if (enemyComponent != null)
                    {
                        enemyComponent.TakeDamage(damage);
                        Debug.Log($"{enemy.name} has taken {damage} damage from AOE skill.");
                    }
                }
            }
            // Süre sonsuz olduğundan elapsedTime kullanımı kaldırıldı
            yield return new WaitForSeconds(damageInterval);
        }
    }

    public override void UpgradeSkill()
    {
        if (currentLevel < maxLevel)
        {
            currentLevel++;
            Debug.Log($"{skillName} upgraded to level {currentLevel}");
        }
    }
}