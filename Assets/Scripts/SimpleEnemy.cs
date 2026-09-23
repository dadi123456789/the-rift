using UnityEngine;

[RequireComponent(typeof(Health))]
public class SimpleEnemy : MonoBehaviour
{
    [SerializeField] float moveSpeed = 2.5f;
    [SerializeField] float detectionRange = 8f;
    [SerializeField] float stopDistance = 1.5f;
    [SerializeField] float contactDamage = 10f;
    [SerializeField] float contactCooldown = 1f;

    Rigidbody rb;
    Health health;
    Renderer rend;
    Color originalColor;
    Transform player;
    float lastContactTime = -999f;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        health = GetComponent<Health>();
        rend = GetComponent<Renderer>();
        originalColor = rend.material.color;

        health.OnHealthChanged += (_, __) => FlashColor();
        health.OnDeath += () => Destroy(gameObject);

        var playerGO = GameObject.Find("Player");
        if (playerGO != null) player = playerGO.transform;
    }

    void FixedUpdate()
    {
        if (player == null || health.CurrentHealth <= 0f) return;

        Vector3 toPlayer = player.position - transform.position;
        toPlayer.y = 0f;
        float distance = toPlayer.magnitude;

        if (distance <= detectionRange && distance > stopDistance)
        {
            Vector3 direction = toPlayer.normalized;
            rb.velocity = new Vector3(direction.x * moveSpeed, rb.velocity.y, direction.z * moveSpeed);
        }
        else
        {
            rb.velocity = new Vector3(0f, rb.velocity.y, 0f);
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.name != "Player") return;
        if (Time.time - lastContactTime < contactCooldown) return;
        lastContactTime = Time.time;

        var playerHealth = collision.gameObject.GetComponent<Health>();
        if (playerHealth != null)
            playerHealth.TakeDamage(contactDamage);
    }

    void FlashColor()
    {
        StopAllCoroutines();
        StartCoroutine(FlashRoutine());
    }

    System.Collections.IEnumerator FlashRoutine()
    {
        rend.material.color = Color.white;
        yield return new WaitForSeconds(0.15f);
        if (rend != null) rend.material.color = originalColor;
    }
}