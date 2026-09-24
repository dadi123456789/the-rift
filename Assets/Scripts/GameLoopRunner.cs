using UnityEngine;

// Safely schedules a rebuild for the NEXT frame, outside of any UI click
// callback stack — this is what fixes "Restart button breaks everything".
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