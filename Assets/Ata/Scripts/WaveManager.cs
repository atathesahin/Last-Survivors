using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaveManager : MonoBehaviour
{
     public static WaveManager Instance;

    public int currentWave = 0;
    public int enemiesPerWave = 5;
    public float preparationTime = 30f;
    public GameObject enemyPrefab;
    public GameObject bossPrefab;
    public Transform spawnPoint;

    private List<GameObject> activeEnemies = new List<GameObject>();
    private bool isPreparationPhase = false;

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

    void Start()
    {
        StartCoroutine(StartWave());
    }

    IEnumerator StartWave()
    {
        while (true)
        {
            yield return SpawnWave();

            // Tüm düşmanlar yok edilene kadar bekle
            yield return new WaitUntil(() => activeEnemies.Count == 0);

            // Tüm düşmanlar yok edildikten sonra hazırlık süresine gir
            yield return PreparationPhase();

            // Hazırlık süresinde rastgele bir yetenek kazandır
            Debug.Log("Hazırlık aşaması bitti, rastgele yetenek kazandırılıyor...");
            Player.Instance.GainRandomSkill();
            Debug.Log("Yetenek kazanımı işlemi çağrıldı.");
        }
    }

    IEnumerator SpawnWave()
    {
        isPreparationPhase = false;
        int enemyCount = currentWave * enemiesPerWave;

        for (int i = 0; i < enemyCount; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
            activeEnemies.Add(enemy); // Aktif düşman listesine ekle
            yield return new WaitForSeconds(0.5f);
        }

        if (currentWave % 10 == 0)  // Her 10 wave'de bir boss
        {
            GameObject boss = Instantiate(bossPrefab, spawnPoint.position, Quaternion.identity);
            activeEnemies.Add(boss); // Boss'u aktif düşman listesine ekle
        }

        currentWave++;
        UIManager.Instance.UpdateWaveUI(currentWave);
    }

    IEnumerator PreparationPhase()
    {
        isPreparationPhase = true;
        float countdown = preparationTime;

        while (countdown > 0)
        {
            countdown -= Time.deltaTime;
            UIManager.Instance.UpdateCountdownUI(countdown);  // Geri sayımı günceller
            yield return null;
        }

        isPreparationPhase = false;
        UIManager.Instance.UpdateCountdownUI(0);  // Geri sayımı sıfırlar
    }

    public bool IsInPreparationPhase()
    {
        return isPreparationPhase;
    }

    // Düşman öldüğünde çağrılacak metod
    public void EnemyDefeated(GameObject enemy)
    {
        if (activeEnemies.Contains(enemy))
        {
            activeEnemies.Remove(enemy);
        }
    }
}
