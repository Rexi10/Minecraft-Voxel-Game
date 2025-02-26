using System;
using System.Collections;
using UnityEngine;

public class Character : MonoBehaviour
{


    [SerializeField]
    private Camera mainCamera;
    [SerializeField]
    private PlayerInput playerInput;
    [SerializeField]
    private PlayerMovement playerMovement;

    public float interactionRayLength = 5;

    public LayerMask groundMask;
    public bool fly = false;

    public Animator animator;

    bool isWaiting = false;

    public World world;

    private void Awake()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
        playerInput = GetComponent<PlayerInput>();
        playerMovement = GetComponent<PlayerMovement>();
        world = FindFirstObjectByType<World>();
    }

    private void Start()
    {
        playerInput.OnMouseClick += HandleMouseClick;
        playerInput.OnFly += HandleFlyClick
        ;
    }

    private void HandleFlyClick()
    {
        fly = !fly;
    }

    void Update()
    {
        if(fly)
        {
            animator.SetFloat("speed", 0);
            animator.SetBool("isGrounded", false);
            animator.ResetTrigger("jump");
            playerMovement.Fly(playerInput.MovementInput, playerInput.isJumping, playerInput.RunningPressed);
        }
        else
        {
            animator.SetBool("isGrounded", playerMovement.isGrounded);
            if(playerMovement.isGrounded && playerInput.isJumping && isWaiting == false)
            {
                animator.SetTrigger("jump");
                isWaiting = true;
                StopAllCoroutines();
                StartCoroutine(ResetWaiting());        
            }
            animator.SetFloat("speed", playerInput.MovementInput.magnitude);
            playerMovement.HandleGravity(playerInput.isJumping);
            playerMovement.Walk(playerInput.MovementInput, playerInput.RunningPressed);
        }
    }

    IEnumerator ResetWaiting()
    {
        yield return new WaitForSeconds(0.1f);
        isWaiting = false;
    }


    private void HandleMouseClick()
    {
        Ray playerRay = new Ray(mainCamera.transform.position, mainCamera.transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(playerRay, out hit, interactionRayLength, groundMask))
        {
            ModifyTerrain(hit);
        }
        
    }

    private void ModifyTerrain(RaycastHit hit)
    {
        world.SetBlock(hit, BlockType.Air);
    }
}
