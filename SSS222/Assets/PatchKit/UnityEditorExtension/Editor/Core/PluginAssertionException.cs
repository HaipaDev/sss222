using System;
using JetBrains.Annotations;
using UnityEditor;

namespace PatchKit.UnityEditorExtension.Core
{
public class PluginAssertionException : Exception
{
    public PluginAssertionException(string message)
        : base(message)
    {
    }
}
}