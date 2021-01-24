using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using JetBrains.Annotations;
using UnityEditor;
#if UNITY_2018_1_OR_NEWER
using UnityEditor.Build.Reporting;
#endif
using UnityEngine.Assertions;

namespace PatchKit.UnityEditorExtension.Core
{
public static class AppBuild
{
    public static AppPlatform? Platform
    {
        get
        {
            switch (EditorUserBuildSettings.activeBuildTarget)
            {
                case BuildTarget.StandaloneWindows:
                    return AppPlatform.Windows32;

                case BuildTarget.StandaloneWindows64:
                    return AppPlatform.Windows64;

                case BuildTarget.StandaloneLinux:
                    return AppPlatform.Linux32;

                case BuildTarget.StandaloneLinux64:
                    return AppPlatform.Linux64;

#if UNITY_2017_3_OR_NEWER
                case BuildTarget.StandaloneOSX:
#else
                case BuildTarget.StandaloneOSXIntel64:
#endif
                    return AppPlatform.Mac64;

                default:
                    return null;
            }
        }
    }

    [NotNull]
    public static IEnumerable<string> Scenes
    {
        get
        {
            EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;

            Assert.IsNotNull(scenes);

            return scenes.Where(x => x != null).Select(s => s.path);
        }
    }

    [NotNull]
    private static readonly string[] WindowsPdbFiles =
    {
        "player_win_x86.pdb",
        "player_win_x86_s.pdb",
        "player_win_x64.pdb",
        "player_win_x64_s.pdb"
    };

    public static string Location
    {
        get
        {
            string value = EditorUserBuildSettings.GetBuildLocation(
                EditorUserBuildSettings.activeBuildTarget);

            string validationError = GetLocationValidationError(value);

            if (validationError == null)
            {
                return value;
            }

            return null;
        }
        set
        {
            string validationError = GetLocationValidationError(value);

            if (validationError != null)
            {
                throw new ValidationException(validationError);
            }

            EditorUserBuildSettings.SetBuildLocation(
                EditorUserBuildSettings.activeBuildTarget,
                value);
        }
    }

    private static bool AreThereOnlyBuildEntries(
        [NotNull] string location,
        params string[] buildFiles)
    {
        if (location == null)
        {
            throw new ArgumentNullException("location");
        }

        if (buildFiles == null)
        {
            throw new ArgumentNullException("buildFiles");
        }

        string parentDirPath = Path.GetDirectoryName(location);

        Assert.IsNotNull(parentDirPath);

        string[] entries = Directory.GetFileSystemEntries(parentDirPath, "*");

        return entries.All(
            x =>
            {
                string fileName = Path.GetFileName(x);

                if (Path.GetDirectoryName(x) != parentDirPath)
                {
                    return true;
                }

                if (buildFiles.Contains(fileName))
                {
                    return true;
                }

                if (Platform.IsWindows() && WindowsPdbFiles.Contains(fileName))
                {
                    return true;
                }

                return false;
            });
    }

    private static string GetLocationValidationError(string location)
    {
        if (location == null)
        {
            return "Build location cannot be null.";
        }

        if (string.IsNullOrEmpty(location))
        {
            return "Build location cannot be empty.";
        }

        switch (Platform)
        {
            case AppPlatform.Windows32:
            case AppPlatform.Windows64:
                if (!location.EndsWith(".exe"))
                {
                    return
                        "Invalid build location file extension. Should be .exe.";
                }

                string winBuildFileName = Path.GetFileName(location);
                string winBuildDirName =
                    winBuildFileName.Replace(".exe", "_Data");

                if (!AreThereOnlyBuildEntries(
                    location,
                    winBuildFileName,
                    winBuildDirName,
                    "MonoBleedingEdge",
                    "Mono",
                    "UnityCrashHandler32.exe",
                    "UnityCrashHandler64.exe",
                    "UnityPlayer.dll",
                    "WinPixEventRuntime.dll"))
                {
                    return "Build location must be an empty directory.";
                }

                break;
            case AppPlatform.Linux32:
            case AppPlatform.Linux64:
                break;
            case AppPlatform.Mac64:
                if (!location.EndsWith(".app"))
                {
                    return
                        "Invalid build location file extension. Should be .app.";
                }

                string macBuildFileName = Path.GetFileName(location);

                if (!AreThereOnlyBuildEntries(location, macBuildFileName))
                {
                    return "Build location must be an empty directory.";
                }

                break;
            case null:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        return null;
    }

    public static bool TryCreate()
    {
        bool success;
#if UNITY_2018_1_OR_NEWER
        BuildReport report = BuildPipeline.BuildPlayer(
            Scenes.ToArray(),
            Location,
            EditorUserBuildSettings.activeBuildTarget,
            BuildOptions.None);

        Assert.IsNotNull(report);

        success = report.summary.result == BuildResult.Succeeded;
#else
        success = string.IsNullOrEmpty(
            BuildPipeline.BuildPlayer(
                Scenes.ToArray(),
                Location,
                EditorUserBuildSettings.activeBuildTarget,
                BuildOptions.None));
#endif

        if (success)
        {
            RemovePdbFiles();
        }

        return success;
    }

    private static void RemovePdbFiles()
    {
        switch (Platform)
        {
            case AppPlatform.Windows32:
            case AppPlatform.Windows64:
                string parentDirPath = Path.GetDirectoryName(Location);
                Assert.IsNotNull(parentDirPath);

                foreach (string pdbFile in WindowsPdbFiles)
                {
                    string pdbFilePath = Path.Combine(parentDirPath, pdbFile);

                    if (File.Exists(pdbFilePath))
                    {
                        File.Delete(pdbFilePath);
                    }
                }

                break;
            case AppPlatform.Linux32:
                break;
            case AppPlatform.Linux64:
                break;
            case AppPlatform.Mac64:
                break;
            case null:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public static void OpenLocationDialog()
    {
        string extension = string.Empty;

        switch (Platform)
        {
            case AppPlatform.Windows32:
            case AppPlatform.Windows64:
                extension = "exe";
                break;
            case AppPlatform.Linux32:
            case AppPlatform.Linux64:
                break;
            case AppPlatform.Mac64:
                extension = "app";
                break;
            case null:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        bool retry;

        do
        {
            retry = false;

            string location = EditorUtility.SaveFilePanel(
                "Select build location:",
                "",
                "",
                extension);

            if (!string.IsNullOrEmpty(location))
            {
                string validationError = GetLocationValidationError(location);

                if (validationError == null)
                {
                    Location = location;
                }
                else
                {
                    EditorUtility.DisplayDialog("Error", validationError, "Ok");
                    retry = true;
                }
            }
        } while (retry);
    }
}
}