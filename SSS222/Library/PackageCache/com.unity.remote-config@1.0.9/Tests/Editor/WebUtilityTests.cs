using UnityEngine;
using System.Reflection;
using System.Collections.Generic;
using NUnit.Framework;
using System;

namespace Unity.RemoteConfig.Editor.Tests
{
    internal class WebUtilityTests
    {
        [Test]
        public void SerializeRSList_ReturnsCorrectJson()
        {
            List<RemoteSettingsKeyValueType> remoteSettings = new List<RemoteSettingsKeyValueType>();
            remoteSettings.Add(new RemoteSettingsKeyValueType("testString", "string", "someString"));
            remoteSettings.Add(new RemoteSettingsKeyValueType("testBool", "bool", "False"));
            remoteSettings.Add(new RemoteSettingsKeyValueType("testInt", "int", "1234"));
            remoteSettings.Add(new RemoteSettingsKeyValueType("testFloat", "float", "1.2"));
            remoteSettings.Add(new RemoteSettingsKeyValueType("testLong", "long", "9223372036854775807"));

            string result = @"[{""key"":""testString"",""type"":""string"",""values"":[""someString""]},{""key"":""testBool"",""type"":""bool"",""values"":[false]},{""key"":""testInt"",""type"":""int"",""values"":[1234]},{""key"":""testFloat"",""type"":""float"",""values"":[1.2]},{""key"":""testLong"",""type"":""long"",""values"":[9223372036854775807]}]";

            var methodInfo = typeof(RemoteConfigWebApiClient).GetMethod("SerializeRSList", BindingFlags.Static | BindingFlags.NonPublic);
            Assert.That(string.Equals(methodInfo.Invoke(null, new object[] { remoteSettings }), result));
        }

        [Test]
        public void SerializeRSList_ThrowsExceptionWhenBadInput()
        {
            List<RemoteSettingsKeyValueType> remoteSettings = new List<RemoteSettingsKeyValueType>();
            remoteSettings.Add(new RemoteSettingsKeyValueType("testBad", "temp", "1234567890"));


            var methodInfo = typeof(RemoteConfigWebApiClient).GetMethod("SerializeRSList", BindingFlags.Static | BindingFlags.NonPublic);
            try
            {
                methodInfo.Invoke(null, new object[] { remoteSettings });
            }
            catch(TargetInvocationException ex)
            {
                Assert.That(ex.InnerException.GetType() == typeof(System.ArgumentException));
            }
        }

        [Test]
        public void SerializeWebRSList_ReturnsCorrectJson()
        {
            List<IRemoteSettingsWebPayload> remoteSettings = new List<IRemoteSettingsWebPayload>();
            remoteSettings.Add(new RemoteSettingsStringKeyValueWebPayload("testString", "someString"));
            remoteSettings.Add(new RemoteSettingsBoolKeyValueWebPayload("testBool", true));
            remoteSettings.Add(new RemoteSettingsIntKeyValueWebPayload("testInt", 1234));
            remoteSettings.Add(new RemoteSettingsFloatKeyValueWebPayload("testFloat", 1.2f));
            remoteSettings.Add(new RemoteSettingsLongKeyValueWebPayload("testLong", 9223372036854775807));

            string result = @"[{""key"":""testString"",""type"":""string"",""value"":""someString""},{""key"":""testBool"",""type"":""bool"",""value"":true},{""key"":""testInt"",""type"":""int"",""value"":1234},{""key"":""testFloat"",""type"":""float"",""value"":1.2},{""key"":""testLong"",""type"":""long"",""value"":9223372036854775807}]";
            var methodInfo = typeof(RemoteConfigWebApiClient).GetMethod("SerializeWebRSList", BindingFlags.Static | BindingFlags.NonPublic);
            Assert.That(string.Equals(methodInfo.Invoke(null, new object[] { remoteSettings }), result));
        }

        [Test]
        public void FetchEnvironments_ThrowsArgumentExceptionOnBadArgs()
        {
            try
            {
                RemoteConfigWebApiClient.FetchEnvironments(null);
            }
            catch(ArgumentException ex)
            {
                Assert.That(ex.GetType() == typeof(ArgumentException));
            }
        }

        [Test]
        public void FetchConfigs_ThrowsArgumentExceptionOnBadArgs()
        {
            try
            {
                RemoteConfigWebApiClient.FetchConfigs(null, null);
            }
            catch (ArgumentException ex)
            {
                Assert.That(ex.GetType() == typeof(ArgumentException));
            }
        }

        [Test]
        public void FetchRules_ThrowsArgumentExceptionOnBadArgs()
        {
            try
            {
                RemoteConfigWebApiClient.FetchRules(null, null);
            }
            catch (ArgumentException ex)
            {
                Assert.That(ex.GetType() == typeof(ArgumentException));
            }
        }

        [Test]
        public void PutConfig_ThrowsArgumentExceptionOnBadArgs()
        {
            try
            {
                RemoteConfigWebApiClient.PutConfig(null, null, null, null);
            }
            catch (ArgumentException ex)
            {
                Assert.That(ex.GetType() == typeof(ArgumentException));
            }
        }

        [Test]
        public void PostConfig_ThrowsArgumentExceptionOnBadArgs()
        {
            try
            {
                RemoteConfigWebApiClient.PostConfig(null, null, null);
            }
            catch (ArgumentException ex)
            {
                Assert.That(ex.GetType() == typeof(ArgumentException));
            }
        }

        [Test]
        public void PostAddRule_ThrowsArgumentExceptionOnBadArgs()
        {
            try
            {
                RemoteConfigWebApiClient.PostAddRule(null, null, null, new Rule());
            }
            catch (ArgumentException ex)
            {
                Assert.That(ex.GetType() == typeof(ArgumentException));
            }
        }

        [Test]
        public void PutEditRule_ThrowsArgumentExceptionOnBadArgs()
        {
            try
            {
                RemoteConfigWebApiClient.PutEditRule(null, null, null, new Rule());
            }
            catch (ArgumentException ex)
            {
                Assert.That(ex.GetType() == typeof(ArgumentException));
            }
        }

        [Test]
        public void DeleteRule_ThrowsArgumentExceptionOnBadArgs()
        {
            try
            {
                RemoteConfigWebApiClient.DeleteRule(null, null, null);
            }
            catch (ArgumentException ex)
            {
                Assert.That(ex.GetType() == typeof(ArgumentException));
            }
        }
    }
}

