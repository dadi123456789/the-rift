using UnityEngine;

// No Rigidbody, no OnTriggerEnter — moves manually and explicitly checks its
// own path every physics step. Guarantees a hit is never missed.
public class Projectile : MonoBehaviour
{
    public float speed = 12f;
    public float damage = 15f;
    public float lifeTime = 3f;
    Vector3 direction;
    float elapsed;

    public void Launch(Vector3 dir)
    {
        direction = dir.normalized;
        DebugOverlay.RegisterFired();
    }

    void FixedUpdate()
    {
        elapsed += Time.fixedDeltaTime;
        if (elapsed > lifeTime)
        {
            Destroy(gameObject);
            return;
        }

        float step = speed * Time.fixedDeltaTime;

        // Catches targets we're already overlapping — SphereCast alone
        // ignores colliders the sphere already intersects at the start.
        Collider[] overlaps = Physics.OverlapSphere(transform.position, 0.2f);
        foreach (var overlap in overlaps)
        {
            if (TryHit(overlap)) return;
        }

        // Catches targets in the path ahead (fast-moving tunneling case).
        if (Physics.SphereCast(transform.position, 0.2f, direction, out RaycastHit hit, step))
        {
            if (TryHit(hit.collider)) return;
        }

        transform.position += direction * step;
    }

    bool TryHit(Collider col)
    {
        if (col.gameObject.name == "Player") return false;

        var health = col.GetComponent<Health>();
        if (health == null) return false;

        DebugOverlay.RegisterHit();
        health.TakeDamage(damage);
        Destroy(gameObject);
        return true;
    }
}