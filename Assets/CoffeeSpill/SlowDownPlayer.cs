using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowDownPlayer : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerControl player = other.GetComponent<PlayerControl>();
            if (player != null)
            {
                player.speed = player.NormalSpeed / 3;
                player.jumpForce = player.NormalJumpForce / 3;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerControl player = other.GetComponent<PlayerControl>();
            if (player != null)
            {
                player.speed = player.NormalSpeed;
                player.jumpForce = player.NormalJumpForce;
            }
        }
    }
}
