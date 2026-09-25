using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    public float speed = 12f;
    public float damage = 15f;
    public float lifeTime = 3f;
    Vector3 direction;
    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity = false;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
    }

    public void Launch(Vector3 dir)
    {
        direction = dir.normalized;
        DebugOverlay.ProjectilesFired++;
        Destroy(gameObject, lifeTime);
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player") return;
        var health = other.GetComponent<Health>();
        if (health != null)
        {
            DebugOverlay.ProjectileHits++;
            health.TakeDamage(damage);
        }
        Destroy(gameObject);
    }
}