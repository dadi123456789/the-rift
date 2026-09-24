using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    Transform target;
    Health health;
    RectTransform barFill;
    Canvas canvas;

    public void Setup(Transform followTarget, Health targetHealth)
    {
        target = followTarget;
        health = targetHealth;

        var canvasGO = new GameObject("EnemyHealthCanvas");
        canvasGO.transform.SetParent(transform, false);
        canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        canvasGO.transform.localScale = Vector3.one * 0.01f;
        var canvasRect = canvasGO.GetComponent<RectTransform>();
        canvasRect.sizeDelta = new Vector2(100f, 15f);

        var bgGO = new GameObject("BarBackground");
        bgGO.transform.SetParent(canvasGO.transform, false);
        bgGO.AddComponent<Image>().color = new Color(0f, 0f, 0f, 0.6f);
        var bgRect = bgGO.GetComponent<RectTransform>();
        bgRect.sizeDelta = new Vector2(100f, 15f);

        var fillGO = new GameObject("BarFill");
        fillGO.transform.SetParent(bgGO.transform, false);
        var fillImage = fillGO.AddComponent<Image>();
        fillImage.color = Color.red;
        barFill = fillGO.GetComponent<RectTransform>();
        barFill.anchorMin = Vector2.zero;
        barFill.anchorMax = Vector2.one;
        barFill.offsetMin = Vector2.zero;
        barFill.offsetMax = Vector2.zero;

        health.OnHealthChanged += (current, max) =>
        {
            barFill.localScale = new Vector3(current / max, 1f, 1f);
        };
    }

    void LateUpdate()
    {
        if (canvas == null) return;
        canvas.transform.position = transform.position + Vector3.up * 1.3f;
        if (Camera.main != null)
            canvas.transform.rotation = Camera.main.transform.rotation;
    }
}