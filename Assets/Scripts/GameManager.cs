public static class GameManager
{
    public static int Kills { get; private set; }
    public static int AliveEnemies { get; private set; }
    public static int Currency { get; private set; }
    public static System.Action<int> OnKillCountChanged;
    public static System.Action<int> OnCurrencyChanged;
    public static System.Action OnPlayerDied;
    public static System.Action OnWaveCleared;

    public static void RegisterKill()
    {
        Kills++;
        OnKillCountChanged?.Invoke(Kills);
    }

    public static void AddCurrency(int amount)
    {
        Currency += amount;
        OnCurrencyChanged?.Invoke(Currency);
    }

    public static void RegisterEnemySpawned() => AliveEnemies++;

    public static void RegisterEnemyDeath()
    {
        AliveEnemies--;
        if (AliveEnemies <= 0)
            OnWaveCleared?.Invoke();
    }

    public static void HandlePlayerDeath() => OnPlayerDied?.Invoke();

    public static void ResetGame()
    {
        Kills = 0;
        AliveEnemies = 0;
        Currency = 0;
        GameBootstrap.RequestRebuild();
    }

    public static void ClearListeners()
    {
        OnKillCountChanged = null;
        OnCurrencyChanged = null;
        OnPlayerDied = null;
        OnWaveCleared = null;
    }
}