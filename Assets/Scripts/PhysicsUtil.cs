using UnityEngine;

public static class PhysicsUtil
{
    public static void DepenetrateFromObstacles(Rigidbody rb, Collider selfCollider)
    {
        if (selfCollider == null) return;

        Physics.SyncTransforms(); // force the physics engine to see our latest manual position writes

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
            {
                rb.position += direction * distance;
                Physics.SyncTransforms();
            }
        }
    }
}