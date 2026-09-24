using UnityEngine.SceneManagement;

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

    public static void ResetGame()
    {
        Kills = 0;
        AliveEnemies = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public static void ClearListeners()
    {
        OnKillCountChanged = null;
        OnPlayerDied = null;
        OnWaveCleared = null;
    }
}