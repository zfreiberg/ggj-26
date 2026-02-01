using UnityEngine;

public class EnemyBulletControl : MonoBehaviour
{
    [SerializeField] float speed = 8f;
    [SerializeField] Vector2 direction = Vector2.right;
    [SerializeField] float speedDamping = 0f; // per-second damping towards 0
    [SerializeField] float lifeTime = 10f;

    private float currentTime = 0f;

    public void Setup(Vector3 pos, Vector2 dir, float spd)
    {
        transform.position = pos;
        direction = dir;
        speed = spd;
    }

    void Update()
    {
        if (speedDamping > 0f)
            speed = Mathf.Lerp(speed, 0f, speedDamping * Time.deltaTime);

        // Move the bullet in the specified direction
        transform.Translate(direction.normalized * speed * Time.deltaTime);

        currentTime += Time.deltaTime;
        if (currentTime >= lifeTime)
        {
            Destroy(gameObject);
        }
    }

    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // damage player
            PlayerControl.Inst.OnEnemyTouch();
        }
        if (other.gameObject.CompareTag("Ground"))
        {
            // destroy the bullet on hitting ground or obstacle
            Destroy(gameObject);
        }
    }
}
