using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyControl : MonoBehaviour
{
    public static List<EnemyControl> AllEnemies { get; private set; } = new();

    [Header("Size")]
    public Vector3 originalSize = Vector3.one;
    public float largeScaleMultiplier = 1.5f;

    [Header("Animation Overrides")]
    public AnimatorOverrideController normalOverride;
    public AnimatorOverrideController bigOverride;

    [Header("Death")]
    public float destroyAfterSeconds = 0.8f;

    public Transform visualRoot;

    Animator anim;
    Collider2D col;
    bool isDead;

    void Awake()
    {
        AllEnemies.Add(this);

        originalSize = transform.localScale;

        anim = GetComponent<Animator>();
        col = GetComponent<Collider2D>();

        // Optional: ensure we start in normal mode
        if (anim != null && normalOverride != null)
            anim.runtimeAnimatorController = normalOverride;
    }

    void OnDestroy()
    {
        AllEnemies.Remove(this);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (isDead) return;

        if (other.gameObject.CompareTag("Player"))
        {
            PlayerControl.Inst.OnEnemyTouch();
        }
    }

    public void BecomeLarger(bool makeLarge)
    {
        if (isDead) return;

        if (visualRoot == null) visualRoot = transform;

        visualRoot.localScale = makeLarge ? (Vector3.one * largeScaleMultiplier) : Vector3.one;

        // transform.localScale = makeLarge ? (originalSize * largeScaleMultiplier) : originalSize;

        if (anim != null)
        {
            var target = makeLarge ? bigOverride : normalOverride;
            if (target != null)
                anim.runtimeAnimatorController = target;
        }
    }

    public void Die()
    {
        if (isDead) return;
        isDead = true;
        if (col != null) col.enabled = false;

        if (anim != null)
            anim.SetTrigger("Die");

        StartCoroutine(DestroyAfterDelay());
    }

    IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(destroyAfterSeconds);
        Destroy(gameObject);
    }
}
