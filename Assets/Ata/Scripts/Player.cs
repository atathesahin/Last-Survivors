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
    public GameObject currentWeapon; // Oyuncunun kullandığı mevcut silah
    public Transform weaponHolder; // Silahın elde tutulacağı pozisyon

    private float lastAttackTime;
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
        EquipWeapon(currentWeapon); // Başlangıç silahını kuşan
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
        UIManager.Instance.UpdateGoldUI(gold); // Altın bilgisini güncelle
    }

    private void Die()
    {
        Debug.Log("Player öldü!");
        // Oyuncu öldüğünde yapılacak işlemler
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
                
                UIManager.Instance.AddSkillIcon(randomSkill.icon);
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
            Destroy(currentWeapon); // Mevcut silahı kaldır
        }
        currentWeapon = Instantiate(weapon, weaponHolder);
        currentWeapon.transform.localPosition = new Vector3(0, 0.5f, 0); // Silahın konumunu ayarlayın
        currentWeapon.transform.localRotation = Quaternion.Euler(-90, 0, 0); // Silahın rotasyonunu ayarlayın
        Debug.Log("Yeni silah kuşanıldı: " + currentWeapon.name);
    }

    private void AttackClosestEnemy()
    {
        float closestDistance = Mathf.Infinity;
        Enemy closestEnemy = null;
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 15f); // 15 birimlik bir menzil içindeki düşmanları kontrol et

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

    public bool TryPurchaseWeapon(GameObject weapon, int cost)
    {
        if (gold >= cost)
        {
            gold -= cost;
            EquipWeapon(weapon);
            UIManager.Instance.UpdateGoldUI(gold); // Altın bilgisini güncelle
            return true;
        }
        else
        {
            Debug.Log("Yeterli altın yok!");
            return false;
        }
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
