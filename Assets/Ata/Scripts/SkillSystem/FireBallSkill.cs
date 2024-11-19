using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "NewFireballSkill", menuName = "Skills/Active/FireballSkill")]
public class FireballSkill : Skill
{
    public GameObject fireballPrefab; 
    public float fireballSpeed = 10f; 
    public int damage = 20; 
    public float spawnInterval = 2f; 
    public float fireballSize = 1f; 
    public float spawnOffset = 1.5f; 
    private Coroutine spawnCoroutine;

    public override void ActivateSkill(Player player)
    {
        if (spawnCoroutine == null)
        {
            spawnCoroutine = player.StartCoroutine(SpawnFireballs(player));
            Debug.Log($"{skillName} activated! Fireballs are being spawned.");
        }
    }

    private IEnumerator SpawnFireballs(Player player)
    {
        while (true)
        {
            if (currentLevel >= 3)
            {
                SpawnMultiFireball(player); 
            }
            else
            {
                SpawnFireball(player); 
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnFireball(Player player)
    {
        
        Vector3 randomDirection = Quaternion.Euler(0, Random.Range(0, 360), 0) * Vector3.forward;

        
        Vector3 spawnPosition = player.transform.position + randomDirection * spawnOffset;
        spawnPosition.y = 1.25f; 

        
        GameObject fireball = Instantiate(fireballPrefab, spawnPosition, Quaternion.identity);
        fireball.transform.localScale = Vector3.one * fireballSize; 
        Fireball fireballComponent = fireball.GetComponent<Fireball>();
        fireballComponent.speed = fireballSpeed;
        fireballComponent.damage = damage;

       
        fireball.transform.forward = randomDirection;
    }

    private void SpawnMultiFireball(Player player)
    {
        
        for (int i = 0; i < 3; i++)
        {
            Vector3 randomDirection = Quaternion.Euler(0, Random.Range(0, 360), 0) * Vector3.forward;

            
            Vector3 spawnPosition = player.transform.position + randomDirection * spawnOffset;
            spawnPosition.y = 1.25f; 

            
            GameObject fireball = Instantiate(fireballPrefab, spawnPosition, Quaternion.identity);
            fireball.transform.localScale = Vector3.one * fireballSize;
            Fireball fireballComponent = fireball.GetComponent<Fireball>();
            fireballComponent.speed = fireballSpeed;
            fireballComponent.damage = damage;
            fireball.transform.forward = randomDirection;
        }
    }

    public override void UpgradeSkill()
    {
        if (currentLevel < maxLevel)
        {
            currentLevel++;

            if (currentLevel == 2)
            {
                fireballSize = 1.5f; 
                Debug.Log($"{skillName} upgraded to level {currentLevel}. Fireball size increased.");
            }
            else if (currentLevel == 3)
            {
                Debug.Log($"{skillName} upgraded to level {currentLevel}. Multi-shot enabled.");
            }
            else if (currentLevel == 4)
            {
                spawnInterval = 1f; 
                Debug.Log($"{skillName} upgraded to level {currentLevel}. Spawn interval decreased.");
            }
        }
    }
}
