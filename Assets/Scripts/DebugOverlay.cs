using UnityEngine;
using UnityEngine.UI;

public static class DebugOverlay
{
    public static int ProjectilesFired = 0;
    public static int ProjectileHits = 0;
    static RectTransform firedFill;
    static RectTransform hitsFill;
    const int MaxDisplay = 10;

    public static void Build(Transform parent)
    {
        firedFill = BuildBar(parent, new Vector2(30f, 260f), new Color(0.7f, 0.7f, 0.7f, 0.9f));
        hitsFill = BuildBar(parent, new Vector2(30f, 230f), new Color(0.3f, 0.9f, 0.3f, 0.9f));
    }

    static RectTransform BuildBar(Transform parent, Vector2 pos, Color color)
    {
        var bgGO = new GameObject("DebugBarBG");
        bgGO.transform.SetParent(parent, false);
        bgGO.AddComponent<Image>().color = new Color(0f, 0f, 0f, 0.4f);
        var bgRect = bgGO.GetComponent<RectTransform>();
        bgRect.sizeDelta = new Vector2(200f, 20f);
        bgRect.anchorMin = new Vector2(0f, 0f);
        bgRect.anchorMax = new Vector2(0f, 0f);
        bgRect.pivot = new Vector2(0f, 0f);
        bgRect.anchoredPosition = pos;

        var fillGO = new GameObject("DebugBarFill");
        fillGO.transform.SetParent(bgGO.transform, false);
        fillGO.AddComponent<Image>().color = color;
        var fillRect = fillGO.GetComponent<RectTransform>();
        fillRect.anchorMin = Vector2.zero;
        fillRect.anchorMax = new Vector2(0f, 1f);
        fillRect.offsetMin = Vector2.zero;
        fillRect.offsetMax = Vector2.zero;
        return fillRect;
    }

    public static void RegisterFired()
    {
        ProjectilesFired++;
        if (firedFill != null)
            firedFill.anchorMax = new Vector2(Mathf.Clamp01((float)ProjectilesFired / MaxDisplay), 1f);
    }

    public static void RegisterHit()
    {
        ProjectileHits++;
        if (hitsFill != null)
            hitsFill.anchorMax = new Vector2(Mathf.Clamp01((float)ProjectileHits / MaxDisplay), 1f);
    }
}