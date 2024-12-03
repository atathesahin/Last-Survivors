using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Animator animationController;
    [SerializeField] private Rigidbody playerRigidbody;
    [SerializeField] private VariableJoystick fixedJoystick;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float inputThreshold = 0.1f;

    public GameObject currentWeapon;
    private float lastAttackTime;

    private Vector3 movementInput;

    void Update()
    {
        GetMovementInput();
        UpdateAnimation();
        AttackClosestEnemy();  // Her karede saldırı kontrolü yap
    }

    void FixedUpdate()
    {
        SetMovement();
        RotateTowardsMovementOrEnemy();
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

    private void UpdateAnimation()
    {
        bool isRunning = movementInput.magnitude > inputThreshold;
        animationController.SetBool("run", isRunning);
    }

    private void RotateTowardsMovementOrEnemy()
    {
        Enemy closestEnemy = GetClosestEnemy();

        if (closestEnemy != null)
        {
            Vector3 direction = (closestEnemy.transform.position - transform.position).normalized;

            if (direction.magnitude > 0.1f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                playerTransform.rotation = Quaternion.Slerp(playerTransform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
            }
        }
        else if (movementInput.magnitude > inputThreshold)
        {
            Quaternion moveRotation = Quaternion.LookRotation(movementInput);
            playerTransform.rotation = Quaternion.Slerp(playerTransform.rotation, moveRotation, rotationSpeed * Time.fixedDeltaTime);
        }
    }

    private Enemy GetClosestEnemy()
    {
        float closestDistance = Mathf.Infinity;
        Enemy closestEnemy = null;
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 15f);

        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy"))
            {
                float distance = Vector3.Distance(transform.position, hitCollider.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = hitCollider.GetComponent<Enemy>();
                }
            }
        }

        return closestEnemy;
    }

    private void AttackClosestEnemy()
    {
        if (currentWeapon != null)
        {

            ProjectileShooter shooter = currentWeapon.GetComponent<ProjectileShooter>();
            if (shooter != null && Time.time >= lastAttackTime + shooter.attackCooldown)
            {
                Enemy closestEnemy = GetClosestEnemy();
                if (closestEnemy != null)
                {
                    PlayAttackAnimation();
                    shooter.ShootProjectile(closestEnemy.transform);
                    lastAttackTime = Time.time;
                }
            }

       
            MeleeWeapon meleeWeapon = currentWeapon.GetComponent<MeleeWeapon>();
            if (meleeWeapon != null && Time.time >= lastAttackTime + meleeWeapon.attackCooldown)
            {
                Enemy closestEnemy = GetClosestEnemy();
                if (closestEnemy != null)
                {
                    PlayAttackAnimation();
                    meleeWeapon.AutoAttackClosestEnemy();
                    lastAttackTime = Time.time;
                }
            }
        }
    }

    public void PlayAttackAnimation()
    {
        if (animationController != null)
        {
            animationController.SetTrigger("Attack");
        }
    }

    public bool IsMoving()
    {
        return movementInput.magnitude > inputThreshold;
    }

    public void IncreaseSpeed(float amount)
    {
        moveSpeed += amount;
    }
}
