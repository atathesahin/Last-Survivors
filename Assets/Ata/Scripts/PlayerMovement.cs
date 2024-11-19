using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Animator animationController;
    [SerializeField] Rigidbody playerRigidbody;
    [SerializeField] private VariableJoystick fixedJoystick;
    [SerializeField] private Transform playerTransform;
    [SerializeField] public int moveSpeed = 5;
    
    private float horizontal;
    private float vertical;
    
    
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        GetMovementInput();
    }
    void FixedUpdate()
    {
        SetMovement();
        SetRotation();
    }

    private void SetMovement()  
    {
        
        playerRigidbody.velocity = GetNewVelocity();
        animationController.SetBool("run",horizontal != 0 || vertical != 0);

    }
    private void SetRotation()
    {
        if (horizontal != 0 && vertical != 0)
        {
            playerTransform.rotation = Quaternion.LookRotation(GetNewVelocity());
    
        }
      

    }

    private Vector3 GetNewVelocity()
    {
        return new Vector3(horizontal, playerRigidbody.velocity.y, vertical) * (moveSpeed * Time.fixedDeltaTime);
    }

    private void GetMovementInput()
    {
        horizontal = fixedJoystick.Horizontal;
        vertical = fixedJoystick.Vertical;
    }
}
