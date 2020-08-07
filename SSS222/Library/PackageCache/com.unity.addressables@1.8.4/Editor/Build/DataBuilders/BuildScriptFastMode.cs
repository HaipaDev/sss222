﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.Initialization;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.AddressableAssets.ResourceProviders;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.ResourceManagement.Util;

namespace UnityEditor.AddressableAssets.Build.DataBuilders
{
    /// <summary>
    /// Build script used for fast iteration in the editor.
    /// </summary>
    [CreateAssetMenu(fileName = "BuildScriptFast.asset", menuName = "Addressables/Content Builders/Use Asset Database (faster)")]
    public class BuildScriptFastMode : BuildScriptBase
    {
        /// <inheritdoc />
        public override string Name
        {
            get
            {
                return "Use Asset Database (faster)";
            }
        }

        /// <inheritdoc />
        public override bool CanBuildData<T>()
        {
            return typeof(T).IsAssignableFrom(typeof(AddressablesPlayModeBuildResult));
        }

        /// <inheritdoc />
        public override void ClearCachedData()
        {
            DeleteFile(string.Format(PathFormat, "", "catalog"));
            DeleteFile(string.Format(PathFormat, "", "settings"));
        }

        /// <inheritdoc />
        public override bool IsDataBuilt()
        {
            var catalogPath = string.Format(PathFormat, "", "catalog");
            var settingsPath = string.Format(PathFormat, "", "settings");
            return File.Exists(catalogPath) &&
                   File.Exists(settingsPath);
        }

        private string m_PathFormatStore;
        private string PathFormat
        {
            get
            {
                if(string.IsNullOrEmpty(m_PathFormatStore))
                    m_PathFormatStore = "{0}Library/com.unity.addressables/{1}_BuildScriptFastMode.json";
                return m_PathFormatStore;
            }
            set { m_PathFormatStore = value; }
        }
        bool m_NeedsLegacyProvider = false;
        
        /// <inheritdoc />
        protected override TResult BuildDataImplementation<TResult>(AddressablesDataBuilderInput context)
        {
            TResult result = default(TResult);
            
            var timer = new Stopwatch();
            timer.Start();
            var aaSettings = context.AddressableSettings;
            PathFormat = context.PathFormat;

            //create runtime data
            var aaContext = new AddressableAssetsBuildContext
            {
                settings = aaSettings,
                runtimeData = new ResourceManagerRuntimeData(),
                bundleToAssetGroup = null,
                locations = new List<ContentCatalogDataEntry>(),
                providerTypes = new HashSet<Type>()
            };
            aaContext.runtimeData.BuildTarget = context.Target.ToString();
            aaContext.runtimeData.LogResourceManagerExceptions = aaSettings.buildSettings.LogResourceManagerExceptions;
            aaContext.runtimeData.ProfileEvents = ProjectConfigData.postProfilerEvents;
            aaContext.runtimeData.CatalogLocations.Add(new ResourceLocationData(new[] { ResourceManagerRuntimeData.kCatalogAddress }, string.Format(PathFormat, "file://{UnityEngine.Application.dataPath}/../", "catalog"), typeof(ContentCatalogProvider), typeof(ContentCatalogData)));

            var errorString = ProcessAllGroups(aaContext);
            if(!string.IsNullOrEmpty(errorString))
                result = AddressableAssetBuildResult.CreateResult<TResult>(null, 0, errorString);

            if (result == null)
            {
                foreach (var io in aaSettings.InitializationObjects)
                {
                    if (io is IObjectInitializationDataProvider)
                        aaContext.runtimeData.InitializationObjects.Add((io as IObjectInitializationDataProvider).CreateObjectInitializationData());
                }

                var settingsPath = string.Format(PathFormat, "", "settings");
                WriteFile(settingsPath, JsonUtility.ToJson(aaContext.runtimeData), context.Registry);

                //save catalog
                var catalogData = new ContentCatalogData(aaContext.locations, ResourceManagerRuntimeData.kCatalogAddress);
                if (m_NeedsLegacyProvider)
                    catalogData.ResourceProviderData.Add(ObjectInitializationData.CreateSerializedInitializationData(typeof(LegacyResourcesProvider)));
                foreach (var t in aaContext.providerTypes)
                    catalogData.ResourceProviderData.Add(ObjectInitializationData.CreateSerializedInitializationData(t));
                catalogData.ResourceProviderData.Add(ObjectInitializationData.CreateSerializedInitializationData<AssetDatabaseProvider>());

                catalogData.InstanceProviderData = ObjectInitializationData.CreateSerializedInitializationData(instanceProviderType.Value);
                catalogData.SceneProviderData = ObjectInitializationData.CreateSerializedInitializationData(sceneProviderType.Value);
                WriteFile(string.Format(PathFormat, "", "catalog"), JsonUtility.ToJson(catalogData), context.Registry);


                //inform runtime of the init data path
                var runtimeSettingsPath = string.Format(PathFormat, "file://{UnityEngine.Application.dataPath}/../", "settings");
                PlayerPrefs.SetString(Addressables.kAddressablesRuntimeDataPath, runtimeSettingsPath);
                result = AddressableAssetBuildResult.CreateResult<TResult>(settingsPath, aaContext.locations.Count);
            }
            
            if(result != null)
                result.Duration = timer.Elapsed.TotalSeconds;

            return result;
        }
        
        /// <inheritdoc />
        protected override string ProcessGroup(AddressableAssetGroup assetGroup, AddressableAssetsBuildContext aaContext)
        {
            float index = 0.0f;
            foreach (var entry in assetGroup.entries)
            {
                if (entry.AssetPath == AddressableAssetEntry.ResourcesPath || entry.AssetPath == AddressableAssetEntry.EditorSceneListPath)
                {
                    GatherAssetsForFolder(entry, aaContext);   
                }
                else
                {
                    FileAttributes file = File.GetAttributes(entry.AssetPath);
                    if (file.HasFlag(FileAttributes.Directory))
                    {
                        GatherAssetsForFolder(entry, aaContext);
                    }
                    else
                        entry.CreateCatalogEntries(aaContext.locations, false, typeof(AssetDatabaseProvider).FullName,
                            null, null, aaContext.providerTypes);
                }

                index++;
            }

            return string.Empty;
        }

        private void GatherAssetsForFolder(AddressableAssetEntry entry, AddressableAssetsBuildContext aaContext)
        {
            var allEntries = new List<AddressableAssetEntry>();
            entry.GatherAllAssets(allEntries, false, true, false);
            foreach (var e in allEntries)
                e.CreateCatalogEntries(aaContext.locations, false, typeof(AssetDatabaseProvider).FullName, null, null, aaContext.providerTypes);
         }
    }
}