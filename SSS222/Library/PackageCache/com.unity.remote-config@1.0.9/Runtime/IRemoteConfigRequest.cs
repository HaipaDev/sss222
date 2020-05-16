using System;
using UnityEngine;

namespace Unity.RemoteConfig
{
    internal interface IRemoteConfigRequest
    {
        event Action BeforeFetchFromServer;
        event Action<bool, bool, int> Completed;
        bool SendDeviceInfoInConfigRequest();
        void QueueConfig(string configKey, object obj);
    }

    internal class RemoteConfigRequest : IRemoteConfigRequest
    {
        public event Action BeforeFetchFromServer;
        public event Action<bool, bool, int> Completed;

        public RemoteConfigRequest()
        {
            RemoteSettings.BeforeFetchFromServer += () => { BeforeFetchFromServer?.Invoke(); };
            RemoteSettings.Completed += (wasUpdatedFromServer, settingsChanged, serverResponse) => { Completed?.Invoke(wasUpdatedFromServer, settingsChanged, serverResponse); };
        }

        public bool SendDeviceInfoInConfigRequest()
        {
            return RemoteConfigSettings.SendDeviceInfoInConfigRequest();
        }

        public void QueueConfig(string configKey, object obj)
        {
            RemoteConfigSettings.QueueConfig(configKey, obj);
        }
       
    }
}