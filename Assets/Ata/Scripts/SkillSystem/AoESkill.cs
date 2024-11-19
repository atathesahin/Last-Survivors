using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAoESkill", menuName = "Skills/Active/AoESkill")]
public class AoESkill : Skill
{
   public float baseRadius = 2f; 
    public int damage = 10; 
    public float damageInterval = 1f; 
    public GameObject aoeIndicatorPrefab; 

    private GameObject aoeIndicatorInstance;

    public override void ActivateSkill(Player player)
    {
        player.StartCoroutine(ApplyAoE(player));
        Debug.Log($"{skillName} activated! AOE damage area created.");

        
        if (aoeIndicatorInstance == null && aoeIndicatorPrefab != null)
        {
            aoeIndicatorInstance = Instantiate(aoeIndicatorPrefab, player.transform.position + new Vector3(0, +0.04f, 0), Quaternion.identity);
aoeIndicatorInstance.transform.localScale = new Vector3(baseRadius * currentLevel * 3, 0.01f, baseRadius * currentLevel * 3);
            aoeIndicatorInstance.transform.parent = player.transform;
        }
        if (aoeIndicatorInstance != null)
        {
            aoeIndicatorInstance.transform.position = player.transform.position + new Vector3(0, +0.04f, 0);
        }
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
            yield return new WaitForSeconds(damageInterval);
        }
    }

    public override void UpgradeSkill()
    {
        if (aoeIndicatorInstance != null)
        {
            aoeIndicatorInstance.transform.localScale = new Vector3(baseRadius * currentLevel * 2, 0.01f, baseRadius * currentLevel * 2);
        }
        if (currentLevel < maxLevel)
        {
            currentLevel++;
            Debug.Log($"{skillName} upgraded to level {currentLevel}");
        }
    }
}