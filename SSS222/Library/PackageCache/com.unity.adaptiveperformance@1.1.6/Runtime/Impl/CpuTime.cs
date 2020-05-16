using System;
using System.Runtime.InteropServices;
using UnityEngine.Rendering;

namespace UnityEngine.AdaptivePerformance
{
 #if UNITY_ANDROID
    internal class RenderThreadCpuTime
    {
        [DllImport("AndroidCpuUsage")]
        private static extern void AndroidCpuUsage_Reset();

        [DllImport("AndroidCpuUsage")]
        private static extern double AndroidCpuUsage_GetLatestCpuTimeDelta();

        [DllImport("AndroidCpuUsage")]
        private static extern IntPtr AndroidCpuUsage_GetPluginCallback();

        CommandBuffer m_Commandbuffer = null;
        public RenderThreadCpuTime()
        {
            m_Commandbuffer = new CommandBuffer();
            m_Commandbuffer.IssuePluginEventAndData(AndroidCpuUsage_GetPluginCallback(), 0, (IntPtr)0);
        }

        public void Reset()
        {
            AndroidCpuUsage_Reset();
        }

        public void Measure()
        {
            Graphics.ExecuteCommandBuffer(m_Commandbuffer);
        }

        public float GetLatestResult()
        {
            return (float) AndroidCpuUsage_GetLatestCpuTimeDelta();
        }
    }
#else
    internal class RenderThreadCpuTime
    {
        public RenderThreadCpuTime() {}
        public void Reset() {}
        public void Measure() {}
        public float GetLatestResult() { return -1.0f; }
    }
#endif

    internal class MainThreadCpuTime
    {
        private double m_LastAbsoluteMainThreadCpuTime = 0.0;
        private float m_LatestMainthreadCpuTime = -1.0f;

#if UNITY_ANDROID
        [DllImport("AndroidCpuUsage")]
        private static extern double AndroidCpuUsage_CpuTimeForCurrentThread();
#endif

        public float GetLatestResult()
        {
            return m_LatestMainthreadCpuTime;
        }

        public void Measure()
        {
            double cpuTime = 0.0;
#if UNITY_ANDROID
            cpuTime = AndroidCpuUsage_CpuTimeForCurrentThread();
#endif
            if (cpuTime > 0.0)
            {
                double dt = cpuTime - m_LastAbsoluteMainThreadCpuTime;
                m_LastAbsoluteMainThreadCpuTime = cpuTime;
                m_LatestMainthreadCpuTime = (float)dt;
            }
        }

        public MainThreadCpuTime()
        {
        }
    }

    internal class CpuTimeProvider
    {
        RenderThreadCpuTime m_RenderThreadCpuTime;
        MainThreadCpuTime m_MainThreadCpuTime;

        public float CpuFrameTime
        {
            get
            {
                if (m_RenderThreadCpuTime != null)
                    return Mathf.Max(m_MainThreadCpuTime.GetLatestResult(), m_RenderThreadCpuTime.GetLatestResult());
                else
                    return m_MainThreadCpuTime.GetLatestResult();
            }
        }

        public CpuTimeProvider()
        {
            m_MainThreadCpuTime = new MainThreadCpuTime();
            if (SystemInfo.graphicsMultiThreaded)
                m_RenderThreadCpuTime = new RenderThreadCpuTime();
        }

        public void Reset()
        {
            if (m_RenderThreadCpuTime != null)
                m_RenderThreadCpuTime.Reset();
        }

        public void LateUpdate()
        {
            if (m_RenderThreadCpuTime != null)
                m_RenderThreadCpuTime.Measure();
        }

        public void EndOfFrame()
        {
            m_MainThreadCpuTime.Measure();
        }
    }
}
