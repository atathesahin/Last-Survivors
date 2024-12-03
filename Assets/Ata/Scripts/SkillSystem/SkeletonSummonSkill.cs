using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSkeletonSummonSkill", menuName = "Skills/Active/SkeletonSummonSkill")]
public class SkeletonSummonSkill : Skill
{
    public GameObject skeletonPrefab;
    public float attackRange = 5f; 
    public int damage = 10; 
    public int skeletonCount = 1; 
    private List<GameObject> activeSkeletons = new List<GameObject>();
    
    public override void ActivateSkill(Player player)
    {
        if (player == null)
        {
            Debug.LogWarning("Player reference is null. Cannot activate skill.");
            return;
        }

       
        foreach (var skeleton in activeSkeletons)
        {
            Destroy(skeleton);
        }
        activeSkeletons.Clear();

        int maxAttempts = 10; 
        for (int i = 0; i < skeletonCount; i++)
        {
            Vector3 spawnOffset;
            int attempts = 0;
            do
            {
                spawnOffset = new Vector3(Random.Range(-2f, 2f), 0, Random.Range(-2f, 2f));
                attempts++;
            } while (Physics.CheckSphere(player.transform.position + spawnOffset, 0.5f) && attempts < maxAttempts);

            Vector3 spawnPosition = player.transform.position + spawnOffset;
            GameObject skeleton = Instantiate(skeletonPrefab, spawnPosition, Quaternion.identity);
            activeSkeletons.Add(skeleton);
            SkeletonController skeletonController = skeleton.GetComponent<SkeletonController>();
            if (skeletonController != null)
            {
                skeletonController.Initialize(player, attackRange, damage);
            }
        }

        Debug.LogWarning($"{skillName} activated! {skeletonCount} skeleton(s) summoned with attack range {attackRange}.");
    }

    public override void UpgradeSkill()
    {
        if (currentLevel < maxLevel)
        {
            currentLevel++;
            
            
            skeletonCount = currentLevel;
            if (currentLevel == 4)
            {
                
            }

            Debug.LogWarning($"{skillName} upgraded to level {currentLevel}. Number of skeletons is now {skeletonCount}, attack range is {attackRange}.");
            
            // Yeni seviyede tekrar iskelet summon et
            Player player = FindObjectOfType<Player>();
            if (player != null)
            {
                ActivateSkill(player);
            }
        }
    }
} 