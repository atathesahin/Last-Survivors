using UnityEngine;
using System.Collections;

public class WeaponManager : MonoBehaviour
{
    private GameObject spawnedWeapon; // Spawnlanan silah
    private int spawnedWeaponCost; // Silahın maliyeti
    private Coroutine pickUpCoroutine; // Coroutine referansı
    public Transform playerWeaponHolder; // Silahın taşınacağı yer
    public Transform[] weaponSpawnPoints; // Silahların oluşturulacağı noktalar
    public GameObject[] weapons; // Mevcut silahlar
    public float weaponPickUpDuration = 3f; // Silah alma için gereken süre

    public void SpawnWeaponForPreparation()
    {
        // Spawn noktaları ve silahlar kontrol edilir
        if (weaponSpawnPoints.Length > 0 && weapons.Length > 0)
        {
            // Rastgele spawn noktası ve silah seçilir
            int randomSpawnIndex = Random.Range(0, weaponSpawnPoints.Length);
            int randomWeaponIndex = Random.Range(0, weapons.Length);

            // Silah spawn edilir
            spawnedWeapon = Instantiate(weapons[randomWeaponIndex], weaponSpawnPoints[randomSpawnIndex].position, Quaternion.identity);
            spawnedWeapon.AddComponent<BoxCollider>().isTrigger = true; // Trigger collider eklenir
            spawnedWeapon.tag = "WeaponBox"; // WeaponBox tag'i atanır

            spawnedWeaponCost = Random.Range(100, 1001); // Rastgele bir maliyet belirlenir
            Debug.Log("Spawnlanan silah maliyeti: " + spawnedWeaponCost);

            // UI'de maliyet bilgisi güncellenir
            UIManager.Instance.UpdateWeaponCostUI(spawnedWeaponCost);
        }
    }

    public void DestroySpawnedWeapon()
    {
        if (spawnedWeapon != null)
        {
            Destroy(spawnedWeapon);
            UIManager.Instance.UpdateWeaponCostUI(0); // UI'deki maliyet sıfırlanır
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && spawnedWeapon != null)
        {
            Debug.Log("Oyuncu kutuya yaklaştı. Silah alma işlemi başlıyor...");
            pickUpCoroutine = StartCoroutine(PickUpWeapon(other.gameObject)); // Coroutine başlatılıyor
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && pickUpCoroutine != null)
        {
            Debug.Log("Oyuncu kutudan uzaklaştı. Silah alma işlemi iptal ediliyor...");
            StopCoroutine(pickUpCoroutine); // Coroutine durduruluyor
            pickUpCoroutine = null;
        }
    }

    private IEnumerator PickUpWeapon(GameObject player)
    {
        Debug.Log("Silah alma işlemi başlatıldı!");

        float elapsedTime = 0f;

        while (elapsedTime < weaponPickUpDuration)
        {
            elapsedTime += Time.deltaTime;

            // Oyuncu kutunun üstünden uzaklaşırsa işlem iptal edilir
            if (Vector3.Distance(player.transform.position, spawnedWeapon.transform.position) > 1.5f)
            {
                Debug.Log("Oyuncu kutudan uzaklaştı. Silah alma işlemi iptal edildi.");
                yield break;
            }

            yield return null; // Bir frame bekle
        }

        // 3 saniye tamamlandığında silah alma işlemi
        Player playerScript = player.GetComponent<Player>();
        if (playerScript != null && playerScript.gold >= spawnedWeaponCost)
        {
            // Oyuncunun yeterli altını varsa
            playerScript.gold -= spawnedWeaponCost;
            playerScript.EquipWeapon(spawnedWeapon);
            UIManager.Instance.UpdateGoldUI(playerScript.gold); // Altın UI'si güncellenir

            // Silah karakterin taşıma noktasına taşınır
            spawnedWeapon.transform.SetParent(playerWeaponHolder);
            spawnedWeapon.transform.localPosition = Vector3.zero;
            spawnedWeapon.transform.localRotation = Quaternion.identity;

            UIManager.Instance.UpdateWeaponCostUI(0); // UI'de maliyet sıfırlanır
            spawnedWeapon = null;
            Debug.Log("Oyuncu yeni bir silah aldı!");
        }
        else
        {
            Debug.Log("Yeterli altın yok!");
        }
    }

    public int GetSpawnedWeaponCost()
    {
        return spawnedWeaponCost;
    }
}
