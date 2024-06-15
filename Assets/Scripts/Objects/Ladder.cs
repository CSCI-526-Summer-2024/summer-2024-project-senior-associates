using UnityEngine;

public class Ladder : MonoBehaviour
{
    // Our logic here: ignore the upper floor's collision if player is climbing
    // climbing = overlapping with ladder AND pressed up/down
    public BoxCollider2D upperFloor;
    private PlayerClimb playerClimb;

    void Update()
    {
        upperFloor.enabled = playerClimb == null || !playerClimb.IsClimbing;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerClimb = other.GetComponent<PlayerClimb>();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerClimb = null;
        }
    }
}
