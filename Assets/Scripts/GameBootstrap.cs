using UnityEngine;
using UnityEngine.SceneManagement;

public static class GameBootstrap
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void Init()
    {
        foreach (var go in SceneManager.GetActiveScene().GetRootGameObjects())
            Object.Destroy(go);

        BuildWorld();
    }

    static void BuildWorld()
    {
        var ground = GameObject.CreatePrimitive(PrimitiveType.Cube);
        ground.name = "Ground";
        ground.transform.position = new Vector3(0f, -0.5f, 0f);
        ground.transform.localScale = new Vector3(100f, 1f, 100f);
        ground.GetComponent<Renderer>().material.color = new Color(0.35f, 0.45f, 0.3f);

        var sunGO = new GameObject("Sun");
        var sun = sunGO.AddComponent<Light>();
        sun.type = LightType.Directional;
        sun.intensity = 1.1f;
        sunGO.transform.rotation = Quaternion.Euler(50f, -30f, 0f);

        var player = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        player.name = "Player";
        player.transform.position = new Vector3(0f, 1f, 0f);
        player.GetComponent<Renderer>().material.color = new Color(0.9f, 0.8f, 0.4f);
        var rb = player.AddComponent<Rigidbody>();
        rb.freezeRotation = true;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        var controller = player.AddComponent<IsometricPlayerController>();
        var combat = player.AddComponent<PlayerCombat>();
        var playerHealth = player.AddComponent<Health>();

        var enemy = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        enemy.name = "Enemy";
        enemy.transform.position = new Vector3(4f, 1f, 4f);
        enemy.GetComponent<Renderer>().material.color = new Color(0.6f, 0.1f, 0.1f);
        var enemyRb = enemy.AddComponent<Rigidbody>();
        enemyRb.freezeRotation = true;
        enemyRb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        enemy.AddComponent<Health>();
        enemy.AddComponent<SimpleEnemy>();

        var camGO = new GameObject("IsometricCamera");
        var cam = camGO.AddComponent<Camera>();
        cam.orthographic = true;
        cam.orthographicSize = 8f;
        cam.nearClipPlane = 0.3f;
        cam.backgroundColor = new Color(0.08f, 0.09f, 0.14f);
        cam.clearFlags = CameraClearFlags.SolidColor;
        var follow = camGO.AddComponent<IsometricCameraFollow>();
        follow.target = player.transform;

        UIBootstrap.BuildUI(controller, combat, playerHealth);
    }
}