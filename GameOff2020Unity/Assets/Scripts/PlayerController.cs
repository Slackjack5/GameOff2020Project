using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float baseSpeed = 100f;
    [SerializeField] private float speedMultiplier = 10f;
    [SerializeField] private float baseJumpSpeed = 20f;
    [SerializeField] private float maxJumpTime = 0.3f;
    [SerializeField] private float movementSmoothTime = 0.1f;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Transform groundCheckPosition;
    [SerializeField] private float respawnTime = 1.5f;

    private Rigidbody2D rb;

    private Vector2 velocity = Vector2.zero;
    private float horizontalInput = 0f;
    private bool jumpKeyHeld = false;
    private float jumpTimeCounter;
    private bool isGrounded = false;
    private Vector2 respawnPosition;

    const float groundCheckDistance = .1f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        respawnPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        isGrounded = Physics2D.Raycast(groundCheckPosition.position, Vector2.down, groundCheckDistance, whatIsGround);

        if (!GameManager.playerIsDead)
        {
            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                jumpTimeCounter = maxJumpTime;
                jumpKeyHeld = true;
            }
            else if (Input.GetButtonUp("Jump"))
            {
                jumpKeyHeld = false;
            }
        }
    }

    private void FixedUpdate()
    {
        if (!GameManager.playerIsDead)
        {
            Move(horizontalInput * Time.fixedDeltaTime);

            if (jumpKeyHeld && jumpTimeCounter > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, baseJumpSpeed);
                jumpTimeCounter -= Time.fixedDeltaTime;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Death"))
        {
            StartCoroutine(Die());
        }
    }

    private void Move(float horizontalMovement)
    {
        float targetVelocityX = baseSpeed * speedMultiplier * horizontalMovement;
        Vector2 targetVelocity = new Vector2(targetVelocityX, rb.velocity.y);
        rb.velocity = Vector2.SmoothDamp(rb.velocity, targetVelocity, ref velocity, movementSmoothTime);
    }

    private IEnumerator Die()
    {
        if (!GameManager.playerIsDead)
        {
            // Drop to the ground
            rb.velocity = new Vector2(0, rb.velocity.y);

            GameManager.playerIsDead = true;
            yield return new WaitForSeconds(respawnTime);

            Respawn();
        }
    }

    private void Respawn()
    {
        GameManager.playerIsDead = false;
        transform.position = respawnPosition;
    }
}
