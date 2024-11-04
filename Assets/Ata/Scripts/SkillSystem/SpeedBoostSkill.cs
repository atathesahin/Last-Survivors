using UnityEngine;

[CreateAssetMenu(fileName = "NewSpeedBoostSkill", menuName = "Skills/Active/SpeedBoost")]
public class SpeedBoostSkill : Skill
{
    public int speedIncreasePerLevel = 1;

    public override void ActivateSkill(Player player)
    {
        player.speed += speedIncreasePerLevel * currentLevel;
    }

    public override void UpgradeSkill()
    {
        if (currentLevel < maxLevel)
        {
            currentLevel++;
        }
    }
}