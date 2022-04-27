using UnityEditor;
using UnityEngine;
using UnityEditor.Build;
using System.Collections.Generic;
using UnityEditor.Compilation;
using System.Reflection;
using System.Linq;
using System;

[InitializeOnLoad]
public class ES3ScriptingDefineSymbols : IActiveBuildTargetChanged
{
    static ES3ScriptingDefineSymbols()
    {
        SetDefineSymbols();
    }


    public int callbackOrder { get { return 0; } }
    public void OnActiveBuildTargetChanged(BuildTarget previousTarget, BuildTarget newTarget)
    {
        SetDefineSymbols();
    }

    static void SetDefineSymbols() 
    {
        foreach (var assembly in CompilationPipeline.GetAssemblies())
        {
            if (assembly.name.Contains("VisualScripting"))
            {
                SetDefineSymbol("UNITY_VISUAL_SCRIPTING");
                break;
            }
        }

        if (Type.GetType("Bolt.Break, Bolt.Flow.Runtime") != null)
            SetDefineSymbol("BOLT_VISUAL_SCRIPTING");
    }

    static void SetDefineSymbol(string symbol)
    {
#if UNITY_2021_2_OR_NEWER
        foreach (var target in GetAllNamedBuildTargets())
        {
            string[] defines;
            PlayerSettings.GetScriptingDefineSymbols(target, out defines);
            if(!defines.Contains(symbol))
                PlayerSettings.SetScriptingDefineSymbols(target, symbol);
        }
#else
        string definesString = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
        var allDefines = new HashSet<string>(definesString.Split(';'));
        if (!allDefines.Contains(symbol))
            PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, string.Join(";", allDefines.Concat(new string[] { symbol }).ToArray()));
#endif
            return;
    }

    static int GetCurrentUnixTimestamp()
    {
        DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        TimeSpan diff = DateTime.Now.ToUniversalTime() - origin;
        return (int)Math.Floor(diff.TotalSeconds);
    }

#if UNITY_2021_2_OR_NEWER
    static List<NamedBuildTarget> GetAllNamedBuildTargets()
    {
        var staticFields = typeof(NamedBuildTarget).GetFields(BindingFlags.Public | BindingFlags.Static);
        var buildTargets = new List<NamedBuildTarget>();

        foreach (var staticField in staticFields)
        {
            // We exclude 'Unknown' because this can throw errors when used with certain methods.
            if (staticField.Name == "Unknown")
                continue;

            if (staticField.FieldType == typeof(NamedBuildTarget))
                buildTargets.Add((NamedBuildTarget)staticField.GetValue(null));
        }

        return buildTargets;
    }
#endif
}
