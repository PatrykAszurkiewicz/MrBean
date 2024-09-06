using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    public float speed;
    public float jumpForce = 5f;  
    public LayerMask groundLayer;  

    public float rotationSpeed = 360f;  

    private Vector2 move;
    private Rigidbody rb;

    private bool isGrounded;
    private bool isJumping = false;
    private float rotationProgress = 0f;

    public float dashSpeed = 10f;  
    public float dashDuration = 0.2f;  
    public float dashCooldown = 1f;  

    private bool isDashing = false;
    private float dashTimeRemaining;
    private float lastDashTime;

    public int JumpAmount = 2;

    private bool isDiving = false;
    private float diveTimeRemaining;
    public float diveDuration = 1f;
    public float diveSpeed = 10f;

    public GameObject planedive;

    public void HoldingMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && JumpAmount > 0 || context.performed && isGrounded)
        {
            Debug.Log(JumpAmount);
            Jump();
        }
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.performed && !isDashing && Time.time > lastDashTime + dashCooldown)
        {
            StartDash();
        }
    }

    public void OnDive(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            StartDive();
        }
        else if (context.canceled)
        {
            StopDive();
        }
    }
    private void Dive()
    {
        Vector3 diveDirection = Vector3.down * diveSpeed * Time.deltaTime;
        rb.MovePosition(rb.position + diveDirection);
    }
    private void StartDive()
    {
        isDiving = true;
        transform.rotation = Quaternion.Euler(-45f, 0f, 0f);  
        planedive.SetActive(false);
    }

    private void StopDive()
    {
        isDiving = false;
        planedive.SetActive(true);
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        CheckGrounded();
        Move();
        RotateDuringJump();
        UpdateDash();
        if (isDiving)
        {
            Dive();
        }
    }

    private void Move()
    {
        if (isDashing) return;

        Vector3 moving = new Vector3(move.x, 0f, move.y);

        if (moving != Vector3.zero)
        {
            moving.Normalize();

            if(!isJumping) 
            {

            Quaternion targetRotation = Quaternion.LookRotation(moving);
            Quaternion rotationOffset = Quaternion.Euler(-90f, 0f, 0f);
            Quaternion finalRotation = targetRotation * rotationOffset;
                
            transform.rotation = Quaternion.Slerp(transform.rotation, finalRotation, 0.2f);
            }

        }

        Vector3 newPosition = rb.position + moving * speed * Time.deltaTime;
        rb.MovePosition(newPosition);
    }
    private void StartDash()
    {
        isDashing = true;
        dashTimeRemaining = dashDuration;
        lastDashTime = Time.time;
    }

    private void UpdateDash()
    {
        if (isDashing)
        {
            if (dashTimeRemaining > 0)
            {
                Vector3 dashDirection = new Vector3(move.x, 0f, move.y).normalized;
                rb.MovePosition(rb.position + dashDirection * dashSpeed * Time.deltaTime);
                dashTimeRemaining -= Time.deltaTime;
            }
            else
            {
                isDashing = false;
            }
        }
    }
    private void Jump()
    {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isJumping = true;
            rotationProgress = 0f;  // Resetuj postêp obrotu
            JumpAmount--;
            Debug.Log(JumpAmount);
        
    }

    private void RotateDuringJump()
    {
        if (isJumping)
        {
            float rotationAmount = rotationSpeed * Time.deltaTime;
            transform.Rotate(Vector3.forward, rotationAmount);
            rotationProgress += rotationAmount;

            if (rotationProgress >= 360f) 
            {
                rotationProgress = 0f;
                isJumping = false;
            }
        }
    }

    private void CheckGrounded()
    {
        
        
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f, groundLayer);
        if (isGrounded && !isJumping)
        {
            isJumping = false;
            JumpAmount = 2;
        }
    }
}
