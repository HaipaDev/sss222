using System;
using System.IO;
using System.Linq;
using PatchKit.Api;
using PatchKit.Api.Models.Main;
using PatchKit.UnityEditorExtension.Core;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

namespace PatchKit.UnityEditorExtension.UI
{
public class SafeBuildAndUploadScreen : Screen
{
    #region GUI

    public override string Title
    {
        get { return null; }
    }

    public override Vector2? Size
    {
        get { return new Vector2(400f, 800f); }
    }

    public override void UpdateIfActive()
    {
        if (_platform != AppBuild.Platform)
        {
            Pop(null);
            return;
        }

        AppSecret? connectedAppSecret = Config.GetConnectedAppSecret(_platform);
        if (!connectedAppSecret.HasValue ||
            connectedAppSecret.Value.Value != _appSecret)
        {
            Pop(null);
        }
    }

    public override void Draw()
    {
        _scrollViewVector = EditorGUILayout.BeginScrollView(_scrollViewVector);
        {
            DrawContent();
        }
        EditorGUILayout.EndScrollView();
    }

    private void DrawApp()
    {
        EditorGUILayout.BeginVertical();
        {
            EditorGUILayout.BeginHorizontal();
            {
                GUILayout.Label("Name:");

                GUILayout.FlexibleSpace();

                GUILayout.Label(
                    _appName,
                    EditorStyles.miniLabel,
                    GUILayout.ExpandWidth(true));
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            {
                GUILayout.Label("Platform:");

                GUILayout.FlexibleSpace();

                GUILayout.Label(
                    _platform.ToDisplayString(),
                    EditorStyles.miniLabel,
                    GUILayout.ExpandWidth(true));
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            {
                GUILayout.Label("Secret:");
                GUILayout.FlexibleSpace();

                GUILayout.Label(
                    _appSecret,
                    EditorStyles.miniLabel,
                    GUILayout.ExpandWidth(true));
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            {
                GUILayout.FlexibleSpace();
                if (GUILayout.Button(
                    new GUIContent("Change application", "Change application"),
                    GUILayout.Width(120)))
                {
                    Dispatch(() => SwitchConnectedApp());
                }
            }
            EditorGUILayout.EndHorizontal();
            using (Style.ColorizeBackground(Color.blue))
            {
                Assert.IsNotNull(GUI.skin);
                var style = new GUIStyle(GUI.skin.label)
                {
                    normal =
                    {
                        textColor = Color.blue
                    }
                };

                if (GUILayout.Button(
                    "Need to change platform?",
                    style,
                    GUILayout.ExpandWidth(false)))
                {
                    EditorUtility.DisplayDialog(
                        "Need to change platform?",
                        Descriptions.NeedToPlatformChange,
                        "OK");
                }

                Rect lastRect = GUILayoutUtility.GetLastRect();
                lastRect.y += lastRect.height - 2;
                lastRect.height = 2;
                GUI.Box(lastRect, "");
            }
        }
        EditorGUILayout.EndVertical();

        EditorGUILayout.Space();
    }

    private void DrawBuild()
    {
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        {
            EditorGUILayout.BeginHorizontal();
            {
                GUILayout.FlexibleSpace();
                GUILayout.Label("Build Summary", EditorStyles.boldLabel);
                GUILayout.FlexibleSpace();
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            {
                GUILayout.FlexibleSpace();
                if (GUILayout.Button(
                    new GUIContent(
                        "Change location",
                        "Button open Build Settings window."),
                    GUILayout.Width(110)))
                {
                    Dispatch(() => ChangeBuildLocation());
                }
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            {
                string shortPath = BuildLocation;
                if (BuildLocation == null)
                {
                    shortPath = "(not set)";
                }
                else
                {
                    if (BuildLocation.Length > 50)
                    {
                        shortPath = BuildLocation.Substring(0, 20) +
                            "..." +
                            BuildLocation.Substring(
                                BuildLocation.Length - 20,
                                20);
                    }
                }


                GUILayout.Label("Location:");
                GUILayout.FlexibleSpace();
                GUILayout.Label(
                    new GUIContent(shortPath, BuildLocation),
                    EditorStyles.miniLabel);
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Separator();
            EditorGUILayout.BeginHorizontal();
            {
                GUILayout.Label("Scenes: ", EditorStyles.boldLabel);
                if (GUILayout.Button(
                    new GUIContent(
                        "Edit Scenes",
                        "Button open Build Settings window."),
                    GUILayout.Width(110)))
                {
                    Dispatch(() => SwitchScenes());
                }
            }
            EditorGUILayout.EndHorizontal();

            string[] scenes = AppBuild.Scenes.ToArray();

            for (int i = 0; i < scenes.Length; i++)
            {
                GUILayout.Label(i + ". " + scenes[i]);
            }
        }
        EditorGUILayout.EndVertical();
        EditorGUILayout.Space();
    }

    private void DrawVersion()
    {
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        {
            EditorGUILayout.BeginHorizontal();
            {
                GUILayout.FlexibleSpace();
                GUILayout.Label("Version Details", EditorStyles.boldLabel);
                GUILayout.FlexibleSpace();
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();

            GUILayout.Label("*Label: ");
            _versionLabel = GUILayout.TextField(_versionLabel);

            EditorGUILayout.Space();
            GUILayout.Label("Changelog: ");
            _versionChangelog = GUILayout.TextArea(
                _versionChangelog,
                GUILayout.MinHeight(200));
        }
        EditorGUILayout.EndVertical();
        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.LabelField("Automatically publish after upload");
            _publishOnUpload = EditorGUILayout.Toggle(_publishOnUpload);
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.LabelField("Overwrite draft version if it exists");
            _overwriteDraftVersion =
                EditorGUILayout.Toggle(_overwriteDraftVersion);
        }
        EditorGUILayout.EndHorizontal();

        if (!_overwriteDraftVersion)
        {
            EditorGUILayout.HelpBox(
                "If a draft version exists, interaction with the console will be necessary.",
                MessageType.Warning);
        }

        bool canUpload = false;
        if (!IsBuildLocationSelected)
        {
            EditorGUILayout.HelpBox(
                "You haven't selected build location.",
                MessageType.Error);
        }
        else if (VersionLabelValidationError != null)
        {
            EditorGUILayout.HelpBox(
                VersionLabelValidationError,
                MessageType.Error);
        }
        else if (VersionChangelogValidationError != null)
        {
            EditorGUILayout.HelpBox(
                VersionChangelogValidationError,
                MessageType.Error);
        }
        else
        {
            canUpload = true;
        }

        using (Style.ChangeEnabled(canUpload))
        {
            using (Style.Colorize(Style.GreenOlive))
            {
                if (GUILayout.Button(
                    new GUIContent("Build & Upload", "Build a new version"),
                    GUILayout.ExpandWidth(true)))
                {
                    if (EditorUtility.DisplayDialog(
                        "Warning",
                        "Are you sure? ",
                        "Yes",
                        "No"))
                    {
                        Dispatch(() => BuildAndUpload());
                    }
                }
            }
        }

        EditorGUILayout.Space();
    }

    private void DrawContent()
    {
        Assert.IsNotNull(GUI.skin);

        EditorGUILayout.BeginHorizontal();
        {
            GUILayout.FlexibleSpace();
            GUILayout.Label(_appName, EditorStyles.boldLabel);
            GUILayout.FlexibleSpace();
        }
        EditorGUILayout.EndHorizontal();

        DrawApp();
        DrawBuild();
        DrawVersion();
    }

    #endregion

    #region Data

    [SerializeField]
    private Vector2 _scrollViewVector;

    [SerializeField]
    private AppPlatform _platform;

    [SerializeField]
    private string _appName;

    [SerializeField]
    private string _appSecret;

    [SerializeField]
    private string _versionLabel;

    [SerializeField]
    private string _versionChangelog;

    [SerializeField]
    private bool _publishOnUpload;

    [SerializeField]
    private bool _overwriteDraftVersion;

    #endregion

    #region Logic

    public void Initialize(AppPlatform platform, AppSecret appSecret)
    {
        _platform = platform;
        _appName = string.Empty;
        _appSecret = appSecret.Value;
        _versionLabel = string.Empty;
        _versionChangelog = string.Empty;
        _publishOnUpload = true;
        _overwriteDraftVersion = true;

        try
        {
            _appName = Core.Api.GetAppInfo(appSecret).Name;
        }
        catch (ApiConnectionException e)
        {
            _appName = "(cannot connect to the API)";

            Debug.LogWarning(e);
        }
        catch (ApiResponseException e)
        {
            Debug.LogWarning(e);

            Config.UnlinkApp(_platform);
        }
    }

    public override void OnActivatedFromTop(object result)
    {
        var connectedResult = result as ConnectAppScreen.ConnectedResult;
        if (connectedResult != null)
        {
            App app = connectedResult.App;

            _appSecret = app.Secret;
            _appName = app.Name;
        }
    }

    private string BuildLocation
    {
        get { return AppBuild.Location; }
    }

    private bool IsBuildLocationSelected
    {
        get { return !string.IsNullOrEmpty(BuildLocation); }
    }

    private string VersionLabelValidationError
    {
        get { return AppVersionLabel.GetValidationError(_versionLabel); }
    }

    private string VersionChangelogValidationError
    {
        get
        {
            return AppVersionChangelog.GetValidationError(_versionChangelog);
        }
    }

    private void SwitchScenes()
    {
        EditorWindow.GetWindow(
            Type.GetType("UnityEditor.BuildPlayerWindow,UnityEditor"));
    }

    private void ChangeBuildLocation()
    {
        AppBuild.OpenLocationDialog();
    }

    private void SwitchConnectedApp()
    {
        Push<ConnectAppScreen>().Initialize(_platform);
    }

    private void BuildAndUpload()
    {
        Assert.IsTrue(IsBuildLocationSelected);
        Assert.IsNull(VersionLabelValidationError);
        Assert.IsNull(VersionChangelogValidationError);

        if (AppBuild.TryCreate())
        {
            ApiKey? apiKey = Config.GetLinkedAccountApiKey();
            Assert.IsTrue(apiKey.HasValue);

            EditorUtility.DisplayProgressBar("Preparing upload...", "", 0.0f);

            try
            {
                using (var tools = new Tools.PatchKitToolsClient())
                {
                    tools.MakeVersion(
                        apiKey.Value.Value,
                        _appSecret,
                        _versionLabel,
                        _versionChangelog,
                        Path.GetDirectoryName(BuildLocation),
                        _publishOnUpload,
                        _overwriteDraftVersion);
                }
            }
            finally
            {
                EditorUtility.ClearProgressBar();
            }

            EditorUtility.DisplayDialog(
                "Uploading",
                "Your game has been successfully built and is being uploaded right now.\n\n" +
                "You can track the progress in console window.",
                "OK");

            Close();
        }
    }

    #endregion
}
}