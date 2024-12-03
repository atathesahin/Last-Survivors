using UnityEngine;
using System.Collections;

public class WeaponManager : MonoBehaviour
{
    private GameObject spawnedWeapon;
    private int spawnedWeaponCost; 
    private Coroutine pickUpCoroutine; 
    public Transform playerWeaponHolder; 
    public Transform[] weaponSpawnPoints; 
    public GameObject[] weapons; 
    public float weaponPickUpDuration = 3f; 

    public void SpawnWeaponForPreparation()
    {
        
        if (weaponSpawnPoints.Length > 0 && weapons.Length > 0)
        {
           
            int randomSpawnIndex = Random.Range(0, weaponSpawnPoints.Length);
            int randomWeaponIndex = Random.Range(0, weapons.Length);

           
            spawnedWeapon = Instantiate(weapons[randomWeaponIndex], weaponSpawnPoints[randomSpawnIndex].position, Quaternion.identity);
            spawnedWeapon.AddComponent<BoxCollider>().isTrigger = true; 
            spawnedWeapon.tag = "WeaponBox"; 
            spawnedWeaponCost = Random.Range(100, 1001);
            UIManager.Instance.UpdateWeaponCostUI(spawnedWeaponCost);
        }
    }

    public void DestroySpawnedWeapon()
    {
        if (spawnedWeapon != null)
        {
            Destroy(spawnedWeapon);
            UIManager.Instance.UpdateWeaponCostUI(0); 
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && spawnedWeapon != null)
        {
            pickUpCoroutine = StartCoroutine(PickUpWeapon(other.gameObject));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && pickUpCoroutine != null)
        {
            StopCoroutine(pickUpCoroutine); 
            pickUpCoroutine = null;
        }
    }

    private IEnumerator PickUpWeapon(GameObject player)
    {

        float elapsedTime = 0f;

        while (elapsedTime < weaponPickUpDuration)
        {
            elapsedTime += Time.deltaTime;

            // Oyuncu kutunun üstünden uzaklaşırsa işlem iptal edilir
            if (Vector3.Distance(player.transform.position, spawnedWeapon.transform.position) > 1.5f)
            {
                yield break;
            }

            yield return null; 
        }
        
        Player playerScript = player.GetComponent<Player>();
        if (playerScript != null && playerScript.gold >= spawnedWeaponCost)
        {

            playerScript.gold -= spawnedWeaponCost;
            playerScript.EquipWeapon(spawnedWeapon);
            UIManager.Instance.UpdateGoldUI(playerScript.gold); 


            spawnedWeapon.transform.SetParent(playerWeaponHolder);
            spawnedWeapon.transform.localPosition = Vector3.zero;
            spawnedWeapon.transform.localRotation = Quaternion.identity;

            UIManager.Instance.UpdateWeaponCostUI(0); 
            spawnedWeapon = null;

        }
    }

    public int GetSpawnedWeaponCost()
    {
        return spawnedWeaponCost;
    }
}
