using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public GameObject player2D;

    [SerializeField] float followSpeed = 5f;
    [SerializeField] Vector2 offset = Vector2.zero;
    [SerializeField] bool followY = true;

    float z;

    void Awake()
    {
        z = transform.position.z; // keep camera’s Z
    }

    void LateUpdate()
    {
        if (!player2D) return;

        Vector2 target2D = (Vector2)player2D.transform.position + offset;
        Vector3 target3D = new Vector3(target2D.x, followY ? target2D.y : transform.position.y, z);

        transform.position = Vector3.Lerp(transform.position, target3D, followSpeed * Time.deltaTime);
    }
}
