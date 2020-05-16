using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Networking;

namespace Unity.RemoteConfig.Editor
{
    internal class RemoteConfigWindowController
    {
        public event Action rulesDataStoreChanged;
        public event Action remoteSettingsStoreChanged;
        public event Action environmentChanged;

        RemoteConfigDataManager m_DataManager;

        bool m_WebRequestReturnedEventSubscribed = false;
        bool m_PostAddRuleEventSubscribed = false;
        bool m_PostSettingsEventSubscribed = false;
        bool m_PutConfigsEventSubscribed = false;
        bool m_PostConfigEventSubscribed;
        string m_CurrentEnvironment;
        bool m_IsLoading = false;

        public string environmentId { get { return m_DataManager.GetCurrentEnvironmentId(); } }
 
        // DialogBox variables
        public readonly string k_RCWindowName = "Remote Config";
        public readonly string k_RCDialogUnsavedChangesTitle = "Unsaved Changes";
        public readonly string k_RCDialogUnsavedChangesMessage = "You have unsaved changes. \n \n" +
                                                       "If you want them saved, click 'Cancel' then 'Push'.\n" +
                                                       "Otherwise, click 'OK' to discard the changes \n";
        public readonly string k_RCDialogUnsavedChangesOK = "OK";
        public readonly string k_RCDialogUnsavedChangesCancel = "Cancel";

        public bool isLoading
        {
            get { return m_IsLoading; }
            set { m_IsLoading = value; }
        }

        public RemoteConfigWindowController(bool shouldFetchOnInit = true)
        {
            m_DataManager = new RemoteConfigDataManager();
            m_DataManager.RulesDataStoreChanged += OnRulesDataStoreChanged;
            m_DataManager.RemoteSettingDataStoreChanged += OnRemoteSettingDataStoreChanged;
            m_DataManager.EnvironmentChanged += OnEnvironmentChanged;
            RemoteConfigWebApiClient.rcRequestFailed += OnFailedRequest;
            if (shouldFetchOnInit)
            {
                FetchEnvironments();
            }
        }

        public void SetManagerDirty()
        {
            m_DataManager.SetDirty();
        }

        private bool SetEnvironmentData(List<Environment> environments)
        {
            if (environments != null && environments.Count > 0)
            {
                m_DataManager.SetEnvironmentsList(environments);
                var currentEnvironment = LoadEnvironments(environments, m_DataManager.GetCurrentEnvironmentName());
                m_DataManager.SetCurrentEnvironment(currentEnvironment);
                m_DataManager.SetLastSelectedEnvironment(currentEnvironment.name);
                return true;
            }

            return false;
        }

        public Environment LoadEnvironments(List<Environment> environments, string currentEnvName)
        {
            if (environments.Count > 0)
            {
                var currentEnvironment = environments[0];  // Default to the first existing one
                foreach (var environment in environments)
                {
                    if (environment.name == currentEnvName)
                    {
                        currentEnvironment = environment;
                        break;
                    }
                }
                return currentEnvironment;
            }
            else
            {
                Debug.LogWarning("No environments loaded. Please restart the Remote Config editor window");
                return new Environment();
            }
        }

        public List<RuleWithSettingsMetadata> GetRulesList()
        {
            var rulesList = m_DataManager.GetRulesList();

            if (rulesList == null)
            {
                rulesList = new List<RuleWithSettingsMetadata>();
            }

            return rulesList;
        }

        public List<RsKvtData> GetSettingsList()
        {
            var settingsList = m_DataManager.GetRSList();

            if (settingsList == null)
            {
                settingsList = new List<RsKvtData>();
            }

            return settingsList;
        }

        public List<RsKvtData> GetLastCachedKeyList()
        {
            var settingsList = m_DataManager.GetLastCachedKeyList();

            if (settingsList == null)
            {
                settingsList = new List<RsKvtData>();
            }

            return settingsList;
        }

        public List<RuleWithSettingsMetadata> GetLastCachedRulesList()
        {
            var rulesList = m_DataManager.GetLastCachedRulesList();

            if (rulesList == null)
            {
                rulesList = new List<RuleWithSettingsMetadata>();
            }

            return rulesList;
        }

        public List<RsKvtData> GetSettingsListForRule(string ruleId)
        {
            var settingsListForRule = m_DataManager.GetSettingsListForRule(ruleId);

            if (settingsListForRule == null)
            {
                settingsListForRule = new List<RsKvtData>();
            }

            return settingsListForRule;
        }

        public void AddDefaultRule()
        {
            RuleWithSettingsMetadata defaultRule = new RuleWithSettingsMetadata(Guid.NewGuid().ToString(), "New Rule", false, RemoteConfigDataManager.defaultRulePriority, "", 100,
                new List<RsKvtData>(), null, null);
            m_DataManager.UpdateRule(defaultRule);
        }

        public void DeleteRule(string ruleId)
        {
            m_DataManager.DeleteRule(ruleId);
        }

        public void EnableOrDisableRule(string ruleId, bool enabled)
        {
            m_DataManager.EnableOrDisableRule(ruleId, enabled);
        }

        public bool HasRules()
        {
            return m_DataManager.HasRules();
        }

        public void UpdateRuleAttributes(string ruleId, RuleWithSettingsMetadata newRule)
        {
            m_DataManager.UpdateRuleAttributes(ruleId, newRule);
        }
        
        public RuleWithSettingsMetadata GetRuleAtIndex(int selectedRuleIndex)
        {
            return m_DataManager.GetRuleAtIndex(selectedRuleIndex);
        }

        public RuleWithSettingsMetadata GetRuleById(string ruleId)
        {
            return m_DataManager.GetRuleByID(ruleId);
        }

        public int GetEnvironmentsCount()
        {
            return m_DataManager.GetEnvironments().Count;
        }

        public string GetCurrentEnvironmentName()
        {
            return m_DataManager.GetCurrentEnvironmentName();
        }

        public GenericMenu BuildPopupListForEnvironments()
        {
            var menu = new GenericMenu();

            for (int i = 0; i < GetEnvironmentsCount(); i++)
            {
                string name = m_DataManager.GetEnvironments()[i].name;
                menu.AddItem(new GUIContent(name), name == m_DataManager.GetCurrentEnvironmentName(), EnvironmentSelectionCallback, name);
            }

            return menu;
        }

        private void EnvironmentSelectionCallback(object obj)
        {
            var envrionmentName = (string)obj;
            var env = m_DataManager.GetEnvironments().Find(x => x.name == envrionmentName);

            // change environment and fetch settings only if current settings are not modified
            if (CompareKeyValueEquality(GetSettingsList(), GetLastCachedKeyList()) &&
                CompareRulesEquality(GetRulesList(), GetLastCachedRulesList()))
            {
                m_DataManager.SetCurrentEnvironment(env);
                FetchDefaultEnvironment(m_DataManager.GetEnvironments());
            }
            else
            {
                if (EditorUtility.DisplayDialog(k_RCDialogUnsavedChangesTitle, k_RCDialogUnsavedChangesMessage, k_RCDialogUnsavedChangesOK, k_RCDialogUnsavedChangesCancel))
                {
                    m_DataManager.SetCurrentEnvironment(env);
                    FetchDefaultEnvironment(m_DataManager.GetEnvironments());
                }
            }
        }
        
        public void AddSettingToRule(string selectedRuleId, string entityId)
        {
            m_DataManager.AddSettingToRule(selectedRuleId, entityId);
        }

        public void Fetch()
        {
            m_IsLoading = true;
            FetchEnvironments();
        }

        private void FetchEnvironments()
        {
            RemoteConfigWebApiClient.fetchEnvironmentsFinished += FetchDefaultEnvironment;
            try
            {
                RemoteConfigWebApiClient.FetchEnvironments(Application.cloudProjectId);
            }
            catch
            {
                RemoteConfigWebApiClient.fetchEnvironmentsFinished -= FetchDefaultEnvironment;
                DoCleanUp();
            }
        }

        private void FetchDefaultEnvironment(List<Environment> environments)
		{
			RemoteConfigWebApiClient.fetchEnvironmentsFinished -= FetchDefaultEnvironment;
            RemoteConfigWebApiClient.getDefaultEnvironmentFinished += RemoteConfigWebApiClient_getDefaultEnvironmentFinished;
			if (SetEnvironmentData(environments))
			{
                try
                {
                    RemoteConfigWebApiClient.FetchDefaultEnvironment(Application.cloudProjectId);
                }
                catch
                {
                    RemoteConfigWebApiClient.getDefaultEnvironmentFinished -= RemoteConfigWebApiClient_getDefaultEnvironmentFinished;
                    DoCleanUp();
                }
            }
		}

        private void RemoteConfigWebApiClient_getDefaultEnvironmentFinished(DefaultEnvironmentResponseStruct defaultEnvironmentResponse)
        {
            m_DataManager.SetDefaultEnvironment(defaultEnvironmentResponse.id);
            RemoteConfigWebApiClient.getDefaultEnvironmentFinished -= RemoteConfigWebApiClient_getDefaultEnvironmentFinished;
            FetchSettings();
        }

        private void FetchSettings()
        {
            RemoteConfigWebApiClient.fetchConfigsFinished += OnFetchRemoteSettingsFinished;
            try
            {
                RemoteConfigWebApiClient.FetchConfigs(Application.cloudProjectId, m_DataManager.GetCurrentEnvironmentId());
            }
            catch
            {
                RemoteConfigWebApiClient.fetchConfigsFinished -= OnFetchRemoteSettingsFinished;
                DoCleanUp();
            }

        }

        private void FetchRules()
        {
            if (!string.IsNullOrEmpty(m_DataManager.configId))
            {
                RemoteConfigWebApiClient.fetchRulesFinished += OnFetchRulesFinished;
                try
                {
                    RemoteConfigWebApiClient.FetchRules(Application.cloudProjectId, m_DataManager.configId);
                }
                catch
                {
                    RemoteConfigWebApiClient.fetchRulesFinished -= OnFetchRulesFinished;
                    DoCleanUp();
                }
            }
        }

        public void Push()
        {

            if (m_DataManager.dataStoreStatus == RemoteConfigDataManager.m_DataStoreStatus.Error)
            {
                Debug.LogError("There are errors in the Local Data Rules and or Settings please resolve them before pushing changes");
            }
            else
            {
                string environmentId = m_DataManager.GetCurrentEnvironmentId();
                if (string.IsNullOrEmpty(m_DataManager.configId))
                {
                    RemoteConfigWebApiClient.postConfigRequestFinished += OnConfigPostFinishedPushHandler;
                    m_PostConfigEventSubscribed = true;
                    PushSettings(environmentId);
                }
                else
                {
                    PushSettings(environmentId);
                    PushAddedRules(environmentId);
                    PushUpdatedRules(environmentId);
                    PushDeletedRules(environmentId);
                }
            }
        }

        private void OnConfigPostFinishedPushHandler(string configId)
        {
            string environmentId = m_DataManager.GetCurrentEnvironmentId();
            m_DataManager.configId = configId;
            PushAddedRules(environmentId);
            PushUpdatedRules(environmentId);
            PushDeletedRules(environmentId);
            if (m_PostConfigEventSubscribed)
            {
                RemoteConfigWebApiClient.postConfigRequestFinished -= OnConfigPostFinishedPushHandler;
                m_PostConfigEventSubscribed = false;
            }
        }

        public void AddSetting()
        {
            RsKvtData setting = new RsKvtData(Guid.NewGuid().ToString(), new RemoteSettingsKeyValueType("Setting" + m_DataManager.settingsCount, "", ""));
            m_DataManager.AddSetting(setting);
        }

        private void OnRuleRequestSuccess(string requestType, string ruleId)
        {
            switch (requestType)
            {
                case UnityWebRequest.kHttpVerbPUT:
                    m_DataManager.RemoveRuleFromUpdatedRuleIDs(ruleId);
                    m_DataManager.SetLastCachedRulesList();
                    break;
                case UnityWebRequest.kHttpVerbDELETE:
                    m_DataManager.RemoveRuleFromDeletedRuleIDs(ruleId);
                    m_DataManager.SetLastCachedRulesList();
                    break;
            }
            DoCleanUp();
        }

        private void OnSettingsRequestFinished()
        {
            m_DataManager.SetLastCachedKeyList();
            DoCleanUp();
        }

        private void OnPostConfigRequestFinished(string configId)
        {
            m_DataManager.configId = configId;
            DoCleanUp();
        }

        private void OnFailedRequest(long errorCode, string errorMsg)
        {
            DoCleanUp();
        }

        List<RemoteSettingsKeyValueType> StripMetadataFromRSList(List<RsKvtData> rsKvtData)
        {
            List<RemoteSettingsKeyValueType> rsList = new List<RemoteSettingsKeyValueType>(rsKvtData.Count);
            for (int i = 0; i < rsKvtData.Count; i++)
            {
                rsList.Add(rsKvtData[i].rs);
            }
            return rsList;
        }

        Rule StringMetadataFromRule(RuleWithSettingsMetadata ruleWithSettingsMetadata)
        {
            return new Rule(ruleWithSettingsMetadata.id, ruleWithSettingsMetadata.name, ruleWithSettingsMetadata.enabled, ruleWithSettingsMetadata.priority,
                ruleWithSettingsMetadata.condition, ruleWithSettingsMetadata.rolloutPercentage, StripMetadataFromRSList(ruleWithSettingsMetadata.value),
                ruleWithSettingsMetadata.startDate, ruleWithSettingsMetadata.endDate);
        }

        private void PushSettings(string environmentId)
        {
            m_IsLoading = true;
            if (string.IsNullOrEmpty(m_DataManager.configId))
            {
                RemoteConfigWebApiClient.postConfigRequestFinished += OnPostConfigRequestFinished;
                m_PostSettingsEventSubscribed = true;
                try
                {
                    RemoteConfigWebApiClient.PostConfig(Application.cloudProjectId, environmentId, StripMetadataFromRSList(m_DataManager.GetRSList()));
                }
                catch
                {
                    DoCleanUp();
                }
            }
            else
            {
                RemoteConfigWebApiClient.settingsRequestFinished += OnSettingsRequestFinished;
                m_PutConfigsEventSubscribed = true;
                try
                {
                    RemoteConfigWebApiClient.PutConfig(Application.cloudProjectId, environmentId, m_DataManager.configId, StripMetadataFromRSList(m_DataManager.GetRSList()));
                }
                catch
                {
                    DoCleanUp();
                }
            }
        }

        private void PushAddedRules(string environmentId)
        {
            var addedRuleIDs = m_DataManager.GetAddedRulesIDs();
            if (addedRuleIDs.Count > 0)
            {
                m_IsLoading = true;
                foreach (var addedRuleID in addedRuleIDs)
                {
                    if (!m_PostAddRuleEventSubscribed)
                    {
                        RemoteConfigWebApiClient.postAddRuleFinished += OnPostAddRuleFinished;
                        m_PostAddRuleEventSubscribed = true;
                    }
                    try
                    {
                        RemoteConfigWebApiClient.PostAddRule(Application.cloudProjectId, environmentId, m_DataManager.configId, StringMetadataFromRule(m_DataManager.GetRuleByID(addedRuleID)));
                    }
                    catch
                    {
                        DoCleanUp();
                    }
                }
            }
        }

        private void PushUpdatedRules(string environmentId)
        {
            var updatedRuleIDs = m_DataManager.GetUpdatedRulesIDs();
            if (updatedRuleIDs.Count > 0)
            {
                m_IsLoading = true;
                if (!m_WebRequestReturnedEventSubscribed)
                {
                    RemoteConfigWebApiClient.ruleRequestSuccess += OnRuleRequestSuccess;
                    m_WebRequestReturnedEventSubscribed = true;
                }
                foreach (var updatedRuleID in updatedRuleIDs)
                {
                    try
                    {
                        RemoteConfigWebApiClient.PutEditRule(Application.cloudProjectId, environmentId, m_DataManager.configId, StringMetadataFromRule(m_DataManager.GetRuleByID(updatedRuleID)));
                    }
                    catch
                    {
                        DoCleanUp();
                    }
                }
            }
        }

        private void PushDeletedRules(string environmentId)
        {
            var deletedRuleIDs = m_DataManager.GetDeletedRulesIDs();
            if (deletedRuleIDs.Count > 0)
            {
                m_IsLoading = true;
                if (!m_WebRequestReturnedEventSubscribed)
                {
                    RemoteConfigWebApiClient.ruleRequestSuccess += OnRuleRequestSuccess;
                    m_WebRequestReturnedEventSubscribed = true;
                }
                foreach (var deletedRuleID in deletedRuleIDs)
                {
                    try
                    {
                        RemoteConfigWebApiClient.DeleteRule(Application.cloudProjectId, environmentId, deletedRuleID);
                    }
                    catch
                    {
                        DoCleanUp();
                    }
                }
            }
        }

        private void OnPostAddRuleFinished(RuleResponse ruleResponse, string oldRuleID)
        {
            var rule = m_DataManager.GetRuleByID(oldRuleID);
            m_DataManager.DeleteRule(oldRuleID);
            rule.id = ruleResponse.id;
            m_DataManager.UpdateRule(rule);
            m_DataManager.RemoveRuleFromAddedRuleIDs(rule.id);
            m_DataManager.SetLastCachedRulesList();
            DoCleanUp();
        }

        private void OnFetchRemoteSettingsFinished(RemoteConfigConfigData config)
        {
            DoCleanUp();
            RemoteConfigWebApiClient.fetchConfigsFinished -= OnFetchRemoteSettingsFinished;
            m_DataManager.SetRSDataStore(config);
            m_DataManager.SetLastCachedKeyList();
            FetchRules();
        }

        private void OnFetchRulesFinished(List<Rule> rules)
        {
            RemoteConfigWebApiClient.fetchRulesFinished -= OnFetchRulesFinished;
            m_DataManager.ClearRuleIDs();
            m_DataManager.SetRulesDataStore(rules);
            m_DataManager.SetLastCachedRulesList();
            DoCleanUp();
        }

        private void OnRulesDataStoreChanged()
        {
            rulesDataStoreChanged?.Invoke();
        }

        private void OnRemoteSettingDataStoreChanged()
        {
            remoteSettingsStoreChanged?.Invoke();
        }

        private void OnEnvironmentChanged()
        {
            m_IsLoading = true;
            environmentChanged?.Invoke();
        }

        private void DoCleanUp()
        {
            if (RemoteConfigWebApiClient.webRequestsAreDone)
            {
                if (m_PostAddRuleEventSubscribed)
                {
                    RemoteConfigWebApiClient.postAddRuleFinished -= OnPostAddRuleFinished;
                    m_PostAddRuleEventSubscribed = false;
                }
                if (m_WebRequestReturnedEventSubscribed)
                {
                    RemoteConfigWebApiClient.ruleRequestSuccess -= OnRuleRequestSuccess;
                    m_WebRequestReturnedEventSubscribed = false;
                }
                if (m_PostSettingsEventSubscribed)
                {
                    RemoteConfigWebApiClient.postConfigRequestFinished -= OnPostConfigRequestFinished;
                    m_PostSettingsEventSubscribed = false;
                }
                if (m_PutConfigsEventSubscribed)
                {
                    RemoteConfigWebApiClient.settingsRequestFinished -= OnSettingsRequestFinished;
                    m_PutConfigsEventSubscribed = false;
                }
                if (m_PostConfigEventSubscribed)
                {
                    RemoteConfigWebApiClient.postConfigRequestFinished -= OnConfigPostFinishedPushHandler;
                    m_PostConfigEventSubscribed = false;
                }
                m_IsLoading = false;
            }
        }

        public bool CompareKeyValueEquality(List<RsKvtData> objectListNew, List<RsKvtData> objectListOld)
        {
            return m_DataManager.CompareKeyValueEquality(objectListNew, objectListOld);
        }

        public bool CompareRulesEquality(List<RuleWithSettingsMetadata> ruleListNew, List<RuleWithSettingsMetadata> ruleListOld)
        {
            return m_DataManager.CompareRulesEquality(ruleListNew, ruleListOld);
        }

        public void UpdateRemoteSetting(RsKvtData oldItem, RsKvtData newItem)
        {
            m_DataManager.UpdateSetting(oldItem, newItem);
        }

        public void UpdateSettingForRule(string ruleId, RsKvtData updatedSetting)
        {
            m_DataManager.UpdateSettingForRule(ruleId, updatedSetting);
        }

        public void DeleteRemoteSetting(string entityId)
        {
            m_DataManager.DeleteSetting(entityId);
        }

        public void DeleteSettingFromRule(string selectedRuleId, string entityId)
        {
            m_DataManager.DeleteSettingFromRule(selectedRuleId, entityId);
        }
    }
}
