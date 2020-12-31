using System;

namespace PatchKit.UnityEditorExtension.Core
{
public class ValidationException : Exception
{
    public ValidationException(string message)
        : base(message)
    {
    }
}
}