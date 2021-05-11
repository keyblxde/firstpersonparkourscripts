using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    
    [Header("Keybinds")]
    [SerializeField] KeyCode jumpKey = KeyCode.Space;
    [SerializeField] KeyCode sprintKey = KeyCode.LeftShift;
    [SerializeField] KeyCode crouchKey = KeyCode.LeftControl;

    [Header("Movement")]
    [SerializeField] Transform orientation = null;
    [SerializeField] [Range(6, 8)] float moveSpeed = 6.0f;
    Vector3 moveDirection, slopeMoveDirection;
    float groundMultiplier = 10.0f;
   

    [Header("Jump")]
    [SerializeField] float jumpForce = 15.0f;
    float airMultiplier = 0.5f;

    [Header("Sprint")]
    [SerializeField] float sprintModifier = 2.0f;

    [Header("Crouch")]
    [SerializeField] public float reduceHeight = 0.7f;
    CapsuleCollider playerCollider;
    float originalHeight;

    [Header("Ground Detection")]
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundMask;
    Rigidbody rb;
    RaycastHit slopeHit;
    bool isGrounded;
    

    private void Start()
    {
        playerCollider = GetComponentInChildren<CapsuleCollider>();
        originalHeight = playerCollider.height;
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void Update()
    {
        GroundCheck();
        SlopeCheck();
        PlayerDrag();
        MoveInput();
    }

    void GroundCheck()
    {
        float groundRadius = 0.2f;
        isGrounded = Physics.CheckSphere(groundCheck.position, groundRadius, groundMask);
    }

    bool SlopeCheck()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, 0.5f))
        {
            if (slopeHit.normal != Vector3.up)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }


    void PlayerDrag()
    {
        float groundDrag = 6.0f;
        float airDrag = 2.0f;

        if (isGrounded)
        {
            rb.drag = groundDrag;
        }
        else if (!isGrounded)
        {
            rb.drag = airDrag;
        }
    }


    void MoveInput()
    {
        float horizontalMovement = Input.GetAxisRaw("Horizontal");
        float verticalMovement = Input.GetAxisRaw("Vertical");

        moveDirection = orientation.forward * verticalMovement + orientation.right * horizontalMovement;
        slopeMoveDirection = Vector3.ProjectOnPlane(moveDirection, slopeHit.normal);

        Jump();
        Sprint();
        Crouch();

    }

    private void Jump()
    {
        if (Input.GetKeyDown(jumpKey) && isGrounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void Sprint()
    {
        if (Input.GetKeyDown(sprintKey) && isGrounded && moveSpeed != 8)
        {
            moveSpeed += sprintModifier;

        }
        else if (Input.GetKeyUp(sprintKey) && moveSpeed != 6)
        {
            moveSpeed -= sprintModifier;
        }
    }

    private void Crouch()
    {
        if (Input.GetKeyDown(crouchKey) && isGrounded)
        {
            playerCollider.height = reduceHeight;
        }

        else if (Input.GetKeyDown(crouchKey))
        {
            playerCollider.height = originalHeight;
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    void MovePlayer()
    {
        if (isGrounded && !SlopeCheck())
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * groundMultiplier, ForceMode.Acceleration);
        }
        else if (isGrounded && SlopeCheck())
        {
            rb.AddForce(slopeMoveDirection.normalized * moveSpeed * groundMultiplier, ForceMode.Acceleration);
        }
        else if (!isGrounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * groundMultiplier * airMultiplier, ForceMode.Acceleration);
        }

    }
}

