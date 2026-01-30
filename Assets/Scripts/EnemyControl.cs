using UnityEngine;

public class EnemyControl : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // damage player
            PlayerControl.Inst.OnEnemyTouch();
        }
    }
}
