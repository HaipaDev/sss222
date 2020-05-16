using System;
using UnityEngine;

namespace Unity.RemoteConfig
{
    /// <summary>
    /// This class represents a single runtime settings configuration. Access its methods and properties via the <c>ConfigManager.appConfig</c> wrapper.
    /// </summary>
    public class RuntimeConfig
    {
        /// <summary>
        /// The Remote Config service generates this unique ID on configuration requests, for reporting and analytic purposes.
        /// </summary>
        /// <returns>
        /// A unique string.
        /// </returns>
        public string assignmentID { get { return metadata.GetString("assignmentId"); } }
        /// <summary>
        /// Retrieves the origin point from which your configuration settings loaded.
        /// </summary>
        /// <returns>
        /// An enum describing the origin point of your most recently loaded configuration settings.
        /// </returns>
        public ConfigOrigin origin { get; private set; }

        static IRemoteConfigSettings config;
        static IRemoteConfigSettings metadata;
        string configKey;

        internal RuntimeConfig(string configKey)
        {
            this.configKey = configKey;
            config = new RCConfig(configKey);
            metadata = new RCConfig(configKey + "Metadata");
            origin = ConfigOrigin.Default;
            config.Updated += Config_Updated;
        }

        void Config_Updated(bool wasUpdatedFromServer)
        {
            origin = wasUpdatedFromServer ? ConfigOrigin.Remote : ConfigOrigin.Cached;
        }

        internal void ForceUpdate()
        {
            config.ForceUpdate();
        }

        /// <summary>
        /// Retrieves the boolean value of a corresponding key, if one exists.
        /// </summary>
        /// <param name="key">The key identifying the corresponding setting.</param>
        /// <param name="defaultValue">The default value to use if the specified key cannot be found or is unavailable.</param>
        public bool GetBool(string key, bool defaultValue = false)
        {
            return config.GetBool(key, defaultValue);
        }

        /// <summary>
        /// Retrieves the float value of a corresponding key from the remote service, if one exists.
        /// </summary>
        /// <param name="key">The key identifying the corresponding setting.</param>
        /// <param name="defaultValue">The default value to use if the specified key cannot be found or is unavailable.</param>
        public float GetFloat(string key, float defaultValue = 0.0F)
        {
            return config.GetFloat(key, defaultValue);
        }

        /// <summary>
        /// Retrieves the int value of a corresponding key, if one exists.
        /// </summary>
        /// <param name="key">The key identifying the corresponding setting.</param>
        /// <param name="defaultValue">The default value to use if the specified key cannot be found or is unavailable.</param>
        public int GetInt(string key, int defaultValue = 0)
        {
            return config.GetInt(key, defaultValue);
        }

        /// <summary>
        /// Retrieves the string value of a corresponding key from the remote service, if one exists.
        /// </summary>
        /// <param name="key">The key identifying the corresponding setting.</param>
        /// <param name="defaultValue">The default value to use if the specified key cannot be found or is unavailable.</param>
        public string GetString(string key, string defaultValue = "")
        {
            return config.GetString(key, defaultValue);
        }

        /// <summary>
        /// Retrieves the long value of a corresponding key from the remote service, if one exists.
        /// </summary>
        /// <param name="key">The key identifying the corresponding setting.</param>
        /// <param name="defaultValue">The default value to use if the specified key cannot be found or is unavailable.</param>
        public long GetLong(string key, long defaultValue = 0L)
        {
            return config.GetLong(key, defaultValue);
        }

        /// <summary>
        /// Checks if a corresponding key exists in your remote settings.
        /// </summary>
        /// <returns><c>true</c>, if the key exists, or <c>false</c> if it doesn't.</returns>
        /// <param name="key">The key to search for.</param>
        public bool HasKey(string key)
        {
            return config.HasKey(key);
        }

        /// <summary>
        /// Returns all keys in your remote settings, as an array.
        /// </summary>
        public string[] GetKeys()
        {
            return config.GetKeys();
        }
    }
}
