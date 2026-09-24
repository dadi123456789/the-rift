public static class GameManager
{
    public static int Kills { get; private set; }
    public static int AliveEnemies { get; private set; }
    public static System.Action<int> OnKillCountChanged;
    public static System.Action OnPlayerDied;
    public static System.Action OnWaveCleared;

    public static void RegisterKill()
    {
        Kills++;
        OnKillCountChanged?.Invoke(Kills);
    }

    public static void RegisterEnemySpawned() => AliveEnemies++;

    public static void RegisterEnemyDeath()
    {
        AliveEnemies--;
        if (AliveEnemies <= 0)
            OnWaveCleared?.Invoke();
    }

    public static void HandlePlayerDeath() => OnPlayerDied?.Invoke();

    // No scene reload — rebuild everything procedurally instead,
    // so leftover template objects saved in the scene file can never resurface.
    public static void ResetGame()
    {
        Kills = 0;
        AliveEnemies = 0;
        GameBootstrap.Rebuild();
    }

    public static void ClearListeners()
    {
        OnKillCountChanged = null;
        OnPlayerDied = null;
        OnWaveCleared = null;
    }
}