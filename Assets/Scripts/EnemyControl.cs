using UnityEngine;
using System.Collections.Generic;

public class EnemyControl : MonoBehaviour
{
    public static List<EnemyControl> AllEnemies { get; private set; } = new List<EnemyControl>();

    public Vector2 originalSize = Vector2.one;

    void Awake()
    {
        AllEnemies.Add(this);
        originalSize = transform.localScale;
    }

    void OnDestroy()
    {
        AllEnemies.Remove(this);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // damage player
            GameControl.Inst.PlayAudioDeath();
            PlayerControl.Inst.OnEnemyTouch();
        }
    }

    public void BecomeLarger( bool isLarger ){
        if (isLarger)
            transform.localScale = originalSize * 1.5f;
        else
            transform.localScale = originalSize;
    }
}
