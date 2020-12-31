using System;
using JetBrains.Annotations;

namespace PatchKit.UnityEditorExtension.Core
{
public static class AppPlatformExtensions
{
    [NotNull]
    public static string ToApiString(this AppPlatform appPlatform)
    {
        switch (appPlatform)
        {
            case AppPlatform.Windows32:
                return "windows_x86";
            case AppPlatform.Windows64:
                return "windows_x86_64";
            case AppPlatform.Linux32:
                return "linux_x86";
            case AppPlatform.Linux64:
                return "linux_x86_64";
            case AppPlatform.Mac64:
                return "mac_x86_64";
            default:
                throw new ArgumentOutOfRangeException(
                    "appPlatform",
                    appPlatform,
                    null);
        }
    }

    [NotNull]
    public static string ToDisplayString(this AppPlatform @this)
    {
        switch (@this)
        {
            case AppPlatform.Windows32:
                return "Windows 32-bit";
            case AppPlatform.Windows64:
                return "Windows 64-bit";
            case AppPlatform.Linux32:
                return "Linux 32-bit";
            case AppPlatform.Linux64:
                return "Linux 64-bit";
            case AppPlatform.Mac64:
                return "Mac OSX 64-bit";
            default:
                throw new ArgumentOutOfRangeException("this", @this, null);
        }
    }

    public static bool IsWindows(this AppPlatform @this)
    {
        switch (@this)
        {
            case AppPlatform.Windows32:
            case AppPlatform.Windows64:
                return true;
            case AppPlatform.Linux32:
            case AppPlatform.Linux64:
            case AppPlatform.Mac64:
                return false;
            default:
                throw new ArgumentOutOfRangeException("this", @this, null);
        }
    }

    public static bool IsWindows(this AppPlatform? @this)
    {
        switch (@this)
        {
            case AppPlatform.Windows32:
            case AppPlatform.Windows64:
                return true;
            case AppPlatform.Linux32:
            case AppPlatform.Linux64:
            case AppPlatform.Mac64:
                return false;
            default:
                throw new ArgumentOutOfRangeException("this", @this, null);
        }
    }
}
}