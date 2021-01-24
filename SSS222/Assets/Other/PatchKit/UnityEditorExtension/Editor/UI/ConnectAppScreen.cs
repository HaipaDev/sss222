using System.Linq;
using JetBrains.Annotations;
using PatchKit.Api.Models.Main;
using PatchKit.UnityEditorExtension.Core;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;
using System.Collections.Generic;

namespace PatchKit.UnityEditorExtension.UI
{
public class ConnectAppScreen : Screen
{
    public class ConnectedResult
    {
        public readonly App App;

        public ConnectedResult(App app)
        {
            App = app;
        }
    }

    #region GUI

    public override string Title
    {
        get { return "Connect App"; }
    }

    public override Vector2? Size
    {
        get { return new Vector2(400f, 600f); }
    }

    public override void UpdateIfActive()
    {
        if (!Config.GetLinkedAccountApiKey().HasValue)
        {
            Push<NotLinkedAccountScreen>().Initialize();
        }
    }

    public override void Draw()
    {
        EditorGUILayout.BeginHorizontal();
        {
            if (GUILayout.Button(
                new GUIContent(
                    PluginResources.Arrow,
                    "Return"),
                GUILayout.Width(35),
                GUILayout.Height(20)))
            {
                Dispatch(() => Cancel());
            }

            GUILayout.Space(220);
            if (GUILayout.Button(
                "Create new app",
                GUILayout.Width(130),
                GUILayout.Height(20)))
            {
                Dispatch(() => CreateNew());
            }
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        {
            GUILayout.FlexibleSpace();
            GUILayout.Label(
                string.Format(
                    "Connect your PatchKit application for {0}.",
                    _platform.ToDisplayString()),
                EditorStyles.boldLabel);
            GUILayout.FlexibleSpace();
        }
        EditorGUILayout.EndHorizontal();

        ShouldFilterByPlatform = EditorGUILayout.Toggle(
            "Filter apps by platform",
            ShouldFilterByPlatform);
        EditorGUILayout.Space();

        _scrollViewVector = EditorGUILayout.BeginScrollView(
            _scrollViewVector,
            EditorStyles.helpBox);


        int appIndex = 0;
        foreach (App app in Apps)
        {
            if (!app.Removed)
            {
                DrawApp(app, appIndex % 2 == 0);
                appIndex++;
            }
        }

        EditorGUILayout.EndScrollView();
    }

    private void DrawApp(App app, bool useAltColor)
    {
        Assert.IsNotNull(app.Name);
        Assert.IsNotNull(app.Platform);

        bool isConnected = app.Secret == _connectedAppSecret;

        Color backgroundColor = useAltColor
            ? new Color(1f, 1f, 1f)
            : new Color(0.9f, 0.9f, 0.9f);


        using (Style.ColorizeBackground(
            isConnected ? new Color(0.502f, 0.839f, 0.031f) : backgroundColor))
        {
            EditorGUILayout.BeginHorizontal(EditorStyles.textArea);

            EditorGUILayout.BeginVertical();
            {
                var style = new GUIStyle(EditorStyles.largeLabel)
                {
                    fontStyle = FontStyle.Bold
                };

                if (app.Name.Length > 30)
                {
                    string shortName = app.Name.Substring(0, 30);
                    shortName += "...";
                    GUILayout.Label(new GUIContent(shortName, app.Name), style);
                }
                else
                {
                    GUILayout.Label(app.Name, style);
                }

                GUILayout.Label("Platform: " + app.Platform);

                EditorGUILayout.BeginHorizontal();
                {
                    GUILayout.Label(
                        "Secret: " + app.Secret,
                        EditorStyles.miniBoldLabel);

                    if (isConnected)
                    {
                        GUILayout.Label(
                            "Currently\nselected",
                            EditorStyles.miniLabel);
                    }
                    else
                    {
                        if (app.Platform == _platform.ToApiString())
                        {
                            using (Style.ColorizeBackground(Style.GreenPastel))
                            {
                                if (GUILayout.Button(
                                    "Select",
                                    GUILayout.Width(80)))
                                {
                                    Dispatch(() => Connect(app));
                                }
                            }
                        }
                        else
                        {
                            using (Style.ColorizeBackground(Style.RedPastel))
                            {
                                if (GUILayout.Button(
                                    "Select",
                                    GUILayout.Width(80)))
                                {
                                    EditorUtility.DisplayDialog(
                                        "Warning",
                                        "You tried to connect an application with wrong platform.\n\n" +
                                        " Connect your PatchKit application for " +
                                        _platform.ToString() +
                                        " platform",
                                        "Ok");
                                }
                            }
                        }
                    }
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.Space();
            }

            EditorGUILayout.EndVertical();

            EditorGUILayout.EndHorizontal();
        }


        Assert.IsNotNull(GUI.skin);
    }

    #endregion

    #region Data

    [SerializeField]
    private AppPlatform _platform;

    [SerializeField]
    private Vector2 _scrollViewVector;

    [SerializeField]
    private string _connectedAppSecret;

    #endregion

    #region Logic

    public void Initialize(AppPlatform platform)
    {
        _platform = platform;

        AppSecret? appSecret = Config.GetConnectedAppSecret(platform);

        if (appSecret.HasValue)
        {
            _connectedAppSecret = appSecret.Value.Value;
        }
        else
        {
            _connectedAppSecret = null;
        }
    }

    public override void OnActivatedFromTop(object result)
    {
        var createdResult = result as CreateAppScreen.CreatedResult;
        if (createdResult != null)
        {
            App app = createdResult.App;

            Config.ConnectApp(new AppSecret(app.Secret), _platform);

            Pop(new ConnectedResult(app));
        }
    }

    private bool _shouldFilterByPlatform = true;

    private bool ShouldFilterByPlatform
    {
        get { return _shouldFilterByPlatform; }

        set { _shouldFilterByPlatform = value; }
    }

    private App[] _apps;

    [NotNull]
    private IEnumerable<App> Apps
    {
        get
        {
            if (_apps == null)
            {
                _apps = Core.Api.GetApps();
            }

            return _apps.Where(
                x => !_shouldFilterByPlatform && x.Platform != "other" ||
                    x.Platform == _platform.ToApiString());
        }
    }

    private void Connect(App app)
    {
        Assert.AreEqual(_platform.ToApiString(), app.Platform);
        Assert.IsTrue(Apps.Contains(app));

        Config.ConnectApp(new AppSecret(app.Secret), _platform);

        Pop(new ConnectedResult(app));
    }

    private void CreateNew()
    {
        Push<CreateAppScreen>().Initialize(_platform);
    }

    private void Cancel()
    {
        Pop(null);
    }

    #endregion
}
}