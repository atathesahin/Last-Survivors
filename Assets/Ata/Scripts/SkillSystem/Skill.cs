using UnityEngine;

public abstract class Skill : ScriptableObject
{
    public string skillName;
    public Sprite icon;
    public int maxLevel = 4;
    public int currentLevel = 1;

    public abstract void ActivateSkill(Player player);
    public virtual void UpgradeSkill()
    {
        if (currentLevel < maxLevel)
        {
            currentLevel++;
            Debug.Log($"{skillName} upgraded to level {currentLevel}.");
        }
        else
        {
            Debug.Log($"{skillName} is already at max level.");
        }
    }
}