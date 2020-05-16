using System;
using UnityEngine;

namespace Unity.RemoteConfig
{
    internal interface IRemoteConfigSettings
    {
        event Action<bool> Updated;
        void ForceUpdate();
        bool GetBool(string key, bool defaultValue = false);
        float GetFloat(string key, float defaultValue = 0.0F);
        int GetInt(string key, int defaultValue = 0);
        string GetString(string key, string defaultValue = "");
        long GetLong(string key, long defaultValue = 0L);
        bool HasKey(string key);
        string[] GetKeys();
    }
    
    internal class RCConfig : IRemoteConfigSettings
    {
        public event Action<bool> Updated;

        private RemoteConfigSettings _remoteConfigSettings;

        public RCConfig(string configKey)
        {
            _remoteConfigSettings = new RemoteConfigSettings(configKey);
            _remoteConfigSettings.Updated += (wasUpdatedFromServer) => { Updated?.Invoke(wasUpdatedFromServer); };
        }
        
        public void ForceUpdate()
        {
            _remoteConfigSettings.ForceUpdate();
        }

        public bool GetBool(string key, bool defaultValue = false)
        {
            return _remoteConfigSettings.GetBool(key, defaultValue);
        }
        
        public float GetFloat(string key, float defaultValue = 0.0F)
        {
            return _remoteConfigSettings.GetFloat(key, defaultValue);
        }
        
        public int GetInt(string key, int defaultValue = 0)
        {
            return _remoteConfigSettings.GetInt(key, defaultValue);
        }
        
        public string GetString(string key, string defaultValue = "")
        {
            return _remoteConfigSettings.GetString(key, defaultValue);
        }
        
        public long GetLong(string key, long defaultValue = 0L)
        {
            return _remoteConfigSettings.GetLong(key, defaultValue);
        }

        public bool HasKey(string key)
        {
            return _remoteConfigSettings.HasKey(key);
        }
        
        public string[] GetKeys()
        {
            return _remoteConfigSettings.GetKeys();
        }
    }
}