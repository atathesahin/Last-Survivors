using UnityEngine;
using System.Collections.Generic;

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

            
            for (int i = 0; i < allSkills.Length; i++)
            {
                allSkills[i] = Instantiate(allSkills[i]);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        ResetSkills(); 
    }

    public Skill GetRandomSkill()
    {
        
        List<Skill> availableSkills = new List<Skill>();

        foreach (Skill skill in allSkills)
        {
            if (skill.currentLevel < skill.maxLevel)
            {
                availableSkills.Add(skill);
            }
        }

       
        if (availableSkills.Count > 0)
        {
            int randomIndex = Random.Range(0, availableSkills.Count);

          
            currentSkill = Instantiate(availableSkills[randomIndex]);
            return currentSkill;
        }
        
        Debug.Log("All skills are at max level.");
        return null;
    }

    public void UpgradeSkill(Skill skill)
    {
        if (skill.currentLevel < skill.maxLevel)
        {
            skill.UpgradeSkill();
            Debug.Log($"{skill.skillName} upgraded to level {skill.currentLevel}.");
        }
        else
        {
            Debug.Log($"{skill.skillName} is already at max level.");
        }
    }

    public void ResetSkills()
    {
        foreach (Skill skill in allSkills)
        {
            skill.currentLevel = 1; 
        }
        Debug.Log("All skills reset to default level.");
    }
}
