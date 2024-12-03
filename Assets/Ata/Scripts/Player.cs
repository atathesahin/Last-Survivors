using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public static Player Instance;

    public int health = 100;
    public int maxHealth = 100;
    public int hpRegenRate = 1;
    public int gold = 0;
    public Transform weaponHolder;
    public PlayerMovement playerMovementAttack;

    private Dictionary<string, Skill> acquiredSkills = new Dictionary<string, Skill>();

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

    void Start()
    {
        GainRandomSkill();
        EquipWeapon(playerMovementAttack.currentWeapon);
    }

    public void TakeDamage(int damageAmount)
    {
        health -= damageAmount;
        if (health < 0) health = 0;
        UIManager.Instance.UpdateHealthUI(health, maxHealth);

        if (health <= 0)
        {
            Die();
        }
    }

    public void GainGold(int amount)
    {
        gold += amount;
        UIManager.Instance.UpdateGoldUI(gold);
    }

    private void Die()
    {
        Destroy(gameObject);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GainRandomSkill()
    {
        List<Skill> availableSkills = new List<Skill>();
        foreach (Skill skill in SkillManager.Instance.allSkills)
        {
            if (!acquiredSkills.ContainsKey(skill.skillName) || acquiredSkills[skill.skillName].currentLevel < skill.maxLevel)
            {
                availableSkills.Add(skill);
            }
        }

        if (availableSkills.Count == 0)
        {
            Debug.LogWarning("Tüm skiller maksimum seviyede. Yeni beceri kazanılamıyor.");
            return;
        }

        Skill randomSkill = availableSkills[UnityEngine.Random.Range(0, availableSkills.Count)];

        if (acquiredSkills.ContainsKey(randomSkill.skillName))
        {
            Skill existingSkill = acquiredSkills[randomSkill.skillName];
            if (existingSkill.currentLevel < existingSkill.maxLevel)
            {
                existingSkill.UpgradeSkill();
                Debug.Log("Yetenek yükseltildi: " + existingSkill.skillName);

                UIManager.Instance.UpdateSkillIcon(existingSkill);
                UIManager.Instance.ShowSkillNotification($"{existingSkill.skillName} upgraded to level {existingSkill.currentLevel}");
            }
        }
        else
        {
            acquiredSkills.Add(randomSkill.skillName, randomSkill);
            randomSkill.ActivateSkill(this);
            Debug.Log("Yeni yetenek kazandınız: " + randomSkill.skillName);

            UIManager.Instance.AddSkillIcon(randomSkill);
            UIManager.Instance.ShowSkillNotification($"New Skill Acquired: {randomSkill.skillName}");
        }
    }

    public void EquipWeapon(GameObject weapon)
    {
        if (weapon == null)
        {
            return;
        }

        if (playerMovementAttack.currentWeapon != null && playerMovementAttack.currentWeapon != weapon)
        {
            Destroy(playerMovementAttack.currentWeapon);
        }
        playerMovementAttack.currentWeapon = Instantiate(weapon, weaponHolder);
        playerMovementAttack.currentWeapon.transform.localPosition = new Vector3(0, 0.5f, 0);
        playerMovementAttack.currentWeapon.transform.localRotation = Quaternion.Euler(-90, 0, 0);
        Debug.Log("Yeni silah kuşanıldı: " + playerMovementAttack.currentWeapon.name);
    }
}
