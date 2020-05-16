using System;

namespace UnityEngine.AdaptivePerformance
{
    /// <summary>
    /// The device performance control interface handles all control elements related to the device performance. You can
	/// change the <see cref="IDevicePerformanceControl.AutomaticPerformanceControl"/> settings or retrieve information about the <see cref="CpuLevel"/> and <see cref="GpuLevel"/>. 
    /// </summary>
    public interface IDevicePerformanceControl
    {
        /// <summary>
        /// When set to `true` (default) <see cref="CpuLevel"/> and <see cref="GpuLevel"/> are set automatically by Adaptive Performance.
        /// </summary>
        /// <value>`true` when Adaptive Performance controls <see cref="CpuLevel"/> and <see cref="GpuLevel"/>, otherwise `false` (default: `true`)</value>
        bool AutomaticPerformanceControl { get; set; }

        /// <summary>
        /// The current PerformanceControlMode.
        /// PerformanceControlMode is affected by <see cref="AutomaticPerformanceControl"/>.
        /// </summary>
        /// <value>The current PerformanceControlMode</value>
        PerformanceControlMode PerformanceControlMode { get; }

        /// <summary>
        /// The maximum valid CPU performance level you use with <see cref="CpuLevel"/>.
        /// The minimum value returned is <see cref="Constants.MinCpuPerformanceLevel"/>.
        /// This value does not change after startup is complete.
        /// </summary>
        int MaxCpuPerformanceLevel { get; }

        /// <summary>
        /// The maximum valid GPU performance level you use with <see cref="GpuLevel"/>.
        /// The minimum value returned is <see cref="Constants.MinGpuPerformanceLevel"/>.
        /// This value does not change after startup is complete.
        /// </summary>
        int MaxGpuPerformanceLevel { get; }

        /// <summary>
        /// The requested CPU performance level.
        /// Higher levels typically allow CPU cores to run at higher clock speeds.
        /// The consequence is that thermal warnings and throttling may happen sooner when the device cannot sustain high clock speeds.
        /// Changes are applied once per frame.
        /// We recommended to set the CpuLevel as low as possible to save power.
        /// The valid value range is [<see cref="Constants.MinCpuPerformanceLevel"/>, <see cref="IDevicePerformanceControl.MaxCpuPerformanceLevel"/>].
        /// </summary>
        /// <value>The requested CPU performance level</value>
        int CpuLevel { get; set; }

        /// <summary>
        /// The requested GPU performance level.
        /// Higher levels typically allow the GPU to run at higher clock speeds.
        /// The consequence is that thermal warnings and throttling may happen sooner when the device cannot sustain high clock speeds.
        /// Changes are applied once per frame.
        /// We recommended to set the GpuLevel as low as possible to save power.
        /// The valid value range is [<see cref="Constants.MinGpuPerformanceLevel"/>, <see cref="IDevicePerformanceControl.MaxGpuPerformanceLevel"/>].
        /// </summary>
        /// <value>The requested GPU performance level</value>
        int GpuLevel { get; set; }
    }

    /// <summary>
	/// Enum used to describe the performance control mode used by Adaptive Performance. Can be read from <see cref="IDevicePerformanceControl.PerformanceControlMode"/>.
	/// </summary>
    public enum PerformanceControlMode
    {
        /// <summary>
        /// Adaptive Performance controls performance levels automatically (default).
        /// This mode is enabled by setting <see cref="IDevicePerformanceControl.AutomaticPerformanceControl"/> to `true`.
        /// </summary>
        Automatic,

        /// <summary>
        /// You can control performance levels via <see cref="IDevicePerformanceControl.CpuLevel"/> and <see cref="IDevicePerformanceControl.GpuLevel"/>.
        /// This mode is enabled by setting <see cref="IDevicePerformanceControl.AutomaticPerformanceControl"/> to `false`.
        /// </summary>
        Manual,

        /// <summary>
        /// The operating system controls performance levels.
        /// This happens in case manual control is not supported or if the system is in a thermal throttling state at which it takes over control automatically.
        /// </summary>
        System
    }
}
