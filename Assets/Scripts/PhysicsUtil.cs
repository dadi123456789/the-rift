using UnityEngine;

public static class PhysicsUtil
{
    // Manually guarantees a body can never end up inside a registered Obstacle,
    // regardless of any PhysX edge case.
    public static void DepenetrateFromObstacles(Rigidbody rb, Collider selfCollider)
    {
        if (selfCollider == null) return;

        foreach (var obstacleCol in Obstacle.All)
        {
            if (obstacleCol == null) continue;

            Vector3 direction;
            float distance;
            bool overlapping = Physics.ComputePenetration(
                selfCollider, rb.position, rb.rotation,
                obstacleCol, obstacleCol.transform.position, obstacleCol.transform.rotation,
                out direction, out distance);

            if (overlapping)
                rb.position += direction * distance;
        }
    }
}