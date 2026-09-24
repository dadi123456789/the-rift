using UnityEngine;
using UnityEngine.SceneManagement;

public static class GameBootstrap
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void Init()
    {
        GameManager.ClearListeners();

        // Destroy everything in the current scene IMMEDIATELY (not deferred),
        // so no leftover physics/collision from the old template can run
        // even for a single frame.
        foreach (var go in SceneManager.GetActiveScene().GetRootGameObjects())
            Object.DestroyImmediate(go);

        // ...AND any leftover DontDestroyOnLoad objects from the original template
        // (this is what was causing the old "A Winner Is You" screen to survive).
        foreach (var go in Resources.FindObjectsOfTypeAll<GameObject>())
        {
            if (go.scene.IsValid() && go.scene.name == "DontDestroyOnLoad" && go.transform.parent == null)
                Object.DestroyImmediate(go);
        }
        BuildWorld();
    }

    static Texture2D BuildGridTexture()
    {
        int size = 64;
        var tex = new Texture2D(size, size);
        Color a = new Color(0.35f, 0.45f, 0.3f);
        Color b = new Color(0.32f, 0.42f, 0.27f);
        for (int y = 0; y < size; y++)
            for (int x = 0; x < size; x++)
                tex.SetPixel(x, y, (x < size / 2) == (y < size / 2) ? a : b);
        tex.filterMode = FilterMode.Point;
        tex.Apply();
        return tex;
    }

    static void BuildWorld()
    {
        var ground = GameObject.CreatePrimitive(PrimitiveType.Cube);
        ground.name = "Ground";
        ground.transform.position = new Vector3(0f, -0.5f, 0f);
        ground.transform.localScale = new Vector3(100f, 1f, 100f);
        var groundMat = ground.GetComponent<Renderer>().material;
        groundMat.mainTexture = BuildGridTexture();
        groundMat.mainTextureScale = new Vector2(50f, 50f);

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
        playerHealth.OnDeath += GameManager.HandlePlayerDeath;

        EnemySpawner.SpawnWave(4, 7f);

        var camGO = new GameObject("IsometricCamera");
        var cam = camGO.AddComponent<Camera>();
        cam.orthographic = true;
        cam.orthographicSize = 10f;
        cam.nearClipPlane = 0.3f;
        cam.backgroundColor = new Color(0.08f, 0.09f, 0.14f);
        cam.clearFlags = CameraClearFlags.SolidColor;
        var follow = camGO.AddComponent<IsometricCameraFollow>();
        follow.target = player.transform;

        UIBootstrap.BuildUI(controller, combat, playerHealth);
    }
}