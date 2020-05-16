using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;

namespace Unity.RemoteConfig.Tests
{
    internal class FetchConfigs_QueueImmediately_MonobehaviorTest : MonoBehaviour, IMonoBehaviourTest, IRCTest
    {
        private bool testFinished = false;
        public bool IsTestFinished
        {
            get { return testFinished; }
        }

        public void StartTest()
        {
            FetchConfigs();
        }

        private void Awake()
        {

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
