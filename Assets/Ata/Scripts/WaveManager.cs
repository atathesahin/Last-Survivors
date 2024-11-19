using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance;

    public int currentWave = 1;
    public int enemiesPerWave = 5;
    public float preparationTime = 30f;
    public GameObject enemyPrefab;
    public GameObject bossPrefab;
    public Transform[] spawnPoints; // Birden fazla spawn noktası için array
    public GameObject[] weapons; 
    public Transform[] weaponSpawnPoints;

    private List<GameObject> activeEnemies = new List<GameObject>();
    private bool isPreparationPhase = false;
    private GameObject spawnedWeapon; 
    private int spawnedWeaponCost; 

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
        UIManager.Instance.UpdateWaveUI(currentWave); 
    }

    IEnumerator StartWave()
    {
        while (true)
        {
            yield return SpawnWave();

            yield return new WaitUntil(() => activeEnemies.Count == 0);

            yield return PreparationPhase();

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
            // Rastgele bir spawn noktası seç
            Transform randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

            // Düşmanı spawn et
            GameObject enemy = Instantiate(enemyPrefab, randomSpawnPoint.position, Quaternion.identity);

            activeEnemies.Add(enemy);
            yield return new WaitForSeconds(0.5f);
        }

        if (currentWave % 10 == 0)  
        {
            Transform randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            GameObject boss = Instantiate(bossPrefab, randomSpawnPoint.position, Quaternion.identity);
            activeEnemies.Add(boss); 
        }

        currentWave++;
        UIManager.Instance.UpdateWaveUI(currentWave); 
    }

    IEnumerator PreparationPhase()
    {
        isPreparationPhase = true;
        float countdown = preparationTime;

        // Bir silah spawn et
        SpawnWeaponForPreparation();

        while (countdown > 0)
        {
            countdown -= Time.deltaTime;
            UIManager.Instance.UpdateCountdownUI(countdown); 
            yield return null;
        }

        if (spawnedWeapon != null)
        {
            Destroy(spawnedWeapon);
            UIManager.Instance.UpdateWeaponCostUI(0); 
        }

        isPreparationPhase = false;
        UIManager.Instance.UpdateCountdownUI(0);  
    }

    public bool IsInPreparationPhase()
    {
        return isPreparationPhase;
    }

    public void EnemyDefeated(GameObject enemy)
    {
        if (activeEnemies.Contains(enemy))
        {
            activeEnemies.Remove(enemy);
        }
    }

    private void SpawnWeaponForPreparation()
    {
        if (weaponSpawnPoints.Length > 0 && weapons.Length > 0)
        {
            int randomSpawnIndex = Random.Range(0, weaponSpawnPoints.Length);
            int randomWeaponIndex = Random.Range(0, weapons.Length);

            spawnedWeapon = Instantiate(weapons[randomWeaponIndex], weaponSpawnPoints[randomSpawnIndex].position, Quaternion.identity);
            spawnedWeaponCost = Random.Range(100, 1001); 
            Debug.Log("Spawnlanan silah maliyeti: " + spawnedWeaponCost);

            UIManager.Instance.UpdateWeaponCostUI(spawnedWeaponCost);
        }
    }

    public void WeaponPickedUp(GameObject weapon)
    {
        List<GameObject> updatedWeapons = new List<GameObject>(weapons);
        updatedWeapons.Remove(weapon);
        weapons = updatedWeapons.ToArray();
    }

    public int GetSpawnedWeaponCost()
    {
        return spawnedWeaponCost;
    }
}
