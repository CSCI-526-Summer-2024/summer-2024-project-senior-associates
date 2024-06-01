using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public readonly float NormalSpeed = 5f;
    public readonly float NormalJumpForce = 10f;
    private const float FaintCountdownStart = 5f;
    public float speed; // may be changed by coffee spill zone
    public float jumpForce; // may be changed by coffee spill zone
    private float faintCountdown = 0f; // used when dropped from crack
    private float horizontalInput;
    private bool isTryingToJump;
    private Rigidbody2D rb;
    private bool isGrounded;
    private ClimbLadder climbLadder;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        climbLadder = GetComponent<ClimbLadder>();
        speed = NormalSpeed;
        jumpForce = NormalJumpForce;
    }

    void Update()
    {
        if (faintCountdown > 0f)
        {
            faintCountdown -= Time.deltaTime;
        }
        SetPlayerSlowDown(faintCountdown > 0f);

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
            SetPlayerSlowDown(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("CoffeeSpill"))
        {
            SetPlayerSlowDown(false);
        }
        else if (other.gameObject.CompareTag("Crack"))
        {
            faintCountdown = FaintCountdownStart; SetPlayerSlowDown(true);
        }
    }

    private void SetPlayerSlowDown(bool slow)
    {
        if (slow)
        {
            speed = NormalSpeed / 3;
            jumpForce = NormalJumpForce / 3;
        }
        else
        {
            speed = NormalSpeed;
            jumpForce = NormalJumpForce;
        }
    }
}
