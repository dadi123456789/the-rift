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
        UIBootstrap.BuildJoystickUI();
    }

    static void BuildWorld()
    {
        var ground = GameObject.CreatePrimitive(PrimitiveType.Cube);
        ground.name = "Ground";
        ground.transform.position = new Vector3(0f, -0.5f, 0f);
        ground.transform.localScale = new Vector3(100f, 1f, 100f);
        ground.GetComponent<Renderer>().material.color = new Color(0.35f, 0.45f, 0.3f);

        // Reference marker so scale and camera framing are easy to judge visually.
        var marker = GameObject.CreatePrimitive(PrimitiveType.Cube);
        marker.name = "ReferenceMarker";
        marker.transform.position = new Vector3(3f, 0.5f, 3f);
        marker.GetComponent<Renderer>().material.color = Color.red;

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
        player.AddComponent<IsometricPlayerController>();

        var camGO = new GameObject("IsometricCamera");
        var cam = camGO.AddComponent<Camera>();
        cam.orthographic = true;
        cam.orthographicSize = 6f;
        cam.nearClipPlane = 0.3f;
        cam.backgroundColor = new Color(0.08f, 0.09f, 0.14f);
        cam.clearFlags = CameraClearFlags.SolidColor;
        var follow = camGO.AddComponent<IsometricCameraFollow>();
        follow.target = player.transform;
    }
}