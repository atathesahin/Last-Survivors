using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Animator animationController;
    [SerializeField] private Rigidbody playerRigidbody;
    [SerializeField] private VariableJoystick fixedJoystick;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private int moveSpeed = 5;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float inputThreshold = 0.1f;

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
        Vector3 newVelocity = movementInput.normalized * moveSpeed;
        newVelocity.y = playerRigidbody.velocity.y;
        playerRigidbody.velocity = newVelocity;
    }

    private void SetRotation()
    {
        if (movementInput.magnitude > inputThreshold)
        {
            Quaternion joystickRotation = Quaternion.LookRotation(movementInput);
            playerTransform.rotation = Quaternion.Slerp(playerTransform.rotation, joystickRotation, rotationSpeed * Time.fixedDeltaTime);
        }
    }

    private void UpdateAnimation()
    {
        bool isRunning = movementInput.magnitude > inputThreshold;
        animationController.SetBool("run", isRunning);
    }

    public void PlayAttackAnimation()
    {
        if (animationController != null)
        {
            animationController.SetTrigger("Attack");
        }
    }

    public void IncreaseSpeed(int amount)
    {
        moveSpeed += amount;
    }

    public bool IsMoving()
    {
        return movementInput.magnitude > inputThreshold;
    }
}