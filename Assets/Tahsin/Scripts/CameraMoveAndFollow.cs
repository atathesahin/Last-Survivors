using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class CameraMoveAndFollow : MonoBehaviour
{
    public Camera mainCamera;
    public GameObject character;
    public Transform portal;
    public float moveSpeed = 3f;
    public float cameraSpeed = 2f;
    public string gameSceneName = "GameScene";
    private Animator characterAnimator;
    private bool isMoving = false;

    public float followDistance = 3f;
    public float followHeight = 2f;
    public float fixedCameraRotationX = 30f; // Sabitlenmi� kamera X rotasyonu

    // Video kontrol� i�in de�i�kenler
    public GameObject videoCanvas; // Video g�steren Canvas
    public float videoDuration = 5f; // Videonun s�resi
    private bool videoPlayed = false; // Videonun oynay�p oynamad���n� kontrol eder
    private bool portalTriggered = false; // Portala de�ildi mi?

    void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
        characterAnimator = character.GetComponent<Animator>();

        // VideoCanvas ba�lang��ta gizli
        if (videoCanvas != null)
        {
            videoCanvas.SetActive(false);
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isMoving)
        {
            StartCoroutine(MoveCharacterAndCamera());
        }

        if (isMoving)
        {
            // Kamera pozisyonunu ayarla
            Vector3 targetCameraPosition = character.transform.position - character.transform.forward * followDistance + Vector3.up * followHeight;
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetCameraPosition, cameraSpeed * Time.deltaTime);

            // Kamera rotasyonunu manuel olarak ayarla (X sabit, di�er eksenler dinamik)
            Quaternion targetRotation = Quaternion.LookRotation(character.transform.position - mainCamera.transform.position);
            Vector3 fixedRotation = targetRotation.eulerAngles;
            fixedRotation.x = fixedCameraRotationX; // X rotasyonunu sabit tut
            mainCamera.transform.rotation = Quaternion.Euler(fixedRotation);
        }

        if (isMoving)
        {
            characterAnimator.SetBool("isRunning", true);
        }
        else
        {
            characterAnimator.SetBool("isRunning", false);
        }
    }

    private IEnumerator MoveCharacterAndCamera()
    {
        isMoving = true;

        while (Vector3.Distance(character.transform.position, portal.position) > 1f)
        {
            character.transform.position = Vector3.MoveTowards(character.transform.position, portal.position, moveSpeed * Time.deltaTime);
            yield return null;
        }

        // Portala ula��ld���nda i�lemleri tetikle
        if (!videoPlayed && !portalTriggered)
        {
            portalTriggered = true; // Portala sadece bir kez de�mesini sa�lar
            StartCoroutine(PortalEffectAndTransition());
        }
    }

    private IEnumerator PortalEffectAndTransition()
    {
        videoPlayed = true;

        if (videoCanvas != null)
        {
            videoCanvas.SetActive(true); // VideoCanvas g�r�n�r yap
        }

        // 5 saniye boyunca bekle (video oynatma s�resi)
        yield return new WaitForSeconds(videoDuration);

        if (videoCanvas != null)
        {
            videoCanvas.SetActive(false); // VideoCanvas'� tekrar gizle
        }

        // 5 saniyeden sonra sahne ge�i�i yap
        SceneManager.LoadScene(gameSceneName);
    }
}
