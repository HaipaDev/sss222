//using UnityEngine;
//using UnityEditor;
//using UnityEngine.TestTools;
//using NUnit.Framework;
//﻿using UnityEngine;
//using UnityEditor;
//using UnityEngine.TestTools;
//using NUnit.Framework;
//using System.Reflection;
//using System.IO;
//using System.Collections.Generic;
//using UnityEditor.AI.RemoteSettings;
//
//class RemoteSettingsEditorWindowControllerTests {
//
//	[Test]
//	public void RestoreLastSelectedEnvironmentReturnsCorrectValueForEditorPref() 
//    {
//        string prefsValue = "test-env";
//        string defaultValue = "default";
//        var controller = GetRemoteSettingsController();
//        var prefsKeyFieldInfo = typeof(RemoteSettingsEditorWindowController).GetField("k_CurrentEnvironment", BindingFlags.Static | BindingFlags.NonPublic);
//        var prefsKey = prefsKeyFieldInfo.GetValue(controller) as string;
//        prefsKey = prefsKey + Application.cloudProjectId;
//        var oldPrefsKey = EditorPrefs.GetString(prefsKey);
//        EditorPrefs.SetString(prefsKey, prefsValue);
//        Assert.AreEqual(prefsValue, controller.RestoreLastSelectedEnvironment(defaultValue));
//        EditorPrefs.SetString(prefsKey, oldPrefsKey);
//	}
//
//    [Test]
//    public void RestoreLastSelectedEnvironmentReturnsDefaultWhenEditorPrefDoesntExist()
//    {
//        string defaultValue = "default";
//        var controller = GetRemoteSettingsController();
//        var prefsKeyFieldInfo = typeof(RemoteSettingsEditorWindowController).GetField("k_CurrentEnvironment", BindingFlags.Static | BindingFlags.NonPublic);
//        var prefsKey = prefsKeyFieldInfo.GetValue(controller) as string;
//        prefsKey = prefsKey + Application.cloudProjectId;
//        var oldPrefsKey = EditorPrefs.GetString(prefsKey);
//        EditorPrefs.DeleteKey(prefsKey);
//        Assert.AreEqual(defaultValue, controller.RestoreLastSelectedEnvironment(defaultValue));
//        EditorPrefs.SetString(prefsKey, oldPrefsKey);
//    }
//
//    [Test]
//    public void SetLastSelectedEnvironmentSetsTheCorrectKeyAndValue ()
//    {
//        string prefsValue = "test-env";
//        var controller = GetRemoteSettingsController();
//        var prefsKeyFieldInfo = typeof(RemoteSettingsEditorWindowController).GetField("k_CurrentEnvironment", BindingFlags.Static | BindingFlags.NonPublic);
//        var prefsKey = prefsKeyFieldInfo.GetValue(controller) as string;
//        prefsKey = prefsKey + Application.cloudProjectId;
//        var oldPrefsKey = EditorPrefs.GetString(prefsKey);
//        controller.SetLastSelectedEnvironment(prefsValue);
//        Assert.AreEqual(prefsValue, EditorPrefs.GetString(prefsKey));
//        EditorPrefs.SetString(prefsKey, oldPrefsKey);
//    }
//
//    [Test]
//    public void CheckAndCreateDataStoreReturnsDataStore()
//    {
//        var controller = GetRemoteSettingsController();
//        Assert.IsInstanceOf<RemoteSettingsHolder>(controller.CheckAndCreateDataStore());
//    }
//
//    [Test]
//    public void CheckAndCreateAssetFolderCreatesFolder()
//    {
//        var controller = GetRemoteSettingsController();
//        var dsPath = typeof(RemoteSettingsEditorWindowController).GetField("k_PathToDataStore", BindingFlags.Static | BindingFlags.NonPublic).GetValue(controller) as string;
//        Directory.Delete(dsPath, true);
//        AssetDatabase.Refresh();
//        AssetDatabase.SaveAssets();
//        Assert.IsFalse(Directory.Exists(dsPath));
//
//        var testMethod = typeof(RemoteSettingsEditorWindowController).GetMethod("CheckAndCreateAssetFolder", BindingFlags.NonPublic | BindingFlags.Instance);
//        testMethod?.Invoke(controller, new object[]{ dsPath });
//        Assert.IsTrue(Directory.Exists(dsPath));
//    }
//
//    [Test]
//    public void CreateDataStoreDictInitializesDataStoreIfNotInited()
//    {
//        var ds = ScriptableObject.CreateInstance<RemoteSettingsHolder>();
//        ds.rsKeyList = RSTestUtils.rs;
//        var controller = GetRemoteSettingsController();
//        controller.CreateDataStoreDict(ref ds);
//
//        Assert.AreEqual(RSTestUtils.rs.Count, ds.rsKeys.Count);
//    }
//
//    [Test]
//    public void CreateDataStoreDictDoesNothingIfNoKeys()
//    {
//        var ds = ScriptableObject.CreateInstance<RemoteSettingsHolder>();
//        ds.rsKeyList = new List<RemoteSettingsKeyValueType>();
//        var controller = GetRemoteSettingsController();
//        controller.CreateDataStoreDict(ref ds);
//        Assert.IsNull(ds.rsKeys);
//    }
//
//    [Test]
//    public void LoadEnvironmentsReturnsCorrectEnvironmentWhenEnvNameExists()
//    {
//        var controller = GetRemoteSettingsController();
//        var envName = RSTestUtils.environments[1].name;
//        Assert.AreEqual(controller.LoadEnvironments(RSTestUtils.environments, envName), RSTestUtils.environments[1]);
//    }
//
//    [Test]
//    public void LoadEnvironmentsReturnsFirstEnvironmentEnvNameDoesntExist()
//    {
//        var controller = GetRemoteSettingsController();
//        var envName = "wrongName";
//        Assert.AreEqual(controller.LoadEnvironments(RSTestUtils.environments, envName), RSTestUtils.environments[0]);
//    }
//
//    [Test]
//    public void LoadSettingsIntoDataStoreShouldUpdateListAndDictionary()
//    {
//        var controller = GetRemoteSettingsController();
//        var ds = ScriptableObject.CreateInstance<RemoteSettingsHolder>();
//        ds.rsKeyList = new List<RemoteSettingsKeyValueType>();
//        controller.LoadSettingsIntoDataStore(RSTestUtils.rs, ref ds);
//        Assert.AreEqual(RSTestUtils.rs.Count, ds.rsKeyList.Count);
//        Assert.AreEqual(RSTestUtils.rs.Count, ds.rsKeys.Count);
//        AssertDSContainsKeysFromUtil(ref ds);
//    }
//
//    [Test]
//    public void LoadSettingsIntoDataStoreClearsDSListBefore()
//    {
//        var controller = GetRemoteSettingsController();
//        var ds = ScriptableObject.CreateInstance<RemoteSettingsHolder>();
//        ds.rsKeyList = new List<RemoteSettingsKeyValueType>();
//        var oldKey = new RemoteSettingsKeyValueType("old-key", "string", "value");
//        ds.rsKeyList.Add(oldKey);
//        controller.LoadSettingsIntoDataStore(RSTestUtils.rs, ref ds);
//        Assert.AreEqual(RSTestUtils.rs.Count, ds.rsKeyList.Count);
//        Assert.AreEqual(RSTestUtils.rs.Count, ds.rsKeys.Count);
//        AssertDSContainsKeysFromUtil(ref ds);
//        Assert.IsFalse(ds.rsKeyList.Contains(oldKey));
//        Assert.IsFalse(ds.rsKeys.ContainsKey(oldKey.key));
//    }
//
//    void AssertDSContainsKeysFromUtil(ref RemoteSettingsHolder ds)
//    {
//        Assert.IsTrue(ds.rsKeys.ContainsKey(RSTestUtils.stringKeyName));
//        Assert.IsTrue(ds.rsKeys.ContainsKey(RSTestUtils.boolKeyName));
//        Assert.IsTrue(ds.rsKeys.ContainsKey(RSTestUtils.floatKeyName));
//        Assert.IsTrue(ds.rsKeys.ContainsKey(RSTestUtils.intKeyName));
//        Assert.IsTrue(ds.rsKeys.ContainsKey(RSTestUtils.longKeyName));
//    }
//
//    public RemoteSettingsEditorWindowController GetRemoteSettingsController()
//    {
//        return new RemoteSettingsEditorWindowController();
//    }
//}
//
//public class RSControllerPrebuildSetup : IPrebuildSetup
//{
//    public void Setup()
//    {
//
//    }
//}
//
//public static class RSTestUtils
//{
//    public const string stringKeyName = "test-string";
//    public const string intKeyName = "test-int";
//    public const string floatKeyName = "test-float";
//    public const string boolKeyName = "test-bool";
//    public const string longKeyName = "test-long";
//    public const string stringValue = "test-value-1";
//    public const int intValue = 1;
//    public const float floatValue = 1.0f;
//    public const bool boolValue = true;
//    public const long longValue = 32L;
//
//    public static List<RemoteSettingsKeyValueType> rs = new List<RemoteSettingsKeyValueType>
//    {
//        new RemoteSettingsKeyValueType(stringKeyName, "string", stringValue),
//        new RemoteSettingsKeyValueType(intKeyName, "int", intValue.ToString()),
//        new RemoteSettingsKeyValueType(floatKeyName, "float", floatValue.ToString()),
//        new RemoteSettingsKeyValueType(boolKeyName, "bool", boolValue.ToString()),
//        new RemoteSettingsKeyValueType(longKeyName, "long", longValue.ToString())
//    };
//
//    public static List<RemoteSettingsEditorWindow.Environment> environments = new List<RemoteSettingsEditorWindow.Environment>
//    {
//        new RemoteSettingsEditorWindow.Environment("env-id-1", "app-id-1", "env-name-1", "env-description-1", "created-at", "updated-at"),
//        new RemoteSettingsEditorWindow.Environment("env-id-2", "app-id-2", "env-name-2", "env-description-2", "created-at", "updated-at")
//    };
//
//    public static RemoteSettingsHolder GetRSDataStore()
//    {
//        FieldInfo fieldInfo;
//        fieldInfo = typeof(RemoteSettings).GetField("RSDataStore", BindingFlags.Static | BindingFlags.NonPublic);
//        Assert.IsNotNull(fieldInfo.GetValue(null));
//        return fieldInfo.GetValue(null) as RemoteSettingsHolder;
//    }
//}
