using System.Collections.Generic;
using System.IO;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;
using UnityEditor.Compilation;

namespace UnityEditor.AddressableAssets.Build
{
    internal class BuildUtility
    {
        static HashSet<string> s_EditorAssemblies = null;
        static HashSet<string> editorAssemblies
        {
            get
            {
                if (s_EditorAssemblies == null)
                {
                    s_EditorAssemblies = new HashSet<string>();
                    foreach (var assembly in CompilationPipeline.GetAssemblies())
                    {
                        if ((assembly.flags & AssemblyFlags.EditorAssembly) != 0)
                            s_EditorAssemblies.Add(assembly.name);
                    }
                }

                return s_EditorAssemblies;
            }
        }
        public static bool IsEditorAssembly(System.Reflection.Assembly assembly)
        {
            var splitName = assembly.FullName.Split(',');
            return splitName.Length > 0 && editorAssemblies.Contains(splitName[0]);
        }
        
        public static string GetNameWithHashNaming(BundledAssetGroupSchema.BundleNamingStyle schemaBundleNaming, string hash, string sourceBundleName)
        {
            string result = sourceBundleName;
            switch (schemaBundleNaming)
            {
                case BundledAssetGroupSchema.BundleNamingStyle.AppendHash:
                    result = sourceBundleName.Replace(".bundle", "_" + hash + ".bundle");
                    break;
                case BundledAssetGroupSchema.BundleNamingStyle.NoHash:
                    break;
                case BundledAssetGroupSchema.BundleNamingStyle.OnlyHash:
                    string fileName = Path.GetFileNameWithoutExtension(sourceBundleName);
                    if(!string.IsNullOrEmpty(fileName))
                        result = sourceBundleName.Replace(fileName, hash);
                    break;
            }

            return result;

        }
    }
}
