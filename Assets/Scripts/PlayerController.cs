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
        MovementUpdate(playerInput);

        Debug.Log("IsGrounded: " + IsGrounded());
    }

    private void MovementUpdate(Vector2 playerInput)
    {
        Vector2 velocity = rb.velocity;

        velocity.x = playerInput.x * moveSpeed;

        if (IsGrounded())
        {
            coyoteTimeCounter = coyoteTime;
        }
        else if (coyoteTimeCounter > 0)
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        if (coyoteTimeCounter > 0 && Input.GetButtonDown("Jump"))
        {
            velocity.y = jumpForce;
            coyoteTimeCounter = 0;
        }

        if (velocity.y < terminalSpeed)
        {
            velocity.y = terminalSpeed;
        }

        rb.velocity = velocity;


        Debug.Log(coyoteTime);
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
