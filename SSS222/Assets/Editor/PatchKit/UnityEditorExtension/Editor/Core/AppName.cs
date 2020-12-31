using System;
using System.Linq;
using JetBrains.Annotations;

namespace PatchKit.UnityEditorExtension.Core
{
public struct AppName
{
    [NotNull]
    public readonly string Value;

    public readonly bool IsValid;

    public AppName(string value)
    {
        string validationError = GetValidationError(value);

        if (validationError != null)
        {
            throw new ValidationException(validationError);
        }

        Value = value;

        IsValid = true;
    }

    [ContractAnnotation("null => notNull")]
    public static string GetValidationError(string value)
    {
        if (value == null)
        {
            return "Application name cannot be null.";
        }

        if (string.IsNullOrEmpty(value))
        {
            return "Application name cannot be empty.";
        }

        if (!value.All(
            c => c >= 'a' && c <= 'z' ||
                c >= 'A' && c <= 'Z' ||
                char.IsWhiteSpace(c) ||
                char.IsPunctuation(c) ||
                char.IsDigit(c)))
        {
            return "Application name only allows English characters and ':', '_' or '-'.";
        }

        return null;
    }
}
}