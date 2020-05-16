using System.Collections;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Reflection;

namespace Unity.RemoteConfig.Tests
{
    public class RuntimeConfigTests
    {
        [UnityTest]
        public IEnumerator Config_Updated_SetsOriginVariableCorrectly()
        {
            var rConfig = new RuntimeConfig("");

            PropertyInfo propertyOrigin = typeof(RuntimeConfig).GetProperty("origin");
            propertyOrigin.DeclaringType.GetProperty("origin");
            propertyOrigin.SetValue(rConfig, ConfigOrigin.Default, BindingFlags.NonPublic | BindingFlags.Static, null, null, null);

            var methodInfo = typeof(RuntimeConfig).GetMethod("Config_Updated", BindingFlags.Instance | BindingFlags.NonPublic);
            methodInfo.Invoke(rConfig, new object[] {true});
            yield return null;
            var originAfter = propertyOrigin.GetValue(rConfig,  BindingFlags.Public | BindingFlags.Static, null, null, null);

            Assert.That(originAfter.Equals(ConfigOrigin.Remote), "originAfter after the request was {0}, should have been {1}", originAfter, ConfigOrigin.Remote);
        }

        [UnityTest]
        public IEnumerator Config_Updated_OriginVariableCachedIfNoUpdateFromServer()
        {
            var rConfig = new RuntimeConfig("");

            PropertyInfo propertyOrigin = typeof(RuntimeConfig).GetProperty("origin");
            propertyOrigin.DeclaringType.GetProperty("origin");
            propertyOrigin.SetValue(rConfig, ConfigOrigin.Default, BindingFlags.NonPublic | BindingFlags.Static, null, null, null);

            var methodInfo = typeof(RuntimeConfig).GetMethod("Config_Updated", BindingFlags.Instance | BindingFlags.NonPublic);
            methodInfo.Invoke(rConfig, new object[] {false});
            yield return null;
            var originAfter = propertyOrigin.GetValue(rConfig,  BindingFlags.Public | BindingFlags.Static, null, null, null);

            Assert.That(originAfter.Equals(ConfigOrigin.Cached), "originAfter after the request was {0}, should have been {1}", originAfter, ConfigOrigin.Cached);
        }
    }
}