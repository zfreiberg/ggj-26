using UnityEngine;

[System.Serializable]
public struct EnemyState { public bool isMoving; public bool isAtTheEdge; public bool isAtTheWall; public float rest; }

public class EnemyControl : MonoBehaviour
{
    Rigidbody2D rb;

    [Header("Movement")]
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float moveSmoothTime = 0.06f; // smaller = snappier
    [SerializeField] float restTime = 2f;
    [SerializeField] float startRestTime = 3f;
    [SerializeField] float startMovingDirectionX = 1f;

    [Header("Ground Check")]
    [SerializeField] float groundCheckDistance = 1f;
    [SerializeField] LayerMask groundLayer = ~0; // default: everything (to set in editor)
    [SerializeField] float checkAtEdgeOffsetX1 = -0.5f; // left side ground check offset
    [SerializeField] float checkAtEdgeOffsetX2 = 0.5f; // right side ground check offset
    [SerializeField] float checkAtWallDistance = 2f; // distance for wall check raycast
    [SerializeField] float checkAtWallYOffset = -0.3f; // vertical offset for wall check raycast


    float moveDirX;
    float velXSmooth; // SmoothDamp ref
    
    [SerializeField] EnemyState enemyState;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate; // render-time interpolation
        enemyState = new EnemyState { isMoving = false, isAtTheEdge = false, isAtTheWall = false, rest = 0f };

        // set a start rest time
        if (startRestTime > 0f){
            enemyState.rest = startRestTime;
            moveDirX = -startMovingDirectionX;
        }
        else{
            enemyState.isMoving = true;
            moveDirX = startMovingDirectionX;
        }
    }

    void Update()
    {
        // 2D ground check via raycast
        enemyState.isAtTheEdge = (moveDirX < 0 && !GroundCheckWithOffsetX(checkAtEdgeOffsetX1)) || (moveDirX > 0 && !GroundCheckWithOffsetX(checkAtEdgeOffsetX2));
        enemyState.isAtTheWall = WallCheck();
        // if the enemy is at the edge, start resting
        if ((enemyState.isAtTheEdge || enemyState.isAtTheWall) && enemyState.rest <= 0f)
        {
            enemyState.rest = restTime;
            enemyState.isMoving = false;
            // set velocity to zero immediately
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
        }
        // handle resting countdown
        if (enemyState.rest > 0f)
        {
            enemyState.rest -= Time.deltaTime;
            if (enemyState.rest <= 0f)
            {
                // after resting, reverse direction and start moving
                moveDirX = -moveDirX;
                enemyState.isMoving = true;
            }
        }
    }

    private bool GroundCheckWithOffsetX(float offsetX)
    {
        Vector2 origin = (Vector2)transform.position + new Vector2(offsetX, 0f) + Vector2.up * 0.1f;
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, groundCheckDistance, groundLayer);
        return hit.collider != null;
    }

    private bool WallCheck()
    {
        Vector2 origin = (Vector2)transform.position + new Vector2(0f, checkAtWallYOffset);
        RaycastHit2D hit = Physics2D.Raycast(origin, new Vector2(Mathf.Sign(moveDirX), 0f), checkAtWallDistance, groundLayer);
        return hit.collider != null;
    }

    void FixedUpdate()
    {
        MoveUnit();
    }

    void MoveUnit()
    {
        if (!enemyState.isMoving) return;
        // Smooth horizontal movement
        float targetX = moveDirX * moveSpeed;
        float newX = Mathf.SmoothDamp(rb.linearVelocity.x, targetX, ref velXSmooth, moveSmoothTime);
        rb.linearVelocity = new Vector2(newX, rb.linearVelocity.y);
    }
}
