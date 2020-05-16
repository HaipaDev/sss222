using System;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.ResourceManagement.Util;

namespace UnityEngine.ResourceManagement.ResourceProviders
{
    /// <summary>
    /// Provides assets stored in an asset bundle.
    /// </summary>
    public class BundledAssetProvider : ResourceProviderBase
    {
        internal class InternalOp
        {
            AssetBundleRequest m_RequestOperation;
            ProvideHandle m_ProvideHandle;
            string subObjectName = null;
            internal static AssetBundle LoadBundleFromDependecies(IList<object> results)
            {
                if (results == null || results.Count == 0)
                    return null;

                AssetBundle bundle = null;
                bool firstBundleWrapper = true;
                for (int i = 0; i < results.Count; i++)
                {
                    var abWrapper = results[i] as IAssetBundleResource;
                    if (abWrapper != null)
                    {
                        //only use the first asset bundle, even if it is invalid
                        var b = abWrapper.GetAssetBundle();
                        if (firstBundleWrapper)
                            bundle = b;
                        firstBundleWrapper = false;
                    }
                }
                return bundle;
            }
            
            public void Start(ProvideHandle provideHandle)
            {
                subObjectName = null;
                m_ProvideHandle = provideHandle;
                m_RequestOperation = null;
                List<object> deps = new List<object>(); // TODO: garbage. need to pass actual count and reuse the list
                m_ProvideHandle.GetDependencies(deps);
                AssetBundle bundle = LoadBundleFromDependecies(deps);
                if (bundle == null)
                {
                    m_ProvideHandle.Complete<AssetBundle>(null, false, new Exception("Unable to load dependent bundle from location " + m_ProvideHandle.Location));
                }
                else
                {
                    var assetPath = m_ProvideHandle.Location.InternalId;
                    if (m_ProvideHandle.Type.IsArray)
                        m_RequestOperation = bundle.LoadAssetWithSubAssetsAsync(assetPath, m_ProvideHandle.Type.GetElementType());
                    else if (m_ProvideHandle.Type.IsGenericType && typeof(IList<>) == m_ProvideHandle.Type.GetGenericTypeDefinition())
                        m_RequestOperation = bundle.LoadAssetWithSubAssetsAsync(assetPath, m_ProvideHandle.Type.GetGenericArguments()[0]);
                    else
                    {
                        var i = assetPath.LastIndexOf('[');
                        if (i > 0)
                        {
                            var i2 = assetPath.LastIndexOf(']');
                            if (i2 < i)
                            {
                                m_ProvideHandle.Complete<AssetBundle>(null, false, new Exception(string.Format("Invalid index format in internal id {0}", assetPath)));
                            }
                            else
                            {
                                subObjectName = assetPath.Substring(i + 1, i2 - (i + 1));
                                assetPath = assetPath.Substring(0, i);
                                m_RequestOperation = bundle.LoadAssetWithSubAssetsAsync(assetPath, m_ProvideHandle.Type);
                            }
                        }
                        else
                        {
                            m_RequestOperation = bundle.LoadAssetAsync(assetPath, m_ProvideHandle.Type);
                        }
                    }
                    m_RequestOperation.completed += ActionComplete;
                    provideHandle.SetProgressCallback(ProgressCallback);
                }
            }

            private void ActionComplete(AsyncOperation obj)
            {
                object result = null;
                if (m_RequestOperation != null)
                {
                    if (m_ProvideHandle.Type.IsArray)
                    {
                        result = ResourceManagerConfig.CreateArrayResult(m_ProvideHandle.Type, m_RequestOperation.allAssets);
                    }
                    else if (m_ProvideHandle.Type.IsGenericType && typeof(IList<>) == m_ProvideHandle.Type.GetGenericTypeDefinition())
                    {
                        result = ResourceManagerConfig.CreateListResult(m_ProvideHandle.Type, m_RequestOperation.allAssets);
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(subObjectName))
                        {
                            result = (m_RequestOperation.asset != null && m_ProvideHandle.Type.IsAssignableFrom(m_RequestOperation.asset.GetType())) ? m_RequestOperation.asset : null;
                        }
                        else
                        {
                            if (m_RequestOperation.allAssets != null)
                            {
                                foreach (var o in m_RequestOperation.allAssets)
                                {
                                    if (o.name == subObjectName)
                                    {
                                        if (m_ProvideHandle.Type.IsAssignableFrom(o.GetType()))
                                        {
                                            result = o;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                m_ProvideHandle.Complete(result, result != null, null);
            }

            public float ProgressCallback() { return m_RequestOperation != null ? m_RequestOperation.progress : 0.0f; }
        }

        /// <inheritdoc/>
        public override void Provide(ProvideHandle provideHandle)
        {
            new InternalOp().Start(provideHandle);
        }
    }
}
