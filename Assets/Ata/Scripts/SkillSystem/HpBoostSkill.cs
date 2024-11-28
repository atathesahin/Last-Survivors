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
            if (currentLevel == 1)
            {
                hpIncreasePerLevel += 20;
            }
            else if (currentLevel == 2)
            {
                hpIncreasePerLevel += 40;
            }
            else if (currentLevel == 3)
            {
                hpIncreasePerLevel += 80;
                
            }
            else if (currentLevel == 4)
            {
                hpIncreasePerLevel += 160;
            }
            ActivateSkill(Player.Instance); 
            Debug.Log($"{skillName} upgraded to level {currentLevel}");
        }
    }
}