using UnityEngine;

public class TeleportCheker : MonoBehaviour
{
    public GameObject teleportPoint;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // teleporting the player to the teleport point
            other.gameObject.transform.position = teleportPoint.transform.position;
        }
    }
}
