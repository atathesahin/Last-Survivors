using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance;

    public Skill[] allSkills;
    public Skill currentSkill; // Mevcut aktif yetenek

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public Skill GetRandomSkill()
    {
        if (allSkills.Length > 0)
        {
            int randomIndex = Random.Range(0, allSkills.Length);
            currentSkill = allSkills[randomIndex]; // Kazanılan yeteneği currentSkill olarak ayarla
            return currentSkill;
        }
        return null;
    }

    public void UpgradeSkill(Skill skill)
    {
        skill.UpgradeSkill();
    }
}