using System.Collections;
using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(fileName = "NewAxeSkill", menuName = "Skills/Active/AxeSkill")]
public class AxeSkill : Skill
{
    public GameObject axePrefab;
    public float spawnInterval = 3f; // Baltaların spawnlanma aralığı
    public int damage = 25; // Başlangıç hasarı
    public float range = 5f; 
    public float minDistanceFromPlayer = 2f; // Karaktere minimum mesafe
    private Coroutine spawnCoroutine;

    public override void ActivateSkill(Player player)
    {
        if (spawnCoroutine == null)
        {
            spawnCoroutine = player.StartCoroutine(SpawnAxes(player));
            Debug.Log($"{skillName} activated! Axes are falling.");
        }
    }

    private IEnumerator SpawnAxes(Player player)
    {
        while (true)
        {
            SpawnMultipleAxes(player, currentLevel);
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnMultipleAxes(Player player, int axeCount)
    {
        for (int i = 0; i < axeCount; i++)
        {
            Vector3 spawnPosition = GetRandomPositionAroundPlayer(player.transform.position, range, minDistanceFromPlayer);
            spawnPosition.y = 10f; // Baltanın yukarıdan doğması

            // Balta oluştur
            GameObject axe = Instantiate(axePrefab, spawnPosition, Quaternion.Euler(Random.Range(45,90), 0, 0)); // Rotasyonu ayarla

            // DOTween animasyonu: Balta aşağı süzülsün
            Vector3 targetPosition = spawnPosition;
            targetPosition.y = 0.5f;
            axe.transform.DOMove(targetPosition, 1f).SetEase(Ease.InQuad).OnComplete(() =>
            {
                Destroy(axe, 1.5f); // Baltayı 2 saniye sonra yok et
            });

            // Axe script'ine hasar bilgisini aktar
            Axe axeComponent = axe.GetComponent<Axe>();
            if (axeComponent != null)
            {
                axeComponent.Initialize(damage);
            }
        }

        Debug.Log($"{axeCount} axes spawned!");
    }

    private Vector3 GetRandomPositionAroundPlayer(Vector3 playerPosition, float range, float minDistance)
    {
        Vector3 spawnPosition;

        do
        {
            Vector3 randomDirection = Random.insideUnitSphere * range;
            randomDirection.y = 0; // Rastgele pozisyonda y eksenini sabitle
            spawnPosition = playerPosition + randomDirection;
        }
        while (Vector3.Distance(playerPosition, spawnPosition) < minDistance); // Minimum mesafeyi kontrol et

        return spawnPosition;
    }

    public override void UpgradeSkill()
    {
        if (currentLevel < maxLevel)
        {
            currentLevel++;
            damage += 15; // Her seviyede hasar +15 artar
            range += 5;
            if (currentLevel == 2)
            {
                Debug.Log($"{skillName} upgraded to level {currentLevel}. Now spawns 2 axes. Damage: {damage}");
            }
            else if (currentLevel == 3)
            {
                Debug.Log($"{skillName} upgraded to level {currentLevel}. Now spawns 3 axes. Damage: {damage}");
            }
            else if (currentLevel == 4)
            {
                spawnInterval = 5; // Spawnlanma süresi azalır
                Debug.Log($"{skillName} upgraded to level {currentLevel}. Spawn interval decreased. Damage: {damage}");
            }
        }
    }
}
