using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float baseSpeed = 40f;
    [SerializeField] private float speedMultiplier = 10f;
    [SerializeField] private float baseJumpForce = 100f;
    [SerializeField] private float movementSmoothTime = 0.1f;

    private Rigidbody2D rb;

    private float horizontalInput = 0f;
    private float jumpInput = 0f;
    private Vector2 velocity = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        jumpInput = Input.GetAxisRaw("Jump");
    }

    private void FixedUpdate()
    {
        Move(horizontalInput * Time.fixedDeltaTime);
        rb.AddForce(jumpInput * baseJumpForce * Vector2.up);
    }

    private void Move(float horizontalMovement)
    {
        float targetVelocityX = baseSpeed * speedMultiplier * horizontalMovement;
        Vector2 targetVelocity = new Vector2(targetVelocityX, rb.velocity.y);
        rb.velocity = Vector2.SmoothDamp(rb.velocity, targetVelocity, ref velocity, movementSmoothTime);
    }
}
