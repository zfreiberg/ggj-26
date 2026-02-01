using UnityEngine;

public class TeleportCheker : MonoBehaviour
{
    public GameObject teleportPoint;
    public GameObject bgToTurnOn;
    public GameObject bgToTurnOff;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // teleporting the player to the teleport point
            other.gameObject.transform.position = teleportPoint.transform.position;
            // set player speed to zero if it has a Rigidbody2D
            Rigidbody2D rb = other.gameObject.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
            }

            bgToTurnOn.SetActive(true);
            bgToTurnOff.SetActive(false);
        }
    }
}
