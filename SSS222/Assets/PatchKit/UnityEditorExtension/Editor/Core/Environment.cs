using System;
using UnityEngine;

namespace PatchKit.UnityEditorExtension.Core
{
public static class Environment
{
    public static EnvironmentPlatform Platform
    {
        get
        {
            switch (Application.platform)
            {
                case RuntimePlatform.WindowsEditor:
                    return EnvironmentPlatform.Windows;
                case RuntimePlatform.OSXEditor:
                    return EnvironmentPlatform.Mac;
#if UNITY_5_5_OR_NEWER || UNITY_EDITOR_LINUX
                case RuntimePlatform.LinuxEditor:
                    return EnvironmentPlatform.Linux;
#endif
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
}