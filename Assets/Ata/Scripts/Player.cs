using System;
using UnityEngine;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    public static Player Instance;
    public int health = 100;
    public int maxHealth = 100;
    public int hpRegenRate = 1;
    //public int speed = 5;
    public int gold = 0;
    public GameObject currentWeapon; 
    public Transform weaponHolder; 

    private float lastAttackTime;
    private List<Skill> acquiredSkills = new List<Skill>();
    private int currentlevel = 1;
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
        EquipWeapon(currentWeapon); 
    }

    void Update()
    {
        AttackClosestEnemy();
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
        Debug.Log("Player öldü!");
      
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
            
                // UI'deki skill level bilgisini güncelle
                UIManager.Instance.UpdateSkillIcon(existingSkill);
            }
            else
            {
                acquiredSkills.Add(randomSkill);
                randomSkill.ActivateSkill(this); 
                Debug.Log("Yeni yetenek kazandınız: " + randomSkill.skillName);
                
                UIManager.Instance.AddSkillIcon(randomSkill);
            }
        }
        else
        {
            Debug.LogWarning("Rastgele beceri atanamadı.");
        }
    }

    public void EquipWeapon(GameObject weapon)
    {
        if (weapon == null)
        {
            return;
        }

        if (currentWeapon != null && currentWeapon != weapon)
        {
            Destroy(currentWeapon);
        }
        currentWeapon = Instantiate(weapon, weaponHolder);
        currentWeapon.transform.localPosition = new Vector3(0, 0.5f, 0); 
        currentWeapon.transform.localRotation = Quaternion.Euler(-90, 0, 0); 
        Debug.Log("Yeni silah kuşanıldı: " + currentWeapon.name);
    }

    private void AttackClosestEnemy()
    {
        float closestDistance = Mathf.Infinity;
        Enemy closestEnemy = null;
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 15f); 

        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy"))
            {
                float distance = Vector3.Distance(transform.position, hitCollider.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = hitCollider.GetComponent<Enemy>();
                }
            }
        }

        if (closestEnemy != null)
        {
            // Mermi fırlat
            if (currentWeapon != null)
            {
                ProjectileShooter shooter = currentWeapon.GetComponent<ProjectileShooter>();
                if (shooter != null && Time.time >= lastAttackTime + shooter.attackCooldown)
                {
                    shooter.ShootProjectile(closestEnemy.transform);
                    lastAttackTime = Time.time;
                }
            }
        }
    }
    
    
}
