using System;
using System.Diagnostics;
using System.IO;
using JetBrains.Annotations;
using UnityEngine.Assertions;
using Debug = UnityEngine.Debug;

namespace PatchKit.UnityEditorExtension.Core
{
public static class FileSystem
{
    public static void AddFileExecutablePermissions(
        [NotNull] string filePath)
    {
        if (filePath == null)
        {
            throw new ArgumentNullException("filePath");
        }

        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException("File not found", filePath);
        }

        if (Environment.Platform == EnvironmentPlatform.Windows)
        {
            return;
        }

        var processInfo = new ProcessStartInfo
        {
            FileName = "chmod",
            Arguments = string.Format("+x \"{0}\"", filePath),
            UseShellExecute = false
        };

        using (Process process = Process.Start(processInfo))
        {
            Assert.IsNotNull(process);
            process.WaitForExit();
        }
    }

    public static void AddDirExecutablePermissionsRecursive(
        [NotNull] string dirPath)
    {
        if (dirPath == null)
        {
            throw new ArgumentNullException("dirPath");
        }

        if (!Directory.Exists(dirPath))
        {
            throw new DirectoryNotFoundException(
                "Directory not found - " + dirPath);
        }

        if (Environment.Platform == EnvironmentPlatform.Windows)
        {
            return;
        }

        var processInfo = new ProcessStartInfo
        {
            FileName = "chmod",
            Arguments = string.Format("-R +x \"{0}\"", dirPath),
            RedirectStandardError = true,
            RedirectStandardOutput = true,
            UseShellExecute = false
        };

        using (Process process = Process.Start(processInfo))
        {
            Assert.IsNotNull(process);

            process.WaitForExit();
        }
    }
}
}