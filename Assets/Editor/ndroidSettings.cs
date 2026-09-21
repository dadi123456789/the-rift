using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static class AndroidSettings
{
    static AndroidSettings()
    {
        PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);
        PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64 | AndroidArchitecture.ARMv7;
        PlayerSettings.defaultInterfaceOrientation = UIOrientation.LandscapeLeft;
    }
}