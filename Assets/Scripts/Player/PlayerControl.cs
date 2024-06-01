using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public readonly float NormalSpeed = 5f;
    public readonly float NormalJumpForce = 10f;
    private const float FaintCountdownStart = 5f;
    private readonly Color NormalColor = Color.white;
    private readonly Color DizzyColor = Color.grey;
    public float speed;
    public float jumpForce;
    private float faintCountdown = 0f; // used when fallen from crack
    private bool isTryingToJump = false;
    private bool isGrounded = true;
    private bool hasFallenFromCrack = false;
    private bool isOnCoffeeSpill = false;
    private float horizontalInput;
    private Rigidbody2D rb;
    private ClimbLadder climbLadder;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        climbLadder = GetComponent<ClimbLadder>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        speed = NormalSpeed;
        jumpForce = NormalJumpForce;
    }

    void Update()
    {
        if (hasFallenFromCrack && isGrounded)
        {
            hasFallenFromCrack = false;
            faintCountdown = FaintCountdownStart;
        }
        if (faintCountdown > 0f)
        {
            faintCountdown -= Time.deltaTime;
        }
        SetPlayerSlowDown(faintCountdown > 0f || isOnCoffeeSpill);

        horizontalInput = Input.GetAxis("Horizontal");
        if (Input.GetKey(KeyCode.Space) && isGrounded && !climbLadder.isClimbing)
        {
            isTryingToJump = true;
        }
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontalInput * speed, isTryingToJump ? jumpForce : rb.velocity.y);
        isTryingToJump = false;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Floor"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Floor"))
        {
            isGrounded = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("CoffeeSpill"))
        {
            isOnCoffeeSpill = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("CoffeeSpill"))
        {
            isOnCoffeeSpill = false;
        }
        else if (other.gameObject.CompareTag("Crack"))
        {
            hasFallenFromCrack = true;
        }
    }

    private void SetPlayerSlowDown(bool slow)
    {
        if (slow)
        {
            speed = NormalSpeed / 3;
            jumpForce = NormalJumpForce / 3;
            spriteRenderer.color = DizzyColor;
        }
        else
        {
            speed = NormalSpeed;
            jumpForce = NormalJumpForce;
            spriteRenderer.color = NormalColor;
        }
    }
}
