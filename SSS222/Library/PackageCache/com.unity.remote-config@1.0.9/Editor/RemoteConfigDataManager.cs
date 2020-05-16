using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEditor;
using System.Runtime.CompilerServices;

namespace Unity.RemoteConfig.Editor
{
    /// <summary>
    /// This class contains all methods needed to perform CRUD operations on the data objects.
    /// No other classes should ever interact with the data objects directly.
    /// </summary>
    internal class RemoteConfigDataManager
    {
        public event Action RulesDataStoreChanged;
        public event Action RemoteSettingDataStoreChanged;
        public event Action EnvironmentChanged; 
        
        const string k_DataStoreAssetFileName = "{0}.asset";
        const string k_DataStoreName = "RemoteConfigDataStoreAsset";
        const string k_PathToDataStore = "Assets/Editor/RemoteConfig/Data";
        const string k_CurrentEnvironment = "UnityRemoteConfigEditorEnvironment";
        
        RemoteConfigDataStore m_DataStore;
        
        public enum m_DataStoreStatus {
            Init = 0,
            UnSynchronized = 1,
            Synchronized = 2,
            Pending = 3,
            Error = 4
        };

        public const int defaultRulePriority = 1000;
        private const int maxRulePriority = defaultRulePriority;
        private const int minRulePriority = 0;
        
        public string configId { get { return m_DataStore.configId; } set { m_DataStore.configId = value; } }
      
        public static readonly List<string> rsTypes = new List<string> { "string", "bool", "float", "int", "long" };
        public m_DataStoreStatus dataStoreStatus { get; set; }
        public int settingsCount { get { return m_DataStore.rsKeyList.Count; } }

        /// <summary>
        /// Constructor: creates amd initalizes the Remote Config data store and restores the last selected environment.
        /// </summary>
        public RemoteConfigDataManager()
        {
            m_DataStore = CheckAndCreateDataStore();
            RestoreLastSelectedEnvironment(m_DataStore.currentEnvironmentId);
            dataStoreStatus = m_DataStoreStatus.Init;
        }

        public void SetDirty()
        {
            if(m_DataStore != null)
            {
                EditorUtility.SetDirty(m_DataStore);
            }
        }

        /// <summary>
        /// Returns the name of the last selected environment that is stored in EditorPrefs.
        /// </summary>
        /// <param name="defaultEnvironment"> The default environment id to be returned if last selected environment is not found</param>
        /// <returns> Name of last selected environment or defaultEnvironment if last selected is not found</returns>
        public string RestoreLastSelectedEnvironment(string defaultEnvironment)
        {
            return EditorPrefs.GetString(k_CurrentEnvironment + Application.cloudProjectId, defaultEnvironment);
        }
        
        /// <summary>
        /// Sets the name of the last selected environment and stores it in EditorPrefs.
        /// </summary>
        /// <param name="environmentName"> Name of environment to be stored</param>
        public void SetLastSelectedEnvironment (string environmentName)
        {
            EditorPrefs.SetString(k_CurrentEnvironment + Application.cloudProjectId, environmentName);
        }

        public void SetDefaultEnvironment(string environmentId)
        {
            for(int i = 0; i < m_DataStore.environments.Count; i++)
            {
                if(m_DataStore.environments[i].id == environmentId)
                {
                    if (m_DataStore.environments[i].name.Contains("(Default)"))
                    {
                        return;
                    }
                    var env = m_DataStore.environments[i];
                    env.name = env.name + " (Default)";
                    m_DataStore.environments[i] = env;
                }
            }
        }
        
        /// <summary>
        /// Checks for the existence of the Remote Config data store. Creates a new data store if one doesn't already exist
        /// and saves it to the AssetDatabase.
        /// </summary>
        /// <returns>Remote Config data store object</returns>
        public RemoteConfigDataStore CheckAndCreateDataStore()
        {
            string formattedPath = Path.Combine(k_PathToDataStore, string.Format(k_DataStoreAssetFileName, k_DataStoreName));
            if (AssetDatabase.FindAssets(k_DataStoreName).Length > 0)
            {
                if (AssetDatabase.LoadAssetAtPath(formattedPath, typeof(RemoteConfigDataStore)) == null)
                {
                    AssetDatabase.DeleteAsset(formattedPath);
                }
            }
            if (AssetDatabase.FindAssets(k_DataStoreName).Length == 0)
            {
                RemoteConfigDataStore asset = InitDataStore();
                CheckAndCreateAssetFolder(k_PathToDataStore);
                AssetDatabase.CreateAsset(asset, formattedPath);
                AssetDatabase.SaveAssets();
            }
            return AssetDatabase.LoadAssetAtPath(formattedPath, typeof(RemoteConfigDataStore)) as RemoteConfigDataStore;
        }

        private RemoteConfigDataStore InitDataStore()
        {
            RemoteConfigDataStore asset = ScriptableObject.CreateInstance<RemoteConfigDataStore>();
            asset.rsKeyList = new List<RsKvtData>();
            asset.currentEnvironmentId = null;
            asset.environments = new List<Environment>();
            asset.rulesList = new List<RuleWithSettingsMetadata>();
            asset.addedRulesIDs = new List<string>();
            asset.updatedRulesIDs = new List<string>();
            asset.deletedRulesIDs = new List<string>();

            return asset;
        }
        
        private void CheckAndCreateAssetFolder(string dataStorePath)
        {
            string[] folders = dataStorePath.Split('/');
            string assetPath = null;
            foreach (string folder in folders)
            {
                if (assetPath == null)
                {
                    assetPath = folder;
                }
                else
                {
                    string folderPath = Path.Combine(assetPath, folder);
                    if (!Directory.Exists(folderPath))
                    {
                        AssetDatabase.CreateFolder(assetPath, folder);
                    }
                    assetPath = folderPath;
                }
            }
        }

        /// <summary>
        /// Gets the Remote Settings list.
        /// </summary>
        /// <returns> List of RsKvtData objects</returns>
        public List<RsKvtData> GetRSList()
        {
            return m_DataStore.rsKeyList;
        }

        /// <summary>
        /// Gets the current environment name.
        /// </summary>
        /// <returns> Name of the current environment</returns>
        public string GetCurrentEnvironmentName()
        {
            for(int i = 0; i < m_DataStore.environments.Count; i++)
            {
                if(m_DataStore.environments[i].id == m_DataStore.currentEnvironmentId)
                {
                    return m_DataStore.environments[i].name;
                }
            }
            return null;
            //return m_DataStore.currentEnvironment;
        }

        /// <summary>
        /// Gets the current environment ID.
        /// </summary>
        /// <returns> ID of the current environment</returns>
        public string GetCurrentEnvironmentId()
        {
            return m_DataStore.currentEnvironmentId;
        }
        
        /// <summary>
        /// Gets a list of all the environments for the current working project.
        /// </summary>
        /// <returns> List of Environment objects containing the name and ID</returns>
        public List<Environment> GetEnvironments()
        {
            return m_DataStore.environments;
        }

        /// <summary>
        /// Gets the Rules list.
        /// </summary>
        /// <returns> List of RuleWithSettingsMetadata objects</returns>
        public List<RuleWithSettingsMetadata> GetRulesList()
        {
            return m_DataStore.rulesList;
        }

        /// <summary>
        /// Copies last list of rules from the Remote Config Data Store
        /// </summary>
        public void SetLastCachedRulesList()
        {
            m_DataStore.lastCachedRulesList = new List<RuleWithSettingsMetadata>(m_DataStore.rulesList.Count);

            for (var i = 0; i < m_DataStore.rulesList.Count; i++)
            {
                var newRule = new RuleWithSettingsMetadata(m_DataStore.rulesList[i].id,
                    m_DataStore.rulesList[i].name,
                    m_DataStore.rulesList[i].enabled,
                    m_DataStore.rulesList[i].priority,
                    m_DataStore.rulesList[i].condition,
                    m_DataStore.rulesList[i].rolloutPercentage,
                    new List<RsKvtData>(m_DataStore.rulesList[i].value),
                    m_DataStore.rulesList[i].startDate,
                    m_DataStore.rulesList[i].endDate);
                m_DataStore.lastCachedRulesList.Add(newRule);
            }
        }

        /// <summary>
        /// Gets last list of rules from the Remote Config Data Store
        /// </summary>
        public List<RuleWithSettingsMetadata> GetLastCachedRulesList()
        {
            return m_DataStore.lastCachedRulesList;
        }

        /// <summary>
        /// Gets the list of added Rule ID's.
        /// </summary>
        /// <returns> List of Rule ID's for new rules that were added since the last push</returns>
        public List<string> GetAddedRulesIDs()
        {
            return m_DataStore.addedRulesIDs;
        }
        
        /// <summary>
        /// Gets the list of updated Rule ID's.
        /// </summary>
        /// <returns> List of Rule ID's for rules that were updated since the last push</returns>
        public List<string> GetUpdatedRulesIDs()
        {
            return m_DataStore.updatedRulesIDs;
        }
        
        /// <summary>
        /// Gets the list of deleted Rule ID's.
        /// </summary>
        /// <returns> List of Rule ID's for rules that were deleted since the last push</returns>
        public List<string> GetDeletedRulesIDs()
        {
            return m_DataStore.deletedRulesIDs;
        }

        /// <summary>
        /// Gets the RuleWithSettingsMetadata at the given index in the rulesList.
        /// </summary>
        /// <param name="selectedRuleIndex">The index of the RuleWithSettingsMetadata we are getting from the rulesList</param>
        /// <returns>The RuleWithSettingsMetadata from the rulesList at the given index</returns>
        public RuleWithSettingsMetadata GetRuleAtIndex(int selectedRuleIndex)
        {
            return m_DataStore.rulesList[selectedRuleIndex];
        }

        /// <summary>
        /// Gets the RuleWithSettingsMetadata for the given Rule ID.
        /// </summary>
        /// <param name="ruleId">The ID of the RuleWithSettingsMetadata that that we want to get</param>
        /// <returns>The RuleWithSettingsMetadata from the rulesList for the given index</returns>
        public RuleWithSettingsMetadata GetRuleByID(string ruleId)
        {
            return m_DataStore.rulesList.Find(rule => rule.id == ruleId);
        }

        /// <summary>
        /// Sets the the current environment ID name.
        /// </summary>
        /// <param name="currentEnvironment">Current Environment object containing the current environment name and ID</param>
        public void SetCurrentEnvironment(Environment currentEnvironment)
        {
            m_DataStore.currentEnvironmentId = currentEnvironment.id;
            EnvironmentChanged?.Invoke();
        }

        /// <summary>
        /// Sets the list of Environment objects containing the name and ID.
        /// </summary>
        /// <param name="environments">List of Environment objects containing the name and ID</param>
        public void SetEnvironmentsList(List<Environment> environments)
        {
            m_DataStore.environments = environments;
        }

        /// <summary>
        /// Sets the config object on the Remote Config Data Store
        /// </summary>
        /// <param name="config">A config object representing the new config</param>
        public void SetRSDataStore(RemoteConfigConfigData config)
        {
            m_DataStore.rsKeyList = new List<RsKvtData>();
            foreach(var val in config.value)
            {
                m_DataStore.rsKeyList.Add(new RsKvtData(Guid.NewGuid().ToString(), val));
            }
            m_DataStore.configId = config.id;
        }

        /// <summary>
        /// Copies last key list of settings from the Remote Config Data Store
        /// </summary>
        public void SetLastCachedKeyList()
        {
            m_DataStore.rsLastCachedKeyList = new List<RsKvtData>(m_DataStore.rsKeyList.Count);
            m_DataStore.rsLastCachedKeyList = m_DataStore.rsKeyList.GetRange(0, m_DataStore.rsKeyList.Count);
        }

        /// <summary>
        /// Gets last key list of settings from the Remote Config Data Store
        /// </summary>
        public List<RsKvtData> GetLastCachedKeyList()
        {
            return m_DataStore.rsLastCachedKeyList;
        }

        /// <summary>
        /// Sets the Rules data store using a list of Rules.
        /// </summary>
        /// <param name="newRulesDataStore">A list of Rule objects</param>
        public void SetRulesDataStore(List<Rule> newRulesDataStore)
        {
            m_DataStore.rulesList = new List<RuleWithSettingsMetadata>();
            var defaultSettings = m_DataStore.rsKeyList;
            foreach(var rule in newRulesDataStore)
            {
                var settingsInRule = new List<RsKvtData>();
                foreach(var setting in rule.value)
                {
                    string entityId = null;
                    var defaultSettingIndex = defaultSettings.FindIndex(item => (item.rs.key == setting.key && item.rs.type == setting.type));
                    if(defaultSettingIndex == -1)
                    {
                        entityId = Guid.NewGuid().ToString();
                    }
                    else
                    {
                        entityId = defaultSettings[defaultSettingIndex].metadata.entityId;
                    }
                    settingsInRule.Add(new RsKvtData(entityId, setting));
                }
                m_DataStore.rulesList.Add(new RuleWithSettingsMetadata(rule.id, rule.name, rule.enabled, rule.priority, rule.condition, rule.rolloutPercentage, settingsInRule, rule.startDate, rule.endDate));
            }
            RulesDataStoreChanged?.Invoke();
            RemoteSettingDataStoreChanged?.Invoke();
        }

        /// <summary>
        /// Adds a rule to the Rules data store. This will add it to the rulesList.
        /// </summary>
        /// <param name="newRule">The RuleWithSettingsMetadata to be added</param>
        public void UpdateRule(RuleWithSettingsMetadata newRule)
        {
            if (newRule.name.StartsWith("New Rule"))
            {
                int maxNewRuleNumber = 1;
                List<RuleWithSettingsMetadata> newRuleList = m_DataStore.rulesList.FindAll(rule => rule.name.Contains(newRule.name));
                if (newRuleList.Count > 0)
                {
                    if (newRuleList[0].name == "New Rule" && newRuleList.Count == 1)
                    {
                        newRule.name = newRule.name.Insert(8, " " + maxNewRuleNumber);
                        AddRule(newRule);
                    }
                    else if (newRuleList.Count == 1)
                    {
                        maxNewRuleNumber = Int32.Parse(newRuleList[0].name.Replace("New Rule ", "")) + 1;
                        newRule.name = newRule.name.Insert(8, " " + maxNewRuleNumber);
                        AddRule(newRule);
                    }
                    else
                    {
                        var maxNewRule = newRuleList.OrderByDescending(rule => rule.name).First().name;
                        maxNewRuleNumber = Int32.Parse(maxNewRule.Replace("New Rule ", "")) + 1;
                        newRule.name = newRule.name.Insert(8, " " + maxNewRuleNumber);
                        AddRule(newRule);
                    }
                }
                else
                {
                    AddRule(newRule);
                }
            }
            else
            {
                AddRule(newRule);
            }
        }



        /// <summary>
        /// Adds a rule to the Rules data store. This will add it to the rulesList.
        /// </summary>
        /// <param name="newRule">The RuleWithSettingsMetadata to be added</param>
        public void AddRule(RuleWithSettingsMetadata newRule)
        {
            m_DataStore.rulesList.Add(newRule);
            RulesDataStoreChanged?.Invoke();
            RemoteSettingDataStoreChanged?.Invoke();
            AddRuleToAddedRuleIDs(newRule);
        }

        private void AddRuleToAddedRuleIDs(RuleWithSettingsMetadata newRule)
        {
            m_DataStore.addedRulesIDs.Add(newRule.id);
        }

        /// <summary>
        /// Deletes a rule from the Rules data store. This will delete it from the rulesList.
        /// </summary>
        /// <param name="deletedRuleID">ID of the Rule to be deleted</param>
        public void DeleteRule(string deletedRuleID)
        {
            AddRuleToDeletedRuleIDs(GetRuleByID(deletedRuleID));
            m_DataStore.rulesList.Remove(m_DataStore.rulesList.Find(rule => rule.id == deletedRuleID));
            RulesDataStoreChanged?.Invoke();
            RemoteSettingDataStoreChanged?.Invoke();
        }

        private void AddRuleToDeletedRuleIDs(RuleWithSettingsMetadata deletedRule)
        {
            bool ruleAdded = false;
            if (m_DataStore.addedRulesIDs.Contains(deletedRule.id))
            {
                m_DataStore.addedRulesIDs.Remove(deletedRule.id);
                ruleAdded = true;
            }

            if (m_DataStore.updatedRulesIDs.Contains(deletedRule.id))
            {
                m_DataStore.updatedRulesIDs.Remove(deletedRule.id);
            }

            if (!ruleAdded)
            {
                m_DataStore.deletedRulesIDs.Add(deletedRule.id);
            }
        }

        /// <summary>
        /// Checks to see if the given Rule's attributes are within the accepted range.
        /// </summary>
        /// <param name="rule">RuleWithSettingsMetadata object to be validated</param>
        /// <returns>true if the rule is valid and false if the rule is not valid</returns>
        public bool ValidateRule(RuleWithSettingsMetadata rule)
        {
            if (ValidateRulePriority(rule) && ValidateRuleName(rule))
            {
                dataStoreStatus = m_DataStoreStatus.UnSynchronized;
                return true;
            }
            else
            {
                dataStoreStatus = m_DataStoreStatus.Error;
                return false;
            }
        }

        /// <summary>
        /// Checks to see if the given rule's name is valid.
        /// </summary>
        /// <param name="rule">RuleWithSettingsMetadata object to be validated</param>
        /// <returns>true if the rule's name is valid, false if it is not valid</returns>
        public bool ValidateRuleName(RuleWithSettingsMetadata rule)
        {
            var duplicateIndex = m_DataStore.rulesList.FindIndex(rules => rules.name == rule.name);

            if (duplicateIndex == -1)
            {
                return true;
            }
            else if ( m_DataStore.rulesList[duplicateIndex].id == rule.id)
            {
                return true;
            }
            else
            { 
                Debug.LogWarning( m_DataStore.rulesList[duplicateIndex].name + " already exists. Rule names must be unique.");
                return false;
            }
        }

        /// <summary>
        /// Checks to see if the given rule's priority is valid.
        /// </summary>
        /// <param name="rule">RuleWithSettingsMetadata object to be validated</param>
        /// <returns>true if the rule's priority is valid, false if it is not valid</returns>
        public bool ValidateRulePriority(RuleWithSettingsMetadata rule)
        {	        
            if (rule.priority < 0 || rule.priority > 1000)
            {
                Debug.LogWarning("Rule: " + rule.name + " has an invalid priority. The set priority is " + rule.priority + ". The values for priority must be between " + minRulePriority + " and " + maxRulePriority);
                return false;
            }
            else
            {
                return true;	            
            }
        }

        /// <summary>
        /// Updates the attributes for a given rule. This will update the rule in the rulesList.
        /// </summary>
        /// <param name="ruleId">ID of the rule being updated</param>
        /// <param name="newRule">RuleWithSettingsMetadata object containing the new attributes</param>
        public void UpdateRuleAttributes(string ruleId, RuleWithSettingsMetadata newRule)
        {
            if (ValidateRule(newRule))
            {
                var ruleIndex = m_DataStore.rulesList.FindIndex(r => r.id == ruleId);
                m_DataStore.rulesList[ruleIndex] = newRule;
                RulesDataStoreChanged?.Invoke();
                AddRuleToUpdatedRuleIDs(newRule.id);
            }
        }
        
        /// <summary>
        /// Enables or disables the given rule.
        /// </summary>
        /// <param name="ruleId">ID of Rule to be enabled or disabled</param>
        /// <param name="enabled">true = enabled, false = disabled</param>
        public void EnableOrDisableRule(string ruleId, bool enabled)
        {
            var ruleIndex = m_DataStore.rulesList.FindIndex(r => r.id == ruleId);
            var rule = m_DataStore.rulesList[ruleIndex];
            rule.enabled = enabled;
            m_DataStore.rulesList[ruleIndex] = rule;
            AddRuleToUpdatedRuleIDs(ruleId);
            RulesDataStoreChanged?.Invoke();
        }

        /// <summary>
        /// Adds the given setting to the given rule.
        /// </summary>
        /// <param name="selectedRuleId">ID of the rule that the setting should be added to</param>
        /// <param name="entityId">EntityId of the setting to be added to the given rule</param>
        public void AddSettingToRule(string selectedRuleId, string entityId)
        {
            var setting = m_DataStore.rsKeyList.Find(s => s.metadata.entityId == entityId);
            m_DataStore.rulesList.Find(r => r.id == selectedRuleId).value.Add(setting);
            RemoteSettingDataStoreChanged?.Invoke();
            AddRuleToUpdatedRuleIDs(selectedRuleId);
        }

        /// <summary>
        /// Deletes the given setting to the given Rule.
        /// </summary>
        /// <param name="ruleId">ID of the rule that the setting should be deleted from</param>
        /// <param name="entityId">EntityId of the setting to be deleted from the given rule</param>
        public void DeleteSettingFromRule(string ruleId, string entityId)
        {
            var setting = m_DataStore.rulesList.Find(r => r.id == ruleId).value.Find(x => x.metadata.entityId == entityId);
            m_DataStore.rulesList.Find(r => r.id == ruleId).value.Remove(setting);
            RemoteSettingDataStoreChanged?.Invoke();
            AddRuleToUpdatedRuleIDs(ruleId);
        }

        /// <summary>
        /// Updates the value of the given setting for the given rule.
        /// </summary>
        /// <param name="ruleId">ID of the rule that the updated setting belong to</param>
        /// <param name="updatedSetting">A RsKvtData containing the updated value</param>
        public void UpdateSettingForRule(string ruleId, RsKvtData updatedSetting)
        {
            var ruleIndex = m_DataStore.rulesList.FindIndex(item => item.id == ruleId);
            var rule = m_DataStore.rulesList[ruleIndex];
            var setting = rule.value.Find(arg => arg.metadata.entityId == updatedSetting.metadata.entityId);
            var settingIndex = rule.value.IndexOf(setting);
            rule.value[settingIndex] = updatedSetting;
            m_DataStore.rulesList[ruleIndex] = rule;
            RemoteSettingDataStoreChanged?.Invoke();
            AddRuleToUpdatedRuleIDs(ruleId);
        }

        private void AddRuleToUpdatedRuleIDs(string updatedRule)
        {
            //this is a new rule, do nothing - the changes will get picked up the add rule request
            if (!m_DataStore.addedRulesIDs.Contains(updatedRule) && !m_DataStore.updatedRulesIDs.Contains(updatedRule))
            {
                m_DataStore.updatedRulesIDs.Add(updatedRule);
            }
        }

        /// <summary>
        /// Removes the given rule ID from the list of added rules ID's.
        /// </summary>
        /// <param name="ruleId">ID of the rule to be removed from the list of added rule ID's</param>
        public void RemoveRuleFromAddedRuleIDs(string ruleId)
        {
            m_DataStore.addedRulesIDs.Remove(ruleId);
        }
        
        /// <summary>
        /// Removes the given rule ID from the list of updated rule ID's.
        /// </summary>
        /// <param name="ruleId">ID of the rule to be removed from the list of updated rule ID's</param>
        public void RemoveRuleFromUpdatedRuleIDs(string ruleId)
        {
            m_DataStore.updatedRulesIDs.Remove(ruleId);
        }
        
        /// <summary>
        /// Removes the given rule ID from the list of deleted rule ID's.
        /// </summary>
        /// <param name="ruleId">ID of the rule to be remove from the list of deleted rule ID's</param>
        public void RemoveRuleFromDeletedRuleIDs(string ruleId)
        {
            m_DataStore.deletedRulesIDs.Remove(ruleId);
        }
        
        /// <summary>
        /// Clears the list of added rule ID's, list of updated rule ID's, and the list of deleted rule ID's.
        /// </summary>
        public void ClearRuleIDs()
        {
            m_DataStore.addedRulesIDs.Clear();
            m_DataStore.updatedRulesIDs.Clear();
            m_DataStore.deletedRulesIDs.Clear();
        }

        /// <summary>
        /// Adds a setting to the Remote Settings data store. This will add the setting to the rsKeyList.
        /// </summary>
        /// <param name="newSetting">The setting to be added</param>
        public void AddSetting(RsKvtData newSetting)
        {
            m_DataStore.rsKeyList.Add(newSetting);
            RemoteSettingDataStoreChanged?.Invoke();
        }

        /// <summary>
        /// Deletes a setting from the Remote Settings data store. This will delete the setting from the rsKeyList.
        /// </summary>
        /// <param name="entityId">The EntityId of the setting to be deleted</param>
        public void DeleteSetting(string entityId)
        {
            m_DataStore.rsKeyList.Remove(m_DataStore.rsKeyList.Find(s => s.metadata.entityId == entityId));
            RemoteSettingDataStoreChanged?.Invoke();
        }

        /// <summary>
        /// Updates a setting in the Remote Settings data store. This will update the setting in the rsKeyList.
        /// </summary>
        /// <param name="oldSetting">The RsKvtData of the setting to be updated</param>
        /// <param name="newSetting">The new setting with the updated fields</param>
        public void UpdateSetting(RsKvtData oldSetting, RsKvtData newSetting)
        {
			var isError = false;
            if(newSetting.rs.key.Length >= 255)
            {
                Debug.LogWarning(newSetting.rs.key + " is at the maximum length of 255 characters.");
				isError = true;
            }
            if(!isError)
            {
                var settingIndex = m_DataStore.rsKeyList.IndexOf(oldSetting);
                m_DataStore.rsKeyList[settingIndex] = newSetting;
                OnRemoteSettingUpdated(oldSetting.rs.key, newSetting);
                RemoteSettingDataStoreChanged?.Invoke();
            }
        }

        /// <summary>
        /// Checks to see if any rules exist
        /// </summary>
        /// <returns>true if there is at leave one rule and false if there are no rules</returns>
        public bool HasRules()
        {
            return m_DataStore.rulesList.Count > 0;
        }

        /// <summary>
        /// Checks if the given setting is being used by the given rule
        /// </summary>
        /// <param name="ruleId">ID of the rule that needs to be checked</param>
        /// <param name="rsEntityId">EntityId of the setting that needs to be checked</param>
        /// <returns>true if the given setting is being used by the given rule</returns>
        public bool IsSettingInRule(string ruleId, string rsEntityId)
        {
            var matchingRS = m_DataStore.rulesList.Find(r => r.id == ruleId).value.Where((arg) => arg.metadata.entityId == rsEntityId).ToList();
            if(matchingRS.Count > 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Returns list of settings for particular rule
        /// </summary>
        /// <param name="ruleId">ID of the rule </param>
        /// <returns>list of settings used by the given rule</returns>
        public List<RsKvtData> GetSettingsListForRule(string ruleId)
        {
            var settingsInRule = new List<RsKvtData>();
            var settings = m_DataStore.rsKeyList;

            for (int i = 0; i < settings.Count; i++)
            {
                var key = settings[i].rs.key;
                var type = settings[i].rs.type;
                var value = settings[i].rs.value;

                if (IsSettingInRule(ruleId, settings[i].metadata.entityId))
                {
                    settingsInRule.Add(new RsKvtData(settings[i].metadata.entityId, new RemoteSettingsKeyValueType(key, type, value)));
                }

            }
            return settingsInRule;
        }
        
        /// <summary>
        /// compares two lists of RsKvtData
        /// </summary>
        /// <param name="objectListNew">first list to compare </param>
        /// <param name="objectListOld">second list to compare </param>
        /// <returns>true if lists are equal</returns>
        public bool CompareKeyValueEquality(List<RsKvtData> objectListNew, List<RsKvtData> objectListOld)
        {

            if (objectListOld.Count != objectListNew.Count)
            {
                return false;
            }

            for (var i = 0; i < objectListNew.Count; i++)
            {

                if (!objectListOld.Contains(objectListNew[i]))
                {
                    return false;
                }

                if (!objectListOld[i].Equals(objectListNew[i]))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// compares two lists of RuleWithSettingsMetadata type
        /// </summary>
        /// <param name="ruleListNew">first list to compare </param>
        /// <param name="ruleListOld">second list to compare </param>
        /// <returns>true if lists are equal</returns>
        public bool CompareRulesEquality(List <RuleWithSettingsMetadata> ruleListNew, List <RuleWithSettingsMetadata> ruleListOld)
        {
            if (ruleListNew.Count != ruleListOld.Count)
            {
                return false;
            }

            for (var i = 0; i < ruleListNew.Count; i++)
            {

                if (!ruleListOld.Contains(ruleListNew[i]))
                {
                    return false;
                }

                if (!ruleListOld[i].Equals(ruleListNew[i]))
                {
                    return false;
                }
            }

            return true;
        }

        private void OnRemoteSettingUpdated(string oldSettingEntityId, RsKvtData newRS)
        {
            for(int i = 0; i < m_DataStore.rulesList.Count; i++)
            {
                var rule = m_DataStore.rulesList[i];
                for(int j = 0; j < rule.value.Count; j++)
                {
                    var setting = rule.value[j];
                    if (setting.metadata.entityId == oldSettingEntityId)
                    {
                        setting.rs.key = newRS.rs.key;
                        setting.rs.type = newRS.rs.type;
                        rule.value[j] = setting;
                    }
                }

                m_DataStore.rulesList[i] = rule;
            }
        }
    }
}