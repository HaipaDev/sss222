using UnityEditor.PackageManager.Requests;
using UnityEditor.PackageManager;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace UnityEditor.AdaptivePerformance
{
    internal class AdaptivePerformancePostProcess : IPreprocessBuildWithReport 
    {
        public int callbackOrder { get { return 0; } }

        public void OnPreprocessBuild(BuildReport report)
        {
            CheckInstalledProvider();
        }

        static ListRequest Request;

        /// <summary>
        /// Requests a list of all installed packages from PackageManager which are processed in CheckInstalledPackages.
        /// </summary>
        static void CheckInstalledProvider()
        {
            Request = Client.List();    // List packages installed for the Project
            EditorApplication.update += CheckInstalledPackages;
        }

        /// <summary>
        /// Processes a list of all installed packages and notifies user via console if no Adaptive Performance Provider package is installed. 
        /// </summary>
        static void CheckInstalledPackages()
        {
            if (Request.IsCompleted)
            {
                if (Request.Status == StatusCode.Success)
                {
                    var installedPackageCount = 0;
                
                    foreach (var package in Request.Result)
                        if(package.name.StartsWith("com.unity.adaptiveperformance."))
                            installedPackageCount++;
                  
                    if(installedPackageCount == 0)
                    { 
                        Debug.LogWarning("No Adaptive Performance provider package installed. Adaptive Performance requires a provider to get useful information during runtime. Please install a provider such as, Adaptive Performance Samsung (Android), via the Unity Package Manager.");
#if UNITY_2019_3_OR_NEWER
                        PackageManager.UI.Window.Open("com.unity.adaptiveperformance.samsung.android");
#endif
                    }
                }
                else if (Request.Status >= StatusCode.Failure)
                    Debug.Log(Request.Error.message);

                EditorApplication.update -= CheckInstalledPackages;
            }
        }
    }
}