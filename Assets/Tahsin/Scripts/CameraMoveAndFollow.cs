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
    public float fixedCameraRotationX = 30f; // Sabitlenmiþ kamera X rotasyonu

    // Video kontrolü için deðiþkenler
    public GameObject videoCanvas; // Video gösteren Canvas
    public float videoDuration = 5f; // Videonun süresi
    private bool videoPlayed = false; // Videonun oynayýp oynamadýðýný kontrol eder
    private bool portalTriggered = false; // Portala deðildi mi?

    void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
        characterAnimator = character.GetComponent<Animator>();

        // VideoCanvas baþlangýçta gizli
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

            // Kamera rotasyonunu manuel olarak ayarla (X sabit, diðer eksenler dinamik)
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

        // Portala ulaþýldýðýnda iþlemleri tetikle
        if (!videoPlayed && !portalTriggered)
        {
            portalTriggered = true; // Portala sadece bir kez deðmesini saðlar
            StartCoroutine(PortalEffectAndTransition());
        }
    }

    private IEnumerator PortalEffectAndTransition()
    {
        videoPlayed = true;

        if (videoCanvas != null)
        {
            videoCanvas.SetActive(true); // VideoCanvas görünür yap
        }

        // 5 saniye boyunca bekle (video oynatma süresi)
        yield return new WaitForSeconds(videoDuration);

        if (videoCanvas != null)
        {
            videoCanvas.SetActive(false); // VideoCanvas'ý tekrar gizle
        }

        // 5 saniyeden sonra sahne geçiþi yap
        SceneManager.LoadScene(gameSceneName);
    }
}
