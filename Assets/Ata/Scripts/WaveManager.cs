using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance;

    public int currentWave = 1;
    public int enemiesPerWave = 5;
    public float preparationTime = 30f;
    public GameObject enemyPrefab;
    public GameObject bossPrefab;
    public Transform[] spawnPoints; 
    public GameObject[] weapons; 
    public Transform[] weaponSpawnPoints;
    public GameObject centralObject; 

    private List<GameObject> activeEnemies = new List<GameObject>();
    private bool isPreparationPhase = false;
    private GameObject spawnedWeapon; 
    private int spawnedWeaponCost; 
    private bool isPlayerInCentralObject = false; 
    
  
    public ObjectPool damagePopupPool; 


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
            
            if (isPlayerInCentralObject)
            {
                Debug.Log("Oyuncu central object üzerinde, yetenek kazandırılıyor...");
                Player.Instance.GainRandomSkill();
            }
            else
            {
                Debug.Log("Oyuncu central object üzerinde değil, yetenek kazandırılmadı.");
            }
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

            // DamagePopupPool'u düşmana ilet
            Enemy enemyScript = enemy.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                enemyScript.SetDamagePopupPool(damagePopupPool);
            }

            activeEnemies.Add(enemy);
            yield return new WaitForSeconds(0.5f);
        }

        if (currentWave % 10 == 0)  
        {
            Transform randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            GameObject boss = Instantiate(bossPrefab, randomSpawnPoint.position, Quaternion.identity);

            
            Enemy bossScript = boss.GetComponent<Enemy>();
            if (bossScript != null)
            {
                bossScript.SetDamagePopupPool(damagePopupPool);
            }

            activeEnemies.Add(boss); 
        }

        currentWave++;
        UIManager.Instance.UpdateWaveUI(currentWave); 
    }

    IEnumerator PreparationPhase()
    {
        isPreparationPhase = true;
        float countdown = preparationTime;

        
        ChangeCentralObjectColor(Color.yellow);

        
        SpawnWeaponForPreparation();

        while (countdown > 0)
        {
            countdown -= Time.deltaTime;
            UIManager.Instance.UpdateCountdownUI(countdown);
            yield return null;
        }

        
        ChangeCentralObjectColor(Color.red);

        if (spawnedWeapon != null)
        {
            Destroy(spawnedWeapon);
            UIManager.Instance.UpdateWeaponCostUI(0);
        }

        // Mesafe kontrolü
        float distance = Vector3.Distance(Player.Instance.transform.position, centralObject.transform.position);
        float activationRadius = 2f; // Oyuncunun "üzerinde" kabul edileceği mesafe

        if (distance <= activationRadius)
        {
           
            Player.Instance.GainRandomSkill();
        }
        else
        {
           
        }

        isPreparationPhase = false;
        UIManager.Instance.UpdateCountdownUI(0);
    }

    private void ChangeCentralObjectColor(Color newColor)
    {
        if (centralObject != null)
        {
            Renderer renderer = centralObject.GetComponent<Renderer>();
            if (renderer != null)
            {
              
                renderer.material.DOColor(newColor, 4f); 
            }
        }
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

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInCentralObject = true;
         
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInCentralObject = false;
        
        }
    }
}
