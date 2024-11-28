using UnityEngine;

[CreateAssetMenu(fileName = "NewSpeedBoostSkill", menuName = "Skills/Active/SpeedBoost")]
public class SpeedBoostSkill : Skill
{
    public int speedIncreasePerLevel = 1;

    public override void ActivateSkill(Player player)
    {
        // Hız artışı aktif edildiğinde oyuncunun hızını artır
        PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
        if (playerMovement != null)
        {
            playerMovement.IncreaseSpeed(speedIncreasePerLevel * currentLevel);
        }
    }

    public override void UpgradeSkill()
    {
        if (currentLevel < maxLevel)
        {
            currentLevel++;
        }
    }
}