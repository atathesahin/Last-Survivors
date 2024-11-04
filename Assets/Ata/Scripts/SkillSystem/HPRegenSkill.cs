using UnityEngine;

[CreateAssetMenu(fileName = "NewHpRegenSkill", menuName = "Skills/Active/HpRegen")]
public class HpRegenSkill : Skill
{
    public float regenAmount = 1f; // Her saniye yenilenecek sağlık miktarı

    public override void ActivateSkill(Player player)
    {
        player.StartCoroutine(ApplyRegen(player));
        Debug.Log("HpRegen etkinleştirildi, yenilenme miktarı: " + regenAmount * currentLevel);
    }

    private System.Collections.IEnumerator ApplyRegen(Player player)
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            player.health = Mathf.Min(player.health + (int)(regenAmount * currentLevel), player.maxHealth);
            Debug.Log($"Health regenerated: {player.health}/{player.maxHealth}");
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