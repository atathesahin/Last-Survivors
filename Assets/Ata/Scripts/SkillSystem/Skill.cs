using UnityEngine;

public abstract class Skill : ScriptableObject
{
    public string skillName;
    public Sprite icon;
    public int maxLevel = 4;
    public int currentLevel = 1;

    public abstract void ActivateSkill(Player player);
    public abstract void UpgradeSkill();
}