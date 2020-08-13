using System;
using System.Linq;
using JetBrains.Annotations;
using PatchKit.UnityEditorExtension.Core;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;
using Object = UnityEngine.Object;

namespace PatchKit.UnityEditorExtension.Tools
{
#if PATCHKIT_UNITY_EDITOR_EXTENSION_DEV
[CreateAssetMenuAttribute]
#endif
public class PatchKitToolsPackages : ScriptableObject
{
    [NotNull]
    private static PatchKitToolsPackages FindInstance()
    {
        string[] guids =
            AssetDatabase.FindAssets("t:" + typeof(PatchKitToolsPackages).Name);

        PluginAssert.IsNotNull(guids);

        PatchKitToolsPackages[] instances = guids
            .Select(x => AssetDatabase.GUIDToAssetPath(x))
            .Select(
                x => AssetDatabase.LoadAssetAtPath<PatchKitToolsPackages>(x))
            .Where(x => x != null)
            .ToArray();

        PluginAssert.AreEqual(1, instances.Length);
        PluginAssert.IsNotNull(instances[0]);
        PluginAssert.IsNotNull(instances[0]._windows32);
        PluginAssert.IsNotNull(instances[0]._windows64);
        PluginAssert.IsNotNull(instances[0]._linux32);
        PluginAssert.IsNotNull(instances[0]._mac64);

        return instances[0];
    }

// Disable warning in Unity console about not assigning private value
#pragma warning disable 0649

#if PATCHKIT_UNITY_EDITOR_EXTENSION_DEV
#else
    [HideInInspector]
#endif
    [SerializeField]
    private Object _windows32;

#if PATCHKIT_UNITY_EDITOR_EXTENSION_DEV
#else
    [HideInInspector]
#endif
    [SerializeField]
    private Object _windows64;

#if PATCHKIT_UNITY_EDITOR_EXTENSION_DEV
#else
    [HideInInspector]
#endif
    [SerializeField]
    private Object _linux32;

#if PATCHKIT_UNITY_EDITOR_EXTENSION_DEV
#else
    [HideInInspector]
#endif
    [SerializeField]
    private Object _linux64;

#if PATCHKIT_UNITY_EDITOR_EXTENSION_DEV
#else
    [HideInInspector]
#endif
    [SerializeField]
    private Object _mac64;

#pragma warning restore 0649

    [NotNull]
    public static string GetPath(EnvironmentPlatform platform)
    {
        switch (platform)
        {
            case EnvironmentPlatform.Windows:
                return Windows32Path;
            case EnvironmentPlatform.Linux:
                return Linux64Path;
            case EnvironmentPlatform.Mac:
                return Mac64Path;
            default:
                throw new ArgumentOutOfRangeException(
                    "platform",
                    platform,
                    null);
        }
    }

    [NotNull]
    public static string Windows32Path
    {
        get { return GetPath(FindInstance()._windows32); }
    }

    [NotNull]
    public static string Windows64Path
    {
        get { return GetPath(FindInstance()._windows64); }
    }

    [NotNull]
    public static string Linux32Path
    {
        get { return GetPath(FindInstance()._linux32); }
    }

    [NotNull]
    public static string Linux64Path
    {
        get { return GetPath(FindInstance()._linux64); }
    }

    [NotNull]
    public static string Mac64Path
    {
        get { return GetPath(FindInstance()._mac64); }
    }

    [NotNull]
    private static string GetPath(Object obj)
    {
        PluginAssert.IsNotNull(obj);

        string result = AssetDatabase.GetAssetPath(obj);
        Assert.IsNotNull(result);
        return result;
    }
}
}