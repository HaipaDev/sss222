using UnityEngine;

namespace PatchKit.UnityEditorExtension.UI
{
public static class PluginResources
{
    public static Texture Logo
    {
        get
        {
            return Resources.Load<Texture>(
                "patchkit-unity-editor-extension-logo");
        }
    }

    public static Texture Arrow
    {
        get
        {
            return Resources.Load<Texture>(
                "patchkit-unity-editor-extension-arrow");
        }
    }

    public static Texture Search
    {
        get
        {
            return Resources.Load<Texture>(
                "patchkit-unity-editor-extension-search");
        }
    }
}
}