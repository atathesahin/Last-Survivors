using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Animator animationController;
    [SerializeField] private Rigidbody playerRigidbody;
    [SerializeField] private VariableJoystick fixedJoystick;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private int moveSpeed = 5;
    [SerializeField] private float rotationSpeed = 10f; // Dönüş hızı
    [SerializeField] private float inputThreshold = 0.1f; // Minimum joystick değeri için eşik

    private Vector3 movementInput;

    void Update()
    {
        GetMovementInput();
        UpdateAnimation();
    }

    void FixedUpdate()
    {
        SetMovement();
        SetRotation();
    }

    private void GetMovementInput()
    {
        // Joystick girişlerini al ve minimum eşik altında kalanları filtrele
        float horizontal = fixedJoystick.Horizontal;
        float vertical = fixedJoystick.Vertical;

        movementInput = new Vector3(horizontal, 0f, vertical);

        if (movementInput.magnitude < inputThreshold)
        {
            movementInput = Vector3.zero;
        }
    }

    private void SetMovement()
    {
        // Hareket hızını ve joystick yönünü uygula
        Vector3 newVelocity = movementInput.normalized * moveSpeed;
        newVelocity.y = playerRigidbody.velocity.y; // Y eksenindeki mevcut hızı koru
        playerRigidbody.velocity = newVelocity;
    }

    private void SetRotation()
    {
        if (movementInput.magnitude > inputThreshold)
        {
            // Hareket yönüne doğru döndürme işlemi
            Quaternion targetRotation = Quaternion.LookRotation(movementInput);
            playerTransform.rotation = Quaternion.Slerp(playerTransform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }
    }

    private void UpdateAnimation()
    {
        // Animasyon parametresini joystick girişine göre ayarla
        bool isRunning = movementInput.magnitude > inputThreshold;
        animationController.SetBool("run", isRunning);
    }
}
