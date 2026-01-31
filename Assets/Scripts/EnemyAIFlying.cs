using UnityEngine;

[System.Serializable]
public struct EnemyAIFlyingData { public bool isMoving; public Vector2 position; public float rest; public Vector2 lastPosition; public bool isCloseToTargetPosition; public float flyingTime; public bool isFlyingTimeUp; }

public class EnemyAIFlying : MonoBehaviour
{
    Rigidbody2D rb;

    [Header("Movement")]
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] float moveSmoothTime = 0.06f; // smaller = snappier
    [SerializeField] float restTime = 2f;
    [SerializeField] float startRestTime = 1f;
    [SerializeField] float flyingTime = 3f;

    Vector2 velSmooth; // SmoothDamp ref
    
    public EnemyAIFlyingData enemyState;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate; // render-time interpolation
        enemyState = new EnemyAIFlyingData { isMoving = false, rest = 0f, position = this.gameObject.transform.position, lastPosition = this.gameObject.transform.position };

        // set a start rest time
        if (startRestTime > 0f)
            enemyState.rest = startRestTime;
        else
        {
            enemyState.isMoving = true;
        }
        enemyState.position = GetTargetPosition();
    }

    void Update()
    {
        var isCloseToTargetPosition = Vector2.Distance(transform.position, enemyState.position) < 0.1f;
        enemyState.isCloseToTargetPosition = isCloseToTargetPosition;

        // handle flying time countdown
        if (enemyState.isMoving)
            enemyState.flyingTime -= Time.deltaTime;
        else
            enemyState.flyingTime = flyingTime;

        enemyState.isFlyingTimeUp = enemyState.flyingTime <= 0f;

        // enemyState.isBlocked is set externally via IsBlockedChecker
        if ((isCloseToTargetPosition || enemyState.isFlyingTimeUp) && enemyState.rest <= 0f)
        {
            enemyState.rest = restTime;
            enemyState.isMoving = false;
            enemyState.position = GetTargetPosition();
            // set velocity to zero immediately
            rb.linearVelocity = new Vector2(0f, 0f);
        }
        // handle resting countdown
        if (enemyState.rest > 0f)
        {
            enemyState.rest -= Time.deltaTime;
            if (enemyState.rest <= 0f)
            {
                enemyState.position = GetTargetPosition();
                enemyState.isMoving = true;
            }
        }
    }

    Vector2 GetTargetPosition() => PlayerControl.PlayerLastPosition;

    void FixedUpdate()
    {
        MoveUnit();
        enemyState.lastPosition = transform.position;
    }

    void MoveUnit()
    {
        if (!enemyState.isMoving) return;
        // Smooth horizontal movement
        Vector2 targetPos = enemyState.position;
        Vector2 moveDir = targetPos - (Vector2)transform.position;
        Vector2 target = moveDir * moveSpeed;
        Vector2 newVelocity = Vector2.SmoothDamp(rb.linearVelocity, target, ref velSmooth, moveSmoothTime);
        rb.linearVelocity = new Vector2(newVelocity.x, newVelocity.y);
    }
}
