namespace UnityEngine.AdaptivePerformance
{
    internal class DevicePerformanceControlImpl : IDevicePerformanceControl
    {
        Provider.IDevicePerformanceLevelControl m_PerformanceLevelControl;
        public DevicePerformanceControlImpl(Provider.IDevicePerformanceLevelControl performanceLevelControl)
        {
            m_PerformanceLevelControl = performanceLevelControl;
            PerformanceControlMode = PerformanceControlMode.Automatic;
            CurrentCpuLevel = Constants.UnknownPerformanceLevel;
            CurrentGpuLevel = Constants.UnknownPerformanceLevel;
            CpuLevel = Constants.UnknownPerformanceLevel;
            GpuLevel = Constants.UnknownPerformanceLevel;
        }

        public bool Update(out PerformanceLevelChangeEventArgs changeArgs)
        {
            changeArgs = new PerformanceLevelChangeEventArgs();
            changeArgs.PerformanceControlMode = PerformanceControlMode;

            if (PerformanceControlMode == PerformanceControlMode.System)
            {
                bool changed = CurrentCpuLevel != Constants.UnknownPerformanceLevel || CurrentGpuLevel != Constants.UnknownPerformanceLevel;
                CurrentCpuLevel = Constants.UnknownPerformanceLevel;
                CurrentGpuLevel = Constants.UnknownPerformanceLevel;

                if (changed)
                {
                    changeArgs.CpuLevel = CurrentCpuLevel;
                    changeArgs.GpuLevel = CurrentGpuLevel;
                    changeArgs.CpuLevelDelta = 0;
                    changeArgs.GpuLevelDelta = 0;
                }
                return changed;
            }

            if (CpuLevel != Constants.UnknownPerformanceLevel || GpuLevel != Constants.UnknownPerformanceLevel)
            {
                if (CpuLevel != CurrentCpuLevel || GpuLevel != CurrentGpuLevel)
                {
                    if (m_PerformanceLevelControl.SetPerformanceLevel(CpuLevel, GpuLevel))
                    {
                        changeArgs.CpuLevelDelta = ComputeDelta(CurrentCpuLevel, CpuLevel);
                        changeArgs.GpuLevelDelta = ComputeDelta(CurrentGpuLevel, GpuLevel);
                        CurrentCpuLevel = CpuLevel;
                        CurrentGpuLevel = GpuLevel;
                    }
                    else
                    {
                        changeArgs.CpuLevelDelta = 0;
                        changeArgs.GpuLevelDelta = 0;
                        CurrentCpuLevel = Constants.UnknownPerformanceLevel;
                        CurrentGpuLevel = Constants.UnknownPerformanceLevel;
                    }

                    changeArgs.CpuLevel = CurrentCpuLevel;
                    changeArgs.GpuLevel = CurrentGpuLevel;

                    return true;
                }
            }

            return false;
        }

        private int ComputeDelta(int oldLevel, int newLevel)
        {
            if (oldLevel < 0 || newLevel < 0)
                return 0;

            return newLevel - oldLevel;
        }

        /// <summary>
        /// DevicePerformanceControlImpl does not implement AutomaticPerformanceControl
        /// </summary>
        public bool AutomaticPerformanceControl { get { return false; } set { } }

        public PerformanceControlMode PerformanceControlMode { get; set; }

        public int MaxCpuPerformanceLevel { get { return m_PerformanceLevelControl != null ? m_PerformanceLevelControl.MaxCpuPerformanceLevel : Constants.UnknownPerformanceLevel; } }

        public int MaxGpuPerformanceLevel { get { return m_PerformanceLevelControl != null ? m_PerformanceLevelControl.MaxGpuPerformanceLevel : Constants.UnknownPerformanceLevel; } }

        public int CpuLevel { get; set; }

        public int GpuLevel { get; set; }

        public int CurrentCpuLevel { get; set; }
        public int CurrentGpuLevel { get; set; }
    }
}
