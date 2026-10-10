using UnityEngine;

[RequireComponent(typeof(Health))]
public class SimpleEnemy : MonoBehaviour
{
    [SerializeField] float moveSpeed = 2.5f;
    [SerializeField] float detectionRange = 8f;
    [SerializeField] float stopDistance = 1.5f;
    [SerializeField] float attackRange = 1.8f;
    [SerializeField] float contactDamage = 10f;
    [SerializeField] float contactCooldown = 1f;

    Rigidbody rb;
    Health health;
    Renderer rend;
    Color originalColor;
    Transform player;
    Health playerHealth;
    float lastContactTime = -999f;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        health = GetComponent<Health>();
        rend = GetComponent<Renderer>();
        originalColor = rend.material.color;

        health.OnHealthChanged += (_, __) => FlashColor();
        health.OnDeath += () =>
        {
            GameManager.RegisterKill();
            GameManager.RegisterEnemyDeath();
            LootPickup.Spawn(transform.position, transform.parent);
            Destroy(gameObject);
        };

        var playerGO = GameObject.Find("Player");
        if (playerGO != null)
        {
            player = playerGO.transform;
            playerHealth = playerGO.GetComponent<Health>();
        }
    }

    void FixedUpdate()
    {
        if (health.CurrentHealth <= 0f || player == null) return;

        Vector3 toPlayer = player.position - transform.position;
        toPlayer.y = 0f;
        float distance = toPlayer.magnitude;

        if (distance <= detectionRange && distance > stopDistance)
        {
            Vector3 direction = toPlayer.normalized;
            // PhysX resolves any collision with walls/other enemies automatically.
            rb.velocity = new Vector3(direction.x * moveSpeed, rb.velocity.y, direction.z * moveSpeed);
        }
        else
        {
            rb.velocity = new Vector3(0f, rb.velocity.y, 0f);
        }

        if (distance <= attackRange && Time.time - lastContactTime >= contactCooldown)
        {
            lastContactTime = Time.time;
            if (playerHealth != null)
                playerHealth.TakeDamage(contactDamage);
        }
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