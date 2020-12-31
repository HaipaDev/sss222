using JetBrains.Annotations;
using PatchKit.UnityEditorExtension.Core;
using UnityEditor;
using UnityEngine;

namespace PatchKit.UnityEditorExtension.UI
{
public class BuildAndUploadWindow : Window
{
    [MenuItem("Tools/PatchKit/Build and Upload %&#b", false, 51)]
    public static void ShowWindow()
    {
        GetWindow(typeof(BuildAndUploadWindow), false, "Build & Upload");
    }

    [SerializeField]
    private AppPlatform? _lastPlatform;

    [UsedImplicitly]
    private void Awake()
    {
        ResetView(AppBuild.Platform);
    }

    private void Update()
    {
        if (AppBuild.Platform != _lastPlatform)
        {
            ResetView(AppBuild.Platform);
        }
    }

    private void ResetView(AppPlatform? platform)
    {
        _lastPlatform = platform;

        if (!platform.HasValue)
        {
            ClearAndPush<UnsupportedPlatformScreen>().Initialize();
        }
        else
        {
            ClearAndPush<BuildAndUploadScreen>().Initialize(platform.Value);
        }
    }
}
}