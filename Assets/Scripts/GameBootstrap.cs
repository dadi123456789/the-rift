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

        // Fixed landmarks (do NOT move) so movement is visually obvious against them.
        var marker1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        marker1.name = "ReferenceMarker_Red";
        marker1.transform.position = new Vector3(6f, 0.5f, 0f);
        marker1.GetComponent<Renderer>().material.color = Color.red;

        var marker2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        marker2.name = "ReferenceMarker_Blue";
        marker2.transform.position = new Vector3(-6f, 0.5f, 6f);
        marker2.GetComponent<Renderer>().material.color = Color.blue;

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
        enemyRb.interpolation = RigidbodyInterpolation.Interpolate;
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