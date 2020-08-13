using System;
using System.Diagnostics;
using System.IO;
using JetBrains.Annotations;
using PatchKit.UnityEditorExtension.Core;
using UnityEngine.Assertions;
using Environment = PatchKit.UnityEditorExtension.Core.Environment;

namespace PatchKit.UnityEditorExtension.Tools
{
public class Terminal
{
    public static void Start(string command)
    {
        ProcessStartInfo processStartInfo = GetInfo(command);

        Process process = Process.Start(processStartInfo);
        Assert.IsNotNull(process);
    }

    [NotNull]
    private static ProcessStartInfo GetInfo(string command)
    {
        switch (Environment.Platform)
        {
            case EnvironmentPlatform.Windows:
                return GetWindowsInfo(command);
            case EnvironmentPlatform.Linux:
                return GetLinuxInfo(command);
            case EnvironmentPlatform.Mac:
                return GetMacInfo(command);
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    [NotNull]
    private static ProcessStartInfo GetWindowsInfo(string command)
    {
        // /K - Carries out the command specified by String and continues.
        return new ProcessStartInfo("cmd.exe", "/K \"" + command + "\"");
    }

    [NotNull]
    private static ProcessStartInfo GetLinuxInfo(string command)
    {
        if (File.Exists("/usr/bin/gnome-terminal"))
        {
            return GetGnomeTerminalInfo(command);
        }

        if (File.Exists("/usr/bin/xterm"))
        {
            return GetXTermTerminalInfo(command);
        }

        if (File.Exists("/usr/bin/konsole"))
        {
            return GetKonsoleTerminalInfo(command);
        }

        throw new InvalidOperationException();
    }

    [NotNull]
    private static ProcessStartInfo GetGnomeTerminalInfo(string command)
    {
        return new ProcessStartInfo("gnome-terminal", "-x " + command);
    }

    [NotNull]
    private static ProcessStartInfo GetXTermTerminalInfo(string command)
    {
        return new ProcessStartInfo("xterm", "-e " + command);
    }

    [NotNull]
    private static ProcessStartInfo GetKonsoleTerminalInfo(string command)
    {
        return new ProcessStartInfo("konsole", "-e " + command);
    }

    [NotNull]
    private static ProcessStartInfo GetMacInfo(string command)
    {
        File.WriteAllText(OsxLaunchScriptPath, OsxLaunchScript);

        FileSystem.AddFileExecutablePermissions(OsxLaunchScriptPath);

        return new ProcessStartInfo(
            "/bin/bash",
            string.Format(
                "-c \"sh {0} '{1}' '{2}'\"",
                OsxLaunchScriptPath,
                command,
                Directory.GetCurrentDirectory()));
    }

    private const string OsxLaunchScriptPath = "Temp/osx_terminal.sh";

    private const string OsxLaunchScript = "#!/bin/bash\n" +
        "cmd=\"$1\";\n" +
        "dir=\"$2\";\n" +
        "osascript <<EOF\n" +
        "    tell application \"Terminal\" to do script \"cd $dir; $cmd\"\n" +
        "EOF\n" +
        "exit 0\n";
}
}