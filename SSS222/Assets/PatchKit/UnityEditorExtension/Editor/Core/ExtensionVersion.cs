using JetBrains.Annotations;

namespace PatchKit.UnityEditorExtension.Core
{
public static class ExtensionVersion
{
    public static int Major = 1;

    public static int Minor = 0;

    public static int Patch = 2;

    [NotNull]
    public static string Name
    {
        get { return "v" + Major + "." + Minor + "." + Patch; }
    }
}
}
