namespace PatchKit.UnityEditorExtension.UI
{
public static class Descriptions
{
    public static readonly string PlatformChangeInfo =
        "While uploading application from Unity, PatchKit target platform is determined by current active project build platform.";

    public static readonly string NeedToPlatformChange = PlatformChangeInfo +
        "\n\n" +
        "To change it, you have to switch project platform in Build Settings window.";
}
}