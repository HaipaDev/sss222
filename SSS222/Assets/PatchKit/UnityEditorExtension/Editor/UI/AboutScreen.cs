using PatchKit.UnityEditorExtension.Core;
using UnityEditor;
using UnityEngine;

namespace PatchKit.UnityEditorExtension.UI
{
public class AboutScreen : Screen
{
    #region GUI

    public override string Title
    {
        get { return "About"; }
    }

    public override Vector2? Size
    {
        get { return new Vector2(400, 400); }
    }

    public override void UpdateIfActive()
    {
    }

    public override void Draw()
    {
        GUILayout.Label("\n");
        EditorGUILayout.BeginHorizontal();
        {
            GUILayout.FlexibleSpace();
            GUILayout.Label(
                PluginResources.Logo,
                GUILayout.Height(150),
                GUILayout.Width(307));
            GUILayout.FlexibleSpace();
        }
        EditorGUILayout.EndHorizontal();
        GUILayout.Label("About", EditorStyles.boldLabel);
        GUILayout.Label(
            "Publish your Unity game to the world in seconds, directly from your\n" +
            "Unity editor.\n\n" +
            "PatchKit Unity Editor Extension integrates your development\n" +
            "environment with your PatchKit account, makes it possible to\n" +
            "share your latest build with your players without leaving\n" +
            "the Unity editor.",
            GUILayout.Width(380));

        EditorGUILayout.Separator();
        EditorGUILayout.BeginHorizontal();
        {
            GUILayout.FlexibleSpace();
            EditorGUILayout.BeginVertical();
            {
                if (GUILayout.Button("Contact", GUILayout.Width(170)))
                {
                    Application.OpenURL(
                        "http://docs.patchkit.net/contact.html");
                }

                if (GUILayout.Button("Documentation", GUILayout.Width(170)))
                {
                    Application.OpenURL("http://docs.patchkit.net/");
                }
            }
            EditorGUILayout.EndVertical();
            GUILayout.FlexibleSpace();
        }
        EditorGUILayout.EndHorizontal();
        GUILayout.Label("\nVersion " + ExtensionVersion.Name, EditorStyles.centeredGreyMiniLabel);
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