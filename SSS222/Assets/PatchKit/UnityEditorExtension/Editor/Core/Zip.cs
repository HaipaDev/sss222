using System;
using Ionic.Zip;
using JetBrains.Annotations;
using UnityEngine.Assertions;

namespace PatchKit.UnityEditorExtension.Core
{
public static class Zip
{
    public static void Unarchive(
        [NotNull] string zipFilePath,
        [NotNull] string targetPath)
    {
        if (zipFilePath == null)
        {
            throw new ArgumentNullException("zipFilePath");
        }

        if (targetPath == null)
        {
            throw new ArgumentNullException("targetPath");
        }

        using (ZipFile zip = ZipFile.Read(zipFilePath))
        {
            Assert.IsNotNull(zip);

            zip.ExtractAll(targetPath);
        }
    }
}
}