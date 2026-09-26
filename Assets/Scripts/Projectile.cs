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

        if (Physics.SphereCast(transform.position, 0.2f, direction, out RaycastHit hit, step))
        {
            if (hit.collider.gameObject.name != "Player")
            {
                var health = hit.collider.GetComponent<Health>();
                if (health != null)
                {
                    DebugOverlay.RegisterHit();
                    health.TakeDamage(damage);
                }
                Destroy(gameObject);
                return;
            }
        }

        transform.position += direction * step;
    }
}