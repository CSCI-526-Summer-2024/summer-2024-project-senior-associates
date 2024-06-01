using UnityEngine;

public class IgnoreFloorCollision : MonoBehaviour
{
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
