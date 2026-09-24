using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 12f;
    public float damage = 15f;
    public float lifeTime = 3f;
    Vector3 direction;

    public void Launch(Vector3 dir)
    {
        direction = dir.normalized;
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
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