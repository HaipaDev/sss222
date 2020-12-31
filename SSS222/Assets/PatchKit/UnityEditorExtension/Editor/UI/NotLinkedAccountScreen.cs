using PatchKit.UnityEditorExtension.Core;
using UnityEditor;
using UnityEngine;

namespace PatchKit.UnityEditorExtension.UI
{
public class NotLinkedAccountScreen : Screen
{
    #region GUI

    public override string Title
    {
        get { return null; }
    }

    public override Vector2? Size
    {
        get { return null; }
    }

    public override void UpdateIfActive()
    {
        if (Config.GetLinkedAccountApiKey().HasValue)
        {
            Pop(null);
        }
    }

    public override void Draw()
    {
        EditorGUILayout.BeginHorizontal();
        {
            GUILayout.FlexibleSpace();
            GUILayout.Label(
                "You haven't linked your PatchKit account yet.",
                EditorStyles.boldLabel);
            GUILayout.FlexibleSpace();
        }
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.Separator();
        
        EditorGUILayout.BeginHorizontal();
        {
            GUILayout.FlexibleSpace();

            if (GUILayout.Button("Fix it!", GUILayout.Width(100)))
            {
                Dispatch(() => OpenLinkScreen());
            }

            GUILayout.FlexibleSpace();
        }
        EditorGUILayout.EndHorizontal();
    }

    #endregion

    #region Logic

    public void Initialize()
    {
    }

    public override void OnActivatedFromTop(object result)
    {
    }

    private void OpenLinkScreen()
    {
        Push<LinkAccountScreen>().Initialize();
    }

    #endregion
}
}