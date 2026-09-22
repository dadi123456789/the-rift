using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public static class UIBootstrap
{
    public static void BuildJoystickUI()
    {
        var esGO = new GameObject("EventSystem");
        esGO.AddComponent<EventSystem>();
        esGO.AddComponent<StandaloneInputModule>();

        var canvasGO = new GameObject("Canvas");
        var canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        var scaler = canvasGO.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        canvasGO.AddComponent<GraphicRaycaster>();

        var bgGO = new GameObject("JoystickBackground");
        bgGO.transform.SetParent(canvasGO.transform, false);
        var bgImage = bgGO.AddComponent<Image>();
        bgImage.color = new Color(1f, 1f, 1f, 0.25f);
        var bgRect = bgGO.GetComponent<RectTransform>();
        bgRect.sizeDelta = new Vector2(220f, 220f);
        bgRect.anchorMin = new Vector2(0f, 0f);
        bgRect.anchorMax = new Vector2(0f, 0f);
        bgRect.pivot = new Vector2(0.5f, 0.5f);
        bgRect.anchoredPosition = new Vector2(160f, 160f);

        var handleGO = new GameObject("JoystickHandle");
        handleGO.transform.SetParent(bgGO.transform, false);
        var handleImage = handleGO.AddComponent<Image>();
        handleImage.color = new Color(1f, 1f, 1f, 0.6f);
        var handleRect = handleGO.GetComponent<RectTransform>();
        handleRect.sizeDelta = new Vector2(100f, 100f);
        handleRect.anchoredPosition = Vector2.zero;

        var joystick = bgGO.AddComponent<VirtualJoystick>();
        var fields = typeof(VirtualJoystick).GetFields(
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        foreach (var f in fields)
        {
            if (f.Name == "background") f.SetValue(joystick, bgRect);
            if (f.Name == "handle") f.SetValue(joystick, handleRect);
        }
    }
}