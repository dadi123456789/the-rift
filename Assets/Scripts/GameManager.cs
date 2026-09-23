using UnityEngine.SceneManagement;

public static class GameManager
{
    public static int Kills { get; private set; }
    public static System.Action<int> OnKillCountChanged;
    public static System.Action OnPlayerDied;

    public static void RegisterKill()
    {
        Kills++;
        OnKillCountChanged?.Invoke(Kills);
    }

    public static void HandlePlayerDeath() => OnPlayerDied?.Invoke();

    public static void ResetGame()
    {
        Kills = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public static void ClearListeners()
    {
        OnKillCountChanged = null;
        OnPlayerDied = null;
    }
}