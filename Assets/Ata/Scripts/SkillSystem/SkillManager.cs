using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance;

    public Skill[] allSkills; // Tüm skilllerin ScriptableObject referansları
    public Skill currentSkill; // Aktif skill

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // ScriptableObject'lerin çalışma zamanı kopyalarını oluştur
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
        ResetSkills(); // Oyun başlangıcında skill seviyelerini sıfırla
    }

    public Skill GetRandomSkill()
    {
        if (allSkills.Length > 0)
        {
            int randomIndex = Random.Range(0, allSkills.Length);

            // Random seçilen skill'i kopyala
            currentSkill = Instantiate(allSkills[randomIndex]);
            return currentSkill;
        }
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
            skill.currentLevel = 1; // Varsayılan seviye
        }
        Debug.Log("All skills reset to default level.");
    }
}