using System.Linq;
using JetBrains.Annotations;

namespace PatchKit.UnityEditorExtension.Core
{
public struct ApiKey
{
    [NotNull]
    public readonly string Value;

    public readonly bool IsValid;

    public ApiKey(string value)
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
            return "API key cannot be null.";
        }

        if (string.IsNullOrEmpty(value))
        {
            return "API key cannot be empty.";
        }

        if (!value.All(char.IsLetterOrDigit))
        {
            return
                "API key cannot have other characters than letters and digits.";
        }

        if (value.Length != 32)
        {
            return "API key must be 32 characters length.";
        }

        return null;
    }
}
}