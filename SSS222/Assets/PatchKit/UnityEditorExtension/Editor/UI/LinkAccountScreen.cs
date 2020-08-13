using PatchKit.UnityEditorExtension.Core;
using UnityEditor;
using UnityEngine;

namespace PatchKit.UnityEditorExtension.UI
{
public class LinkAccountScreen : Screen
{
    public class LinkedResult
    {
        public readonly ApiKey ApiKey;

        public LinkedResult(ApiKey apiKey)
        {
            ApiKey = apiKey;
        }
    }

    #region GUI

    public override string Title
    {
        get { return "Link Account"; }
    }

    public override Vector2? Size
    {
        get { return new Vector2(400f, 125f); }
    }

    public override void UpdateIfActive()
    {
    }

    public override void Draw()
    {
        EditorGUILayout.BeginHorizontal();
        {
            GUILayout.FlexibleSpace();
            GUILayout.Label(
                "Link your PatchKit account",
                EditorStyles.boldLabel);
            GUILayout.FlexibleSpace();
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        {
            GUILayout.Label("API Key:");
            _newApiKey = EditorGUILayout.TextField(_newApiKey);

            if (GUILayout.Button(
                new GUIContent(
                    PluginResources.Search,
                    "Find your API key"),
                GUILayout.Width(20),
                GUILayout.Height(20)))
            {
                Dispatch(() => OpenProfileWebpage());
            }
        }
        EditorGUILayout.EndHorizontal();

        if (!string.IsNullOrEmpty(_newApiKey))
        {
            if (NewApiKeyValidationError != null)
            {
                EditorGUILayout.HelpBox(
                    NewApiKeyValidationError,
                    MessageType.Error);
            }
            else
            {
                EditorGUILayout.Separator();
                EditorGUILayout.BeginHorizontal();
                {
                    GUILayout.FlexibleSpace();

                    if (GUILayout.Button("Submit", GUILayout.Width(100)))
                    {
                        Dispatch(() => Link());
                    }

                    GUILayout.FlexibleSpace();
                }
                EditorGUILayout.EndHorizontal();
            }
        }
        else
        {
            EditorGUILayout.HelpBox(
                "Please enter your API key to link your PatchKit account with extension.",
                MessageType.Info);
        }

        EditorGUILayout.Separator();
    }

    #endregion

    #region Data

    [SerializeField]
    private string _newApiKey;

    #endregion

    #region Logic

    public void Initialize()
    {
        ApiKey? savedApiKey = Config.GetLinkedAccountApiKey();

        _newApiKey = savedApiKey.HasValue
            ? savedApiKey.Value.Value
            : string.Empty;
    }

    public override void OnActivatedFromTop(object result)
    {
    }

    private string NewApiKeyValidationError
    {
        get { return ApiKey.GetValidationError(_newApiKey); }
    }

    private void Link()
    {
        var apiKey = new ApiKey(_newApiKey);

        Config.LinkAccount(apiKey);

        Pop(new LinkedResult(apiKey));
    }

    private void OpenProfileWebpage()
    {
        Application.OpenURL("https://panel.patchkit.net/users/profile");
    }

    #endregion
}
}