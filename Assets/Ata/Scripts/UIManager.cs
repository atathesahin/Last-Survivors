using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using DG.Tweening; // DOTween kütüphanesi

public class UIManager : MonoBehaviour
{
     public static UIManager Instance;

    public TextMeshProUGUI healthText; 
    public TextMeshProUGUI goldText; 
    public TextMeshProUGUI waveText; 
    public TextMeshProUGUI countdownText; 
    public TextMeshProUGUI weaponCostText; 
    public Transform skillPanel;
    public GameObject skillIconPrefab; 

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UpdateHealthUI(int currentHealth, int maxHealth)
    {
        if (healthText != null)
        {
            healthText.text = "Health: " + currentHealth + "/" + maxHealth;
        }
    }

    public void UpdateGoldUI(int gold)
    {
        if (goldText != null)
        {
            goldText.text = "Gold: " + gold;
        }
    }

    public void UpdateWaveUI(int wave)
    {
        if (waveText != null)
        {
            waveText.text = "Wave: " + wave;
        }
    }

    public void UpdateCountdownUI(float countdown)
    {
        if (countdownText != null)
        {
            countdownText.text = "Next Wave In: " + Mathf.CeilToInt(countdown) + "s";
        }
    }

    public void UpdateWeaponCostUI(int cost)
    {
        if (weaponCostText != null)
        {
            if (cost > 0)
            {
                weaponCostText.text = "Weapon Cost: " + cost + " Gold";
            }
            else
            {
                weaponCostText.text = "";
            }
        }
    }

    public void AddSkillIcon(Skill skill)
    {
        if (skillPanel != null && skillIconPrefab != null)
        {
            // Yeni yetenek ikonu oluştur
            GameObject newIcon = Instantiate(skillIconPrefab, skillPanel);
            Image iconImage = newIcon.GetComponent<Image>();
            if (iconImage != null)
            {
                iconImage.sprite = skill.icon;
            }

            // Seviye bilgisi için Text bileşeni oluştur
            TextMeshProUGUI levelText = newIcon.GetComponentInChildren<TextMeshProUGUI>();
            if (levelText != null)
            {
                levelText.text = "LvL: " + skill.currentLevel;
            }
        }
    }
    public void UpdateSkillIcon(Skill skill)
    {
        foreach (Transform child in skillPanel)
        {
            Image iconImage = child.GetComponent<Image>();
            if (iconImage != null && iconImage.sprite == skill.icon)
            {
                // Seviye metnini güncelle ve rengini değiştir
                TextMeshProUGUI levelText = child.GetComponentInChildren<TextMeshProUGUI>();
                if (levelText != null)
                {
                    levelText.text = "LvL: " + skill.currentLevel;

                    // Renkleri seviye bazında değiştirme
                    Color targetColor = Color.white; // Varsayılan renk
                    switch (skill.currentLevel)
                    {
                        case 1:
                            targetColor = Color.green; // Level 1 - Yeşil
                            break;
                        case 2:
                            targetColor = Color.blue; // Level 2 - Mavi
                            break;
                        case 3:
                            targetColor = new Color(0.5f, 0f, 0.5f); // Mor (RGB: 128, 0, 128)
                            break;
                        case 4:
                            targetColor = new Color(1f, 0.65f, 0f); // Turuncu (RGB: 255, 165, 0)
                            break;
                    }

                    // DOTween ile metin rengini değiştirme
                    levelText.DOColor(targetColor, 0.5f); // 0.5 saniyelik geçiş süresi
                }
            }
        }
    }

}
