using UnityEngine;

[CreateAssetMenu(fileName = "NewHPBoostSkill", menuName = "Skills/Active/HPBoost")]
public class HPBoostSkill : Skill
{
    public int hpIncreasePerLevel = 10;

    public override void ActivateSkill(Player player)
    {
        player.maxHealth += hpIncreasePerLevel;
        player.health += hpIncreasePerLevel;
        Debug.Log($"{skillName} activated! Player max health increased to {player.maxHealth}");
    }

    public override void UpgradeSkill()
    {
        if (currentLevel < maxLevel)
        {
            currentLevel++;
            ActivateSkill(Player.Instance); // Her seviyede tekrar etkinleştir
            Debug.Log($"{skillName} upgraded to level {currentLevel}");
        }
    }
}