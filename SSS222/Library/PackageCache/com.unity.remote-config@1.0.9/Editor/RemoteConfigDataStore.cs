using System.Collections.Generic;
using UnityEngine;

namespace Unity.RemoteConfig.Editor
{
    internal class RemoteConfigDataStore : ScriptableObject
    {
        // Data stores for Remote Setings
        public List<RsKvtData> rsKeyList;
        public List<RsKvtData> rsLastCachedKeyList;
        public string configId;

        public string currentEnvironmentId;
        public List<Environment> environments;

        // Data stores for Rules
        public List<RuleWithSettingsMetadata> rulesList;
        public List<RuleWithSettingsMetadata> lastCachedRulesList;

        public List<string> addedRulesIDs;
        public List<string> updatedRulesIDs;
        public List<string> deletedRulesIDs;
    }
}