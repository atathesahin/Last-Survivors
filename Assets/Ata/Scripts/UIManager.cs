using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using DG.Tweening; // DOTween kütüphanesi

public class UIManager : MonoBehaviour
{
     public static UIManager Instance;

    public TextMeshProUGUI healthText; // Oyuncunun sağlık bilgisini gösteren UI
    public TextMeshProUGUI goldText; // Oyuncunun altın miktarını gösteren UI
    public TextMeshProUGUI waveText; // Şu anki dalga bilgisini gösteren UI
    public TextMeshProUGUI countdownText; // Hazırlık aşamasındaki geri sayımı gösteren UI
    public TextMeshProUGUI weaponCostText; // Silah maliyetini gösteren UI
    public Transform skillPanel; // Skillerin ikonlarını gösterecek panel
    public GameObject skillIconPrefab; // Skill ikonları için prefab

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

    public void AddSkillIcon(Sprite skillIcon)
    {
        if (skillPanel != null && skillIconPrefab != null)
        {
            // Yeni skill ikonu oluştur ve panelin altına ekle
            GameObject newIcon = Instantiate(skillIconPrefab, skillPanel);
            Image iconImage = newIcon.GetComponent<Image>();
            if (iconImage != null)
            {
                iconImage.sprite = skillIcon;
            }
        }
    }
}
