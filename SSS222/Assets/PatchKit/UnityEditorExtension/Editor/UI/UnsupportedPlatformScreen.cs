using UnityEditor;
using UnityEngine;

namespace PatchKit.UnityEditorExtension.UI
{
public class UnsupportedPlatformScreen : Screen
{
    #region GUI

    public override void UpdateIfActive()
    {
    }

    public override void Draw()
    {
        EditorGUILayout.HelpBox(
            "Currently selected platform is not supported by PatchKit.\n\n" +
            Descriptions.NeedToPlatformChange,
            MessageType.Warning);
    }

    public override string Title
    {
        get { return "Not supported"; }
    }

    public override Vector2? Size
    {
        get { return new Vector2(600f, 400f); }
    }

    #endregion

    #region Logic

    public void Initialize()
    {
    }

    public override void OnActivatedFromTop(object result)
    {
    }

    #endregion
}
}