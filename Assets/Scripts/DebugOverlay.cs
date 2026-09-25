using UnityEngine;
using UnityEngine.UI;

public class DebugOverlay : MonoBehaviour
{
    public static int ProjectileHits = 0;
    public static int ProjectilesFired = 0;
    Text text;

    void Awake() => text = GetComponent<Text>();

    void Update()
    {
        text.text = $"Fired: {ProjectilesFired}  Hits: {ProjectileHits}  Obstacles: {Obstacle.All.Count}";
    }

    public static void Build(Transform parent)
    {
        var go = new GameObject("DebugOverlay");
        go.transform.SetParent(parent, false);
        var text = go.AddComponent<Text>();
        text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        text.fontSize = 28;
        text.color = Color.cyan;
        text.alignment = TextAnchor.LowerLeft;
        var rect = go.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0f, 0f);
        rect.anchorMax = new Vector2(0f, 0f);
        rect.pivot = new Vector2(0f, 0f);
        rect.sizeDelta = new Vector2(600f, 60f);
        rect.anchoredPosition = new Vector2(30f, 200f);
        go.AddComponent<DebugOverlay>();
    }
}