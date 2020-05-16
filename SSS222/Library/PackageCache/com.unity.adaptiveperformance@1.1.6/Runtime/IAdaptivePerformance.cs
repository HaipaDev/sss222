using System;

namespace UnityEngine.AdaptivePerformance
{
    /// <summary>
    /// Constants used by Adaptive Performance.
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// The minimum temperature level.
        /// See <see cref="ThermalMetrics.TemperatureLevel"/>.
        /// </summary>
        /// <value>0.0</value>
        public const float MinTemperatureLevel = 0.0f;

        /// <summary>
        /// The maximum temperature level.
        /// See <see cref="ThermalMetrics.TemperatureLevel"/>.
        /// </summary>
        /// <value>1.0</value>
        public const float MaxTemperatureLevel = 1.0f;

        /// <summary>
        /// The minimum CPU level.
        /// Used by <see cref="IDevicePerformanceControl.CpuLevel"/> and <see cref="PerformanceMetrics.CurrentCpuLevel"/>.
        /// </summary>
        /// <value>0</value>
        public const int MinCpuPerformanceLevel = 0;

        /// <summary>
        /// The minimum GPU level.
        /// Used by <see cref="IDevicePerformanceControl.GpuLevel"/> and <see cref="PerformanceMetrics.CurrentGpuLevel"/>.
        /// </summary>
        /// <value>0</value>
        public const int MinGpuPerformanceLevel = 0;

        /// <summary>
        /// UnknownPerformanceLevel is the value of <see cref="IDevicePerformanceControl.GpuLevel"/>, <see cref="PerformanceMetrics.CurrentGpuLevel"/>,
        /// <see cref="IDevicePerformanceControl.CpuLevel"/>, and <see cref="PerformanceMetrics.CurrentCpuLevel"/> if the current performance level is unknown.
        /// This may happen when AdaptivePerformance is not supported or when the device is in throttling state (see <see cref="WarningLevel.Throttling"/>). 
        /// </summary>
        /// <value>-1</value>
        public const int UnknownPerformanceLevel = -1;

        /// <summary>
        /// The number of past frames that are considered to calculate average frame times.
        /// </summary>
        /// <value>100</value>
        public const int DefaultAverageFrameCount = 100;
    }

    /// <summary>
    /// The main interface to access Adaptive Performance.
    /// None of the properties in this interface change after startup.
    /// This means the references returned by the properties may be cached by the user.
    /// </summary>
    public interface IAdaptivePerformance
    {
        /// <summary>
        /// Returns `true` if Adaptive Performance was initialized successfully, `false` otherwise.
        /// This means that Adaptive Performance is enabled in StartupSettings and the application runs on a device that supports Adaptive Performance.
        /// </summary>
        /// <value>`true` when Adaptive Performance is available, `false` otherwise</value>
        bool Active { get; }

        /// <summary>
        /// Access thermal status information of the device.
        /// </summary>
        /// <value>Interface to access thermal status information of the device</value>
        IThermalStatus ThermalStatus { get; }

        /// <summary>
        /// Access performance status information of the device and your application.
        /// </summary>
        /// <value>Interface to access performance status information of the device and your application</value>
        IPerformanceStatus PerformanceStatus { get; }

        /// <summary>
        /// Control CPU and GPU performance of the device.
        /// </summary>
        /// <value>Interface to control CPU and GPU performance levels of the device</value>
        IDevicePerformanceControl DevicePerformanceControl { get; }

        /// <summary>
        /// Access to development (logging) settings.
        /// </summary>
        /// <value>Interface to control CPU and GPU performance levels of the device</value>
        IDevelopmentSettings DevelopmentSettings { get; }
    }

    /// <summary>
    /// Global access to the default AdaptivePerformance interface.
    /// </summary>
    public static class Holder
    {
        /// <summary>
        /// Global access to the default AdaptivePerformance interface to access the main manager object.
        /// </summary>
        static public IAdaptivePerformance Instance { get; internal set; }
    }
}
