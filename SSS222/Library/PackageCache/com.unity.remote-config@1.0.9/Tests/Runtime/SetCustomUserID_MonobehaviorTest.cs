using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;

namespace Unity.RemoteConfig.Tests
{
    internal class SetCustomUserID_MonobehaviorTest : MonoBehaviour, IMonoBehaviourTest, IRCTest
    {
        private bool testFinished = false;
        public bool IsTestFinished
        {
            get { return testFinished; }
        }

        public void StartTest()
        {
            ConfigManager.SetCustomUserID(ConfigManagerTestUtils.userId);
            testFinished = true;
        }

        private void Awake()
        {

        }
    }
}
