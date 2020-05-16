using System;
using UnityEngine;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleToAttribute("Unity.RemoteConfig.Tests")]

namespace Unity.RemoteConfig
{
    /// <summary>
    /// Use this class to fetch and apply your configuration settings at runtime.
    /// </summary>
    public static class ConfigManager
    {
        /// <summary>
        /// Returns the status of the current configuration request from the service.
        /// </summary>
        /// <returns>
        /// An enum representing the status of the current Remote Config request.
        /// </returns>
        public static ConfigRequestStatus requestStatus { get; private set; }
        /// <summary>
        /// This event fires when the configuration manager successfully fetches settings from the service.
        /// </summary>
        /// <returns>
        /// A struct representing the response of a Remote Config fetch.
        /// </returns>
        public static event Action<ConfigResponse> FetchCompleted;
        /// <summary>
        /// Retrieves the <c>RuntimeConfig</c> object for handling Remote Config settings.
        /// </summary>
        /// <remarks>
        /// <para> Use this property to access the following <c>RuntimeConfig</c> methods and classes:</para>
        /// <para><c>public string assignmentID</c> is a unique string identifier used for reporting and analytic purposes. The Remote Config service generate this ID upon configuration requests.</para>
        /// <para><c>public bool GetBool (string key, bool defaultValue)</c> retrieves the boolean value of a corresponding key from the remote service, if one exists.</para>
        /// <para><c>public float GetFloat (string key, float defaultValue)</c> retrieves the float value of a corresponding key from the remote service, if one exists.</para>
        /// <para><c>public long GetLong (string key, long defaultValue)</c> retrieves the long value of a corresponding key from the remote service, if one exists.</para>
        /// <para><c>public int GetInt (string key, int defaultValue)</c> retrieves the integer value of a corresponding key from the remote service, if one exists.</para>
        /// <para><c>public string GetString (string key, string defaultValue)</c> retrieves the string value of a corresponding key from the remote service, if one exists.</para>
        /// <para><c>public bool HasKey (string key)</c> checks if a corresponding key exists in your remote settings.</para>
        /// <para><c>public string[] GetKeys ()</c> returns all keys in your remote settings, as an array.</para>
        /// </remarks>
        /// <returns>
        /// A class representing a single runtime settings configuration.
        /// </returns>
        public static RuntimeConfig appConfig { get; private set; }

        private static IRemoteConfigRequest remoteConfigRequest;

        static bool requestInProgress;
        static bool queueRequest;
        static bool initialized;


        struct Delivery
        {
            public string customUserId;
            public string packageVersion;
        }

        static Delivery deliveryPayload;

        [RuntimeInitializeOnLoadMethod]
        private static void Initialize()
        {
            Init();
        }

        static void Init()
        {
            if (!initialized)
            {
                appConfig = new RuntimeConfig("settings");
                deliveryPayload.packageVersion = RemoteConfigRuntimeEnvConf.pluginVersion;
                
                remoteConfigRequest = new RemoteConfigRequest();

                remoteConfigRequest.BeforeFetchFromServer += RemoteSettings_BeforeFetchFromServer;
                remoteConfigRequest.Completed += RemoteSettings_Completed;

                requestStatus = ConfigRequestStatus.None;
                initialized = true;
            }
        }

        static void RemoteSettings_Completed(bool wasUpdatedFromServer, bool settingsChanged, int serverResponse)
        {
            requestInProgress = false;
            var origin = ConfigOrigin.Default;

            if (wasUpdatedFromServer)
            {
                if (serverResponse == 200)
                {
                    requestStatus = ConfigRequestStatus.Success;
                }
                else
                {
                    requestStatus = ConfigRequestStatus.Failed;
                }
                origin = ConfigOrigin.Remote;
            }
            else 
            {
                requestStatus = ConfigRequestStatus.Success;
                origin = ConfigOrigin.Cached;
            }
            FetchCompleted?.Invoke(new ConfigResponse() { requestOrigin = origin, status = requestStatus });

            if (queueRequest)
            {
                appConfig.ForceUpdate();
                queueRequest = false;
            }
        }


        static void RemoteSettings_BeforeFetchFromServer()
        {
            requestInProgress = true;
            requestStatus = ConfigRequestStatus.Pending;
        }

        /// <summary>
        /// Sets a custom user identifier for the Remote Config delivery payload.
        /// </summary>
        /// <param name="customUserID">Custom user identifier.</param>
        public static void SetCustomUserID(string customUserID)
        {
            deliveryPayload.customUserId = customUserID;
        }
        /// <summary>
        /// Fetchs app configuration settings from the remote server.
        /// </summary>
        /// <param name="userAttributes">A struct containing custom user attributes. If none apply, use an empty struct.</param>
        /// <param name="appAttributes">A struct containing custom app attributes. If none apply, use an empty struct.</param>
        /// <typeparam name="T">The type of the <c>userAttributes</c> struct.</typeparam>
        /// <typeparam name="T2">The type of the <c>appAttributes</c> struct.</typeparam>
        public static void FetchConfigs<T, T2>(T userAttributes, T2 appAttributes) where T : struct where T2 : struct
        {
            if(!initialized)
            {
                Init();
            }

            remoteConfigRequest.SendDeviceInfoInConfigRequest();
            remoteConfigRequest.QueueConfig("delivery", deliveryPayload);
            remoteConfigRequest.QueueConfig("deliveryUserAttributes", userAttributes);
            remoteConfigRequest.QueueConfig("deliveryAppAttributes", appAttributes);

            if(requestInProgress)
            {
                queueRequest = true;
            }
            else
            {
                appConfig.ForceUpdate();
            }
        }
    }

    /// <summary>
    /// An enum describing the origin point of your most recently loaded configuration settings.
    /// </summary>
    public enum ConfigOrigin
    {
        /// <summary>
        /// Indicates that no configuration settings loaded in the current session.
        /// </summary>
        Default,
        /// <summary>
        /// Indicates that the configuration settings loaded in the current session are cached from a previous session (in other words, no new configuration settings loaded).
        /// </summary>
        Cached,
        /// <summary>
        /// Indicates that new configuration settings loaded from the remote server in the current session.
        /// </summary>
        Remote
    }

    /// <summary>
    /// An enum representing the status of the current Remote Config request.
    /// </summary>
    public enum ConfigRequestStatus
    {
        /// <summary>
        /// Indicates that no Remote Config request has been made.
        /// </summary>
        None,
        /// <summary>
        /// Indicates that the Remote Config request failed.
        /// </summary>
        Failed,
        /// <summary>
        /// Indicates that the Remote Config request succeeded.
        /// </summary>
        Success,
        /// <summary>
        /// Indicates that the Remote Config request is still processing.
        /// </summary>
        Pending
    }

    /// <summary>
    /// A struct representing the response of a Remote Config fetch.
    /// </summary>
    public struct ConfigResponse
    {
        /// <summary>
        /// The origin point of the last retrieved configuration settings.
        /// </summary>
        /// <returns>
        /// An enum describing the origin point of your most recently loaded configuration settings.
        /// </returns>
        public ConfigOrigin requestOrigin;
        /// <summary>
        /// The status of the current Remote Config request.
        /// </summary>
        /// <returns>
        /// An enum representing the status of the current Remote Config request.
        /// </returns>
        public ConfigRequestStatus status;
    }
}
