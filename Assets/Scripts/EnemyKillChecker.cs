using UnityEngine;

public class EnemyKillChecker : MonoBehaviour
{
    public GameObject enemyObject;

    void OnTriggerEnter2D(Collider2D other)
    {
        // if (other.gameObject.CompareTag("Ground"))
        // {
        //     // kill the enemy
        //     Destroy(enemyObject);
        // }
    }
}
