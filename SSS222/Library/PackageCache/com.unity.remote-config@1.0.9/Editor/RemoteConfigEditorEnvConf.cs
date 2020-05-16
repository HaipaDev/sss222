using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unity.RemoteConfig.Editor
{
    internal static class RemoteConfigEditorEnvConf
    {
        //REST API Paths
        internal const string basePath = "https://remote-config-api.uca.cloud.unity3d.com/";
        internal const string queryParam = "?projectId={0}";
        internal const string environmentPath = basePath + "environments" + queryParam;
        internal const string getConfigPath = basePath + "environments/{1}/configs" + queryParam;
        internal const string postConfigPath = basePath + "configs" + queryParam;
        internal const string putConfigPath = basePath + "configs/{1}" + queryParam;
        internal const string multiRulesPath = basePath + "configs/{1}/rules" + queryParam;
        internal const string postRulePath = basePath + "rules" + queryParam;
        internal const string singleRulePath = basePath + "rules/{1}" + queryParam;
        internal const string getDefaultEnvironmentPath = basePath + "environments/default" + queryParam;
        internal const string pluginVersion = "1.0.9";
        //Dashboard URLs
        internal const string dashboardBasePath = "https://app.remote-config.unity3d.com/";
        internal const string dashboardProjectIdQueryParam = "projectId={0}";
        internal const string dashboardEnvironmentIdQueryParam = "environmentId={1}";
        internal const string dashboardEnvironmentsPath = dashboardBasePath + "environments?" + dashboardProjectIdQueryParam;
        internal const string dashboardConfigsPath = dashboardBasePath + "configs?" + dashboardEnvironmentIdQueryParam + "&" + dashboardProjectIdQueryParam;
        //Docs URLs
        internal const string apiDocsBasePath = "https://remote-config-api-docs.uca.cloud.unity3d.com/";
        internal const string apiDocsRulesPath = "#tag/Rules";
    }
}
