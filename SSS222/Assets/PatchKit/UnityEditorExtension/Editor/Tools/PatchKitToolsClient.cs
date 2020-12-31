using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using PatchKit.UnityEditorExtension.Core;
using UnityEngine;
using UnityEngine.Assertions;
using Environment = PatchKit.UnityEditorExtension.Core.Environment;

namespace PatchKit.UnityEditorExtension.Tools
{
public class PatchKitToolsClient : IDisposable
{
    private const string TempLocation = "Temp/patchkit-tools";

    public PatchKitToolsClient()
    {
        UnpackTools();
    }

    private const string MakeVersionChangelogFilePath =
        "Temp/make_version_changelog.txt";

    public void MakeVersion(
        string apiKey,
        string appSecret,
        string label,
        string changelog,
        string buildDir,
        bool autoPublishAfterUpload,
        bool forceOverrideDraftVersion)
    {
        File.WriteAllText(MakeVersionChangelogFilePath, changelog);

        var arguments = new List<string>
        {
            "--secret",
            appSecret,
            "-a",
            apiKey,
            "--label",
            label,
            "--changelogfile",
            MakeVersionChangelogFilePath,
            "--files",
            buildDir
        };

        if (forceOverrideDraftVersion)
        {
            arguments.Add("-x");
            arguments.Add("true");
        }

        if (autoPublishAfterUpload)
        {
            arguments.Add("-p");
            arguments.Add("true");
        }

        string[] argumentsArray = arguments.ToArray();

        Execute("make-version", argumentsArray);
    }

    public void Execute(string tool, string[] args)
    {
        Execute(tool, args, true);
    }

    public void Execute(string tool, string[] args, bool unpackIfFailure)
    {
        try
        {
            string command = tool;

            if (args != null)
            {
                command += " ";
                foreach (string arg in args)
                {
                    Assert.IsNotNull(arg);
                    if (arg.Contains(' '))
                    {
                        command += "\"" + arg + "\" ";
                    }
                    else
                    {
                        command += arg + " ";
                    }
                }
            }

            switch (Environment.Platform)
            {
                case EnvironmentPlatform.Windows:
                    ExecuteWindows(command);
                    break;
                case EnvironmentPlatform.Linux:
                    ExecuteLinux(command);
                    break;
                case EnvironmentPlatform.Mac:
                    ExecuteMac(command);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        catch (Exception e)
        {
            if (!unpackIfFailure)
            {
                throw;
            }

            Debug.LogWarning(e);
            ForceUnpackTools();
            Execute(tool, args, false);
        }
    }

    private void ExecuteLinux(string command)
    {
        string path =
            Path.GetFullPath(Path.Combine(TempLocation, "patchkit-tools"));

        PluginAssert.IsTrue(File.Exists(path));

        string executable = string.Format("bash -c '{0} {1}'", path, command);

        Terminal.Start(executable);
    }

    private void ExecuteMac(string command)
    {
        string path =
            Path.GetFullPath(Path.Combine(TempLocation, "patchkit-tools"));

        PluginAssert.IsTrue(File.Exists(path));

        string executable = string.Format("{0} {1}", path, command);

        Terminal.Start(executable);
    }

    private void ExecuteWindows(string command)
    {
        string path = Path.GetFullPath(
            Path.Combine(TempLocation, "patchkit-tools.bat"));

        PluginAssert.IsTrue(File.Exists(path));

        string executable = "\"" + path + "\" " + command;

        Terminal.Start(executable);
    }

    private void UnpackTools()
    {
        if (Directory.Exists(TempLocation))
        {
            return;
        }

        ForceUnpackTools();
    }

    private void ForceUnpackTools()
    {
        if (Directory.Exists(TempLocation))
        {
            Directory.Delete(TempLocation, true);
        }

        Zip.Unarchive(
            PatchKitToolsPackages.GetPath(Environment.Platform),
            TempLocation);

        FileSystem.AddDirExecutablePermissionsRecursive(TempLocation);
    }

    public void Dispose()
    {
        //Clear();
    }
}
}