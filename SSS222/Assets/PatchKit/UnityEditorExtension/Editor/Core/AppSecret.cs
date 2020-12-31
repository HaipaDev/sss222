using System;
using System.Linq;
using JetBrains.Annotations;

namespace PatchKit.UnityEditorExtension.Core
{
public struct AppSecret
{
    [NotNull]
    public readonly string Value;

    public readonly bool IsValid;

    public AppSecret(string value)
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
            return "Application secret cannot be null.";
        }

        if (string.IsNullOrEmpty(value))
        {
            return "Application secret cannot be empty.";
        }

        if (!value.All(c => char.IsLetterOrDigit(c)))
        {
            return
                "Application secret cannot have other characters than letters and digits.";
        }

        return null;
    }
}
}