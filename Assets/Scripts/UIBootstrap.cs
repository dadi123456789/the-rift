using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public static class UIBootstrap
{
    public static void BuildUI(IsometricPlayerController player, PlayerCombat combat, Health playerHealth)
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

        BuildJoystick(canvasGO.transform);
        BuildJumpButton(canvasGO.transform, player);
        BuildAttackButton(canvasGO.transform, combat);
        BuildHealthBar(canvasGO.transform, playerHealth);
        BuildKillCounter(canvasGO.transform);
        BuildGameOverPanel(canvasGO.transform);
    }

    static void BuildJoystick(Transform parent)
    {
        var bgGO = new GameObject("JoystickBackground");
        bgGO.transform.SetParent(parent, false);
        bgGO.AddComponent<Image>().color = new Color(1f, 1f, 1f, 0.25f);
        var bgRect = bgGO.GetComponent<RectTransform>();
        bgRect.sizeDelta = new Vector2(220f, 220f);
        bgRect.anchorMin = new Vector2(0f, 0f);
        bgRect.anchorMax = new Vector2(0f, 0f);
        bgRect.pivot = new Vector2(0.5f, 0.5f);
        bgRect.anchoredPosition = new Vector2(160f, 160f);

        var handleGO = new GameObject("JoystickHandle");
        handleGO.transform.SetParent(bgGO.transform, false);
        handleGO.AddComponent<Image>().color = new Color(1f, 1f, 1f, 0.6f);
        var handleRect = handleGO.GetComponent<RectTransform>();
        handleRect.sizeDelta = new Vector2(100f, 100f);
        handleRect.anchoredPosition = Vector2.zero;

        var joystick = bgGO.AddComponent<VirtualJoystick>();
        joystick.Initialize(bgRect, handleRect);
    }

    static void BuildJumpButton(Transform parent, IsometricPlayerController player)
    {
        var btnGO = new GameObject("JumpButton");
        btnGO.transform.SetParent(parent, false);
        btnGO.AddComponent<Image>().color = new Color(1f, 0.85f, 0.4f, 0.7f);
        var rect = btnGO.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(150f, 150f);
        rect.anchorMin = new Vector2(1f, 0f);
        rect.anchorMax = new Vector2(1f, 0f);
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = new Vector2(-140f, 160f);

        btnGO.AddComponent<Button>().onClick.AddListener(player.RequestJump);
    }

    static void BuildAttackButton(Transform parent, PlayerCombat combat)
    {
        var btnGO = new GameObject("AttackButton");
        btnGO.transform.SetParent(parent, false);
        btnGO.AddComponent<Image>().color = new Color(0.8f, 0.2f, 0.2f, 0.7f);
        var rect = btnGO.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(150f, 150f);
        rect.anchorMin = new Vector2(1f, 0f);
        rect.anchorMax = new Vector2(1f, 0f);
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = new Vector2(-320f, 160f);

        btnGO.AddComponent<Button>().onClick.AddListener(combat.RequestAttack);
    }

    static void BuildHealthBar(Transform parent, Health playerHealth)
    {
        var bgGO = new GameObject("HealthBarBackground");
        bgGO.transform.SetParent(parent, false);
        bgGO.AddComponent<Image>().color = new Color(0f, 0f, 0f, 0.5f);
        var bgRect = bgGO.GetComponent<RectTransform>();
        bgRect.sizeDelta = new Vector2(300f, 30f);
        bgRect.anchorMin = new Vector2(0f, 1f);
        bgRect.anchorMax = new Vector2(0f, 1f);
        bgRect.pivot = new Vector2(0f, 1f);
        bgRect.anchoredPosition = new Vector2(30f, -30f);

        var fillGO = new GameObject("HealthBarFill");
        fillGO.transform.SetParent(bgGO.transform, false);
        var fillImage = fillGO.AddComponent<Image>();
        fillImage.color = new Color(0.8f, 0.2f, 0.2f, 0.9f);
        fillImage.type = Image.Type.Filled;
        fillImage.fillMethod = Image.FillMethod.Horizontal;
        fillImage.fillAmount = 1f;
        var fillRect = fillGO.GetComponent<RectTransform>();
        fillRect.anchorMin = Vector2.zero;
        fillRect.anchorMax = Vector2.one;
        fillRect.offsetMin = Vector2.zero;
        fillRect.offsetMax = Vector2.zero;

        if (playerHealth != null)
            playerHealth.OnHealthChanged += (current, max) => fillImage.fillAmount = current / max;
    }

    static void BuildKillCounter(Transform parent)
    {
        var textGO = new GameObject("KillCounterText");
        textGO.transform.SetParent(parent, false);
        var text = textGO.AddComponent<Text>();
        text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        text.fontSize = 36;
        text.color = Color.white;
        text.alignment = TextAnchor.UpperRight;
        text.text = "Kills: 0";
        var rect = textGO.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(1f, 1f);
        rect.anchorMax = new Vector2(1f, 1f);
        rect.pivot = new Vector2(1f, 1f);
        rect.sizeDelta = new Vector2(300f, 50f);
        rect.anchoredPosition = new Vector2(-30f, -30f);

        GameManager.OnKillCountChanged += (count) => text.text = $"Kills: {count}";
    }

    static void BuildGameOverPanel(Transform parent)
    {
        var panelGO = new GameObject("GameOverPanel");
        panelGO.transform.SetParent(parent, false);
        panelGO.AddComponent<Image>().color = new Color(0f, 0f, 0f, 0.8f);
        var panelRect = panelGO.GetComponent<RectTransform>();
        panelRect.anchorMin = Vector2.zero;
        panelRect.anchorMax = Vector2.one;
        panelRect.offsetMin = Vector2.zero;
        panelRect.offsetMax = Vector2.zero;
        panelGO.SetActive(false);

        var textGO = new GameObject("GameOverText");
        textGO.transform.SetParent(panelGO.transform, false);
        var text = textGO.AddComponent<Text>();
        text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        text.fontSize = 60;
        text.color = Color.white;
        text.alignment = TextAnchor.MiddleCenter;
        text.text = "GAME OVER\nTap to Restart";
        var textRect = textGO.GetComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = Vector2.zero;
        textRect.offsetMax = Vector2.zero;

        panelGO.AddComponent<Button>().onClick.AddListener(GameManager.ResetGame);
        GameManager.OnPlayerDied += () => panelGO.SetActive(true);
    }
}