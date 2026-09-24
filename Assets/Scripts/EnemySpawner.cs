using UnityEngine;

public static class EnemySpawner
{
    public static Transform Parent;

    public static void SpawnWave(int count, float radius)
    {
        for (int i = 0; i < count; i++)
        {
            float angle = (360f / count) * i * Mathf.Deg2Rad;
            Vector3 pos = new Vector3(Mathf.Cos(angle), 1f, Mathf.Sin(angle)) * radius;
            pos.y = 1f;
            SpawnEnemy(pos);
        }
    }

    static void SpawnEnemy(Vector3 position)
    {
        GameManager.RegisterEnemySpawned();

        var enemy = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        enemy.name = "Enemy";
        if (Parent != null) enemy.transform.SetParent(Parent);
        enemy.transform.position = position;
        enemy.GetComponent<Renderer>().material.color = new Color(0.6f, 0.1f, 0.1f);
        var rb = enemy.AddComponent<Rigidbody>();
        rb.freezeRotation = true;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        enemy.AddComponent<Health>();
        enemy.AddComponent<SimpleEnemy>();
    }
}