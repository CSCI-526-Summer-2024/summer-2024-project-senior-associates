using UnityEngine;

public class IgnoreFloorCollision : MonoBehaviour
{
    // Our logic here: ignore the upper floor's collision if player is climbing
    // climbing = overlapping with ladder AND pressed up/down
    public BoxCollider2D floorBc;
    private ClimbLadder ladderMovement;

    void Update()
    {
        floorBc.enabled = !(ladderMovement != null && ladderMovement.isClimbing);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ladderMovement = other.GetComponent<ClimbLadder>();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ladderMovement = null;
        }
    }
}
