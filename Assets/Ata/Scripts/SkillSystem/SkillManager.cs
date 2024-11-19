using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance;

    public Skill[] allSkills;
    public Skill currentSkill; 

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
            currentSkill = allSkills[randomIndex]; 
            return currentSkill;
        }
        return null;
    }

    public void UpgradeSkill(Skill skill)
    {
        skill.UpgradeSkill();
    }
}