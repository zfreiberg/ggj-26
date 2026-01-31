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

            bgToTurnOn.SetActive(true);
            bgToTurnOff.SetActive(false);
        }
    }
}
