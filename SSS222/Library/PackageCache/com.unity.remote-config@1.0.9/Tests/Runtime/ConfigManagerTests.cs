using System.Collections;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Reflection;

namespace Unity.RemoteConfig.Tests
{
    internal class ConfigManagerTests
    {

        [UnityTest]
        public IEnumerator SetCustomUserID_SetsCustomUserID()
        {
            var monoTest = new MonoBehaviourTest<SetCustomUserID_MonobehaviorTest>(false);
            monoTest.component.StartTest();
            yield return monoTest;
            FieldInfo deliveryFieldInfo = typeof(ConfigManager).GetField("deliveryPayload", BindingFlags.Static | BindingFlags.NonPublic);
            FieldInfo customUserIdFieldInfo = deliveryFieldInfo.FieldType.GetField("customUserId");
            Assert.That(string.Equals(customUserIdFieldInfo.GetValue(deliveryFieldInfo.GetValue(null)), ConfigManagerTestUtils.userId));
        }

        [UnityTest]
        public IEnumerator RemoteSettings_Completed_WorksWithGoodResponse()
        {
            var methodInfo = typeof(ConfigManager).GetMethod("RemoteSettings_Completed", BindingFlags.Static | BindingFlags.NonPublic);
            methodInfo.Invoke(null, new object[] { true, true, 200 });
            yield return !(ConfigManager.requestStatus == ConfigRequestStatus.Pending);
            Assert.That(ConfigManager.requestStatus == ConfigRequestStatus.Success, "ConfigManager.requestStatus was {0}, should have been {1}", ConfigManager.requestStatus, ConfigRequestStatus.Success);
        }

        [UnityTest]
        public IEnumerator RemoteSettings_Completed_WorksWithBadResponse()
        {
            var methodInfo = typeof(ConfigManager).GetMethod("RemoteSettings_Completed", BindingFlags.Static | BindingFlags.NonPublic);
            methodInfo.Invoke(null, new object[] { true, true, 500 });
            yield return !(ConfigManager.requestStatus == ConfigRequestStatus.Pending);
            Assert.That(ConfigManager.requestStatus == ConfigRequestStatus.Failed, "ConfigManager.requestStatus was {0}, should have been {1}", ConfigManager.requestStatus, ConfigRequestStatus.Success);
        }

        [UnityTest]
        public IEnumerator RemoteSettings_Completed_ReturnsQueuedRequestIsFalseIfNotQueued()
        {
            typeof(ConfigManager).GetField("queueRequest", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.SetProperty).SetValue(null,null);
            var methodInfo = typeof(ConfigManager).GetMethod("RemoteSettings_Completed", BindingFlags.Static | BindingFlags.NonPublic);
            methodInfo.Invoke(null, new object[] { true, true, 200 });
            yield return !(ConfigManager.requestStatus == ConfigRequestStatus.Pending);
            var queueRequestAfter = typeof(ConfigManager).GetField("queueRequest",BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.SetProperty).GetValue(null);
            Assert.That(queueRequestAfter.Equals(false), "queueRequest after the request was {0}, should have been {1}", queueRequestAfter, false);
        }

        [UnityTest]
        public IEnumerator RemoteSettings_Completed_ReturnsQueuedRequestIsFalseIfQueued()
        {
            typeof(ConfigManager).GetField("queueRequest", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.SetProperty).SetValue(null,true);
            var methodInfo = typeof(ConfigManager).GetMethod("RemoteSettings_Completed", BindingFlags.Static | BindingFlags.NonPublic);
            methodInfo.Invoke(null, new object[] { true, true, 200 });
            yield return !(ConfigManager.requestStatus == ConfigRequestStatus.Pending);
            var queueRequestAfter = typeof(ConfigManager).GetField("queueRequest",BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.SetProperty).GetValue(null);
            Assert.That(queueRequestAfter.Equals(false), "queueRequest after the request was {0}, should have been {1}", queueRequestAfter, false);
        }

        [UnityTest]
        public IEnumerator RemoteSettings_BeforeFetchFromServer_SetsRequestVariablesCorrectly()
        {
            typeof(ConfigManager).GetField("requestInProgress", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.SetProperty).SetValue(null,false);

            PropertyInfo propertyRequestStatus = typeof(ConfigManager).GetProperty("requestStatus");
            propertyRequestStatus.DeclaringType.GetProperty("requestStatus");
            propertyRequestStatus.SetValue(null, ConfigRequestStatus.None, BindingFlags.NonPublic | BindingFlags.Static, null, null, null);  // If the setter might be public, add the BindingFlags.Public flag.

            var methodInfo = typeof(ConfigManager).GetMethod("RemoteSettings_BeforeFetchFromServer", BindingFlags.Static | BindingFlags.NonPublic);
            methodInfo.Invoke(null, null);
            yield return (ConfigManager.requestStatus == ConfigRequestStatus.Pending);
            var requestInProgressAfter = typeof(ConfigManager).GetField("requestInProgress", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null);
            var requestStatusAfter = propertyRequestStatus.GetValue(null,  BindingFlags.Public | BindingFlags.Static, null, null, null);

            Assert.That(requestInProgressAfter.Equals(true), "requestInProgressAfter after the request was {0}, should have been {1}", requestInProgressAfter, true);
            Assert.That(requestStatusAfter.Equals(ConfigRequestStatus.Pending), "requestStatusAfter after the request was {0}, should have been {1}", requestStatusAfter, ConfigRequestStatus.Pending);
        }

        [UnityTest]
        public IEnumerator FetchConfigs_SetsQueueRequestVariablesCorrectlyIfRequestInProgress()
        {
            typeof(ConfigManager).GetField("requestInProgress", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.SetProperty).SetValue(null,true);
            ConfigManager.FetchConfigs(new ConfigManagerTestUtils.UserAttributes(), new ConfigManagerTestUtils.AppAttributes());

            yield return null;
            var requestInProgressAfter = typeof(ConfigManager).GetField("requestInProgress",BindingFlags.Static | BindingFlags.NonPublic).GetValue(null);
            var queueRequest = typeof(ConfigManager).GetField("queueRequest",BindingFlags.Static | BindingFlags.NonPublic).GetValue(null);

            Assert.That(requestInProgressAfter.Equals(true), "requestInProgressAfter after the request was {0}, should have been {1}", queueRequest, true);
            Assert.That(queueRequest.Equals(true), "queueRequest after the request was {0}, should have been {1}", queueRequest, true);
        }

    }

    internal static class ConfigManagerTestUtils
    {
        public const string userId = "some-user-id";

        public struct UserAttributes
        {

        }

        public struct AppAttributes
        {

        }
    }

    internal interface IRCTest
    {
        void StartTest();
    }
}