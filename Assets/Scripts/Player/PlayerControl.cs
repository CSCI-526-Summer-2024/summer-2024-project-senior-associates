using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public readonly float NormalSpeed = 5f;
    public readonly float NormalJumpForce = 10f;
    public float speed;
    public float jumpForce;
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            isGrounded = false;
        }
    }
}
