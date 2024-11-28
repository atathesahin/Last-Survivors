using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance;

    [Header("Wave Settings")]
    public int currentWave = 1;
    public int enemiesPerWave = 5;
    public float preparationTime = 30f;
    [Header("Enemies")]
    public GameObject enemyPrefab;
    [Header("Spawn Settings")]
    public Transform[] spawnPoints; 
    public GameObject centralObject; 

    private List<GameObject> activeEnemies = new List<GameObject>();
    private bool isPreparationPhase = false;
    [SerializeField] private WeaponManager weaponManager;
    
    public ObjectPool damagePopupPool; 

    private float centralObjectRadius = 3f; 
    private bool isPlayerInCentralObjectArea = false; 
    
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
        if (weaponManager == null)
        {
            Debug.LogError("WeaponManager referansı atanmadı!");
        }
        else
        {
            Debug.Log("WeaponManager başarıyla bulundu.");
        }

        StartCoroutine(StartWave());
        UIManager.Instance.UpdateWaveUI(currentWave); 
        StartCoroutine(CheckPlayerPosition());
    }

    IEnumerator StartWave()
    {
        while (true)
        {
            yield return SpawnWave();

            yield return new WaitUntil(() => activeEnemies.Count == 0);

            yield return PreparationPhase();

            Debug.Log("Hazırlık aşaması bitti, rastgele yetenek kazandırılıyor...");
            
            if (isPlayerInCentralObjectArea)
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
            Transform randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            GameObject enemy = Instantiate(enemyPrefab, randomSpawnPoint.position, Quaternion.identity);

            Enemy enemyScript = enemy.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                enemyScript.SetDamagePopupPool(damagePopupPool);
               
                enemyScript.SetHealth(20 + (currentWave * 5)); 
                enemyScript.SetSpeed(0.05f * currentWave);
            }

            activeEnemies.Add(enemy);
            yield return new WaitForSeconds(0.5f);
        }

        currentWave++;
        UIManager.Instance.UpdateWaveUI(currentWave); 
    }

    IEnumerator PreparationPhase()
    {
        isPreparationPhase = true;
        float countdown = preparationTime;

        ChangeCentralObjectColor(Color.yellow);
        weaponManager.SpawnWeaponForPreparation();

        while (countdown > 0)
        {
            countdown -= Time.deltaTime;
            UIManager.Instance.UpdateCountdownUI(countdown);
            yield return null;
        }

        ChangeCentralObjectColor(Color.red);

        weaponManager.DestroySpawnedWeapon();

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

    IEnumerator CheckPlayerPosition()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f); 
            float distance = Vector3.Distance(Player.Instance.transform.position, centralObject.transform.position);
            if (distance <= centralObjectRadius)
            {
                if (!isPlayerInCentralObjectArea)
                {
                    Debug.Log("Oyuncu central object alanına girdi.");
                    isPlayerInCentralObjectArea = true;
                }
            }
            else
            {
                if (isPlayerInCentralObjectArea)
                {
                    Debug.Log("Oyuncu central object alanından çıktı.");
                    isPlayerInCentralObjectArea = false;
                }
            }
        }
    }
}