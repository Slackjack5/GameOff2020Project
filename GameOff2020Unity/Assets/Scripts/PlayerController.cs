using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float baseSpeed = 100f;
    [SerializeField] private float speedMultiplier = 10f;
    [SerializeField] private float baseJumpSpeed = 20f;
    [SerializeField] private float maxJumpTime = 0.3f;
    [SerializeField] private float slideMovementSmoothTime = 0.3f;
    [SerializeField] private float defaultMovementSmoothTime = 0.4f;
    [SerializeField] private float slideTime = 0.8f;
    [SerializeField] private float airmovementSmoothTime = 0.2f;
    [SerializeField] private float slideForce = 1500;
    [SerializeField] private float slideCooldown = 3;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Transform groundCheckPosition;
    [SerializeField] private GameObject styleTier;
    [SerializeField] private float respawnTime = 1.5f;

    private Rigidbody2D rb;

    private Vector2 velocity = Vector2.zero;
    private float horizontalInput = 0f;
    private bool jumpKeyHeld = false;
    private float jumpTimeCounter;
    private bool isGrounded = false;
    private float currentMovementSmoothTime = 0.1f;
    private Vector2 startPosition;
    private Vector2 respawnPosition;

    //Evasion Variables
    private bool evading = false;
    private float storedDirection = 0;
    private bool dashed = false;
    private bool cooldown = false;

    const float groundCheckDistance = .1f;

    //Animation
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPosition = transform.position;
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

            //Evading
            if (Input.GetKeyDown(KeyCode.LeftShift) && horizontalInput != 0 && !dashed && !cooldown)
            {
                evading = true;
                storedDirection = horizontalInput;
                if (!isGrounded)
                {
                    slideMovementSmoothTime = airmovementSmoothTime;
                }
            }
        }

        //Change speed on Skill Tier
        Style styleScript = styleTier.GetComponent<Style>();
        if (styleScript.tier == 1)
        {
            baseSpeed = 100f;
        }
        else if (styleScript.tier == 2)
        {
            baseSpeed = 125f;
        }
        else if (styleScript.tier == 3)
        {
            baseSpeed = 150f;
        }
        else if (styleScript.tier == 4)
        {
            baseSpeed = 175f;
        }
    }

    private void FixedUpdate()
    {
        if (!GameManager.playerIsDead)
        {
            if (dashed && !cooldown)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0);
            }

            //If Player Is sliding
            if (!evading)
            {
                Move(horizontalInput * Time.fixedDeltaTime);
            }
            else
            {
                SlideMove(storedDirection);
                StartCoroutine(StopSlide());
            }

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
        rb.velocity = Vector2.SmoothDamp(rb.velocity, targetVelocity, ref velocity, currentMovementSmoothTime);

        //Animation
        if (horizontalMovement>0)
        {
            animator.SetFloat("Speed", 10);
            animator.SetBool("MovingRight", true);
        }
        else if(horizontalMovement < 0)
        {
            animator.SetFloat("Speed", 10);
            animator.SetBool("MovingRight", false);
        }
        else
        {
            animator.SetFloat("Speed", 0);
            animator.SetBool("MovingRight", false);
        }
        
    }

    private void SlideMove(float horizontalMovement)
    {
        if (!cooldown)
        {
            currentMovementSmoothTime = slideMovementSmoothTime;
            if (!dashed)
            {
                if (horizontalMovement == 1)
                {
                    rb.AddForce(Vector2.right * slideForce);
                }
                else
                {
                    rb.AddForce(Vector2.left * slideForce);
                }
                dashed = true;
                evading = false;
                //Animation
                animator.SetBool("Sliding", true);
            }
        }
    }

    IEnumerator StopSlide()
    {
        yield return new WaitForSeconds(slideTime);
        currentMovementSmoothTime = defaultMovementSmoothTime;
        slideMovementSmoothTime = .4f;
        cooldown = true;
        dashed = false;
        animator.SetBool("Sliding", false);
        StartCoroutine(SlideCooldown());
    }

    IEnumerator SlideCooldown()
    {
        yield return new WaitForSeconds(slideCooldown);
        cooldown = false;
    }

    private IEnumerator Die()
    {
        if (!GameManager.playerIsDead)
        {
            // Drop to the ground
            rb.velocity = new Vector2(0, rb.velocity.y);

            GameManager.playerIsDead = true;

            Weapon weapon = GetComponent<Weapon>();
            weapon.Reset();

            yield return new WaitForSeconds(respawnTime);

            GameManager.playerIsDead = false;
            Respawn();
        }
    }

    private void Respawn()
    {
        transform.position = respawnPosition;
    }

    public void Reset()
    {
        transform.position = startPosition;
    }

    public void Checkpoint(Transform checkpointTransform)
    {
        respawnPosition = checkpointTransform.position;
    }
}
