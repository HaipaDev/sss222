using System;

namespace PatchKit.UnityEditorExtension.Core
{
public class InvalidArgumentException : ArgumentException
{
    public InvalidArgumentException(string paramName)
        : base("Invalid argument value.", paramName)
    {
    }
}
}