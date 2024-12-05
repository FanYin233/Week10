using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;

    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    public float jumpForce = 10f;

    public float terminalSpeed = -10f;

    public float coyoteTime = 0.2f;
    private float coyoteTimeCounter = 0f;

    public float dashSpeed = 15f;
    public float dashTime = 0.2f;
    private bool isDashing = false;
    private float dashTimer = 0f;

    private bool canDoubleJump = true;

    private bool isGravityReversed = false;


    public enum FacingDirection
    {
        left,
        right
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Vector2 playerInput = new Vector2(Input.GetAxis("Horizontal"), 0);

        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing)
        {
            isDashing = true;
            dashTimer = dashTime;
        }

        if (isDashing)
        {
            dashTimer -= Time.deltaTime;
            if (dashTimer <= 0)
            {
                isDashing = false;
            }
        }

        MovementUpdate(playerInput);

        if (Input.GetKeyDown(KeyCode.G))
        {
            isGravityReversed = !isGravityReversed;
            rb.gravityScale *= -1;
            transform.Rotate(0, 180, 0);
        }

        MovementUpdate(playerInput);
    }

    private void MovementUpdate(Vector2 playerInput)
    {
        Vector2 velocity = rb.velocity;

        if (isDashing)
        {
            velocity.x = (GetFacingDirection() == FacingDirection.right ? 1 : -1) * dashSpeed;
        }
        else
        {
            velocity.x = playerInput.x * moveSpeed;
        }

        if (IsGrounded())
        {
            canDoubleJump = true;
        }

        if (Input.GetButtonDown("Jump"))
        {
            if (IsGrounded())
            {
                velocity.y = jumpForce * (isGravityReversed ? -1 : 1);
            }
            else if (canDoubleJump)
            {
                velocity.y = jumpForce * (isGravityReversed ? -1 : 1);
                canDoubleJump = false;
            }
        }

        rb.velocity = velocity;
    }

    public bool IsWalking()
    {
        return Mathf.Abs(rb.velocity.x) > 0.1f;
    }

    public FacingDirection GetFacingDirection()
    {
        return rb.velocity.x >= 0 ? FacingDirection.right : FacingDirection.left;
    }

    public bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    private void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
