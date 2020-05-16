#if UNITY_ANDROID

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using UnityEngine.Scripting;
using UnityEngine.AdaptivePerformance.Provider;

#if UNITY_2018_3_OR_NEWER
[assembly: AlwaysLinkAssembly]
#endif
namespace UnityEngine.AdaptivePerformance.Samsung.Android
{
    internal static class GameSDKLog
    {
        [Conditional("DEVELOPMENT_BUILD")]
        public static void Debug(string format, params object[] args)
        {
            if (StartupSettings.Logging)
                UnityEngine.Debug.Log(System.String.Format("[Samsung GameSDK] " + format, args));
        }
    }

    internal class AsyncUpdater : IDisposable
    {
        private Thread m_Thread;
        private bool m_Disposed = false;
        private bool m_Quit = false;

        private List<Action> m_UpdateAction = new List<Action>();
        private int[] m_UpdateRequests = null;
        private bool[] m_RequestComplete = null;
        private int m_UpdateRequestReadIndex = 0;
        private int m_UpdateRequestWriteIndex = 0;

        private object m_Mutex = new object();
        private Semaphore m_Semaphore = null;

        public int Register(Action action)
        {
            if (m_Thread.IsAlive)
                return -1;

            int handle = m_UpdateAction.Count;
            m_UpdateAction.Add(action);

            return handle;
        }

        public void Start()
        {
            int maxRequests = m_UpdateAction.Count;
            if (maxRequests <= 0)
                return;

            m_Semaphore = new Semaphore(0, maxRequests);
            m_UpdateRequests = new int[maxRequests];
            m_RequestComplete = new bool[maxRequests];

            m_Thread.Start();
        }

        public bool RequestUpdate(int handle)
        {
            lock (m_Mutex)
            {
                int newWriteIndex = (m_UpdateRequestWriteIndex + 1) % m_UpdateRequests.Length;
                if (newWriteIndex == m_UpdateRequestReadIndex)
                {
                    return false;
                }
                m_UpdateRequests[m_UpdateRequestWriteIndex] = handle;
                m_RequestComplete[handle] = false;
                m_UpdateRequestWriteIndex = newWriteIndex;
            }

            m_Semaphore.Release();

            return true;
        }

        public bool IsRequestComplete(int handle)
        {
            lock (m_Mutex)
            {
                return m_RequestComplete[handle];
            }
        }

        public AsyncUpdater()
        {
            m_Thread = new Thread(new ThreadStart(ThreadProc));
            m_Thread.Name = "SamsungGameSDK";
        }

        private void ThreadProc()
        {
            AndroidJNI.AttachCurrentThread();

            while (true)
            {
                try
                {
                    m_Semaphore.WaitOne();
                }
                catch(Exception)
                {
                    break;
                }

                int handle = -1;

                lock (m_Mutex)
                {
                    if (m_Quit)
                        break;

                    if (m_UpdateRequestReadIndex != m_UpdateRequestWriteIndex)
                    {
                        handle = m_UpdateRequests[m_UpdateRequestReadIndex];
                        m_UpdateRequestReadIndex = (m_UpdateRequestReadIndex + 1) % m_UpdateRequests.Length;
                    }
                }

                if (handle >= 0)
                {
                    try
                    {
                        m_UpdateAction[handle].Invoke();
                    }
                    catch(Exception)
                    {

                    }
                    
                    lock (m_Mutex)
                    {
                        m_RequestComplete[handle] = true;
                    }
                }
            }

            AndroidJNI.DetachCurrentThread();
        }

        private void Dispose(bool disposing)
        {
            if (m_Disposed)
                return;

            if (disposing)
            {
                if (m_Thread.IsAlive)
                {
                    lock (m_Mutex)
                    {
                        m_Quit = true;
                    }

                    m_Semaphore.Release();
                    m_Thread.Join();
                }
            }
            m_Disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }

    internal class AsyncValue<T>
    {
        private AsyncUpdater updater = null;
        private int updateHandle = -1;
        private bool pendingUpdate = false;
        private Func<T> updateFunc = null;
        private T newValue;

        private float updateTimeDeltaSeconds;
        private float updateTimestamp;

        public AsyncValue(AsyncUpdater updater, T value, float updateTimeDeltaSeconds, Func<T> updateFunc)
        {
            this.updater = updater;
            this.updateTimeDeltaSeconds = updateTimeDeltaSeconds;
            this.updateFunc = updateFunc;
            this.value = value;
            this.updateHandle = updater.Register(() => newValue = updateFunc());
        }

        public bool Update(float timestamp)
        {
            bool changed = false;

            if (pendingUpdate && updater.IsRequestComplete(updateHandle))
            {
                changed = !value.Equals(newValue);
                if (changed)
                    changeTimestamp = timestamp;

                value = newValue;
                updateTimestamp = timestamp;
                pendingUpdate = false;
            }

            if (!pendingUpdate)
            {
                if (timestamp - updateTimestamp > updateTimeDeltaSeconds)
                {
                    pendingUpdate = updater.RequestUpdate(updateHandle);
                }
            }
            return changed;
        }

        public void SyncUpdate(float timestamp)
        {
            var oldValue = value;
            updateTimestamp = timestamp;
            value = updateFunc();
            if (!value.Equals(oldValue))
                changeTimestamp = timestamp;
        }

        public T value { get; private set; }
        public float changeTimestamp { get; private set; }
    }

    [Preserve]
    public class SamsungGameSDKAdaptivePerformanceSubsystem : AdaptivePerformanceSubsystem, IApplicationLifecycle, IDevicePerformanceLevelControl
    {
        private const string sceneName = "UnityScene";
        private NativeApi m_Api = null;

        private AsyncUpdater m_AsyncUpdater;

        private PerformanceDataRecord m_Data = new PerformanceDataRecord();
        private object m_DataLock = new object();

        private AsyncValue<float> m_MainTemperature = null;
        private AsyncValue<float> m_SkinTemp = null;
        private AsyncValue<float> m_PSTLevel = null;
        private AsyncValue<double> m_GPUTime = null;
        private bool m_UseHighPrecisionSkinTemp = false;

        private Version m_Version = null;

        private float m_MinTempLevel = 0.0f;
        private float m_MaxTempLevel = 7.0f;
        private bool m_UseSetFreqLevels = false;

        override public IApplicationLifecycle ApplicationLifecycle { get { return this; } }
        override public IDevicePerformanceLevelControl PerformanceLevelControl { get { return this; } }

        public int MaxCpuPerformanceLevel { get; set; }
        public int MaxGpuPerformanceLevel { get; set; }

        public SamsungGameSDKAdaptivePerformanceSubsystem()
        {
            MaxCpuPerformanceLevel = 3;
            MaxGpuPerformanceLevel = 3;

            m_Api = new NativeApi(OnPerformanceWarning, OnPerformanceLevelTimeout);
            m_AsyncUpdater = new AsyncUpdater();
            m_PSTLevel = new AsyncValue<float>(m_AsyncUpdater, -1.0f, 3.3f, () => (float)m_Api.GetPSTLevel());
            m_SkinTemp = new AsyncValue<float>(m_AsyncUpdater, -1.0f, 2.7f, () => GetSkinTempLevel());
            m_GPUTime = new AsyncValue<double>(m_AsyncUpdater, -1.0, 0.0f, () => m_Api.GetGpuFrameTime());

            Capabilities = Feature.CpuPerformanceLevel | Feature.GpuPerformanceLevel | Feature.PerformanceLevelControl | Feature.TemperatureLevel | Feature.WarningLevel | Feature.GpuFrameTime;

            m_MainTemperature = m_SkinTemp;

            m_AsyncUpdater.Start();
        }

        public float GetSkinTempLevel()
        {
            return m_UseHighPrecisionSkinTemp ? (float) m_Api.GetHighPrecisionSkinTempLevel() : (float) m_Api.GetSkinTempLevel();
        }

        private void OnPerformanceWarning(WarningLevel warningLevel)
        {
            lock (m_DataLock)
            {
                m_Data.ChangeFlags |= Feature.WarningLevel;
                m_Data.ChangeFlags |= Feature.PerformanceLevelControl;
                m_Data.WarningLevel = warningLevel;

                // GameSDK v3.2 >= always offers CPU/GPU frequency control. PerformanceLevelControlAvailable is always available.
                // GameSDK v3.0 <= can not control CPU/GPU frequency once WarningLevel 2 is reached
                if (m_UseSetFreqLevels)
                    return;

                if (warningLevel == WarningLevel.Throttling)
                {
                    m_Data.ChangeFlags |= Feature.CpuPerformanceLevel;
                    m_Data.ChangeFlags |= Feature.GpuPerformanceLevel;
                    m_Data.CpuPerformanceLevel = Constants.UnknownPerformanceLevel;
                    m_Data.GpuPerformanceLevel = Constants.UnknownPerformanceLevel;
                    m_Data.PerformanceLevelControlAvailable = false;
                }
                else
                {
                    m_Data.PerformanceLevelControlAvailable = true;
                }
            }
        }

        private void OnPerformanceLevelTimeout()
        {
            lock(m_DataLock)
            {
                m_Data.ChangeFlags |= Feature.CpuPerformanceLevel;
                m_Data.ChangeFlags |= Feature.GpuPerformanceLevel;
                m_Data.CpuPerformanceLevel = Constants.UnknownPerformanceLevel;
                m_Data.GpuPerformanceLevel = Constants.UnknownPerformanceLevel;
            }
        }

        private void ImmediateUpdateTemperature()
        {
			var timestamp = Time.time;
            m_MainTemperature.SyncUpdate(timestamp);

            lock (m_DataLock)
            {
                m_Data.ChangeFlags |= Feature.TemperatureLevel;
                m_Data.TemperatureLevel = GetTemperatureLevel();
            }
        }

        private static bool TryParseVersion(string versionString, out Version version)
        {
            try
            {
                version = new Version(versionString);
            }
            catch(Exception)
            {
                version = null;
                return false;
            }
            return true;
        }

        override public void Start()
        {
            if (m_Api.Initialize())
            {
                if (TryParseVersion(m_Api.GetVersion(), out m_Version))
                {
                    if (m_Version >= new Version(3, 2)) 
                    {
                        m_MaxTempLevel = 10.0f;
                        m_MinTempLevel = 0.0f;
                        initialized = true;
                        m_UseHighPrecisionSkinTemp = true;
                        MaxCpuPerformanceLevel = m_Api.GetMaxCpuPerformanceLevel();
                        MaxGpuPerformanceLevel = m_Api.GetMaxGpuPerformanceLevel();
                        m_MainTemperature = m_SkinTemp;
                        m_UseSetFreqLevels = true;
                    }
                    else if (m_Version >= new Version(3, 0))
                    {
                        initialized = true;
                        m_UseHighPrecisionSkinTemp = true;
                        MaxCpuPerformanceLevel = m_Api.GetMaxCpuPerformanceLevel();
                        MaxGpuPerformanceLevel = m_Api.GetMaxGpuPerformanceLevel();
                        m_MainTemperature = m_SkinTemp;
                    }
                    else if (m_Version >= new Version(2, 0))
                    {
                        initialized = true;
                        m_UseHighPrecisionSkinTemp = true;
                    }
                    else if (m_Version >= new Version(1, 6))
                    {
                        initialized = true;
                        m_UseHighPrecisionSkinTemp = false;
                    }
                    else if (m_Version >= new Version(1, 5))
                    {
                        m_MaxTempLevel = 6.0f;
                        m_MinTempLevel = 0.0f;
                        initialized = true;
                        m_MainTemperature = m_PSTLevel;
                        m_SkinTemp = null;
                        m_UseHighPrecisionSkinTemp = false;
                    }
                    else
                    {
                        m_Api.Terminate();
                        initialized = false;
                    }
                }

                m_Data.PerformanceLevelControlAvailable = true;
            }

            if (initialized)
            {
                ImmediateUpdateTemperature();
            }
        }

        override public void Stop()
        {
        }

#if UNITY_2019_3_OR_NEWER
        protected override void OnDestroy() { DestroyInternal(); }
#else
        public override void Destroy() { DestroyInternal(); }
#endif

        private void DestroyInternal()
        {
            if (initialized)
            {
                m_Api.Terminate();
                initialized = false;
            }

            m_AsyncUpdater.Dispose();
        }

        override public string Stats
        {
            get
            {
                return String.Format("SkinTemp={0} PSTLevel={1}", m_SkinTemp != null ? m_SkinTemp.value : -1, m_PSTLevel != null ? m_PSTLevel.value : -1);
            }
        }

        override public PerformanceDataRecord Update()
        {
            // GameSDK API is very slow (~4ms per call), so update those numbers once per frame from another thread

            float timeSinceStartup = Time.time;

            m_GPUTime.Update(timeSinceStartup);

            bool tempChanged = m_MainTemperature.Update(timeSinceStartup);

            lock (m_DataLock)
            {
                if (tempChanged)
                {
                    m_Data.ChangeFlags |= Feature.TemperatureLevel;
                    m_Data.TemperatureLevel = GetTemperatureLevel();
                }

                m_Data.GpuFrameTime = LatestGpuFrameTime();
                m_Data.ChangeFlags |= Feature.GpuFrameTime;

                PerformanceDataRecord result = m_Data;
                m_Data.ChangeFlags = Feature.None;

                return result;
            }
        }

        public override Version Version
        {
            get
            {
                return m_Version;
            }
        }

        private static float NormalizeTemperatureLevel(float currentTempLevel, float minValue, float maxValue)
        {
            float tempLevel = -1.0f;
            if (currentTempLevel >= minValue && currentTempLevel <= maxValue)
            {
                tempLevel = currentTempLevel / maxValue;
                tempLevel = Math.Min(Math.Max(tempLevel, Constants.MinTemperatureLevel), maxValue);
            }
            return tempLevel;
        }

        private float NormalizeTemperatureLevel(float currentTempLevel)
        {
            return NormalizeTemperatureLevel(currentTempLevel, m_MinTempLevel, m_MaxTempLevel);
        }

        private float GetTemperatureLevel()
        {
            return NormalizeTemperatureLevel(m_MainTemperature.value);
        }

        private float LatestGpuFrameTime()
        {
            var frameTimeMs = m_GPUTime.value;
            // Until GameSDK 1.6 we get 0.0 in some error cases, so we only treat values > 0.0 as valid.
            if (frameTimeMs > 0.0)
            {
                return (float)(frameTimeMs / 1000.0);
            }
            return -1.0f;
        }

        public bool SetPerformanceLevel(int cpuLevel, int gpuLevel)
        {
            if (cpuLevel < 0)
                cpuLevel = 0;
            else if (cpuLevel > MaxCpuPerformanceLevel)
                cpuLevel = MaxCpuPerformanceLevel;

            if (gpuLevel < 0)
                gpuLevel = 0;
            else if (gpuLevel > MaxGpuPerformanceLevel)
                gpuLevel = MaxGpuPerformanceLevel;

            bool success = false;
            if (m_UseSetFreqLevels)
            {
                int result = m_Api.SetFreqLevels(cpuLevel, gpuLevel);
                success = result != 0;
            }
            else
            {
                success = m_Api.SetLevelWithScene(sceneName, cpuLevel, gpuLevel);
            }

            lock (m_DataLock)
            {
                var oldCpuLevel = m_Data.CpuPerformanceLevel;
                var oldGpuLevel = m_Data.GpuPerformanceLevel;

                m_Data.CpuPerformanceLevel = success ? cpuLevel : Constants.UnknownPerformanceLevel;
                m_Data.GpuPerformanceLevel = success ? gpuLevel : Constants.UnknownPerformanceLevel;

                if (m_Data.CpuPerformanceLevel != oldCpuLevel)
                    m_Data.ChangeFlags |= Feature.CpuPerformanceLevel;
                if (m_Data.GpuPerformanceLevel != oldGpuLevel)
                    m_Data.ChangeFlags |= Feature.GpuPerformanceLevel;
            }
            return success;
        }

        public void ApplicationPause() {}

        public void ApplicationResume()
        {
            lock (m_DataLock)
            {
                // TODO: check if levels are actually unknown or 0 on resume!
                m_Data.CpuPerformanceLevel = Constants.UnknownPerformanceLevel;
                m_Data.GpuPerformanceLevel = Constants.UnknownPerformanceLevel;
                m_Data.ChangeFlags |= Feature.CpuPerformanceLevel;
                m_Data.ChangeFlags |= Feature.GpuPerformanceLevel;
            }

            ImmediateUpdateTemperature();
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void RegisterDescriptor()
        {
            if (!SystemInfo.deviceModel.StartsWith("samsung", StringComparison.OrdinalIgnoreCase))
                return;

            if (!NativeApi.IsAvailable())
                return;

            AdaptivePerformanceSubsystemDescriptor.RegisterDescriptor(new AdaptivePerformanceSubsystemDescriptor.Cinfo
            {
                id = "SamsungGameSDK",
                subsystemImplementationType = typeof(SamsungGameSDKAdaptivePerformanceSubsystem)
            });
        }

        internal class NativeApi : AndroidJavaProxy
        {
            static private AndroidJavaObject s_GameSDK = null;
            static private IntPtr s_GameSDKRawObjectID;
            static private IntPtr s_GetGpuFrameTimeID;
            static private IntPtr s_GetPSTLevelID;
            static private IntPtr s_GetSkinTempLevelID;
            static private IntPtr s_GetHighPrecisionSkinTempLevelID;

            static private bool s_isAvailable = false;
            static private jvalue[] s_NoArgs = new jvalue[0];

            private Action<WarningLevel> PerformanceWarningEvent;
            private Action PerformanceLevelTimeoutEvent;

            public NativeApi(Action<WarningLevel> sustainedPerformanceWarning, Action sustainedPerformanceTimeout)
                : base("com.samsung.android.gamesdk.GameSDKManager$Listener")
            {
                PerformanceWarningEvent = sustainedPerformanceWarning;
                PerformanceLevelTimeoutEvent = sustainedPerformanceTimeout;
                StaticInit();              
            }

            [Preserve]
            void onHighTempWarning(int warningLevel)
            {
                GameSDKLog.Debug("Listener: onHighTempWarning(warningLevel={0})", warningLevel);
                if (warningLevel == 0)
                    PerformanceWarningEvent(WarningLevel.NoWarning);
                else if (warningLevel == 1)
                    PerformanceWarningEvent(WarningLevel.ThrottlingImminent);
                else if (warningLevel == 2)
                    PerformanceWarningEvent(WarningLevel.Throttling);
            }

            [Preserve]
            void onReleasedByTimeout()
            {
                GameSDKLog.Debug("Listener: onReleasedByTimeout()");
                PerformanceLevelTimeoutEvent();
            }

            [Preserve]
            void onRefreshRateChanged()
            {
                GameSDKLog.Debug("Listener: onRefreshRateChanged()");
                // Not used in 1.x.x. Available in 2.0.0 but the callback is needed to avoid that Samsung GameSDK is correctly calling other callbacks on VRR enabled devices. 
            }

            static IntPtr GetJavaMethodID(IntPtr classId, string name, string sig)
            {
                AndroidJNI.ExceptionClear();
                var mid = AndroidJNI.GetMethodID(classId, name, sig);

                IntPtr ex = AndroidJNI.ExceptionOccurred();
                if (ex != (IntPtr)0)
                {
                    AndroidJNI.ExceptionDescribe();
                    AndroidJNI.ExceptionClear();
                    return (IntPtr)0;
                }
                else
                {
                    return mid;
                }
            }

            static private void StaticInit()
            {
                if (s_GameSDK == null)
                {
                    Int64 startTime = DateTime.Now.Ticks;
                    try
                    {
                        s_GameSDK = new AndroidJavaObject("com.samsung.android.gamesdk.GameSDKManager");
                        if (s_GameSDK != null)
                            s_isAvailable = s_GameSDK.CallStatic<bool>("isAvailable");
                    }
                    catch (Exception)
                    {
                        s_isAvailable = false;
                        s_GameSDK = null;
                    }

                    if (s_isAvailable)
                    {
                        s_GameSDKRawObjectID = s_GameSDK.GetRawObject();
                        var classID = s_GameSDK.GetRawClass();

                        s_GetPSTLevelID = GetJavaMethodID(classID, "getTempLevel", "()I");
                        s_GetGpuFrameTimeID = GetJavaMethodID(classID, "getGpuFrameTime", "()D");
                        s_GetSkinTempLevelID = GetJavaMethodID(classID, "getSkinTempLevel", "()I");
                        s_GetHighPrecisionSkinTempLevelID = GetJavaMethodID(classID, "getHighPrecisionSkinTempLevel", "()D");

                        if (s_GetGpuFrameTimeID == (IntPtr)0 || s_GetSkinTempLevelID == (IntPtr)0)
                            s_isAvailable = false;
                    }
                }
            }

            static public bool IsAvailable()
            {
                StaticInit();
                return s_isAvailable;
            }

            public bool RegisterListener()
            {
                bool success = false;
                try
                {
                    success = s_GameSDK.Call<bool>("setListener", this);
                }
                catch (Exception)
                {
                    success = false;
                   
                }

                if (!success)
                    GameSDKLog.Debug("failed to register listener");

                return success;
            }

            public void UnregisterListener()
            {
                bool success = true;
                try
                {
                    GameSDKLog.Debug("setListener(null)");
                    success = s_GameSDK.Call<bool>("setListener", (Object)null);
                }
                catch (Exception)
                {
                    success = false;
                }

                if (!success)
                    GameSDKLog.Debug("setListener(null) failed!");
            }

            public bool Initialize()
            {
                bool isInitialized = false;
                try
                {
                    Version initVersion;
                    if (TryParseVersion(GetVersion(), out initVersion))
                    {
                        if (initVersion < new Version(3, 0))
                        {
                            isInitialized = s_GameSDK.Call<bool>("initialize");
                        }
                        else
                        {
                            // There is a critical bug which can lead to overheated devices in GameSDK 3.1 so we will not initialize GameSDK or Adaptive Performance
                            if (initVersion == new Version(3, 1))
                            {
                                GameSDKLog.Debug("GameSDK 3.1 is not supported and will not be initialized, Adaptive Performance will not be used.");
                            } 
                            else
                            {
                                isInitialized = s_GameSDK.Call<bool>("initialize", initVersion.ToString());
                            }
                        }

                        if (isInitialized)
                        {
                            isInitialized = RegisterListener();
                        }
                        else
                        {
                            GameSDKLog.Debug("GameSDK.initialize() failed!");
                        }
                    }
                }
                catch (Exception)
                {
                    GameSDKLog.Debug("[Exception] GameSDK.initialize() failed!");
                }

                return isInitialized;
            }

            public void Terminate()
            {
                UnregisterListener();

                bool success = true;

                try
                {
                    var packageName = Application.identifier;
                    GameSDKLog.Debug("GameSDK.finalize({0})", packageName);
                    success = s_GameSDK.Call<bool>("finalize", packageName);
                }
                catch (Exception)
                {
                    success = false;
                }

                if (!success)
                    GameSDKLog.Debug("GameSDK.finalize() failed!");
            }

            public string GetVersion()
            {
                string sdkVersion = "";
                try
                {
                    sdkVersion = s_GameSDK.Call<string>("getVersion");
                }
                catch (Exception)
                {
                    GameSDKLog.Debug("[Exception] GameSDK.getVersion() failed!");
                }
                return sdkVersion;
            }

            public int GetPSTLevel()
            {
                int currentTempLevel = -1;
                try
                {
                    currentTempLevel = AndroidJNI.CallIntMethod(s_GameSDKRawObjectID, s_GetPSTLevelID, s_NoArgs);
                    if (AndroidJNI.ExceptionOccurred() != IntPtr.Zero)
                    {
                        AndroidJNI.ExceptionDescribe();
                        AndroidJNI.ExceptionClear();
                    }
                }
                catch (Exception)
                {
                    GameSDKLog.Debug("[Exception] GameSDK.getPSTLevel() failed!");
                }
                return currentTempLevel;
            }

            public int GetSkinTempLevel()
            {
                int currentTempLevel = -1;
                try
                {
                    currentTempLevel = AndroidJNI.CallIntMethod(s_GameSDKRawObjectID, s_GetSkinTempLevelID, s_NoArgs);
                    if (AndroidJNI.ExceptionOccurred() != IntPtr.Zero)
                    {
                        AndroidJNI.ExceptionDescribe();
                        AndroidJNI.ExceptionClear();
                    }
                }
                catch (Exception)
                {
                    GameSDKLog.Debug("[Exception] GameSDK.getSkinTempLevel() failed!");
                }
                return currentTempLevel;
            }

            public double GetHighPrecisionSkinTempLevel()
            {
                double currentTempLevel = -1.0;
                try
                {
                    currentTempLevel = AndroidJNI.CallDoubleMethod(s_GameSDKRawObjectID, s_GetHighPrecisionSkinTempLevelID, s_NoArgs);
                    if (AndroidJNI.ExceptionOccurred() != IntPtr.Zero)
                    {
                        AndroidJNI.ExceptionDescribe();
                        AndroidJNI.ExceptionClear();
                    }
                }
                catch (Exception)
                {
                    GameSDKLog.Debug("[Exception] GameSDK.getHighPrecisionSkinTempLevel() failed!");
                }
                return currentTempLevel;
            }

            public double GetGpuFrameTime()
            {
                double gpuFrameTime = -1.0;
                try
                {
                    gpuFrameTime = AndroidJNI.CallDoubleMethod(s_GameSDKRawObjectID, s_GetGpuFrameTimeID, s_NoArgs);
                    if (AndroidJNI.ExceptionOccurred() != IntPtr.Zero)
                    {
                        AndroidJNI.ExceptionDescribe();
                        AndroidJNI.ExceptionClear();
                    }
                }
                catch (Exception)
                {
                    GameSDKLog.Debug("[Exception] GameSDK.getGpuFrameTime() failed!");
                }

                return gpuFrameTime;
            }

            public bool SetLevelWithScene(string scene, int cpu, int gpu)
            {
                bool success = false;
                try
                {
                    success = s_GameSDK.Call<bool>("setLevelWithScene", scene, cpu, gpu);
                    GameSDKLog.Debug("setLevelWithScene({0}, {1}, {2}) -> {3}", scene, cpu, gpu, success);
                }
                catch (Exception)
                {
                    GameSDKLog.Debug("[Exception] GameSDK.setLevelWithScene({0}, {1}, {2}) failed!", scene, cpu, gpu);
                }
                return success;
            }

            public int SetFreqLevels(int cpu, int gpu)
            {
                int result = 0;
                try
                {
                    result = s_GameSDK.Call<int>("setFreqLevels", cpu, gpu);
                    GameSDKLog.Debug("setFreqLevels({0}, {1}) -> {2}", cpu, gpu, result);
                }
                catch (Exception x)
                {
                    GameSDKLog.Debug("[Exception] GameSDK.setFreqLevels({0}, {1}) failed: {2}", cpu, gpu, x);
                }
                return result;
            }

            public int GetMaxCpuPerformanceLevel()
            {
                int maxCpuPerformanceLevel = -1;
                try
                {
                    maxCpuPerformanceLevel = s_GameSDK.Call<int>("getCPULevelMax");
                }
                catch (Exception)
                {
                    GameSDKLog.Debug("[Exception] GameSDK.getCPULevelMax() failed!");
                }

                return maxCpuPerformanceLevel;
            }

            public int GetMaxGpuPerformanceLevel()
            {
                int maxGpuPerformanceLevel = -1;
                try
                {
                    maxGpuPerformanceLevel = s_GameSDK.Call<int>("getGPULevelMax");
                }
                catch (Exception)
                {
                    GameSDKLog.Debug("[Exception] GameSDK.getCPULevelMax() failed!");
                }

                return maxGpuPerformanceLevel;
            }

        }

    }
}

#endif
