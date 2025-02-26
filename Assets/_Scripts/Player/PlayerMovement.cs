using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private CharacterController controller;

    [SerializeField]
    private float playerSpeed = 5.0f, playerRunSpeed = 8.0f;

    [SerializeField]
    private float jumpHeight = 1.0f;

    [SerializeField]
    private float gravityValue = -9.81f;

    [SerializeField]
    private float flySpeed = 2;

    private Vector3 playerVelocity;

    [Header("Ground Check:")]
    [SerializeField]
    private LayerMask groundMask;

    [SerializeField]
    private float rayDistance = 1;

    [field: SerializeField]
    public bool isGrounded { get; private set; }



    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    } 

    private Vector3 GetMovementDirection(Vector3 movementInput)
    {
        return transform.right * movementInput.x + transform.forward * movementInput.z;
    }

    public void Fly(Vector3 movementInput, bool ascendInput, bool descendInput)
    {
        Vector3 movementDirection = GetMovementDirection(movementInput);

        if(ascendInput)
        {
            movementDirection += Vector3.up * flySpeed;
        }
        else if(descendInput)
        {
            movementDirection -= Vector3.up * flySpeed;
        }
        controller.Move(movementDirection * playerSpeed * Time.deltaTime);

    }

    public void Walk(Vector3 movementInput, bool runningInput)
    {
        Vector3 movementDirection = GetMovementDirection(movementInput);
        float speed = runningInput ? playerRunSpeed : playerSpeed;
        controller.Move(movementDirection * speed * Time.deltaTime);
    }

    public void HandleGravity(bool isJumping)
    {
        if (controller.isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }
        if (isJumping && isGrounded)
        {
            AddJumpForce();
        }
        ApplyGracityForce();
        controller.Move(playerVelocity * Time.deltaTime);
    }

    private void ApplyGracityForce()
    {
        playerVelocity.y += gravityValue * Time.deltaTime;
        playerVelocity.y = Mathf.Clamp(playerVelocity.y, gravityValue, 10);
    }

    private void AddJumpForce()
    {
        playerVelocity.y = jumpHeight;
    }

    private void Update()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, rayDistance, groundMask);
    }

    private void OawGizmos()
    {
        Gizmos.DrawRay(transform.position, Vector3.down * rayDistance);       
    }
}
