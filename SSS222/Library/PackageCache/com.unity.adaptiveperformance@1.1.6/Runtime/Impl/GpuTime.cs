namespace UnityEngine.AdaptivePerformance
{
    internal class GpuTimeProvider
    {
        private UnityEngine.FrameTiming[] m_FrameTiming = new UnityEngine.FrameTiming[1];

        public GpuTimeProvider()
        {
        }

        public float GpuFrameTime
        {
            get
            {
                if (UnityEngine.FrameTimingManager.GetLatestTimings(1, m_FrameTiming) >= 1)
                {
                    double gpuFrameTime = m_FrameTiming[0].gpuFrameTime;
                    if (gpuFrameTime > 0.0)
                        return (float)(gpuFrameTime * 0.001);
                }
                return -1.0f;
            }
        }

        public void Measure()
        {
            UnityEngine.FrameTimingManager.CaptureFrameTimings();
        }
    }
}
