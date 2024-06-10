using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    private readonly float NormalSpeed = 5f;
    private readonly float NormalJumpForce = 10f;
    private readonly Color NormalColor = Color.white;
    private readonly Color DizzyColor = Color.grey;
    private float speed;
    private float jumpForce;
    private bool isTryingToJump = false;
    private bool isGrounded = true;
    private bool isTouchingCoffeeSpill = false;
    private float horizontalInput;
    private Rigidbody2D rb;
    private ClimbLadder climbLadder;
    private PlayerEnergy playerEnergy;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        climbLadder = GetComponent<ClimbLadder>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        speed = NormalSpeed;
        jumpForce = NormalJumpForce;
        playerEnergy = GetComponent<PlayerEnergy>();
    }

    void Update()
    {
        SetPlayerSlowDown(isTouchingCoffeeSpill);

        horizontalInput = Input.GetAxis("Horizontal");
        if (Input.GetKey(KeyCode.Space) && isGrounded && !climbLadder.IsClimbing)
        {
            isTryingToJump = true;
        }
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(playerEnergy.IsSleeping ? 0f : horizontalInput * speed, isTryingToJump ? jumpForce : rb.velocity.y);
        isTryingToJump = false;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Floor"))
        {
            ContactPoint2D contact = other.GetContact(0);
            Bounds bounds = GetComponent<Collider2D>().bounds;
            bool isCollisionFromBottom = contact.point.y < bounds.center.y - bounds.extents.y * 0.9f;
            if (isCollisionFromBottom)
            {
                isGrounded = true;
            }
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
            isTouchingCoffeeSpill = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("CoffeeSpill"))
        {
            isTouchingCoffeeSpill = false;
        }
    }

    private void SetPlayerSlowDown(bool slow)
    {
        if (slow || playerEnergy.Tired)
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
