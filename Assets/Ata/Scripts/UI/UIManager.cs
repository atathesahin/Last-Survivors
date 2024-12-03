﻿using UnityEngine;
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

    public TextMeshProUGUI skillNotificationText; 

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
      
            GameObject newIcon = Instantiate(skillIconPrefab, skillPanel);
            Image iconImage = newIcon.GetComponent<Image>();
            if (iconImage != null)
            {
                iconImage.sprite = skill.icon;
            }

      
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
        
                TextMeshProUGUI levelText = child.GetComponentInChildren<TextMeshProUGUI>();
                if (levelText != null)
                {
                    levelText.text = "LvL: " + skill.currentLevel;

               
                    Color targetColor = Color.white; 
                    switch (skill.currentLevel)
                    {
                        case 1:
                            targetColor = Color.green; 
                            break;
                        case 2:
                            targetColor = Color.blue; 
                            break;
                        case 3:
                            targetColor = new Color(0.5f, 0f, 0.5f); 
                            break;
                        case 4:
                            targetColor = new Color(1f, 0.65f, 0f); 
                            break;
                    }

               
                    levelText.DOColor(targetColor, 0.5f); 
                }
            }
        }
    }
    public void ShowSkillNotification(string skillName)
    {
        if (skillNotificationText != null)
        {
            skillNotificationText.text = skillName;
            skillNotificationText.gameObject.SetActive(true);

        
            skillNotificationText.DOFade(0, 3.5f).SetDelay(2f).OnComplete(() =>
            {
                skillNotificationText.gameObject.SetActive(false);
                skillNotificationText.alpha = 1; 
            });
        }
    }

}