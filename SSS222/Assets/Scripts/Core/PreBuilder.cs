using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

public class PreBuilder : IPreprocessBuildWithReport{
    int IOrderedCallback.callbackOrder => throw new System.NotImplementedException();

    public void OnPreprocessBuild(BuildReport report){
        GameSession.instance.buildVersion++;
    }
}
