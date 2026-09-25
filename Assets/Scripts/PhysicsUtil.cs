using UnityEngine;

public static class PhysicsUtil
{
    // Predicts the next position BEFORE moving there — if it would overlap any
    // Obstacle, the movement for this step is cancelled entirely. This prevents
    // tunneling instead of trying to fix it after the fact.
    public static Vector3 BlockMovementIntoObstacles(Vector3 currentPosition, Vector3 horizontalVelocity, Quaternion rotation, Collider selfCollider)
    {
        if (selfCollider == null || horizontalVelocity.sqrMagnitude < 0.0001f)
            return horizontalVelocity;

        Vector3 predictedPosition = currentPosition + horizontalVelocity * Time.fixedDeltaTime;

        Physics.SyncTransforms();

        foreach (var obstacleCol in Obstacle.All)
        {
            if (obstacleCol == null) continue;

            Vector3 direction;
            float distance;
            bool wouldOverlap = Physics.ComputePenetration(
                selfCollider, predictedPosition, rotation,
                obstacleCol, obstacleCol.transform.position, obstacleCol.transform.rotation,
                out direction, out distance);

            if (wouldOverlap)
                return Vector3.zero; // hard stop this step — simple and bulletproof
        }

        return horizontalVelocity;
    }
}