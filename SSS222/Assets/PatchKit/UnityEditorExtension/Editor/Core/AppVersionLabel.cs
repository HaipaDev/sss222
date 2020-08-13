using System;
using System.Linq;
using JetBrains.Annotations;

namespace PatchKit.UnityEditorExtension.Core
{
public struct AppVersionLabel
{
    [NotNull]
    public readonly string Value;

    public readonly bool IsValid;

    public AppVersionLabel(string value)
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
            return "Application version label cannot be null.";
        }

        if (string.IsNullOrEmpty(value))
        {
            return "Application version label cannot be empty.";
        }

        if (!value.All(
            c => c >= 'a' && c <= 'z' ||
                c >= 'A' && c <= 'Z' ||
                char.IsWhiteSpace(c) ||
                char.IsPunctuation(c) ||
                char.IsDigit(c)))
        {
            return
                "Application version label contains forbidden characters.\n" +
                "Use only English characters and ':', '_' or '-'.\n\n" +
                "Unfortunately PatchKit Unity Editor Extensions doesn't support other languages " +
                "encoding. If you need to write correct information, please login to your PatchKit Panel " +
                "and set Version Properties for your application.";
        }

        return null;
    }
}
}