using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEditor;

namespace Unity.RemoteConfig.Editor
{
    /// <summary>
    /// This is a utility class for Remote Config to handle all Remote Config CRUD operations.
    /// </summary>
    internal static class RemoteConfigWebApiClient
    {
        public static event Action<List<Environment>> fetchEnvironmentsFinished;
        public static event Action<RemoteConfigConfigData> fetchConfigsFinished;
        public static event Action<List<Rule>> fetchRulesFinished;
        public static event Action<RuleResponse, string> postAddRuleFinished;
        public static event Action<string, string> ruleRequestSuccess;
        public static event Action settingsRequestFinished;
        public static event Action<string> postConfigRequestFinished;
        public static event Action<long, string> rcRequestFailed;
        public static event Action<DefaultEnvironmentResponseStruct> getDefaultEnvironmentFinished;

        static List<IEnumerator<AsyncOperation>> m_WebRequestEnumerators = new List<IEnumerator<AsyncOperation>>();

        static bool m_UpdateListenerAlreadyAdded;

        private static string m_NoEnvErrorMsg = "There is no currently selected environment. Aborting operation.";
        private static string m_NoConfigId = "There is no config ID for this config. Aborting operation.";
        private static string m_NoCloudProjectIdErrorMsg = "This app does not have a cloud project ID, please go to Window > Services, and follow the prompts to associate this project with a Unity Organization";

        /// <summary>
        /// Checks if there are any unfinished web requests. Returns true if all web requests are done.
        /// </summary>
        public static bool webRequestsAreDone
        {
            get { return m_WebRequestEnumerators.Count == 0; }
        }

        public static void FetchDefaultEnvironment(string cloudProjectId, Action<Exception> responseParseErrorCallback = null)
        {
            if (!IsStringNullOrEmpty(cloudProjectId, m_NoCloudProjectIdErrorMsg))
            {
                m_WebRequestEnumerators.Add(_FetchDefaultEnvironment(cloudProjectId, responseParseErrorCallback));
                AddUpdateListenerIfNeeded();
            }
        }

        /// <summary>
        /// Fetches all environments for the current project.
        /// </summary>
        /// <param name="cloudProjectId">Cloud Project ID for this Unity application.</param>
        /// <param name="responseParseErrorCallback">Optional callback for parsing errors.</param>
        public static void FetchEnvironments(string cloudProjectId, Action<Exception> responseParseErrorCallback = null)
        {
            if (!IsStringNullOrEmpty(cloudProjectId, m_NoCloudProjectIdErrorMsg))
            {
                m_WebRequestEnumerators.Add(_FetchEnvironments(cloudProjectId, responseParseErrorCallback));
                AddUpdateListenerIfNeeded();
            }
        }

        /// <summary>
        /// Fetches all configs for the given environment ID.
        /// </summary>
        /// <param name="cloudProjectId">Cloud Project ID for this Unity application.</param>
        /// <param name="environmentId">ID of the environment that we want to fetch configs for</param>
        /// <param name="responseParseErrorCallback">Optional callback for parsing errors.</param>
        public static void FetchConfigs(string cloudProjectId, string environmentId, Action<Exception> responseParseErrorCallback = null)
        {
            if (!IsStringNullOrEmpty(cloudProjectId, m_NoCloudProjectIdErrorMsg) && !IsStringNullOrEmpty(environmentId, m_NoEnvErrorMsg))
            {
                m_WebRequestEnumerators.Add(_FetchConfigs(cloudProjectId, environmentId, responseParseErrorCallback));
                AddUpdateListenerIfNeeded();
            }
        }

        /// <summary>
        /// Fetches all rules for the given config ID.
        /// </summary>
        /// <param name="cloudProjectId">Cloud Project ID for this Unity application.</param>
        /// <param name="configId">ID of the config that we want to fetch rules for</param>
        /// <param name="responseParseErrorCallback">Optional callback for parsing errors.</param>
        public static void FetchRules(string cloudProjectId, string configId, Action<Exception> responseParseErrorCallback = null)
        {
            if (!IsStringNullOrEmpty(cloudProjectId, m_NoCloudProjectIdErrorMsg) && !IsStringNullOrEmpty(configId, m_NoConfigId))
            {
                m_WebRequestEnumerators.Add(_FetchRules(cloudProjectId, configId, responseParseErrorCallback));
                AddUpdateListenerIfNeeded();
            }
        }

        /// <summary>
        /// Pushes updates to the given existing config to the server for the given environment.
        /// </summary>
        /// <param name="cloudProjectId">Cloud Project ID for this Unity application.</param>
        /// <param name="environmentId">ID of the environment containing the config to be pushed.</param>
        /// <param name="configId">ID of the config to be pushed.</param>
        /// <param name="configValue">List of settings to be pushed.</param>
        public static void PutConfig(string cloudProjectId, string environmentId, string configId, List<RemoteSettingsKeyValueType> configValue)
        {
            if (!IsStringNullOrEmpty(cloudProjectId, m_NoCloudProjectIdErrorMsg) && !IsStringNullOrEmpty(environmentId, m_NoEnvErrorMsg) && !IsStringNullOrEmpty(configId, m_NoConfigId))
            {
                var payload = SerializeConfigStruct(environmentId, configValue);
                m_WebRequestEnumerators.Add(_PutConfig(cloudProjectId, configId, payload));
                AddUpdateListenerIfNeeded();
            }
        }

        /// <summary>
        /// Pushes a new config to the server in the given environment.
        /// </summary>
        /// <param name="cloudProjectId">Cloud Project ID for this Unity application.</param>
        /// <param name="environmentId">ID of the environment containing the config to be pushed.</param>
        /// <param name="configValue">List of settings to be pushed.</param>
        /// <param name="responseParseErrorCallback">Optional callback for parsing errors.</param>
        public static void PostConfig(string cloudProjectId, string environmentId, List<RemoteSettingsKeyValueType> configValue, Action<Exception> responseParseErrorCallback = null)
        {
            if (!IsStringNullOrEmpty(cloudProjectId, m_NoCloudProjectIdErrorMsg) && !IsStringNullOrEmpty(environmentId, m_NoEnvErrorMsg))
            {
                var payload = SerializeConfigStruct(environmentId, configValue);
                m_WebRequestEnumerators.Add(_PostConfig(cloudProjectId, payload, responseParseErrorCallback));
                AddUpdateListenerIfNeeded();
            }
        }

        /// <summary>
        /// Sends a POST request to add a new rule to the given config in the given environment.
        /// </summary>
        /// <param name="cloudProjectId">Cloud Project ID for this Unity application.</param>
        /// <param name="environmentId">ID of the environment where the new rule was added</param>
        /// <param name="configId">ID of the config containing this rule</param>
        /// <param name="rule">The rule that was added</param>
        /// <param name="responseParseErrorCallback">Optional callback for parsing errors.</param>
        public static void PostAddRule(string cloudProjectId, string environmentId, string configId, Rule rule, Action<Exception> responseParseErrorCallback = null)
        {
            if (!IsStringNullOrEmpty(cloudProjectId, m_NoCloudProjectIdErrorMsg) && !IsStringNullOrEmpty(configId, m_NoEnvErrorMsg))
            {
                var oldRuleId = rule.id;
                var payload = SerializeRuleWebRequest(environmentId, configId, rule);
                m_WebRequestEnumerators.Add(_PostAddRule(cloudProjectId, payload, oldRuleId, responseParseErrorCallback));
                AddUpdateListenerIfNeeded();
            }
        }

        /// <summary>
        /// Sends a PUT request to update a rule in the given environment and config.
        /// </summary>
        /// <param name="cloudProjectId">Cloud Project ID for this Unity application.</param>
        /// <param name="environmentId">ID of the environment where the updated rule exists</param>
        /// <param name="configId">ID of the config where the updated rule exists</param>
        /// <param name="rule">The updated rule with the new attributes</param>
        public static void PutEditRule(string cloudProjectId, string environmentId, string configId, Rule rule)
        {
            if (!IsStringNullOrEmpty(cloudProjectId, m_NoCloudProjectIdErrorMsg) && !IsStringNullOrEmpty(environmentId, m_NoEnvErrorMsg))
            {
                var payload = SerializeRuleWebRequest(environmentId, configId, rule);
                m_WebRequestEnumerators.Add(_PutEditRule(cloudProjectId, rule.id, payload));
                AddUpdateListenerIfNeeded();
            }
        }

        /// <summary>
        /// Delete a rule from the given environment.
        /// </summary>
        /// <param name="cloudProjectId">Cloud Project ID for this Unity application.</param>
        /// <param name="environmentId">ID of the environment where the rule was deleted from</param>
        /// <param name="ruleId">ID of the deleted rule</param>
        public static void DeleteRule(string cloudProjectId, string environmentId, string ruleId)
        {
            if (!IsStringNullOrEmpty(cloudProjectId, m_NoCloudProjectIdErrorMsg) && !IsStringNullOrEmpty(environmentId, m_NoEnvErrorMsg))
            {
                m_WebRequestEnumerators.Add(_DeleteRule(cloudProjectId, ruleId));
                AddUpdateListenerIfNeeded();
            }

        }

        private static IEnumerator<AsyncOperation> _FetchDefaultEnvironment(string cloudProjectId, Action<Exception> responseParseErrorCallback = null)
        {
            string url = string.Format(RemoteConfigEditorEnvConf.getDefaultEnvironmentPath, cloudProjectId);
            var request = Authorize(UnityWebRequest.Get(url));
            yield return request.SendWebRequest();

            CleanupCurrentRequest();

            if(request.isNetworkError || request.isHttpError)
            {
                Debug.LogWarning("Failed to fetch default environment: " + request.error + " and with response code " + request.responseCode);
                rcRequestFailed?.Invoke(request.responseCode, request.error);
                yield break;
            }

            ParseDefaultEnvironment(request.downloadHandler.text, responseParseErrorCallback);
        }

        private static IEnumerator<AsyncOperation> _FetchEnvironments(string cloudProjectId, Action<Exception> responseParseErrorCallback = null)
        {
            string url = string.Format(RemoteConfigEditorEnvConf.environmentPath, cloudProjectId);
            var request = Authorize(UnityWebRequest.Get(url));
            yield return request.SendWebRequest();

            CleanupCurrentRequest();

            if (request.isNetworkError || request.isHttpError)
            {
                Debug.LogWarning("Failed to fetch remote configurations: " + request.error + " and with response code " + request.responseCode);
                rcRequestFailed?.Invoke(request.responseCode, request.error);
                yield break;
            }
            ParseEnvironments(request.downloadHandler.text, responseParseErrorCallback);
        }

        private static IEnumerator<AsyncOperation> _FetchConfigs(string cloudProjectId, string environmentId, Action<Exception> responseParseErrorCallback = null)
        {
            string remoteSettingsUrl = string.Format(RemoteConfigEditorEnvConf.getConfigPath, cloudProjectId, environmentId);
            var request = Authorize(UnityWebRequest.Get(remoteSettingsUrl));
            yield return request.SendWebRequest();

            CleanupCurrentRequest();

            if (request.isNetworkError || request.isHttpError)
            {
                Debug.LogWarning("Failed to fetch remote config: " + request.error);
                rcRequestFailed?.Invoke(request.responseCode, request.error);
                yield break;
            }
            string remoteSettingsJson = request.downloadHandler.text;
            ParseConfigs(remoteSettingsJson, responseParseErrorCallback);
        }

        private static IEnumerator<AsyncOperation> _PutConfig(string cloudProjectId, string configId, string payload)
        {
            string url = string.Format(RemoteConfigEditorEnvConf.putConfigPath, cloudProjectId, configId);

            var request = Authorize(UnityWebRequest.Put(url, payload));
            yield return request.SendWebRequest();

            CleanupCurrentRequest();

            if (request.isNetworkError || request.isHttpError)
            {
                Debug.LogWarning("Failed to push remote config: " + request.error + ". Error message:\n" + request.downloadHandler.text);
                rcRequestFailed?.Invoke(request.responseCode, request.error);
            }
            settingsRequestFinished?.Invoke();
        }

        private static IEnumerator<AsyncOperation> _PostConfig(string cloudProjectId, string payload, Action<Exception> responseParseErrorCallback = null)
        {
            string url = string.Format(RemoteConfigEditorEnvConf.postConfigPath, cloudProjectId);

            var request = new UnityWebRequest(url, UnityWebRequest.kHttpVerbPOST);
            request.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(payload));
            request = Authorize(request);
            request.downloadHandler = new DownloadHandlerBuffer();
            yield return request.SendWebRequest();

            CleanupCurrentRequest();

            string configId = null;

            if (request.isNetworkError || request.isHttpError)
            {
                Debug.LogWarning("Failed to push remote config: " + request.error);
                rcRequestFailed?.Invoke(request.responseCode, request.error);
            }
            else
            {
                string postConfigResponseJson = request.downloadHandler.text;
                configId = ParsePostConfigResponse(postConfigResponseJson, responseParseErrorCallback).id;
            }
            postConfigRequestFinished?.Invoke(configId);
        }

        private static IEnumerator<AsyncOperation> _FetchRules(string cloudProjectId, string configId, Action<Exception> responseParseErrorCallback = null)
        {
            string url = string.Format(RemoteConfigEditorEnvConf.multiRulesPath, cloudProjectId, configId);
            var request = Authorize(UnityWebRequest.Get(url));
            yield return request.SendWebRequest();

            CleanupCurrentRequest();

            if (request.isNetworkError || request.isHttpError)
            {
                Debug.LogWarning("Failed to GET all rules: " + request.error);
                rcRequestFailed?.Invoke(request.responseCode, request.error);
            }
            else
            {
                ParseRules(request.downloadHandler.text, responseParseErrorCallback);
            }
        }

        private static IEnumerator<AsyncOperation> _PostAddRule(string cloudProjectId, string payload, string oldRuleId, Action<Exception> responseParseErrorCallback = null)
        {
            string url = string.Format(RemoteConfigEditorEnvConf.postRulePath, cloudProjectId);

            //Make sure the POST request doesn't send up the data as a form
            var request = new UnityWebRequest(url, UnityWebRequest.kHttpVerbPOST);
            request.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(payload));
            request = Authorize(request);
            request.downloadHandler = new DownloadHandlerBuffer();
            yield return request.SendWebRequest();

            CleanupCurrentRequest();

            if (request.isNetworkError || request.isHttpError)
            {
                LogBadRequest(request);
                rcRequestFailed?.Invoke(request.responseCode, request.error);
            }
            else
            {
                string addRuleResponseJson = request.downloadHandler.text;
                ParseAddRuleResponse(addRuleResponseJson, oldRuleId, responseParseErrorCallback);
                ruleRequestSuccess?.Invoke(UnityWebRequest.kHttpVerbPOST, oldRuleId);
            }
        }

        private static IEnumerator<AsyncOperation> _PutEditRule(string cloudProjectId, string ruleId, string payload)
        {
            string url = string.Format(RemoteConfigEditorEnvConf.singleRulePath, cloudProjectId, ruleId);

            var request = Authorize(UnityWebRequest.Put(url, payload));
            yield return request.SendWebRequest();

            CleanupCurrentRequest();

            if (request.isNetworkError || request.isHttpError)
            {
                LogBadRequest(request);
                rcRequestFailed?.Invoke(request.responseCode, request.error);
            }
            else
            {
                ruleRequestSuccess?.Invoke(UnityWebRequest.kHttpVerbPUT, ruleId);
            }
        }

        private static IEnumerator<AsyncOperation> _DeleteRule(string cloudProjectId, string ruleId)
        {
            string url = string.Format(RemoteConfigEditorEnvConf.singleRulePath, cloudProjectId, ruleId);

            var request = Authorize(UnityWebRequest.Delete(url));
            yield return request.SendWebRequest();

            CleanupCurrentRequest();

            if (request.isNetworkError || request.isHttpError)
            {
                switch (request.responseCode)
                {
                    case 400:
                        Debug.LogWarning("error: " + request.error + "\n" + "message: " + " Invalid projectId || environmentId || ruleId || JSON Parse Error");
                        break;
                    case 401:
                        Debug.LogWarning("error: " + request.error + "\n" + "message: " + " Unauthorized");
                        break;
                    case 404:
                        Debug.LogWarning("error: " + request.error + "\n" + "message: " + " Could not find requested Rule resource");
                        break;
                    case 409:
                        Debug.LogWarning("error: " + request.error + "\n" + "message: " + " Conflict: Rule is enabled and can't be deleted");
                        break;
                    default:
                        Debug.LogWarning("error: " + request.error);
                        break;
                }

                rcRequestFailed?.Invoke(request.responseCode, request.error);
            }
            else
            {
                ruleRequestSuccess?.Invoke(UnityWebRequest.kHttpVerbDELETE, ruleId);
            }
        }

        private static void ParseDefaultEnvironment(string json, Action<Exception> responseParseErrorCallback)
        {
            DefaultEnvironmentResponseStruct defaultEnvironmentResponse;
            try
            {
                defaultEnvironmentResponse = JsonUtility.FromJson<DefaultEnvironmentResponseStruct>(json);
            }
            catch(Exception e)
            {
                Debug.LogWarning("Default environments response was not valid JSON:\n" + json + "\n" + e);
                responseParseErrorCallback?.Invoke(e);
                defaultEnvironmentResponse = new DefaultEnvironmentResponseStruct();
            }

            getDefaultEnvironmentFinished?.Invoke(defaultEnvironmentResponse);
        }

        private static void ParseRules(string json, Action<Exception> responseParseErrorCallback)
        {
            RulesResponse rulesResponse;
            try
            {
                rulesResponse = JsonUtility.FromJson<RulesResponse>(json);
            }
            catch (Exception e)
            {
                Debug.LogWarning("Rules response was not valid JSON:\n" + json + "\n" + e);
                responseParseErrorCallback?.Invoke(e);
                rulesResponse = new RulesResponse();
            }

            List<Rule> rules = new List<Rule>();

            foreach (RuleResponseWebStruct ruleResponse in rulesResponse.rules)
            {
                var rule = new Rule();
                rule.id = ruleResponse.id;
                rule.name = ruleResponse.name;
                rule.rolloutPercentage = ruleResponse.rolloutPercentage;
                rule.condition = ruleResponse.condition;
                rule.enabled = ruleResponse.enabled;
                rule.type = ruleResponse.ruleType;
                rule.value = new List<RemoteSettingsKeyValueType>();
                rule.priority = ruleResponse.priority;
                rule.startDate = ruleResponse.startDate;
                rule.endDate = ruleResponse.endDate;

                if (ruleResponse.value.Count > 0)
                {
                    foreach (var setting in ruleResponse.value)
                    {
                        //how to get the guid out of here?
                        rule.value.Add(new RemoteSettingsKeyValueType(setting.key, setting.type, setting.values[0]));
                    }
                }
                rules.Add(rule);
            }

            fetchRulesFinished?.Invoke(rules);
        }

        private static void ParseEnvironments(string json, Action<Exception> responseParseErrorCallback)
        {
            List<Environment> environments;
            try
            {
                environments = JsonUtility.FromJson<EnvironmentsHolder>(json).environments;
            }
            catch (Exception e)
            {
                Debug.LogWarning("Remote Config response was not valid JSON:\n" + json + "\n" + e);
                responseParseErrorCallback?.Invoke(e);
                environments = null;
            }
            fetchEnvironmentsFinished?.Invoke(environments);
        }

        private static void ParseConfigs(string json, Action<Exception> responseParseErrorCallback)
        {
            RemoteConfigConfigData config = new RemoteConfigConfigData()
            {
                value = new List<RemoteSettingsKeyValueType>()
            };
            try
            {
                var configData = JsonUtility.FromJson<GetConfigsWebStruct>(json);
                for (int i = 0; i < configData.configs.Count; i++)
                {
                    if (configData.configs[i].type == "settings")
                    {
                        config = configData.configs[i];
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning("Remote Config response was not valid JSON:\n" + json + "\n" + e);
                responseParseErrorCallback?.Invoke(e);
            }

            fetchConfigsFinished?.Invoke(config);
        }

        private static PostConfigWebResponseStruct ParsePostConfigResponse(string json, Action<Exception> responseParseErrorCallback)
        {
            PostConfigWebResponseStruct response;
            try
            {
                response = JsonUtility.FromJson<PostConfigWebResponseStruct>(json);
            }
            catch (Exception e)
            {
                response = new PostConfigWebResponseStruct();
                Debug.LogWarning("POST config reponse was not valid JSON:\n" + json + "\n" + e);
                responseParseErrorCallback?.Invoke(e);
            }
            return response;
        }

        private static void ParseAddRuleResponse(string json, string oldRuleId, Action<Exception> responseParseErrorCallback)
        {
            RuleResponse response;
            try
            {
                response = JsonUtility.FromJson<RuleResponse>(json);
            }
            catch (Exception e)
            {
                response = new RuleResponse();
                Debug.LogWarning("POST Add Rule reponse was not valid JSON:\n" + json + "\n" + e);
                responseParseErrorCallback?.Invoke(e);
            }
            postAddRuleFinished?.Invoke(response, oldRuleId);
        }

        private static RuleWebRequestStruct ConvertRuleToRuleWebRequestStruct(Rule rule, string environmentId, string configId, string guid, string guid2)
        {
            return new RuleWebRequestStruct(rule.name, rule.condition, rule.priority, rule.rolloutPercentage,
                guid, string.IsNullOrEmpty(rule.type) ? guid2 : rule.type, rule.enabled, environmentId, configId, rule.startDate, rule.endDate);
        }

        private static string SerializeRSList(List<RemoteSettingsKeyValueType> settings)
        {
            var settingString = "[";
            for (int i = 0; i < settings.Count; i++)
            {
                settingString += ConvertSettingToRuleSetting(settings[i]);
                if (i != settings.Count - 1)
                {
                    settingString += ",";
                }
            }

            return settingString += "]";
        }

        private static string SerializeWebRSList(List<IRemoteSettingsWebPayload> rs)
        {
            string json = "[";
            string jsonFooter = "]";

            List<string> stringList = new List<string>(rs.Count);
            for (int i = 0; i < rs.Count; i++)
            {

                var tempJson = JsonUtility.ToJson(rs[i]);

                if (rs[i].GetType() == typeof(RemoteSettingsFloatKeyValueWebPayload))
                {
                    var fValue = ((RemoteSettingsFloatKeyValueWebPayload)(rs[i])).value;
                    string[] stringSeparators = { fValue };
                    var splitJson = tempJson.Split(stringSeparators, StringSplitOptions.None);
                    tempJson = splitJson[0].TrimEnd('"') + fValue + "}";
                }

                stringList.Add(tempJson);
            }

            for (int i = 0; i < stringList.Count; i++)
            {
                json += stringList[i];
                if (i != stringList.Count - 1)
                {
                    json += ",";
                }
            }
            json += jsonFooter;
            return json;
        }

        private static string SerializeConfigStruct(string environmentId, List<RemoteSettingsKeyValueType> configValue)
        {
            var configValPayloadString = SerializeWebRSList(ConvertRSToWebRS(configValue));
            var guid = Guid.NewGuid().ToString();
            var payloadStruct = new ConfigWebRequestStruct()
            {
                environmentId = environmentId,
                type = "settings",
                value = guid
            };
            var payload = JsonUtility.ToJson(payloadStruct);
            return payload.Replace(@"""" + guid + @"""", configValPayloadString);
        }

        private static List<IRemoteSettingsWebPayload> ConvertRSToWebRS(List<RemoteSettingsKeyValueType> rs)
        {
            List<IRemoteSettingsWebPayload> retRs = new List<IRemoteSettingsWebPayload>();
            foreach (RemoteSettingsKeyValueType remoteSettings in rs)
            {
                try
                {
                    switch (remoteSettings.type)
                    {
                        case "string":
                            retRs.Add(new RemoteSettingsStringKeyValueWebPayload(remoteSettings.key, remoteSettings.value));
                            break;
                        case "int":
                            retRs.Add(new RemoteSettingsIntKeyValueWebPayload(remoteSettings.key, int.Parse(remoteSettings.value)));
                            break;
                        case "float":
                            retRs.Add(new RemoteSettingsFloatKeyValueWebPayload(remoteSettings.key, float.Parse(remoteSettings.value)));
                            break;
                        case "bool":
                            retRs.Add(new RemoteSettingsBoolKeyValueWebPayload(remoteSettings.key, bool.Parse(remoteSettings.value)));
                            break;
                        case "long":
                            retRs.Add(new RemoteSettingsLongKeyValueWebPayload(remoteSettings.key, long.Parse(remoteSettings.value)));
                            break;
                        default:
                            throw new ArgumentException(@"Cannot parse key : """ + remoteSettings.key + @""" of type: """ + remoteSettings.type + @""" because it is unsupported");
                    }
                }
                catch (Exception ex)
                {
                    if(ex.GetType() == typeof(ArgumentException))
                    {
                        throw ex;
                    }
                    throw new ArgumentException(@"Parsing key """ + remoteSettings.key + @""" to type " + remoteSettings.type + " failed with exception: " + ex.Message);
                }
            }
            return retRs;
        }

        private static string ConvertSettingToRuleSetting(RemoteSettingsKeyValueType remoteSetting)
        {
            try
            {
                switch (remoteSetting.type)
                {
                    case "string":
                        return StringSerializer(new RuleRemoteSettingsStringKeyValuesType(remoteSetting.key, remoteSetting.type, new List<string> { remoteSetting.value }));
                    case "int":
                        return IntSerializer(new RuleRemoteSettingsIntKeyValuesType(remoteSetting.key, remoteSetting.type, new List<int> { int.Parse(remoteSetting.value) }));
                    case "float":
                        return FloatSerializer(new RuleRemoteSettingsFloatKeyValuesType(remoteSetting.key, remoteSetting.type, new List<float> { float.Parse(remoteSetting.value) }));
                    case "bool":
                        return BoolSerializer(new RuleRemoteSettingsBoolKeyValuesType(remoteSetting.key, remoteSetting.type, new List<bool> { bool.Parse(remoteSetting.value) }));
                    case "long":
                        return LongSerializer(new RuleRemoteSettingsLongKeyValuesType(remoteSetting.key, remoteSetting.type, new List<long> { long.Parse(remoteSetting.value) }));
                    default:
                        throw new ArgumentException(@"Cannot parse key : """ + remoteSetting.key + @""" of type: """ + remoteSetting.type + @""" because it is unsupported");
                }
            }
            catch (Exception ex)
            {
                if (ex.GetType() == typeof(ArgumentException))
                {
                    throw ex;
                }
                throw new ArgumentException(@"Parsing key """ + remoteSetting.key + @""" to type " + remoteSetting.type + " failed with exception: " + ex.Message);
            }
        }

        private static string StringSerializer(RuleRemoteSettingsStringKeyValuesType setting)
        {
            return JsonUtility.ToJson(setting);
        }

        private static string IntSerializer(RuleRemoteSettingsIntKeyValuesType setting)
        {
            return JsonUtility.ToJson(setting);
        }

        private static string FloatSerializer(RuleRemoteSettingsFloatKeyValuesType setting)
        {
            var tempJson = JsonUtility.ToJson(setting);
            string[] stringSeparators = { "values" };
            var splitJson = tempJson.Split(stringSeparators, StringSplitOptions.None);
            tempJson = splitJson[0] + "values\":[" + string.Join(",", setting.values.ToArray()) + "]}";
            return tempJson;
        }

        private static string BoolSerializer(RuleRemoteSettingsBoolKeyValuesType setting)
        {
            return JsonUtility.ToJson(setting);
        }

        private static string LongSerializer(RuleRemoteSettingsLongKeyValuesType setting)
        {
            return JsonUtility.ToJson(setting);
        }

        private static void AddUpdateListenerIfNeeded()
        {
            if (!m_UpdateListenerAlreadyAdded)
            {
                EditorApplication.update += Update;
                m_UpdateListenerAlreadyAdded = true;
            }
        }

        private static UnityWebRequest Authorize(UnityWebRequest request)
        {
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("User-Agent", "Unity Editor " + Application.unityVersion + " RC " + RemoteConfigEditorEnvConf.pluginVersion);
            request.SetRequestHeader("Authorization", string.Format("Bearer {0}", CloudProjectSettings.accessToken));
            CloudProjectSettings.RefreshAccessToken((accessTokenRefreshed) => { });
            return request;
        }

        private static bool IsStringNullOrEmpty(string stringToCheck, string errorMsg)
        {
            if (string.IsNullOrEmpty(stringToCheck))
            {
                if (!string.IsNullOrEmpty(errorMsg))
                {
                    Debug.LogWarning(errorMsg);
                }
                throw new ArgumentException(errorMsg);
            }
            return false;
        }

        private static void Update()
        {
            UpdateCoroutine();
        }

        private static void UpdateCoroutine()
        {
            if (m_WebRequestEnumerators.Count > 0)
            {
                var m_webRequestEnumerator = m_WebRequestEnumerators[0];
                if (m_webRequestEnumerator != null)
                {
                    if (m_webRequestEnumerator.Current == null)
                    {
                        m_webRequestEnumerator.MoveNext();
                    }
                    else if (m_webRequestEnumerator.Current.isDone)
                    {
                        m_webRequestEnumerator.MoveNext();
                    }
                }
            }
        }

        private static void CleanupCurrentRequest()
        {
            m_WebRequestEnumerators.RemoveAt(0);
            if (webRequestsAreDone && m_UpdateListenerAlreadyAdded)
            {
                EditorApplication.update -= Update;
                m_UpdateListenerAlreadyAdded = false;
            }
        }

        private static void LogBadRequest(UnityWebRequest request)
        {
            Debug.LogWarning("error: " + request.error + "\n" +
                             "message: " + request.downloadHandler.text + " for rule: " + Encoding.Default.GetString(request.uploadHandler.data) + "\n" +
                             "code: " + request.responseCode + "\n");
        }

        private static string SerializeRuleWebRequest(string environmentId, string configId, Rule rule)
        {
            var settingString = SerializeRSList(rule.value);
            var guid = Guid.NewGuid().ToString();
            var guid2 = Guid.NewGuid().ToString();
            var payload = JsonUtility.ToJson(ConvertRuleToRuleWebRequestStruct(rule, environmentId, configId, guid, guid2));
            payload = payload.Replace(@"""" + guid + @"""", settingString);
            payload = payload.Replace(@"""" + guid2 + @"""", "null");
            return payload;
        }

        private static void LogWebRequest(string url, string payload)
        {
            Debug.Log("Sending request to: " + url + " with payload:\n" + payload);
        }
    }
}