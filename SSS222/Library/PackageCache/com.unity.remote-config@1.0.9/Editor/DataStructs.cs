using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using UnityEngine;

[assembly: InternalsVisibleTo("Unity.RemoteConfig.Editor.Tests")]

namespace Unity.RemoteConfig.Editor
{

    /// <summary>
    /// The Remote Settings struct containing the Remote Setting's key, type, and value.
    /// Implements IEquatable interface for determining equality of its instances.
    /// </summary>
    [Serializable]
    internal struct RemoteSettingsKeyValueType : IEquatable<RemoteSettingsKeyValueType>
    {
        public string key;
        public string type;
        public string value;

        public RemoteSettingsKeyValueType(string k, string t, string v)
        {
            key = k;
            type = t;
            value = v;
        }

        public bool Equals(RemoteSettingsKeyValueType other)
        {
            if(key != other.key || type != other.type)
            {
                return false;
            }
            if (type == "float")
            {
                float floatVal;
                float floatOtherVal;
                float.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat, out floatVal);
                float.TryParse(other.value, NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat, out floatOtherVal);
                if (Math.Abs(floatVal - floatOtherVal) > float.Epsilon)
                {
                    return false;
                }
            }
            else
            {
                if(value != other.value)
                {
                    return false;
                }
            }
            return true;
        }
    }


    /// <summary>
    /// The environment struct containing the environment's ID, the app it belongs to, the name and description of the environment,
    /// and the created at and updated at time of the environment.
    /// </summary>
    [Serializable]
    internal struct Environment
    {
        public string id;
        public string appId;
        public string name;
        public string description;
        public string createdAt; // DateTime?
        public string updatedAt;

        public Environment(string id, string appId, string name, string description, string createdAt, string updatedAt)
        {
            this.id = id;
            this.appId = appId;
            this.name = name;
            this.description = description;
            this.createdAt = createdAt;
            this.updatedAt = updatedAt;
        }
    }

    /// <summary>
    /// The Remote Settings struct used by the editor containing the Remote Setting as a RemoteSettingsKeyValueType struct, along with its
    /// associated metadata in a RsKvtMetadata struct.
    /// Implements IEquatable interface for determining equality of its instances.
    /// </summary>
    [Serializable]
    internal struct RsKvtData : IEquatable<RsKvtData>
    {
        public RsKvtMetadata metadata;
        public RemoteSettingsKeyValueType rs;

        public RsKvtData (string entityId, RemoteSettingsKeyValueType rs)
        {
            metadata.entityId = entityId;
            this.rs = rs;
        }

        public bool Equals(RsKvtData other)
        {
            return metadata.entityId == other.metadata.entityId && rs.Equals(other.rs);
        }
    }

    /// <summary>
    /// The Remote Settings Metadata struct used by the editor containing the entity id for the Remote Setting.
    /// </summary>
    [Serializable]
    internal struct RsKvtMetadata
    {
        public string entityId;
    }

    /// <summary>
    /// The rule struct containing the rule's ID, name, enabled status, priority, type, audience condition, rollout percentage,
    /// list of Remote Settings with some metadata, start date, and end date.
    /// Implements IEquatable interface for determining equality of its instances.
    /// </summary>
    [Serializable]
    internal struct RuleWithSettingsMetadata : IEquatable<RuleWithSettingsMetadata>
    {
        public string id;
        public string name;
        public bool enabled;
        public int priority;
        public string type;
        public string condition;
        public int rolloutPercentage;
        public List<RsKvtData> value;
        public string startDate;
        public string endDate;

        public RuleWithSettingsMetadata(string id, string name, bool enabled, int priority, string condition, int rolloutPercentage,
            List<RsKvtData> settings, string startDate = null, string endDate = null)
        {
            this.id = id;
            this.name = name;
            this.enabled = enabled;
            this.priority = priority;
            this.type = null;
            this.condition = condition;
            this.rolloutPercentage = rolloutPercentage;
            this.value = settings;
            this.startDate = startDate;
            this.endDate = endDate;
        }

        public bool Equals(RuleWithSettingsMetadata other)
        {

            if (value.Count != other.value.Count)
            {
                return false;
            }

            for (var i = 0; i < value.Count; i++)
            {

                if (!other.value.Contains(value[i]))
                {
                    return false;
                }

                if (!other.value[i].Equals(value[i]))
                {
                    return false;
                }
            }

            if ((id != other.id) || (name != other.name) || (enabled != other.enabled) ||
                (priority != other.priority) || (type != other.type) || (condition != other.condition) ||
                (rolloutPercentage != other.rolloutPercentage) ||
                (startDate != other.startDate) || (endDate != other.endDate))
            {
                return false;
            }

            return true;
        }
    }

    /// <summary>
    /// The rule struct containing the rule's ID, name, enabled status, priority, type, audience condition, rollout percentage,
    /// list of Remote Settings, start date, and end date.
    /// Implements IEquatable interface for determining equality of its instances.
    /// </summary>
    [Serializable]
    internal struct Rule : IEquatable<Rule>
    {
        public string id;
        public string name;
        public bool enabled;
        public int priority;
        public string type;
        public string condition;
        public int rolloutPercentage;
        public List<RemoteSettingsKeyValueType> value;
        public string startDate;
        public string endDate;

        public Rule(string id, string name, bool enabled, int priority, string condition, int rolloutPercentage,
            List<RemoteSettingsKeyValueType> settings, string startDate = null, string endDate = null)
        {
            this.id = id;
            this.name = name;
            this.enabled = enabled;
            this.priority = priority;
            this.type = null;
            this.condition = condition;
            this.rolloutPercentage = rolloutPercentage;
            this.value = settings;
            this.startDate = startDate;
            this.endDate = endDate;
        }

        public bool Equals(Rule other)
        {

            if (value.Count != other.value.Count)
            {
                return false;
            }

            for (var i = 0; i < value.Count; i++)
            {

                if (!other.value.Contains(value[i]))
                {
                    return false;
                }

                if (!other.value[i].Equals(value[i]))
                {
                    return false;
                }
            }

            if ((id != other.id) || (name != other.name) || (enabled != other.enabled) ||
                (priority != other.priority) || (type != other.type) || (condition != other.condition) ||
                (rolloutPercentage != other.rolloutPercentage) ||
                (startDate != other.startDate) || (endDate != other.endDate))
            {
                return false;
            }

            return true;
        }
    }

    /// <summary>
    /// A struct containing a list of Configs. This is used after fetching the Configs from the server to deserialize
    /// the payload.
    /// </summary>
    [Serializable]
    internal struct GetConfigsWebStruct
    {
        public List<RemoteConfigConfigData> configs;
    }

    /// <summary>
    /// A struct containing a config and its Settings. This is used after fetching the Configs from the server
    /// to deserialize the payload.
    /// </summary>
    [Serializable]
    internal struct RemoteConfigConfigData
    {
        public string type;
        public string id;
        public List<RemoteSettingsKeyValueType> value;
    }

    /// <summary>
    /// A struct containing a list of environments. This is used after fetching the environments 
    /// </summary>
    [Serializable]
    internal struct EnvironmentsHolder
    {
        public List<Environment> environments;
    }

    /// <summary>
    /// Interface for Remote Settings to serialize with to construct web payloads.
    /// </summary>
    internal interface IRemoteSettingsWebPayload { }

    /// <summary>
    /// A struct for Remote Settings of type "int" to serialize and construct web payloads.
    /// </summary>
    [Serializable]
    internal struct RemoteSettingsIntKeyValueWebPayload : IRemoteSettingsWebPayload
    {
        public string key;
        public string type;
        public int value;

        public RemoteSettingsIntKeyValueWebPayload(string k, int v)
        {
            key = k;
            type = "int";
            value = v;
        }
    }

    /// <summary>
    /// A struct for Remote Settings of type "long" to serialize and construct web payloads.
    /// </summary>
    [Serializable]
    internal struct RemoteSettingsLongKeyValueWebPayload : IRemoteSettingsWebPayload
    {
        public string key;
        public string type;
        public long value;

        public RemoteSettingsLongKeyValueWebPayload(string k, long v)
        {
            key = k;
            type = "long";
            value = v;
        }
    }

    /// <summary>
    /// A struct for Remote Settings of type "float" to serialize and construct web payloads.
    /// </summary>
    [Serializable]
    internal struct RemoteSettingsFloatKeyValueWebPayload : IRemoteSettingsWebPayload
    {
        public string key;
        public string type;
        public string value;

        public RemoteSettingsFloatKeyValueWebPayload(string k, float v)
        {
            key = k;
            type = "float";
            value = v.ToString();
        }
    }

    /// <summary>
    /// A struct for Remote Settings of type "bool" to serialize and construct web payloads.
    /// </summary>
    [Serializable]
    internal struct RemoteSettingsBoolKeyValueWebPayload : IRemoteSettingsWebPayload
    {
        public string key;
        public string type;
        public bool value;

        public RemoteSettingsBoolKeyValueWebPayload(string k, bool v)
        {
            key = k;
            type = "bool";
            value = v;
        }
    }

    /// <summary>
    /// A struct for Remote Settings of type "string" to serialize and construct web payloads.
    /// </summary>
    [Serializable]
    internal struct RemoteSettingsStringKeyValueWebPayload : IRemoteSettingsWebPayload
    {
        public string key;
        public string type;
        public string value;

        public RemoteSettingsStringKeyValueWebPayload(string k, string v)
        {
            key = k;
            type = "string";
            value = v;
        }
    }

#pragma warning disable 0649
    [Serializable]
    internal struct RulesResponse
    {
        public List<RuleResponseWebStruct> rules;
    }
#pragma warning restore 0649

    /// <summary>
    /// A Rule-like struct used to deserialize Rules from web requests.
    /// </summary>
    [Serializable]
    internal struct RuleResponseWebStruct
    {
        public string id;
        public string name;
        public string condition;
        public int priority;
        public int rolloutPercentage;
        public string ruleType;
        public List<RuleRemoteSettingsKeyValuesType> value;
        public bool enabled;
        public string startDate;
        public string endDate;

        public RuleResponseWebStruct(string name, string condition, int priority, int rolloutPercentage,
            List<RuleRemoteSettingsKeyValuesType> value, string id, string type, bool enabled, string startDate, string endDate)
        {
            this.id = id;
            this.name = name;
            this.condition = condition;
            this.priority = priority;
            this.rolloutPercentage = rolloutPercentage;
            this.value = value;
            this.ruleType = type;
            this.enabled = enabled;
            this.startDate = startDate;
            this.endDate = endDate;
        }
    }

    /// <summary>
    /// A struct for Remote Settings that belong to a rule. This struct stores a list of Remote Settings values for each
    /// key instead of a single Remote Settings value for each key.
    /// </summary>
    [Serializable]
    internal struct RuleRemoteSettingsKeyValuesType
    {
        public string key;
        public string type;
        public List<string> values;

        public RuleRemoteSettingsKeyValuesType(string k, string t, List<string> v)
        {
            key = k;
            type = t;
            values = v;
        }
    }

    /// <summary>
    /// A struct used to serialize Remote Settings of type int.
    /// </summary>
    [Serializable]
    internal struct RuleRemoteSettingsIntKeyValuesType
    {
        public string key;
        public string type;
        public List<int> values;

        public RuleRemoteSettingsIntKeyValuesType(string k, string t, List<int> v)
        {
            key = k;
            type = t;
            values = v;
        }
    }

    /// <summary>
    /// A struct used to serialize Remote Settings of type long.
    /// </summary>
    [Serializable]
    internal struct RuleRemoteSettingsLongKeyValuesType
    {
        public string key;
        public string type;
        public List<long> values;

        public RuleRemoteSettingsLongKeyValuesType(string k, string t, List<long> v)
        {
            key = k;
            type = t;
            values = v;
        }
    }

    /// <summary>
    /// A struct used to serialize Remote Settings of type float.
    /// </summary>
    [Serializable]
    internal struct RuleRemoteSettingsFloatKeyValuesType
    {
        public string key;
        public string type;
        public List<float> values;

        public RuleRemoteSettingsFloatKeyValuesType(string k, string t, List<float> v)
        {
            key = k;
            type = t;
            values = v;
        }
    }

    /// <summary>
    /// A struct used to serialize Remote Settings of type bool.
    /// </summary>
    [Serializable]
    internal struct RuleRemoteSettingsBoolKeyValuesType
    {
        public string key;
        public string type;
        public List<bool> values;

        public RuleRemoteSettingsBoolKeyValuesType(string k, string t, List<bool> v)
        {
            key = k;
            type = t;
            values = v;
        }
    }

    /// <summary>
    /// A struct used to serialize Remote Settings of type string.
    /// </summary>
    [Serializable]
    internal struct RuleRemoteSettingsStringKeyValuesType
    {
        public string key;
        public string type;
        public List<string> values;

        public RuleRemoteSettingsStringKeyValuesType(string k, string t, List<string> v)
        {
            key = k;
            type = t;
            values = v;
        }
    }

    /// <summary>
    /// A struct for serializing a Rule for a web request for PUT edit rule, and POST new rule.
    /// </summary>
    [Serializable]
    internal struct RuleWebRequestStruct
    {
        public string name;
        public string condition;
        public int priority;
        public int rolloutPercentage;
        public string ruleType;
        public string value;
        public string startDate;
        public string endDate;
        public bool enabled;
        public string environmentId;
        public string configId;

        public RuleWebRequestStruct(string name, string condition, int priority, int rolloutPercentage,
            string value, string type, bool enabled, string environmentId, string configId, string startDate = null, string endDate = null)
        {
            this.name = name;
            this.condition = condition;
            this.priority = priority;
            this.rolloutPercentage = rolloutPercentage;
            this.value = value;
            this.ruleType = type;
            this.enabled = enabled;
            this.startDate = startDate;
            this.endDate = endDate;
            this.environmentId = environmentId;
            this.configId = configId;
        }
    }

    /// <summary>
    /// A struct for deserializing Rule web request responses.
    /// </summary>
    [Serializable]
    internal struct RuleResponse
    {
        public string id;
        public string environmentId;
        public string createdAt;
        public bool enabled;
    }

    /// <summary>
    /// A struct for serializing the payload for GET configs and PUT config web requests.
    /// </summary>
    [Serializable]
    internal struct ConfigWebRequestStruct
    {
        public string environmentId;
        public string type;
        public string value;
    }

    /// <summary>
    /// A struct for deserializing the payload for POST config web response.
    /// </summary>
    [Serializable]
    internal struct PostConfigWebResponseStruct
    {
        public string id;
        public string createdAt;
    }

    [Serializable]
    internal struct DefaultEnvironmentResponseStruct
    {
        public string id;
    }
}