using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] float attackRange = 2f;
    [SerializeField] float attackDamage = 25f;

    public void RequestAttack()
    {
        Vector3 center = transform.position + Vector3.up * 0.5f + transform.forward * (attackRange * 0.5f);
        Collider[] hits = Physics.OverlapSphere(center, attackRange * 0.5f);

        foreach (var hit in hits)
        {
            if (hit.gameObject == gameObject) continue;
            var health = hit.GetComponent<Health>();
            if (health != null)
                health.TakeDamage(attackDamage);
        }
    }
}