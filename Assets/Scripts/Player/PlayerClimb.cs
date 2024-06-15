using UnityEngine;

public class PlayerClimb : MonoBehaviour
{
    private readonly float Speed = 5f;
    private readonly float NormalGravity = 4f;
    private float verticalInput;
    private bool isNearLadder; // is overlapping with ladder or not
    private bool isClimbing; public bool IsClimbing => isClimbing; // isClimbing = overlapping with ladder AND pressed up/down
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        verticalInput = Input.GetAxis("Vertical");
        if (isNearLadder && Mathf.Abs(verticalInput) > 0f)
        {
            isClimbing = true;
        }
    }

    void FixedUpdate()
    {
        if (isClimbing)
        {
            rb.gravityScale = 0f;
            rb.velocity = new Vector2(rb.velocity.x, verticalInput * Speed);
        }
        else
        {
            rb.gravityScale = NormalGravity;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isNearLadder = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isNearLadder = false;
            isClimbing = false;
        }
    }
}