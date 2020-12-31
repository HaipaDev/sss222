using System.Linq;
using JetBrains.Annotations;
using PatchKit.Api.Models.Main;
using PatchKit.UnityEditorExtension.Core;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

namespace PatchKit.UnityEditorExtension.UI
{
public class CreateAppScreen : Screen
{
    public class CreatedResult
    {
        public readonly App App;

        public CreatedResult(App app)
        {
            App = app;
        }
    }

    #region GUI

    public override string Title
    {
        get { return "Create App"; }
    }

    public override Vector2? Size
    {
        get { return new Vector2(400f, 155f); }
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
        if (GUILayout.Button(
            new GUIContent(
                PluginResources.Arrow,
                "Return"),
            GUILayout.Width(35),
            GUILayout.Height(20)))
        {
            Dispatch(() => Cancel());
        }

        EditorGUILayout.BeginHorizontal();
        {
            GUILayout.FlexibleSpace();
            GUILayout.Label(
                string.Format(
                    "Create new PatchKit app for {0}",
                    _platform.ToDisplayString()),
                EditorStyles.boldLabel);
            GUILayout.FlexibleSpace();
        }
        EditorGUILayout.EndHorizontal();


        GUILayout.BeginHorizontal();
        {
            GUILayout.Label("Name:");
            _name = EditorGUILayout.TextField(_name);
        }
        GUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        {
            GUILayout.Label(
                new GUIContent(
                    "Target platform:     " +
                    EditorUserBuildSettings.activeBuildTarget,
                    Descriptions.PlatformChangeInfo));
            GUILayout.FlexibleSpace();
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Separator();
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

        if (string.IsNullOrEmpty(_name))
        {
            EditorGUILayout.HelpBox(
                "Please insert your application name",
                MessageType.Info);
        }
        else
        {
            if (NameValidationError != null)
            {
                EditorGUILayout.HelpBox(NameValidationError, MessageType.Error);
            }
            else
            {
                EditorGUILayout.Separator();
                GUILayout.BeginHorizontal();
                {
                    GUILayout.FlexibleSpace();

                    using (Style.Colorize(Style.GreenPastel))
                    {
                        if (GUILayout.Button("Create", GUILayout.Width(100)))
                        {
                            Dispatch(() => Create());
                        }
                    }

                    GUILayout.FlexibleSpace();
                }
                GUILayout.EndHorizontal();
            }
        }

        EditorGUILayout.Space();
    }

    #endregion

    #region Data

    [SerializeField]
    private AppPlatform _platform;

    [SerializeField]
    private string _name;

    #endregion

    #region Logic

    public void Initialize(AppPlatform platform)
    {
        _platform = platform;

        _name = string.Empty;
    }

    public override void OnActivatedFromTop(object result)
    {
    }

    private App[] _apps;

    [NotNull]
    private App[] Apps
    {
        get { return _apps ?? (_apps = Core.Api.GetApps()); }
    }

    private string NameValidationError
    {
        get
        {
            if (Apps.Any(x => x.Name == _name))
            {
                return "This name is already taken.";
            }

            return AppName.GetValidationError(_name);
        }
    }

    private void Create()
    {
        App app = Core.Api.CreateNewApp(new AppName(_name), _platform);

        Pop(new CreatedResult(app));
    }

    private void Cancel()
    {
        Pop(null);
    }

    #endregion
}
}