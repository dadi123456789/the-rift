using UnityEditor;
using UnityEngine;

// Ensures every build targets modern 64-bit Android devices
// while remaining compatible with slightly older ones too.
[InitializeOnLoad]
public static class AndroidSettings
{
    static AndroidSettings()
    {
        PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);
        PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64 | AndroidArchitecture.ARMv7;
    }
}