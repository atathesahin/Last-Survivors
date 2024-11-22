using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSkeletonSummonSkill", menuName = "Skills/Active/SkeletonSummonSkill")]
public class SkeletonSummonSkill : Skill
{
    public GameObject skeletonPrefab;
    public float attackRange = 5f; // Range within which the skeleton attacks enemies
    public int damage = 10; // Damage dealt by the skeleton
    public int skeletonCount = 1; // Number of skeletons summoned
    private List<GameObject> activeSkeletons = new List<GameObject>();

    public override void ActivateSkill(Player player)
    {
        for (int i = 0; i < skeletonCount; i++)
        {
            Vector3 spawnPosition = player.transform.position + new Vector3(Random.Range(-2f, 2f), 0, Random.Range(-2f, 2f));
            GameObject skeleton = Instantiate(skeletonPrefab, spawnPosition, Quaternion.identity);
            activeSkeletons.Add(skeleton);
            SkeletonController skeletonController = skeleton.GetComponent<SkeletonController>();
            skeletonController.Initialize(player, attackRange, damage);
        }

        Debug.Log($"{skillName} activated! {skeletonCount} skeleton(s) summoned.");
    }

    public override void UpgradeSkill()
    {
        if (currentLevel < maxLevel)
        {
            currentLevel++;

            if (currentLevel == 2)
            {
                skeletonCount = 2; // Increase number of skeletons
                Debug.Log($"{skillName} upgraded to level {currentLevel}. Number of skeletons increased to {skeletonCount}.");
            }
            else if (currentLevel == 3)
            {
                skeletonCount = 3; // Increase number of skeletons
                Debug.Log($"{skillName} upgraded to level {currentLevel}. Number of skeletons increased to {skeletonCount}.");
            }
            else if (currentLevel == 4)
            {
                attackRange = 8f; // Increase attack range
                Debug.Log($"{skillName} upgraded to level {currentLevel}. Attack range increased to {attackRange}.");
            }
        }
    }
}