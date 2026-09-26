using UnityEngine;
using UnityEngine.UI;

public class WallCheckIndicator : MonoBehaviour
{
    Image img;
    Transform player;

    void Awake()
    {
        img = GetComponent<Image>();
        var p = GameObject.Find("Player");
        if (p != null) player = p.transform;
    }

    void Update()
    {
        if (player == null) return;

        bool blocked = Physics.SphereCast(player.position, 0.55f, player.forward, out _, 1f, 1 << MovementBlocker.ObstacleLayer);
        img.color = blocked ? Color.red : Color.green;
    }

    public static void Build(Transform parent)
    {
        var go = new GameObject("WallCheckIndicator");
        go.transform.SetParent(parent, false);
        go.AddComponent<Image>().color = Color.green;
        var rect = go.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(60f, 60f);
        rect.anchorMin = new Vector2(0.5f, 1f);
        rect.anchorMax = new Vector2(0.5f, 1f);
        rect.pivot = new Vector2(0.5f, 1f);
        rect.anchoredPosition = new Vector2(0f, -40f);
        go.AddComponent<WallCheckIndicator>();
    }
}