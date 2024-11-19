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

    public void AddSkillIcon(Sprite skillIcon)
    {
        if (skillPanel != null && skillIconPrefab != null)
        {
       
            GameObject newIcon = Instantiate(skillIconPrefab, skillPanel);
            Image iconImage = newIcon.GetComponent<Image>();
            if (iconImage != null)
            {
                iconImage.sprite = skillIcon;
            }
        }
    }
}
