using System;
using UnityEngine;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    public static Player Instance;
    public int health = 100;
    public int maxHealth = 100;
    public int hpRegenRate = 1;
    public int gold = 0;
    public GameObject currentWeapon; 
    public Transform weaponHolder; 
    public Animator animator;
    private float lastAttackTime;
    private Dictionary<string, Skill> acquiredSkills = new Dictionary<string, Skill>();
    public PlayerMovement playerMovement;
    private Renderer playerRenderer;
    [SerializeField] private float rotationSpeed = 5f; // Smooth dönüşüm için float olarak güncellendi

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
        playerRenderer = GetComponent<Renderer>();
        GainRandomSkill();  
        EquipWeapon(currentWeapon); 
    }

    void Update()
    {
        RotateTowardsClosestEnemy(); // Her zaman en yakın düşmana dön
        AttackClosestEnemy();
    }

    private void RotateTowardsClosestEnemy()
    {
        if (!playerMovement.IsMoving()) // Eğer karakter hareket etmiyorsa en yakın düşmana dön
        {
            Enemy closestEnemy = GetClosestEnemy();
            if (closestEnemy != null)
            {
                Vector3 direction = (closestEnemy.transform.position - transform.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
            }
        }
    }

    private Enemy GetClosestEnemy()
    {
        float closestDistance = Mathf.Infinity;
        Enemy closestEnemy = null;
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 15f); // Arama yarıçapı

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

        return closestEnemy;
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
                ApplyGlowEffect();
            }
        }
        else
        {
            acquiredSkills.Add(randomSkill.skillName, randomSkill);
            randomSkill.ActivateSkill(this);
            Debug.Log("Yeni yetenek kazandınız: " + randomSkill.skillName);

            UIManager.Instance.AddSkillIcon(randomSkill);
            ApplyGlowEffect();
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
        if (currentWeapon != null && playerMovement != null)
        {
            ProjectileShooter shooter = currentWeapon.GetComponent<ProjectileShooter>();

            if (shooter != null)
            {
                Enemy closestEnemy = GetClosestEnemy();
                if (closestEnemy != null && Time.time >= lastAttackTime + shooter.attackCooldown)
                {
                    playerMovement.PlayAttackAnimation();
                    shooter.ShootProjectile(closestEnemy.transform);
                    lastAttackTime = Time.time;
                }
            }
        }
    }

    private void ApplyGlowEffect()
    {
        Renderer[] renderers = GetComponentsInChildren<Renderer>();

        foreach (Renderer renderer in renderers)
        {
            Material[] materials = renderer.materials;

            foreach (Material mat in materials)
            {
                mat.SetColor("_EmissionColor", Color.yellow);
                mat.EnableKeyword("_EMISSION");
            }
        }

        StartCoroutine(RemoveGlowEffect(3f)); 
    }

    private System.Collections.IEnumerator RemoveGlowEffect(float delay)
    {
        yield return new WaitForSeconds(delay);
        
        Renderer[] renderers = GetComponentsInChildren<Renderer>();

        foreach (Renderer renderer in renderers)
        {
            Material[] materials = renderer.materials;

            foreach (Material mat in materials)
            {
                mat.SetColor("_EmissionColor", Color.black);
            }
        }
    }
}