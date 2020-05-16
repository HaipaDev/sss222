using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.AddressableAssets.GUI;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.Build.Pipeline;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace UnityEditor.AddressableAssets.Build.AnalyzeRules
{
    class BuildBundleLayout : BundleRuleBase
    {
        public override bool CanFix { get{return false;} }
        public override string ruleName { get {return "Build Bundle Layout";}}

        public override List<AnalyzeResult> RefreshAnalysis(AddressableAssetSettings settings)
        {
            ClearAnalysis();

            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                Debug.LogError("Cannot run Analyze with unsaved scenes");
                m_Results.Add(new AnalyzeResult {resultName = ruleName + "Cannot run Analyze with unsaved scenes"});
                return m_Results;
            }

            CalculateInputDefinitions(settings);
            RefreshBuild(GetBuildContext(settings));

            m_Results = (from bundleBuild in m_AllBundleInputDefs
                         let bundleName = bundleBuild.assetBundleName
                         from asset in bundleBuild.assetNames
                         select new AnalyzeResult { resultName = bundleName + kDelimiter + "Explicit" + kDelimiter + asset }).ToList();

            m_Results.AddRange((from fileToBundle in m_ExtractData.WriteData.FileToBundle
                                from guid in GetImplicitGuidsForBundle(fileToBundle.Key)
                                let bundleName = fileToBundle.Value
                                let assetPath = AssetDatabase.GUIDToAssetPath(guid.ToString())
                                where AddressableAssetUtility.IsPathValidForEntry(assetPath)
                                select new AnalyzeResult { resultName = bundleName + kDelimiter + "Implicit" + kDelimiter + assetPath }).ToList());

            if (m_Results.Count == 0)
                m_Results.Add(noErrors);

            return m_Results;
        }

        [InitializeOnLoad]
        class RegisterBuildBundleLayout
        {
            static RegisterBuildBundleLayout()
            {
                AnalyzeWindow.RegisterNewRule<BuildBundleLayout>();
            }
        }
    }
}
