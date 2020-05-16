using System;

namespace UnityEngine.AdaptivePerformance.Provider
{

    /// <summary>
    /// Feature flags
    /// See <see cref="PerformanceDataRecord.ChangeFlags"/> and <seealso cref="AdaptivePerformanceSubsystem.Capabilities"/>.
    /// </summary>
    [Flags]
    public enum Feature
    {
        /// <summary>
        /// No features
        /// </summary>
        None = 0,
        /// <summary>
        /// See <see cref="PerformanceDataRecord.WarningLevel"/>
        /// </summary>
        WarningLevel = 0x1,
        /// <summary>
        /// See <see cref="PerformanceDataRecord.TemperatureLevel"/>
        /// </summary>
        TemperatureLevel = 0x2,
        /// <summary>
        /// See <see cref="PerformanceDataRecord.TemperatureTrend"/>
        /// </summary>
        TemperatureTrend = 0x4,
        /// <summary>
        /// See <see cref="PerformanceDataRecord.CpuPerformanceLevel"/> and <seealso cref="IDevicePerformanceLevelControl.SetPerformanceLevel(int, int)"/>
        /// </summary>
        CpuPerformanceLevel = 0x8,
        /// <summary>
        /// See <see cref="PerformanceDataRecord.GpuPerformanceLevel"/> and <seealso cref="IDevicePerformanceLevelControl.SetPerformanceLevel(int, int)"/>
        /// </summary>
        GpuPerformanceLevel = 0x10,
        /// <summary>
        /// See <see cref="PerformanceDataRecord.PerformanceLevelControl"/> and <seealso cref="AdaptivePerformanceSubsystem.PerformanceLevelControl"/>
        /// </summary>
        PerformanceLevelControl = 0x20,
        /// <summary>
        /// See <see cref="PerformanceDataRecord.GpuFrameTime"/>
        /// </summary>
        GpuFrameTime = 0x40,
        /// <summary>
        /// See <see cref="PerformanceDataRecord.CpuFrameTime"/>
        /// </summary>
        CpuFrameTime = 0x80,
        /// <summary>
        /// See <see cref="PerformanceDataRecord.OverallFrameTime"/>
        /// </summary>
        OverallFrameTime = 0x100
    }

    /// <summary>
    /// The performance data record stores all information about the thermal and performance status and uses it to deliver it from the provider subsystem to Adaptive Performance for further processing. 
    /// </summary>
    public struct PerformanceDataRecord
    {
        /// <summary>
        /// A bitset of features which indicate if their value changed in the last frame or at startup.
        /// Unsupported features will never change. 
        /// Fields not changing always have valid data as long as its capability is supported.
        /// </summary>
        /// <value>Bitset</value>
        public Feature ChangeFlags { get; set; }

        /// <summary>
        /// The current normalized temperature level in the range of [0.0, 1.0] or -1.0 when not supported or available right now.
        /// A level of 1.0 means that the device is thermal throttling.
        /// The temperature level has changed when the <see cref="Feature.TemperatureLevel"/> bit is set in <see cref="ChangeFlags"/>.
        /// </summary>
        /// <value>Temperature level in the range of [0.0, 1.0] or -1.0</value>
        public float TemperatureLevel { get; set; }

        /// <summary>
        /// The current temperature trend in the range of [-1.0, 1.0] that is a metric of temperature change over time.
        /// The temperature trend is constant at 0.0 in case the feature is not supported.
        /// The temperature trend has changed when <see cref="Feature.TemperatureTrend"/> bit is set in <see cref="ChangeFlags"/>.
        /// </summary>
        /// <value>Temperature trend in the range of [-1.0, 1.0]</value>
        public float TemperatureTrend { get; set; }

        /// <summary>
        /// The current warning level as documented in <see cref="WarningLevel"/>
        /// The warning level has changed when <see cref="Feature.WarningLevel"/> bit is set in <see cref="ChangeFlags"/>.
        /// </summary>
        /// <value>The current warning level</value>
        public WarningLevel WarningLevel { get; set; }

        /// <summary>
        /// The currently active CPU performance level. This is typically the value previously set with <see cref="IDevicePerformanceLevelControl.SetPerformanceLevel"/> once the levels are successfully applied.
        /// Adaptive performance may also change this level on its own. This typically happens when the device is thermal throttling or when <see cref="IDevicePerformanceLevelControl.SetPerformanceLevel"/> failed.
        /// CPU performance level has a value in the range of [<see cref="Constants.MinCpuPerformanceLevel"/>, <see cref="IDevicePerformanceLevelControl.MaxCpuPerformanceLevel"/>] or <seealso cref="Constants.UnknownPerformanceLevel"/>.
        /// A value of <see cref="Constants.UnknownPerformanceLevel"/> means that Adaptive Performance took control of performance levels.
        /// CPU performance level has changed when <see cref="Feature.CpuPerformanceLevel"/> bit is set in <see cref="ChangeFlags"/>.
        /// </summary>
        /// <value></value>
        public int CpuPerformanceLevel { get; set; }

        /// <summary>
        /// The currently active GPU performance level. This is typically the value previously set with <see cref="IDevicePerformanceLevelControl.SetPerformanceLevel"/> once the levels are successfully applied.
        /// Adaptive performance may also change this level on its own. This typically happens when the device is thermal throttling or when <see cref="IDevicePerformanceLevelControl.SetPerformanceLevel"/> failed.
        /// GPU performance level has a value in the range of [<see cref="Constants.MinCpuPerformanceLevel"/>, <see cref="IDevicePerformanceLevelControl.MaxGpuPerformanceLevel"/>] or <seealso cref="Constants.UnknownPerformanceLevel"/>.
        /// A value of <see cref="Constants.UnknownPerformanceLevel"/> means that Adaptive Performance took control of performance levels.
        /// GPU performance level has changed when <see cref="Feature.GpuPerformanceLevel"/> bit is set in <see cref="ChangeFlags"/>.
        /// </summary>
        public int GpuPerformanceLevel { get; set; }

        /// <summary>
        /// `true` if performance levels may currently be controlled manually and aren't controlled by Adaptive Performance or the operating system. 
        /// Has changed when <see cref="Feature.PerformanceLevelControl"/> bit is set in <see cref="ChangeFlags"/>.
        /// </summary>
        public bool PerformanceLevelControlAvailable { get; set; }

        /// <summary>
        /// The time in seconds spent by the CPU for rendering the last complete frame.
        /// Has changed when <see cref="Feature.CpuFrameTime"/> bit is set in <see cref="ChangeFlags"/>.
        /// </summary>
        public float CpuFrameTime { get; set; }

        /// <summary>
        /// The time in seconds spent by the GPU for rendering the last complete frame.
        /// Has changed when <see cref="Feature.GpuFrameTime"/> bit is set in <see cref="ChangeFlags"/>.
        /// </summary>
        public float GpuFrameTime { get; set; }

        /// <summary>
        /// The total time in seconds spent for the frame.
        /// Has changed when <see cref="Feature.OverallFrameTime"/> bit is set in <see cref="ChangeFlags"/>.
        /// </summary>
        public float OverallFrameTime { get; set; }
    }

    /// <summary>
    /// The interface describes how the Adaptive Performance provider lifecycle behaves. 
    /// </summary>
    public interface IApplicationLifecycle
    {
        /// <summary>
        /// Called before an application paused.
        /// To be called from `MonoBehaviour.OnApplicationPause`.
        /// </summary>
        void ApplicationPause();

        /// <summary>
        /// Called after an application resumes.
        /// To be called from `MonoBehaviour.OnApplicationPause`.
        /// </summary>
        void ApplicationResume();
    }

    /// <summary>
    /// The device performance level control lets you change CPU and GPU levels and informs you about the current levels.
    /// </summary>
    public interface IDevicePerformanceLevelControl
    {
        /// <summary>
        /// Maximum supported CPU performance level, it should not change after startup.
        /// <see cref="Constants.UnknownPerformanceLevel"/> in case performance levels are not supported.
        /// Value in the range of [<see cref="Constants.MinCpuPerformanceLevel"/>, 10].
        /// </summary>
        /// <value>Value in the range of [<see cref="Constants.MinCpuPerformanceLevel"/>, 10]</value>
        int MaxCpuPerformanceLevel { get; }

        /// <summary>
        /// Maximum supported GPU performance level, it should not change after startup.
        /// <see cref="Constants.UnknownPerformanceLevel"/> in case performance levels are not supported.
        /// Value in the range of [<see cref="Constants.MinGpuPerformanceLevel"/>, 10].
        /// </summary>
        /// <value>Value in the range of [<see cref="Constants.MinGpuPerformanceLevel"/>, 10]</value>
        int MaxGpuPerformanceLevel { get; }

        /// <summary>
        /// Request a performance level change.
        /// If <see cref="Constants.UnknownPerformanceLevel"/> is passed the subsystem picks the level to be used.
        /// </summary>
        /// <param name="cpu">
        /// The new performance level. May be <see cref="Constants.UnknownPerformanceLevel"/> or range of [<see cref="Constants.MinCpuPerformanceLevel"/>, <see cref="IDevicePerformanceLevelControl.MaxCpuPerformanceLevel"/>].
        /// If <see cref="Feature.CpuPerformanceLevel"/> is not supported (see <see cref="AdaptivePerformanceSubsystem.Capabilities"/>) this parameter is ignored.
        /// </param>
        /// <param name="gpu">
        /// The new performance level. May be <see cref="Constants.UnknownPerformanceLevel"/> or range of [<see cref="Constants.MinCpuPerformanceLevel"/>, <see cref="IDevicePerformanceLevelControl.MaxGpuPerformanceLevel"/>].
        /// If <see cref="Feature.GpuPerformanceLevel"/> is not supported (see <see cref="AdaptivePerformanceSubsystem.Capabilities"/>) this parameter is ignored.
        /// </param>
        /// <returns>`true` on success. When this fails, it means that the system took control of the active performance levels.</returns>
        bool SetPerformanceLevel(int cpu, int gpu);
    }

    /// <summary>
    /// Use the Adaptive Performance Subsystem class to create your custom provider subsystem to deliver data from your provider to Adaptive Performance.
    /// </summary>
    public abstract class AdaptivePerformanceSubsystem : AdaptivePerformanceSubsystemBase
    {
        /// <summary>
        /// Main constructor, not used in the subsystem specifically.
        /// </summary>
        protected AdaptivePerformanceSubsystem()
        {
        }

        /// <summary>
        /// Bitset of supported features.
        /// Does not change after startup.
        /// </summary>
        /// <value>Bitset</value>
        public Feature Capabilities { get; protected set; }

        /// <summary>
        /// To be called once per frame.
        /// The returned data structure's fields are populated with the latest available data, according to the supported <see cref="Capabilities"/>.
        /// </summary>
        /// <returns>Data structure with the most recent performance data.</returns>
        public abstract PerformanceDataRecord Update();

        /// <summary>
        /// Application life-cycle events to be consumed by subsystem.
        /// May be `null` in case the subsystem does not need special handling on life-cycle events.
        /// The returned reference does not change after startup.
        /// </summary>
        /// <value>Application life-cycle object</value>
        public abstract IApplicationLifecycle ApplicationLifecycle { get; }

        /// <summary>
        /// Control CPU/GPU performance levels of the device.
        /// May be null in case the subsystem does not support controlling CPU/GPU performance levels.
        /// `null` when <see cref="Feature.PerformanceLevelControl"/> bit is not set in <see cref="Capabilities"/>.
        /// The returned reference does not change after startup.
        /// </summary>
        /// <value>Performance level control object</value>
        public abstract IDevicePerformanceLevelControl PerformanceLevelControl { get; }

        /// <summary>
        /// Returns the version of the subsystem implementation.
        /// Can be used together with SubsystemDescriptor to identify a subsystem.
        /// </summary>
        /// <value>Version number</value>
        public abstract Version Version { get; }

        /// <summary>
        /// Generates a human readable string of subsystem internal stats.
        /// Optional and only used for development.
        /// </summary>
        /// <value>String with subsystem specific statistics</value>
        public virtual string Stats { get { return "";  } }
    }

#if UNITY_2019_2_OR_NEWER
    /// <summary>
    /// This is the base class for <see cref="AdaptivePerformanceSubsystem"/> and acts as a stub for backwards compability. 
    /// </summary>
    public abstract class AdaptivePerformanceSubsystemBase : UnityEngine.Subsystem<AdaptivePerformanceSubsystemDescriptor>
    {
        /// <summary>
        /// Returns if the provider subsystem is currently running. 
        /// </summary>
        override public bool running { get { return initialized; } }
        /// <summary>
        /// Returns if the provider subsystem was initialized successfully. 
        /// </summary>
        public bool initialized { get; protected set; } 
    }

#elif UNITY_2018_3_OR_NEWER
    /// <summary>
    /// This is the base class for <see cref="AdaptivePerformanceSubsystem"/> and acts as a stub for backwards compability. 
    /// </summary>
    public abstract class AdaptivePerformanceSubsystemBase : UnityEngine.Experimental.Subsystem<AdaptivePerformanceSubsystemDescriptor>
    {
        /// <summary>
        /// Returns if the provider subsystem was initialized successfully. 
        /// </summary>
        public bool initialized { get; protected set; } 
    }

#else
    /// <summary>
    /// This is the base class for <see cref="AdaptivePerformanceSubsystem"/> and acts as a stub for backwards compability. 
    /// </summary>
    public abstract class AdaptivePerformanceSubsystemBase
    {
        /// <summary>
        /// Called when the provider subsystem starts. 
        /// </summary>
        public abstract void Start();
        /// <summary>
        /// Called when the provider subsystem stops. 
        /// </summary>
        public abstract void Stop();
        /// <summary>
        /// Called when the provider subsystem gets destroyed. 
        /// </summary>
        public abstract void Destroy();
        /// <summary>
        /// Returns if the provider subsystem was initialized successfully. 
        /// </summary>
        public bool initialized { get; protected set; } 
    }

#endif

}
