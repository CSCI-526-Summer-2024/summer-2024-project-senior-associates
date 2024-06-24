using UnityEngine;

public class Ladder : MonoBehaviour
{
    // Our logic here: ignore the upper floor's collision if player is climbing
    // climbing = overlapping with ladder AND pressed up/down
    public BoxCollider2D upperFloor;
    private PlayerControl playerControl;

    void Update()
    {
        upperFloor.enabled = playerControl == null || !playerControl.IsClimbing;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerControl = other.GetComponent<PlayerControl>();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerControl = null;
        }
    }
}
