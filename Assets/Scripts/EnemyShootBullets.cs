using UnityEngine;

public class EnemyShootBullets : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float shootInterval = 2f;
    public float bulletSpeed = 8f;
    public Vector2 shootDirection = Vector2.left;

    float shootTimer = 0f;
    void Update()
    {
        shootTimer += Time.deltaTime;
        if (shootTimer >= shootInterval)
        {
            ShootBullet();
            shootTimer = 0f;
        }
    }

    void ShootBullet()
    {
        Vector3 shootPos = transform.position;
        Vector2 shootDir = shootDirection;
        GameControl.CreateBullet(bulletPrefab, shootPos, shootDir, bulletSpeed);
    }
}
