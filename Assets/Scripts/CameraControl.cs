using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public GameObject player2D;

    [SerializeField] float followSpeed = 5f;
    [SerializeField] Vector2 offset = Vector2.zero;

    float z;

    void Awake()
    {
        z = transform.position.z; // keep camera’s Z
    }

    void LateUpdate()
    {
        if (!player2D) return;

        Vector2 target2D = (Vector2)player2D.transform.position + offset;
        Vector3 target3D = new Vector3(target2D.x, target2D.y, z);

        // if distance is larger than 1 units, snap to target
        if (Vector3.Distance(transform.position, target3D) > 5f)
        {
            transform.position = target3D;
            return;
        }
        transform.position = Vector3.Lerp(transform.position, target3D, followSpeed * Time.deltaTime);
    }
}
