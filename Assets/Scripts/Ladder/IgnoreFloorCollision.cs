using UnityEngine;

public class IgnoreFloorCollision : MonoBehaviour
{
    // Our logic here: ignore the upper floor's collision if player is climbing
    // climbing = overlapping with ladder AND pressed up/down
    public BoxCollider2D floorBc;
    private ClimbLadder climbLadder;

    void Update()
    {
        floorBc.enabled = climbLadder == null || !climbLadder.IsClimbing;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            climbLadder = other.GetComponent<ClimbLadder>();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            climbLadder = null;
        }
    }
}
