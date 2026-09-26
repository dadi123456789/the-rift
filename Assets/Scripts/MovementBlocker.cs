using UnityEngine;

// Explicit physics check instead of relying on Unity's automatic collision/trigger
// events (which proved unreliable in this project). This directly asks the physics
// world "is anything in my path?" every step — no ambiguity, no silent failures.
public static class MovementBlocker
{
    public const int ObstacleLayer = 8; // a raw, unnamed Unity layer — no Editor setup needed

    public static Vector3 ClampMovement(Vector3 position, Vector3 velocity, float radius)
    {
        if (velocity.sqrMagnitude < 0.0001f) return velocity;

        float moveDistance = velocity.magnitude * Time.fixedDeltaTime + 0.05f;
        Vector3 direction = velocity.normalized;
        int mask = 1 << ObstacleLayer;

        if (Physics.SphereCast(position, radius, direction, out RaycastHit hit, moveDistance, mask))
            return Vector3.zero;

        return velocity;
    }
}