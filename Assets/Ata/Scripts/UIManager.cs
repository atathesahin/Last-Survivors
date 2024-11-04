using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using DG.Tweening; // DOTween kütüphanesi

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public TextMeshProUGUI goldText;              // Altın miktarını gösterecek Text bileşeni
    public TextMeshProUGUI waveText;              // Dalga sayısını gösterecek Text bileşeni
    public TextMeshProUGUI countdownText;         // Geri sayımı gösterecek Text bileşeni
    public TextMeshProUGUI healthText;            // Sağlık durumunu gösterecek Text bileşeni

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

    public void UpdateGoldUI(int gold)
    {
        if (goldText != null)
        {
            goldText.text = "Gold: " + gold;

            // Altın miktarı güncellenirken basit bir animasyon
            goldText.transform.DOScale(1.2f, 0.2f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.OutBack);
        }
    }

    public void UpdateWaveUI(int wave)
    {
        if (waveText != null)
        {
            waveText.text = "Wave: " + wave;

            // Wave numarası güncellenirken basit bir animasyon
            waveText.transform.DOScale(1.2f, 0.2f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.OutBack);
        }
    }

    public void UpdateCountdownUI(float seconds)
    {
        if (countdownText != null)
        {
            countdownText.text = "Next Wave In: " + Mathf.CeilToInt(seconds).ToString() + "s";
        }
    }

    public void UpdateHealthUI(int currentHealth, int maxHealth)
    {
        if (healthText != null)
        {
            healthText.text = $"Health: {currentHealth}/{maxHealth}";

            // Sağlık güncellenirken basit bir animasyon
            healthText.transform.DOShakeScale(0.3f, 0.2f).SetEase(Ease.InOutBounce);
        }
    }
}
