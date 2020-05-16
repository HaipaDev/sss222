using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Unity.RemoteConfig.Tests
{
    internal class FetchConfigsComplete_MonobehaviorTest : MonoBehaviour, IMonoBehaviourTest, IRCTest
    {
        private bool testFinished = false;
        public bool IsTestFinished
        {
            get { return testFinished; }
        }

        private void Awake()
        {

        }

        void ConfigManager_FetchCompleted2(ConfigResponse obj)
        {
            ConfigManager.FetchCompleted -= ConfigManager_FetchCompleted2;
            FetchConfigs();
        }

        void FetchConfigs()
        {
            ConfigManager.FetchCompleted += ConfigManager_FetchCompleted;
            ConfigManager.FetchConfigs<
                ConfigManagerTestUtils.UserAttributes,
                ConfigManagerTestUtils.AppAttributes>
                (new ConfigManagerTestUtils.UserAttributes(),
                new ConfigManagerTestUtils.AppAttributes());
        }

        void ConfigManager_FetchCompleted(ConfigResponse configResponse)
        {
            ConfigManager.FetchCompleted -= ConfigManager_FetchCompleted;
            Assert.That(configResponse.requestOrigin == ConfigOrigin.Remote, "Request orgin was {0}, should have been {1}", configResponse.requestOrigin, ConfigOrigin.Remote);
            Assert.That(configResponse.status == ConfigRequestStatus.Success, "Request status was {0}, should have been {1}", configResponse.status, ConfigRequestStatus.Success);
            testFinished = true;
        }

        public void StartTest()
        {
            if (ConfigManager.requestStatus == ConfigRequestStatus.Pending)
            {
                ConfigManager.FetchCompleted += ConfigManager_FetchCompleted2;
            }
            else
            {
                FetchConfigs();
            }
        }
    }
}
