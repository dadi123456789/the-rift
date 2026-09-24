using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField] float maxHealth = 100f;
    public float CurrentHealth { get; private set; }
    public UnityAction<float, float> OnHealthChanged; // current, max
    public UnityAction OnDeath;
    public UnityAction<GameObject> RequestHealthBarSetup;

    void Awake() => CurrentHealth = maxHealth;

    public void TakeDamage(float amount)
    {
        if (CurrentHealth <= 0f) return;

        CurrentHealth = Mathf.Max(0f, CurrentHealth - amount);
        OnHealthChanged?.Invoke(CurrentHealth, maxHealth);

        if (CurrentHealth <= 0f)
            OnDeath?.Invoke();
    }
}