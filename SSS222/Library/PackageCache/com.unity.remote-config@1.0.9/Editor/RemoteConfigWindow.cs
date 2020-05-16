using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace Unity.RemoteConfig.Editor
{
    internal class RemoteConfigWindow : EditorWindow
    {
        //Window state
        public bool shouldFetchOnInit;
        [NonSerialized] bool m_Initialized;
        RulesTreeView m_RulesTreeView;
        [SerializeField] TreeViewState m_RulesTreeViewState;
        [SerializeField] MultiColumnHeaderState m_RulesMultiColumnHeaderState;
        SettingsTreeView m_SettingsTreeView;
        [SerializeField] TreeViewState m_SettingsTreeViewState;
        [SerializeField] MultiColumnHeaderState m_SettingsMultiColumnHeaderState;

        RulesMultiColumnHeader m_RulesMultiColumnHeader;
        SettingsMultiColumnHeader m_RettingsMultiColumnHeader;

        string m_SelectedRuleId = k_DefaultRuleId;

        static string m_SettingsDropdownSelectedKey;
        
        RemoteConfigWindowController m_Controller;
        
        //GUI Content
        GUIContent m_pullRulesButtonContent = new GUIContent("Pull");
        GUIContent m_pushRulesButtonContent = new GUIContent("Push");
        GUIContent m_createRuleButtonContent = new GUIContent("Add Rule");
        GUIContent m_loadingMessage = new GUIContent("Loading, please wait.");
        GUIContent m_EnvironmentsLabelContent = new GUIContent("Environment");
        GUIContent m_CreateSettingButtonContent = new GUIContent("Add Setting");
        GUIContent m_AnalyticsNotEnabledContent = new GUIContent("To get started with Unity Remote Config, you must first link your project to a Unity Cloud Project ID.\n\nA Unity Cloud Project ID is an online identifier which is used across all Unity Services. These can be created within the Services window itself, or online on the Unity Services website.\n\nThe simplest way is to use the Services window within Unity, as follows:\nTo open the Services Window, go to Window > General > Services.\n\nNote: using Unity Remote Config does not require that you turn on any additional, individual cloud services like Analytics, Ads, Cloud Build, etc.");


        //UI Style variables
        const float k_LineHeight = 22f;
        const float k_LineHeightBuffer = k_LineHeight - 2;
        
        const string k_DefaultRuleId = "defaultRule";
        private const string utcDateFormat = "YYYY-MM-DDThh:mm:ssZ";
        const string m_NoSettingsContent = "To get started, please add a setting";
        const string m_NoSettingsForTheRuleContent = "Please add at least one setting to your rule";
        private GUIStyle guiStyleLabel = new GUIStyle();
        private GUIStyle guiStyleSubLabel = new GUIStyle();
        private const string ruleConditionFormat = "JEXL Syntax";

        public static string defaultRuleId
        {
            get { return k_DefaultRuleId; }
        }

        public static string settingsDropdownSelectedKey
        {
            get { return m_SettingsDropdownSelectedKey; }
            set { m_SettingsDropdownSelectedKey = value; }
        }

        Rect ruleTableRect
        {
            get {
                return new Rect(0, m_RulesTreeView.multiColumnHeader.height + k_LineHeight, position.width * .3f, (position.height - (k_LineHeight * 2.25f))); 
            }
        }

        Rect GetRuleSettingsRect(float currentY)
        {
            return new Rect(position.width * .3f, currentY + k_LineHeight, position.width * .7f, (position.height - currentY));
        }
        
        Rect GetRuleSettingsTableRect(Rect ruleSettingsRect)
        {
            return new Rect(ruleSettingsRect.x, ruleSettingsRect.y + k_LineHeight, ruleSettingsRect.width, ruleSettingsRect.height - (k_LineHeight * 2f));
        }
        
        Rect rulesMultiColumnTreeViewRect
        {
            get { return new Rect(20, 30, 500, position.height-60); }
        }

        Rect rsTableRect
        {
            get { return new Rect(position.width * .3f, m_RulesTreeView.multiColumnHeader.height + k_LineHeight, position.width * .7f, position.height - (k_LineHeight * 2.25f)); }
        }

        [MenuItem("Window/Remote Config")]
        public static void GetWindow()
        {
            var RSWindow = GetWindow<RemoteConfigWindow>();
            RSWindow.titleContent = new GUIContent("Remote Config");
            RSWindow.minSize = new Vector2(425, 300);
            RSWindow.Focus();
            RSWindow.Repaint();
        }
        
        private void OnEnable()
        {
            if (AreServicesEnabled())
            {
                InitIfNeeded();
            }
        }

        private void OnDisable()
        {
            m_Controller.SetManagerDirty();
            try
            {
                m_RulesTreeView.DeleteRule -= OnDeleteRule;
                m_RulesTreeView.RuleEnabledOrDisabled -= OnRuleEnabledOrDisabled;
                m_RulesTreeView.RuleAttributesChanged -= OnRuleAttributesChanged;

                m_SettingsTreeView.UpdateSetting -= OnUpdateSetting;
                m_SettingsTreeView.DeleteSetting -= OnDeleteSetting;
                m_SettingsTreeView.SetActiveOnSettingChanged -= OnAddSettingToRule;

                m_Controller.rulesDataStoreChanged -= OnRulesDataStoreChanged;
                m_Controller.remoteSettingsStoreChanged -= OnRemoteSettingsStoreChanged;
                m_Controller.environmentChanged -= OnEnvironmentChanged;
                EditorApplication.quitting -= m_Controller.SetManagerDirty;
                EditorApplication.playModeStateChanged -= EditorApplication_playModeStateChanged;
            }
#pragma warning disable CS0168 // Variable is declared but never used
            catch (NullReferenceException e)
#pragma warning restore CS0168 // Variable is declared but never used
            { }
        }

        private void OnGUI()
        {
            if (AreServicesEnabled(true))
            {
                InitIfNeeded();
                
                EditorGUI.BeginDisabledGroup(IsLoading());
                float currentY = 2f;
                if(GUI.Button(new Rect(position.width - (position.width/4) + 5, currentY, (position.width / 4) - 5, 20), "View in Dashboard"))
                {
                    if (string.IsNullOrEmpty(m_Controller.environmentId))
                    {
                        //no environment id set - take them to the enviornments view
                        Help.BrowseURL(string.Format(RemoteConfigEditorEnvConf.dashboardEnvironmentsPath, Application.cloudProjectId));
                    }
                    else
                    {
                        //looking at a config, so take them to the config
                        Help.BrowseURL(string.Format(RemoteConfigEditorEnvConf.dashboardConfigsPath, Application.cloudProjectId, m_Controller.environmentId));
                    }
                }
                currentY += k_LineHeight;
                DrawEnvironmentDropdown(currentY);
                DrawPushPullButtons(currentY);
                currentY += k_LineHeight;

                Rect rulesTreeViewRect = ruleTableRect;
                m_RulesTreeView.OnGUI(rulesTreeViewRect);
                DrawPaneSeparator(rulesTreeViewRect);

                if(!IsLoading())
				{
					if (GUI.Button(new Rect(2f, ruleTableRect.height + k_LineHeight + 4f, ruleTableRect.width - 5f, k_LineHeight),
					m_createRuleButtonContent))
					{
						m_Controller.AddDefaultRule();
						m_SelectedRuleId = m_Controller.GetRulesList().Last().id;
						m_RulesTreeView.SetSelection(m_SelectedRuleId);

						//TODO: move this logic elsewhere
						m_SettingsTreeView.settingsList = m_Controller.GetSettingsList();
						m_SettingsTreeView.activeSettingsList = m_Controller.GetRuleById(m_SelectedRuleId).value;
						m_SettingsTreeView.Reload();
                    }
				}

                if (m_SelectedRuleId == k_DefaultRuleId)
                {
                    DrawRemoteSettingsPane();
                }
                else
                {
                    if (m_Controller.HasRules())
                    {
                        var currentRule = m_Controller.GetRuleById(m_SelectedRuleId);
                        EditorGUI.BeginDisabledGroup(currentRule.enabled);
                        currentY = DrawConfigurationsPane(currentRule);
                        DrawRuleSettingsRect(currentY);
                    }
                }

                EditorGUI.EndDisabledGroup();
                AddFooterButtons();
            }
        }

        private void InitIfNeeded()
        {
            if (!m_Initialized)
            {
                m_Controller = new RemoteConfigWindowController(shouldFetchOnInit);
                EditorApplication.quitting += m_Controller.SetManagerDirty;
                EditorApplication.playModeStateChanged += EditorApplication_playModeStateChanged;

                if (m_RulesTreeViewState == null)
                {
                    m_RulesTreeViewState = new TreeViewState();
                }

                bool firstInit = m_RulesMultiColumnHeaderState == null;
                var headerState = CreateRulesMultiColumnHeaderState(rulesMultiColumnTreeViewRect.width);
                if (MultiColumnHeaderState.CanOverwriteSerializedFields(m_RulesMultiColumnHeaderState, headerState))
                    MultiColumnHeaderState.OverwriteSerializedFields(m_RulesMultiColumnHeaderState, headerState);
                m_RulesMultiColumnHeaderState = headerState;
                
                foreach(MultiColumnHeaderState.Column column in m_RulesMultiColumnHeaderState.columns)
                {
                    column.autoResize = true;
                }
                
                m_RulesMultiColumnHeader = new RulesMultiColumnHeader(headerState);
                if (firstInit)
                {
                    m_RulesMultiColumnHeader.ResizeToFit();
                }
                m_RulesTreeView = new RulesTreeView(m_RulesTreeViewState, m_RulesMultiColumnHeader, m_Controller.GetRulesList());

                if (m_SettingsTreeViewState == null)
                {
                    m_SettingsTreeViewState = new TreeViewState();
                }
                
                firstInit = m_SettingsMultiColumnHeaderState == null;
                headerState = CreateSettingsMultiColumnHeaderState(position.width * .7f);
                if (MultiColumnHeaderState.CanOverwriteSerializedFields(m_SettingsMultiColumnHeaderState, headerState))
                    MultiColumnHeaderState.OverwriteSerializedFields(m_SettingsMultiColumnHeaderState, headerState);
                m_SettingsMultiColumnHeaderState = headerState;
                
                foreach(MultiColumnHeaderState.Column column in m_SettingsMultiColumnHeaderState.columns)
                {
                    column.autoResize = true;
                }
                
                m_RettingsMultiColumnHeader = new SettingsMultiColumnHeader(headerState);
                if (firstInit)
                {
                    m_RettingsMultiColumnHeader.ResizeToFit();
                }
                m_SettingsTreeView = new SettingsTreeView(m_SettingsTreeViewState, m_RettingsMultiColumnHeader, m_Controller.GetSettingsList(), m_Controller.GetRulesList());

                m_RulesTreeView.SelectionChangedEvent += selectedRuleId =>
                {
                    this.m_SelectedRuleId = selectedRuleId;
                    if (this.m_SelectedRuleId == k_DefaultRuleId)
                    {
                        m_SettingsTreeView.settingsList = m_Controller.GetSettingsList();
                        m_SettingsTreeView.activeSettingsList = m_Controller.GetSettingsList();
                    }
                    else
                    {
                        m_SettingsTreeView.settingsList = m_Controller.GetSettingsList();
                        m_SettingsTreeView.activeSettingsList = m_Controller.GetRuleById(selectedRuleId).value;
                    }
                    m_SettingsTreeView.Reload();
                    m_SettingsDropdownSelectedKey = "";
                };
                m_RulesTreeView.DeleteRule += OnDeleteRule;
                m_RulesTreeView.RuleEnabledOrDisabled += OnRuleEnabledOrDisabled;
                m_RulesTreeView.RuleAttributesChanged += OnRuleAttributesChanged;

                m_SettingsTreeView.UpdateSetting += OnUpdateSetting;
                m_SettingsTreeView.DeleteSetting += OnDeleteSetting;
                m_SettingsTreeView.SetActiveOnSettingChanged += OnAddSettingToRule;
                
                m_Controller.rulesDataStoreChanged += OnRulesDataStoreChanged;
                m_Controller.remoteSettingsStoreChanged += OnRemoteSettingsStoreChanged;
                m_Controller.environmentChanged += OnEnvironmentChanged;

                m_SelectedRuleId = k_DefaultRuleId;
                m_RulesTreeView.SetSelection(k_DefaultRuleId);

                m_Initialized = true;
            }
        }

        private void OnDestroy()
        {
            if (!(m_Controller.CompareKeyValueEquality(m_Controller.GetSettingsList(), m_Controller.GetLastCachedKeyList()) &&
                  m_Controller.CompareRulesEquality(m_Controller.GetRulesList(), m_Controller.GetLastCachedRulesList())))
            {
                if (!EditorUtility.DisplayDialog(m_Controller.k_RCDialogUnsavedChangesTitle,
                    m_Controller.k_RCDialogUnsavedChangesMessage,
                    m_Controller.k_RCDialogUnsavedChangesOK,
                    m_Controller.k_RCDialogUnsavedChangesCancel))
                {
                    createNewRCWindow();
                }
            }
        }

        private void createNewRCWindow()
        {
            RemoteConfigWindow newWindow = (RemoteConfigWindow) CreateInstance(typeof(RemoteConfigWindow));
            newWindow.titleContent.text = m_Controller.k_RCWindowName;
            newWindow.shouldFetchOnInit = true;
            newWindow.Show();
        }

        private void EditorApplication_playModeStateChanged(PlayModeStateChange obj)
        {
            if(obj == PlayModeStateChange.EnteredPlayMode)
            {
                m_Controller.SetManagerDirty();
            }
        }

        private bool AreServicesEnabled(bool calledFromOnGui = false)
        {
            if (string.IsNullOrEmpty(CloudProjectSettings.projectId) || string.IsNullOrEmpty(CloudProjectSettings.organizationId))
            {
                if(calledFromOnGui)
                {
                    GUIStyle style = GUI.skin.label;
                    style.wordWrap = true;
                    EditorGUILayout.LabelField(m_AnalyticsNotEnabledContent, style);
                }
                return false;
            }
            return true;
        }

        private void OnEnvironmentChanged()
        {
            m_SelectedRuleId = k_DefaultRuleId;
            m_RulesTreeView.SetSelection(m_SelectedRuleId);
        }

        private void OnDeleteSetting(string entityId)
        {
            if (m_SelectedRuleId == k_DefaultRuleId)
            {
                m_Controller.DeleteRemoteSetting(entityId);
            }
            else
            {
                m_Controller.DeleteSettingFromRule(m_SelectedRuleId, entityId);
            }
        }

        private void OnUpdateSetting(RsKvtData oldItem, RsKvtData newitem)
        {
            if (m_SelectedRuleId == k_DefaultRuleId)
            {
                m_Controller.UpdateRemoteSetting(oldItem, newitem);
            }
            else
            {
                m_Controller.UpdateSettingForRule(m_SelectedRuleId, newitem);
            }
        }

        private void AddFooterButtons()
        {
            if (IsLoading())
            {
                GUI.Label(new Rect(0, position.height - k_LineHeight, position.width, k_LineHeight), m_loadingMessage);
            }
        }

        private bool IsLoading()
        {
            bool isLoading = m_Controller.isLoading;
            m_SettingsTreeView.isLoading = isLoading;
            return isLoading;
        }

        private void OnDeleteRule(string ruleId)
        {
            m_SelectedRuleId = k_DefaultRuleId;
            m_Controller.DeleteRule(ruleId);
            m_RulesTreeView.SetSelection(m_SelectedRuleId);
        }

        private void OnRuleAttributesChanged(string ruleId, RuleWithSettingsMetadata newRule)
        {
            m_Controller.UpdateRuleAttributes(ruleId, newRule);
        }

        private void OnRuleEnabledOrDisabled(string ruleId, bool enabled)
        {
            m_Controller.EnableOrDisableRule(ruleId, enabled);
        }

        private void OnRulesDataStoreChanged()
        {
            m_RulesTreeView.rulesList = m_Controller.GetRulesList();
            m_RulesTreeView.Reload();
        }
        
        private void OnRemoteSettingsStoreChanged()
        {
            if (m_SelectedRuleId == k_DefaultRuleId)
            {
                m_SettingsTreeView.settingsList = m_Controller.GetSettingsList();
                m_SettingsTreeView.activeSettingsList = m_Controller.GetSettingsList();
            }
            else
            {
                m_SettingsTreeView.settingsList = m_Controller.GetSettingsList();
                m_SettingsTreeView.activeSettingsList = m_Controller.GetRuleById(m_SelectedRuleId).value;
            }
            m_SettingsTreeView.Reload();
        }

        private void DrawEnvironmentDropdown(float currentY)
        {
            var totalWidth = position.width / 2;
            EditorGUI.BeginDisabledGroup(m_Controller.GetEnvironmentsCount() <= 1 || IsLoading());
            GUI.Label(new Rect(0, currentY, totalWidth / 2, 20), m_EnvironmentsLabelContent);
            GUIContent ddBtnContent = new GUIContent(m_Controller.GetCurrentEnvironmentName());
            Rect ddRect = new Rect(totalWidth / 2, currentY, totalWidth / 2, 20);
            if (GUI.Button(ddRect, ddBtnContent, EditorStyles.popup))
            {
                m_Controller.BuildPopupListForEnvironments().DropDown(ddRect);
            }
            EditorGUI.EndDisabledGroup();
        }

        private void DrawPushPullButtons(float currentY)
        {
            float boundingBoxPadding = 5f;
            var paddedRect = new Rect((position.width / 2) + boundingBoxPadding, currentY,(position.width / 2) - (2 * boundingBoxPadding), 20);
            var buttonWidth = (paddedRect.width / 2);
            if (GUI.Button(new Rect(paddedRect.x, paddedRect.y, buttonWidth, 20),
                m_pushRulesButtonContent))
            {
                m_Controller.Push();
                m_SelectedRuleId = k_DefaultRuleId;
                m_RulesTreeView.SetSelection(m_SelectedRuleId);
            }
            if (GUI.Button(new Rect(paddedRect.x + buttonWidth + boundingBoxPadding, paddedRect.y, buttonWidth, 20),
                    m_pullRulesButtonContent))
            {
                m_Controller.Fetch();
                m_SelectedRuleId = k_DefaultRuleId;
                m_RulesTreeView.SetSelection(m_SelectedRuleId);
            }
        }

        private void DrawRemoteSettingsPane()
        {
            var remoteSettingsPaneRect = rsTableRect;
            m_SettingsTreeView.enableEditingSettingsKeys = true;
            m_SettingsTreeView.rulesList = m_Controller.GetRulesList();
            m_SettingsTreeView.OnGUI(new Rect(remoteSettingsPaneRect.x, remoteSettingsPaneRect.y, remoteSettingsPaneRect.width, remoteSettingsPaneRect.height - k_LineHeight));

            if (!m_Controller.GetSettingsList().Any())
            {
                var messageRect = new Rect(remoteSettingsPaneRect.x + 1f,remoteSettingsPaneRect.y + k_LineHeight + 6f, 400f, k_LineHeight);
                showMessage(messageRect, m_NoSettingsContent);
            }

            AddRemoteSettingsPaneFooterButtons(remoteSettingsPaneRect);
        }

        private void AddRemoteSettingsPaneFooterButtons(Rect remoteSettingsPaneRect)
        {
            var buttonWidth = remoteSettingsPaneRect.width;

            if (!IsLoading())
            {
                if (GUI.Button(new Rect(remoteSettingsPaneRect.x + 2.5f, remoteSettingsPaneRect.height + k_LineHeight + 4f, buttonWidth - 5f, k_LineHeight),
                    m_CreateSettingButtonContent))
                {
                    m_Controller.AddSetting();
                }
            }
            else
            {
                GUI.Label(new Rect(0, position.height - k_LineHeight, position.width, k_LineHeight), m_loadingMessage);
            }
        }
        
        private void DrawPaneSeparator(Rect rulesTreeViewRect)
        {
            EditorGUI.DrawRect(new Rect(rulesTreeViewRect.width - 1, rulesTreeViewRect.y, 1, rulesTreeViewRect.height), Color.black);
        }
        
        private float DrawConfigurationsPane(RuleWithSettingsMetadata rule)
        {
            var configPaneRect = new Rect(position.width * .3f, m_RulesTreeView.multiColumnHeader.height, position.width * .7f, position.height * .25f);
            var currentY = configPaneRect.y +k_LineHeight;

            var name = CreateLabelAndTextField("Name: ", rule.name, currentY, configPaneRect);
            currentY +=  1.4f*k_LineHeight;

            var condition = CreateLabelWithSubLabelTextFieldAndHelpButton("Condition: ", ruleConditionFormat, rule.condition, currentY, configPaneRect, RemoteConfigEditorEnvConf.apiDocsBasePath + RemoteConfigEditorEnvConf.apiDocsRulesPath);
            currentY +=  1.4f*k_LineHeight;

            var rolloutPercentage = CreateLabelAndSlider("Rollout Percentage: ", rule.rolloutPercentage, 1.0F, 100.0F, currentY, configPaneRect);
            currentY +=  1.4f*k_LineHeight;

            var startDate = CreateLabelWithSubLabelAndTextField("Start Date and Time: ", utcDateFormat, rule.startDate, currentY, configPaneRect);
            currentY += 1.4f*k_LineHeight;

            var endDate = CreateLabelWithSubLabelAndTextField("End Date and Time: ", utcDateFormat, rule.endDate, currentY, configPaneRect);
            currentY += 1.4f * k_LineHeight;

            if (name != rule.name || condition != rule.condition || rolloutPercentage != rule.rolloutPercentage || startDate != rule.startDate || endDate != rule.endDate)
            {
                var newRule = new RuleWithSettingsMetadata(rule.id, name, rule.enabled, rule.priority, condition, rolloutPercentage, rule.value, startDate, endDate);
                m_Controller.UpdateRuleAttributes(rule.id, newRule);
            }

            if (m_Controller.GetSettingsListForRule(rule.id).Count == 0)
            {
                var messageRect = new Rect(configPaneRect.x + 6f, currentY, configPaneRect.width - 12f, k_LineHeight);
                showMessage(messageRect, m_NoSettingsForTheRuleContent);
            }
            currentY += 1.4f*k_LineHeight;
            return currentY;
        }

        private void DrawRuleSettingsRect(float currentY)
        {
            var settingsRect = GetRuleSettingsRect(currentY);
            
            GUI.Label(new Rect(settingsRect.x, settingsRect.y, settingsRect.width, k_LineHeight), "Settings");
            
            m_SettingsTreeView.enableEditingSettingsKeys = false;
            m_SettingsTreeView.rulesList = m_Controller.GetRulesList();
            m_SettingsTreeView.OnGUI(GetRuleSettingsTableRect(settingsRect));
        }

        private void OnAddSettingToRule(string entityId, bool active)
        {
            if (active)
            {
                m_Controller.AddSettingToRule(m_SelectedRuleId, entityId);
            }
            else
            {
                m_Controller.DeleteSettingFromRule(m_SelectedRuleId, entityId);
            }
        }


        private string CreateLabelAndTextField(string labelText, string textFieldText, float currentY, Rect configPaneRect)
        {
            var labelX = configPaneRect.x + 5;
            var labelWidth = 125f;
            var textFieldX = labelX + labelWidth + 5;
            var textFieldWidth = configPaneRect.width - labelWidth - 15;
            
            GUI.Label(new Rect(labelX, currentY, labelWidth, k_LineHeightBuffer), labelText);
            var textFieldRect = new Rect(textFieldX, currentY, textFieldWidth, k_LineHeightBuffer);
            EditorGUIUtility.AddCursorRect(textFieldRect, MouseCursor.Text);
            return GUI.TextField(textFieldRect, textFieldText);
        }
        
        private string CreateLabelWithSubLabelAndTextField(string labelText, string subLabelText, string textFieldText, float currentY, Rect configPaneRect)
        {
            var labelX = configPaneRect.x + 5;
            var labelWidth = 125f;
            var textFieldX = labelX + labelWidth + 5;
            var textFieldWidth = configPaneRect.width - labelWidth - 15;
            var labelHeight = (k_LineHeightBuffer * 0.8f);
            var subLabelHeight = (k_LineHeightBuffer * 0.8f);
            var subLabelColor = new Color(0.4f, 0.4f, 0.4f, 1.0f);

            guiStyleLabel = GUI.skin.label;
            guiStyleSubLabel.fontSize = 8;
            guiStyleSubLabel.normal.textColor = subLabelColor;

            GUI.Label(new Rect(labelX, currentY, labelWidth, labelHeight), labelText, guiStyleLabel);
            GUI.Label(new Rect(labelX, currentY+labelHeight, labelWidth, subLabelHeight), subLabelText, guiStyleSubLabel);
            var textFieldRect = new Rect(textFieldX, currentY, textFieldWidth, k_LineHeightBuffer);
            EditorGUIUtility.AddCursorRect(textFieldRect, MouseCursor.Text);
            return GUI.TextField(textFieldRect, textFieldText);
        }
        
        private string CreateLabelWithSubLabelTextFieldAndHelpButton(string labelText, string subLabelText, string textFieldText, float currentY, Rect configPaneRect, string helpButtonPath)
        {
            var labelX = configPaneRect.x + 5;
            var labelWidth = 125f;
            var textFieldX = labelX + labelWidth + 5;
            var textFieldWidth = configPaneRect.width - labelWidth - 15;
            var labelHeight = (k_LineHeightBuffer * 0.8f);
            var subLabelHeight = (k_LineHeightBuffer * 0.8f);
            var subLabelColor = new Color(0.4f, 0.4f, 0.4f, 1.0f);
            var buttonSize = 25f;

            guiStyleLabel = GUI.skin.label;
            guiStyleSubLabel.fontSize = 8;
            guiStyleSubLabel.normal.textColor = subLabelColor;
            Texture helpButtonTexture = EditorGUIUtility.FindTexture("_Help");

            GUI.Label(new Rect(labelX, currentY, labelWidth, labelHeight), labelText, guiStyleLabel);
            GUI.Label(new Rect(labelX, currentY+labelHeight, labelWidth, subLabelHeight), subLabelText, guiStyleSubLabel);
            var textFieldRect = new Rect(textFieldX, currentY, textFieldWidth, k_LineHeightBuffer);
            EditorGUIUtility.AddCursorRect(textFieldRect, MouseCursor.Text);
            if (GUI.Button(new Rect(textFieldX - (2f*buttonSize), currentY, buttonSize, buttonSize), new GUIContent(helpButtonTexture, "Jexl Syntax Help\nOpens the Remote Config API Documentation in a Web Browser"), new GUIStyle(GUIStyle.none)))
            {
                Help.BrowseURL(helpButtonPath);
            }

            return GUI.TextField(textFieldRect, textFieldText);
        }

        private int CreateLabelAndSlider(string labelText, float hSliderValue, float leftValue, float rightValue, float currentY, Rect configPaneRect)
        {
            var labelX = configPaneRect.x + 5;
            var labelWidth = 125f;
            var sliderFieldX = labelX + labelWidth + 5;
            var sliderFieldWidth = configPaneRect.width - 70 - labelWidth;
            var sliderValuePositionX = labelX + labelWidth + sliderFieldWidth + 30;
            hSliderValue = GUI.HorizontalSlider(new Rect(sliderFieldX, currentY, sliderFieldWidth, k_LineHeightBuffer), hSliderValue, leftValue, rightValue);

            GUI.Label(new Rect(labelX, currentY, labelWidth, k_LineHeightBuffer), labelText);
            hSliderValue = Mathf.Clamp(EditorGUI.IntField(new Rect(sliderValuePositionX, currentY, 30, k_LineHeightBuffer), (int)hSliderValue), 1, 100);
            return (int)(hSliderValue);
        }

        private void showMessage(Rect messageRect, string messageText)
        {
            EditorGUI.HelpBox(messageRect, messageText, MessageType.Warning);
        }

        public static MultiColumnHeaderState CreateRulesMultiColumnHeaderState(float treeViewWidth)
        {
            var columns = new[]
            {
                new MultiColumnHeaderState.Column
                {
                    headerContent = new GUIContent("Enabled"),
                    headerTextAlignment = TextAlignment.Left,
                    canSort = false,
                    sortedAscending = true,
                    sortingArrowAlignment = TextAlignment.Center,
                    width = 16,
                    minWidth = 16,
                    autoResize = false,
                    allowToggleVisibility = false
                },
                new MultiColumnHeaderState.Column
                {
                    headerContent = new GUIContent("Name"),
                    headerTextAlignment = TextAlignment.Left,
                    canSort = false,
                    sortedAscending = true,
                    sortingArrowAlignment = TextAlignment.Center,
                    width = 28,
                    minWidth = 28,
                    autoResize = false,
                    allowToggleVisibility = false
                },
                new MultiColumnHeaderState.Column
                {
                    headerContent = new GUIContent("Priority"),
                    headerTextAlignment = TextAlignment.Left,
                    canSort = false,
                    sortedAscending = true,
                    sortingArrowAlignment = TextAlignment.Center,
                    width = 16,
                    minWidth = 16,
                    autoResize = false,
                    allowToggleVisibility = false
                },
                new MultiColumnHeaderState.Column
                {
                    headerTextAlignment = TextAlignment.Left,
                    sortedAscending = true,
                    canSort = false,
                    sortingArrowAlignment = TextAlignment.Center,
                    width = 30,
                    minWidth = 30,
                    maxWidth = 30,
                    autoResize = false,
                    allowToggleVisibility = false
                }
            };
            var state = new MultiColumnHeaderState(columns);
            return state;
        }
        
        public static MultiColumnHeaderState CreateSettingsMultiColumnHeaderState(float treeViewWidth)
        {
            var columns = new[]
            {
                new MultiColumnHeaderState.Column
                {
                    headerContent = new GUIContent("Key"),
                    headerTextAlignment = TextAlignment.Left,
                    sortedAscending = true,
                    sortingArrowAlignment = TextAlignment.Center,
                    width = 150,
                    minWidth = 60,
                    autoResize = false,
                    allowToggleVisibility = false
                },
                new MultiColumnHeaderState.Column
                {
                    headerContent = new GUIContent("Type"),
                    headerTextAlignment = TextAlignment.Left,
                    sortedAscending = true,
                    sortingArrowAlignment = TextAlignment.Center,
                    width = 150,
                    minWidth = 60,
                    autoResize = false,
                    allowToggleVisibility = false
                },
                new MultiColumnHeaderState.Column
                {
                    headerContent = new GUIContent("Value"),
                    headerTextAlignment = TextAlignment.Left,
                    sortedAscending = true,
                    sortingArrowAlignment = TextAlignment.Center,
                    width = 150,
                    minWidth = 60,
                    autoResize = false,
                    allowToggleVisibility = false
                },
                new MultiColumnHeaderState.Column
                {
                    headerTextAlignment = TextAlignment.Left,
                    sortedAscending = true,
                    sortingArrowAlignment = TextAlignment.Center,
                    width = 30,
                    minWidth = 30,
                    maxWidth = 30,
                    autoResize = false,
                    allowToggleVisibility = false
                }
            };
            var state = new MultiColumnHeaderState(columns);
            return state;
        }
    }
    
    // Displays all the rules
    internal class RulesTreeView : TreeView
    {
        public List<RuleWithSettingsMetadata> rulesList;

        public event Action<string> SelectionChangedEvent;
        public event Action<string> DeleteRule;
        public event Action<string, bool> RuleEnabledOrDisabled;
        public event Action<string, RuleWithSettingsMetadata> RuleAttributesChanged;

        public RulesTreeView(TreeViewState state, MultiColumnHeader multiColumnHeader, List<RuleWithSettingsMetadata> rulesList) : base(state, multiColumnHeader)
        {
            this.rulesList = rulesList;
            useScrollView = true;
            Reload();
        }

        protected override TreeViewItem BuildRoot()
        {
            var root = new TreeViewItem<RuleWithSettingsMetadata>(0, -1, "Root", new RuleWithSettingsMetadata());
            var id = 0;
            var allItems = new List<TreeViewItem>();
            if (rulesList != null)
            {
                RuleWithSettingsMetadata defaultRule = new RuleWithSettingsMetadata();
                defaultRule.priority = RemoteConfigDataManager.defaultRulePriority;
                defaultRule.id = RemoteConfigWindow.defaultRuleId;
                defaultRule.enabled = true;
                allItems.Add(new TreeViewItem<RuleWithSettingsMetadata>(id++, 0, "Settings Config", defaultRule));
                allItems.AddRange(rulesList.Select(x => new TreeViewItem<RuleWithSettingsMetadata>(id++, 0, x.name, x))
                    .ToList<TreeViewItem>());
            }
            SetupParentsAndChildrenFromDepths(root, allItems);

            return root;
        }

        protected override void RowGUI(RowGUIArgs args)
        {
            var item = (TreeViewItem<RuleWithSettingsMetadata>) args.item;
            for (int i = 0; i < args.GetNumVisibleColumns(); ++i)
            {
                CellGUI(args.GetCellRect(i), item, args.GetColumn(i), ref args);
            }
        }

        private void CellGUI(Rect cellRect, TreeViewItem<RuleWithSettingsMetadata> item, int column, ref RowGUIArgs args)
        {
            CenterRectUsingSingleLineHeight(ref cellRect);

            switch (column)
            {
                case 0:
                    if (item.data.id != RemoteConfigWindow.defaultRuleId)
                    {
                        Rect toggleRect = cellRect;
                        toggleRect.x += cellRect.width - 18;
                        var toggle = GUI.Toggle(toggleRect, item.data.enabled, "");
                        if (toggle != item.data.enabled)
                        {
                            RuleEnabledOrDisabled?.Invoke(item.data.id, !item.data.enabled);
                        }
                    }
                    break;
                case 1:
                    var ruleNameStyle = EditorStyles.label;
                    ruleNameStyle.wordWrap = false;
                    GUI.Label(cellRect, item.displayName, ruleNameStyle);
                    break;
                case 2:
                    if (item.data.id != RemoteConfigWindow.defaultRuleId)
                    {
                        EditorGUI.BeginDisabledGroup(item.data.enabled);
                        var newPriority = EditorGUI.IntField(cellRect, item.data.priority);
                        if (newPriority != item.data.priority)
                        {
                            var rule = item.data;
                            rule.priority = newPriority;
                            RuleAttributesChanged?.Invoke(item.data.id, rule);
                        }
                        EditorGUI.EndDisabledGroup();
                    }
                    break;
                case 3:
                    if(item.data.id != RemoteConfigWindow.defaultRuleId)
                    {
                        EditorGUI.BeginDisabledGroup(item.data.enabled);
                        if (GUI.Button(cellRect, EditorGUIUtility.FindTexture("d_TreeEditor.Trash")))
                        {
                            DeleteRule?.Invoke(item.data.id);
                        }
                        EditorGUI.EndDisabledGroup();
                    }
                    break;
            }
        }

        protected override void SelectionChanged(IList<int> selectedIds)
        {
            base.SelectionChanged(selectedIds);
            var treeViewItems = GetRows() as List<TreeViewItem>;
            var treeViewItem = treeViewItems.Find(x => x.id == selectedIds[0]) as TreeViewItem<RuleWithSettingsMetadata>;
            var ruleId = treeViewItem.data.id;
            if (SelectionChangedEvent != null)
            {
                SelectionChangedEvent(ruleId);
            }
        }

        public void SetSelection(string selectRuleId)
        {
            var treeViewItems = GetRows() as List<TreeViewItem>;
            var selections = new List<int>();
            foreach (TreeViewItem<RuleWithSettingsMetadata> treeViewitem in treeViewItems)
            {
                if (selectRuleId == treeViewitem.data.id)
                {
                    selections.Add(treeViewitem.id);
                }
            }
            SetSelection(selections, TreeViewSelectionOptions.FireSelectionChanged);

        }
    }

    internal class RulesMultiColumnHeader : MultiColumnHeader
    {
        public RulesMultiColumnHeader(MultiColumnHeaderState state) : base(state)
        {
            canSort = false;
        }
    }

    internal class SettingsTreeView : TreeView
    {
        public List<RsKvtData> settingsList;
        public List<RsKvtData> activeSettingsList;
        public List<RuleWithSettingsMetadata> rulesList;

        public event Action<RsKvtData, RsKvtData> UpdateSetting;
        public event Action<string> DeleteSetting;
        public event Action<string, bool> SetActiveOnSettingChanged;

        public bool isLoading = false;

        public bool enableEditingSettingsKeys;

        private struct RSTypeChangedStruct
        {
            public RsKvtData rs;
            public string newType;
        }

        public SettingsTreeView(TreeViewState state, MultiColumnHeader multiColumnHeader, List<RsKvtData> settingsList,
            List<RuleWithSettingsMetadata> rulesList, bool enableEditingSettingsKeys = true) : base(state, multiColumnHeader)
        {
            this.rowHeight = 18f;
            this.settingsList = settingsList;
            this.rulesList = rulesList;
            this.enableEditingSettingsKeys = enableEditingSettingsKeys;
            useScrollView = true;
            Reload();
        }

        private bool isActiveSettingInSettingsList(List<TreeViewItem<RsKvtData>> settingsList, string entityId)
        {
            foreach (var setting in settingsList)
            {
                if (setting.data.metadata.entityId == entityId)
                {
                    return true;
                }
            }

            return false;
        }

        private List<TreeViewItem<RsKvtData>> AddDeletedSettings(
            List<TreeViewItem<RsKvtData>> tempItems, List<RsKvtData> activeSettings, int id)
        {
            foreach (var setting in activeSettings)
            {
                if (!isActiveSettingInSettingsList(tempItems, setting.metadata.entityId))
                {
                    tempItems.Add(new TreeViewItem<RsKvtData>(id++, 0, setting.rs.key, setting, false));
                }
            }
            return tempItems;
        }

        protected override TreeViewItem BuildRoot()
        {
            var root = new TreeViewItem<RsKvtData>(0, -1, "Root", new RsKvtData());
            var id = 0;
            var allItems = new List<TreeViewItem>();
            if (settingsList != null && settingsList.Count > 0 && activeSettingsList != null)
            {

                var tempItems = settingsList
                    .Select(x => new TreeViewItem<RsKvtData>(id++, 0, x.rs.key, x, false))
                    .ToList<TreeViewItem<RsKvtData>>();

                tempItems = AddDeletedSettings(tempItems, activeSettingsList, id);

                foreach (var activeRS in activeSettingsList)
                {
                    var item = tempItems.FirstOrDefault(x => x.data.metadata.entityId == activeRS.metadata.entityId);
                    if (default(TreeViewItem<RsKvtData>) == item)
                    {
                        continue;
                    }
                    item.data = activeRS;
                    item.enabled = true;
                }

                allItems = tempItems.ToList<TreeViewItem>();
            }

            SetupParentsAndChildrenFromDepths(root, allItems);

            return root;
        }

        protected override void RowGUI(RowGUIArgs args)
        {
            var item = (TreeViewItem<RsKvtData>) args.item;
            for (int i = 0; i < args.GetNumVisibleColumns(); ++i)
            {
                CellGUI(args.GetCellRect(i), item, args.GetColumn(i), ref args);
            }
        }

        void CellGUI(Rect cellRect, TreeViewItem<RsKvtData> item, int column, ref RowGUIArgs args)
        {
            var isDisabled = enableEditingSettingsKeys && IsKeyInRules(item.data.metadata.entityId, rulesList);
            switch (column)
            {
                case 0:
                    EditorGUI.BeginDisabledGroup(!enableEditingSettingsKeys || isDisabled || isLoading);
                    var newKey = GUI.TextField(cellRect, item.data.rs.key);
                    if(enableEditingSettingsKeys && !isLoading)
                    {
                        EditorGUIUtility.AddCursorRect(cellRect, MouseCursor.Text);
                    }
                    EditorGUI.EndDisabledGroup();
                    if (newKey != item.data.rs.key)
                    {
                        UpdateSetting?.Invoke(item.data,
                            new RsKvtData(item.data.metadata.entityId, new RemoteSettingsKeyValueType(newKey, item.data.rs.type, item.data.rs.value)));
                    }
                    break;
                case 1:
                    CenterRectUsingSingleLineHeight(ref cellRect);
                    EditorGUI.BeginDisabledGroup(!enableEditingSettingsKeys || isDisabled || isLoading);
                    GUIContent ddBtnContent = new GUIContent(string.IsNullOrEmpty(item.data.rs.type) ? "Select a type" : item.data.rs.type);
                    if (GUI.Button(cellRect, ddBtnContent, EditorStyles.popup))
                    {
                        BuildPopupListForSettingTypes(item).DropDown(cellRect);
                    }
                    EditorGUI.EndDisabledGroup();

                    break;
                case 2:
                    EditorGUI.BeginDisabledGroup(isLoading || item.enabled == false);
                    string newValue = item.data.rs.value;
                    switch (item.data.rs.type)
                    {
                        case "string":
                            newValue = GUI.TextField(cellRect, item.data.rs.value);
                            if(!isLoading && item.enabled)
                            {
                                EditorGUIUtility.AddCursorRect(cellRect, MouseCursor.Text);
                            }
                            break;
                        case "bool":
                            bool boolVal = false;
                            try
                            {
                                boolVal = bool.Parse(item.data.rs.value);
                            }
                            catch (FormatException)
                            {
                                UpdateSetting?.Invoke(item.data, new RsKvtData(item.data.metadata.entityId, new RemoteSettingsKeyValueType(item.data.rs.key, item.data.rs.type, boolVal.ToString())));
                            }
                            var menu = new GenericMenu();
                            menu.AddItem(new GUIContent("True"), boolVal == true, () => UpdateSetting?.Invoke(item.data,
                                new RsKvtData(item.data.metadata.entityId, new RemoteSettingsKeyValueType(item.data.rs.key, item.data.rs.type, true.ToString()))));
                            menu.AddItem(new GUIContent("False"), boolVal == false, () => UpdateSetting?.Invoke(item.data,
                                new RsKvtData(item.data.metadata.entityId, new RemoteSettingsKeyValueType(item.data.rs.key, item.data.rs.type, false.ToString()))));
                            GUIContent boolDdBtnContent = new GUIContent(boolVal.ToString());
                            if (GUI.Button(cellRect, boolDdBtnContent, EditorStyles.popup))
                            {
								menu.DropDown(cellRect);
                            }
                            break;
                        case "float":
                            float floatVal = 0.0f;
                            try
                            {
                                floatVal = float.Parse(item.data.rs.value);
                            }
                            catch(FormatException)
                            {
                                //Do nothing
                            }
                            newValue = EditorGUI.FloatField(cellRect, floatVal).ToString();
                            break;
                        case "int":
                            int intVal = 0;
                            try
                            {
                                intVal = int.Parse(item.data.rs.value);
                            }
                            catch(FormatException)
                            {
                                //Do nothing
                            }
                            newValue = EditorGUI.IntField(cellRect, intVal).ToString();
                            break;
                        case "long":
                            long longVal = 0L;
                            try
                            {
                                longVal = long.Parse(item.data.rs.value);
                            }
                            catch(FormatException)
                            {
                                //Do nothing
                            }
                            newValue = EditorGUI.LongField(cellRect, longVal).ToString();
                            break;
                    }
                    
                    EditorGUI.EndDisabledGroup();
                    if (newValue != item.data.rs.value)
                    {
                        UpdateSetting?.Invoke(item.data,
                            new RsKvtData(item.data.metadata.entityId, new RemoteSettingsKeyValueType(item.data.rs.key, item.data.rs.type, newValue)));
                    }

                    break;

                case 3:
                    CenterRectUsingSingleLineHeight(ref cellRect);
                    if (enableEditingSettingsKeys)
                    {
                        EditorGUI.BeginDisabledGroup(isDisabled || isLoading);
                        if (GUI.Button(cellRect,
                            new GUIContent(EditorGUIUtility.FindTexture("d_TreeEditor.Trash"), isDisabled ? "Can't remove a setting used in a rule" : "")))
                        {
                            DeleteSetting?.Invoke(item.data.metadata.entityId);
                        }

                        EditorGUI.EndDisabledGroup();
                        break;
                    }
                    else
                    {
                        var toggle = GUI.Toggle(cellRect, item.enabled, "");
                        if (toggle != item.enabled)
                        {
                            SetActiveOnSettingChanged?.Invoke(item.data.metadata.entityId, toggle);
                        }
                        break;
                    }
            }       
        }

        private GenericMenu BuildPopupListForSettingTypes(TreeViewItem<RsKvtData> treeViewItem)
        {
            var menu = new GenericMenu();

            for (int i = 0; i < RemoteConfigDataManager.rsTypes.Count; i++)
            {
                string name = RemoteConfigDataManager.rsTypes[i];
                menu.AddItem(new GUIContent(name), string.Equals(name, treeViewItem.data.rs.type), RSTypeChangedCallback, new RSTypeChangedStruct() { newType = name, rs = treeViewItem.data });
            }

            return menu;
        }

        private void RSTypeChangedCallback(object obj)
        {
            var rSTypeChangedStruct = (RSTypeChangedStruct)obj;
            if (rSTypeChangedStruct.newType != rSTypeChangedStruct.rs.rs.type)
            {
                UpdateSetting?.Invoke(rSTypeChangedStruct.rs,
                    new RsKvtData(rSTypeChangedStruct.rs.metadata.entityId, new RemoteSettingsKeyValueType(rSTypeChangedStruct.rs.rs.key, rSTypeChangedStruct.newType, rSTypeChangedStruct.rs.rs.value)));
            }
        }

        bool IsKeyInRules(string entityId, List<RuleWithSettingsMetadata> rulesList)
        {
            //TODO: Simplify into Linq query
            foreach (var rule in rulesList)
            {
                if(rule.enabled)
                {
                    foreach (var setting in rule.value)
                    {
                        if (entityId == setting.metadata.entityId)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }
    }

    internal class SettingsMultiColumnHeader : MultiColumnHeader
    {
        public SettingsMultiColumnHeader(MultiColumnHeaderState state) : base(state)
        {
            canSort = false;
        }
    }

    internal class TreeViewItem<T> : TreeViewItem
    {
        public T data;
        public bool enabled;

        public TreeViewItem(int id, int depth, string displayName, T data, bool enabled = true) : base(id, depth,
            displayName)
        {
            this.data = data;
            this.enabled = enabled;
        }

    }
}
