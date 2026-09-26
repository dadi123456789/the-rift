using UnityEngine;
using UnityEngine.UI;

public static class BuildStamp
{
    // I will increase this number in every future code drop.
    // If this bar's width doesn't change after you install a "new" APK,
    // you are testing a stale build — not a code problem.
    public const int Version = 1;

    public static void Build(Transform parent)
    {
        var bgGO = new GameObject("BuildStampBG");
        bgGO.transform.SetParent(parent, false);
        bgGO.AddComponent<Image>().color = new Color(0f, 0f, 0f, 0.4f);
        var bgRect = bgGO.GetComponent<RectTransform>();
        bgRect.sizeDelta = new Vector2(400f, 20f);
        bgRect.anchorMin = new Vector2(0.5f, 1f);
        bgRect.anchorMax = new Vector2(0.5f, 1f);
        bgRect.pivot = new Vector2(0.5f, 1f);
        bgRect.anchoredPosition = new Vector2(0f, -10f);

        var fillGO = new GameObject("BuildStampFill");
        fillGO.transform.SetParent(bgGO.transform, false);
        fillGO.AddComponent<Image>().color = new Color(0.9f, 0.3f, 0.9f, 1f); // bright magenta — impossible to miss
        var fillRect = fillGO.GetComponent<RectTransform>();
        fillRect.anchorMin = Vector2.zero;
        fillRect.anchorMax = new Vector2(Mathf.Clamp01(Version / 20f), 1f);
        fillRect.offsetMin = Vector2.zero;
        fillRect.offsetMax = Vector2.zero;
    }
}