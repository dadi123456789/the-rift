using UnityEngine;

// Standard Unity pattern: kinematic Rigidbody + trigger Collider + OnTriggerEnter.
// This is the native, documented way Unity expects fast-moving hit-detection objects to work.
[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    public float damage = 15f;
    [SerializeField] float speed = 12f;
    [SerializeField] float lifeTime = 3f;

    Rigidbody rb;
    Vector3 direction;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity = false;
        rb.interpolation = RigidbodyInterpolation.Interpolate;

        var col = GetComponent<Collider>();
        col.isTrigger = true;
    }

    public void Launch(Vector3 dir)
    {
        direction = dir.normalized;
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
            health.TakeDamage(damage);

        Destroy(gameObject);
    }
}