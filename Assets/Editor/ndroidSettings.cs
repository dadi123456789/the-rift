using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static class AndroidSettings
{
    static AndroidSettings()
    {
        PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);
        PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64; // 64-bit only: fast single-arch build, no compatibility warning
        PlayerSettings.defaultInterfaceOrientation = UIOrientation.LandscapeLeft;
    }
}