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
        BuildRangedButton(canvasGO.transform, combat);
        BuildWeaponSwitchButton(canvasGO.transform, combat);
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

    static void BuildRangedButton(Transform parent, PlayerCombat combat)
    {
        var btnGO = new GameObject("RangedButton");
        btnGO.transform.SetParent(parent, false);
        btnGO.AddComponent<Image>().color = new Color(0.9f, 0.7f, 0.2f, 0.7f);
        var rect = btnGO.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(150f, 150f);
        rect.anchorMin = new Vector2(1f, 0f);
        rect.anchorMax = new Vector2(1f, 0f);
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = new Vector2(-500f, 160f);

        btnGO.AddComponent<Button>().onClick.AddListener(combat.RequestRangedAttack);
    }

        static void BuildWeaponSwitchButton(Transform parent, PlayerCombat combat)
    {
        var btnGO = new GameObject("SwitchWeaponButton");
        btnGO.transform.SetParent(parent, false);
        btnGO.AddComponent<Image>().color = new Color(0.6f, 0.6f, 0.9f, 0.7f);
        var rect = btnGO.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(120f, 120f);
        rect.anchorMin = new Vector2(1f, 1f);
        rect.anchorMax = new Vector2(1f, 1f);
        rect.pivot = new Vector2(1f, 1f);
        rect.anchoredPosition = new Vector2(-30f, -100f);

        bool usingSword = false;
        btnGO.AddComponent<Button>().onClick.AddListener(() =>
        {
            usingSword = !usingSword;
            combat.EquipWeapon(usingSword ? WeaponLibrary.Sword : WeaponLibrary.Fists);
        });
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
    fillGO.AddComponent<Image>().color = new Color(0.85f, 0.2f, 0.2f, 0.95f);
    var fillRect = fillGO.GetComponent<RectTransform>();
    // Width-based shrink via anchors — reliable, doesn't depend on any sprite.
    fillRect.anchorMin = new Vector2(0f, 0f);
    fillRect.anchorMax = new Vector2(1f, 1f);
    fillRect.offsetMin = Vector2.zero;
    fillRect.offsetMax = Vector2.zero;

    if (playerHealth != null)
        playerHealth.OnHealthChanged += (current, max) =>
            fillRect.anchorMax = new Vector2(Mathf.Clamp01(current / max), 1f);
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
        panelGO.AddComponent<Image>().color = new Color(0.05f, 0f, 0f, 0.92f);
        var panelRect = panelGO.GetComponent<RectTransform>();
        panelRect.anchorMin = Vector2.zero;
        panelRect.anchorMax = Vector2.one;
        panelRect.offsetMin = Vector2.zero;
        panelRect.offsetMax = Vector2.zero;
        panelGO.AddComponent<CanvasGroup>();
        panelGO.AddComponent<FadeInOnEnable>();
        panelGO.SetActive(false);

        BuildShard(panelGO.transform, 45f);
        BuildShard(panelGO.transform, -45f);

        // Banner: gold border + dark red fill — matches The Rift's signature color identity.
        var borderGO = new GameObject("BannerBorder");
        borderGO.transform.SetParent(panelGO.transform, false);
        borderGO.AddComponent<Image>().color = new Color(0.95f, 0.78f, 0.35f, 1f);
        var borderRect = borderGO.GetComponent<RectTransform>();
        borderRect.anchorMin = new Vector2(0.5f, 0.5f);
        borderRect.anchorMax = new Vector2(0.5f, 0.5f);
        borderRect.sizeDelta = new Vector2(520f, 120f);
        borderRect.anchoredPosition = new Vector2(0f, 60f);

        var bannerGO = new GameObject("BannerFill");
        bannerGO.transform.SetParent(borderGO.transform, false);
        bannerGO.AddComponent<Image>().color = new Color(0.5f, 0.07f, 0.07f, 1f);
        var bannerRect = bannerGO.GetComponent<RectTransform>();
        bannerRect.anchorMin = Vector2.zero;
        bannerRect.anchorMax = Vector2.one;
        bannerRect.offsetMin = new Vector2(6f, 6f);
        bannerRect.offsetMax = new Vector2(-6f, -6f);

        // Restart button: same gold-border treatment, green fill.
        var restartBorderGO = new GameObject("RestartBorder");
        restartBorderGO.transform.SetParent(panelGO.transform, false);
        restartBorderGO.AddComponent<Image>().color = new Color(0.95f, 0.78f, 0.35f, 1f);
        var restartBorderRect = restartBorderGO.GetComponent<RectTransform>();
        restartBorderRect.anchorMin = new Vector2(0.5f, 0.5f);
        restartBorderRect.anchorMax = new Vector2(0.5f, 0.5f);
        restartBorderRect.sizeDelta = new Vector2(220f, 90f);
        restartBorderRect.anchoredPosition = new Vector2(0f, -70f);

        var restartFillGO = new GameObject("RestartFill");
        restartFillGO.transform.SetParent(restartBorderGO.transform, false);
        restartFillGO.AddComponent<Image>().color = new Color(0.2f, 0.75f, 0.35f, 1f);
        var restartFillRect = restartFillGO.GetComponent<RectTransform>();
        restartFillRect.anchorMin = Vector2.zero;
        restartFillRect.anchorMax = Vector2.one;
        restartFillRect.offsetMin = new Vector2(5f, 5f);
        restartFillRect.offsetMax = new Vector2(-5f, -5f);

        restartBorderGO.AddComponent<Button>().onClick.AddListener(GameManager.ResetGame);
        GameManager.OnPlayerDied += () => panelGO.SetActive(true);
    }

    static void BuildShard(Transform parent, float rotationZ)
    {
        var go = new GameObject("Shard");
        go.transform.SetParent(parent, false);
        go.AddComponent<Image>().color = new Color(0.95f, 0.78f, 0.35f, 0.9f);
        var rect = go.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.sizeDelta = new Vector2(140f, 10f);
        rect.anchoredPosition = new Vector2(0f, 160f);
        rect.localRotation = Quaternion.Euler(0f, 0f, rotationZ);
    }
}