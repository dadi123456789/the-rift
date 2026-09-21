using UnityEngine;
using UnityEngine.SceneManagement;

/// Runs automatically the moment the game starts.
/// Wipes any leftover demo content and builds our world from scratch.
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
        // Ground
        var ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
        ground.name = "Ground";
        ground.transform.localScale = new Vector3(10f, 1f, 10f);

        // Sun
        var sunGO = new GameObject("Sun");
        var sun = sunGO.AddComponent<Light>();
        sun.type = LightType.Directional;
        sun.intensity = 1.1f;
        sunGO.transform.rotation = Quaternion.Euler(50f, -30f, 0f);

        // Player
        var player = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        player.name = "Player";
        player.transform.position = new Vector3(0f, 1f, 0f);
        var rb = player.AddComponent<Rigidbody>();
        rb.freezeRotation = true;
        player.AddComponent<IsometricPlayerController>();

        // Isometric camera
        var camGO = new GameObject("IsometricCamera");
        var cam = camGO.AddComponent<Camera>();
        cam.nearClipPlane = 0.3f;
        var follow = camGO.AddComponent<IsometricCameraFollow>();
        follow.target = player.transform;
    }
}