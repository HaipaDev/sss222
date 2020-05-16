using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;

namespace Unity.RemoteConfig.Tests
{
    internal class FetchConfigs_MonobehaviorTest : MonoBehaviour, IMonoBehaviourTest, IRCTest
    {
        private bool testFinished = false;
        public bool IsTestFinished
        {
            get { return testFinished; }
        }

        public void StartTest()
        {
            if (ConfigManager.requestStatus == ConfigRequestStatus.Pending)
            {
                ConfigManager.FetchCompleted += ConfigManager_FetchCompleted;
            }
            else
            {
                FetchConfigs();
            }
        }

        private void Awake()
        {

        }

        void ConfigManager_FetchCompleted(ConfigResponse obj)
        {
            ConfigManager.FetchCompleted -= ConfigManager_FetchCompleted;
            FetchConfigs();
        }

        void FetchConfigs()
        {
            ConfigManager.FetchConfigs<
                ConfigManagerTestUtils.UserAttributes,
                ConfigManagerTestUtils.AppAttributes>
                (new ConfigManagerTestUtils.UserAttributes(),
                new ConfigManagerTestUtils.AppAttributes());
            testFinished = true;
        }
    }
}
