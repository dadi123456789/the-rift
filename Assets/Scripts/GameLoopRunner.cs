using UnityEngine;

public class GameLoopRunner : MonoBehaviour
{
    public static GameLoopRunner Instance { get; private set; }
    bool rebuildPending;

    void Awake() => Instance = this;

    public void ScheduleRebuild() => rebuildPending = true;

    void LateUpdate()
    {
        if (rebuildPending)
        {
            rebuildPending = false;
            GameBootstrap.Rebuild();
        }
    }
}