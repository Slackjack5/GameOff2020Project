using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float baseSpeed = 40f;
    [SerializeField] private float speedMultiplier = 10f;
    [SerializeField] private float baseJumpForce = 400f;
    [SerializeField] private float movementSmoothTime = 0.1f;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Transform groundCheckPosition;

    private Rigidbody2D rb;

    private Vector2 velocity = Vector2.zero;
    private float horizontalInput = 0f;
    private bool isJumping = false;
    private bool isGrounded = false;

    const float groundCheckDistance = .1f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        isGrounded = Physics2D.Raycast(groundCheckPosition.position, Vector2.down, groundCheckDistance, whatIsGround);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            isJumping = true;
        }
    }

    private void FixedUpdate()
    {
        Move(horizontalInput * Time.fixedDeltaTime);

        if (isJumping)
        {
            rb.AddForce(baseJumpForce * Vector2.up);
            isJumping = false;
        }
        
    }

    private void Move(float horizontalMovement)
    {
        float targetVelocityX = baseSpeed * speedMultiplier * horizontalMovement;
        Vector2 targetVelocity = new Vector2(targetVelocityX, rb.velocity.y);
        rb.velocity = Vector2.SmoothDamp(rb.velocity, targetVelocity, ref velocity, movementSmoothTime);
    }
}
