using System;
using UnityEngine;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    public static Player Instance;
    public int health = 100;
    public int maxHealth = 100;
    public int hpRegenRate = 1;
    public int speed = 5;
    public int gold = 0;

    private List<Skill> acquiredSkills = new List<Skill>(); // Kazanılmış yeteneklerin listesi

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
        GainRandomSkill();  // Oyunun başında rastgele bir yetenek kazandır
    }
    /*
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemyComponent = other.GetComponent<Enemy>();
            if (enemyComponent != null && acquiredSkills.Exists(skill => skill is AoESkill))
            {
                AoESkill aoeSkill = (AoESkill)acquiredSkills.Find(skill => skill is AoESkill);
                if (aoeSkill != null)
                {
                    enemyComponent.TakeDamage((int)aoeSkill.damage);
                    Debug.Log($"{other.name} has taken {aoeSkill.damage} damage from AOE skill.");
                }
            }
        }
    }
    */

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

    private void Die()
    {
        Debug.Log("Player öldü!");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SkillPoint") && WaveManager.Instance != null && WaveManager.Instance.IsInPreparationPhase())
        {
            Debug.Log("SkillPoint tetiklendi, hazırlık aşamasındayız.");
            GainRandomSkill();  // Hazırlık süresinde rastgele bir yetenek kazandırır veya yükseltir
        }
    }

    public void GainRandomSkill()
    {
        Skill randomSkill = SkillManager.Instance.GetRandomSkill();

        if (randomSkill != null)
        {
            Skill existingSkill = acquiredSkills.Find(skill => skill.skillName == randomSkill.skillName);
            if (existingSkill != null)
            {
                existingSkill.UpgradeSkill();
                Debug.Log("Yetenek yükseltildi: " + existingSkill.skillName);
            }
            else
            {
                acquiredSkills.Add(randomSkill);
                randomSkill.ActivateSkill(this); // Yetenek etkinleştiriliyor
                Debug.Log("Yeni yetenek kazandınız: " + randomSkill.skillName);
            }
        }
        else
        {
            Debug.LogWarning("Rastgele beceri atanamadı.");
        }
    }

    public void GainGold(int amount)
    {
        gold += amount;
        UIManager.Instance.UpdateGoldUI(gold);
    }

    public void GainSkill(Skill newSkill)
    {
        acquiredSkills.Add(newSkill);
    }

    void OnDrawGizmos()
    {
        // Sadece AOE yeteneği aktifken etki alanını görselleştirin
        if (SkillManager.Instance != null && SkillManager.Instance.currentSkill is AoESkill)
        {
            Gizmos.color = Color.red;
            float radius = ((AoESkill)SkillManager.Instance.currentSkill).baseRadius * ((AoESkill)SkillManager.Instance.currentSkill).currentLevel;
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }
}