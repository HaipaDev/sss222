using System;

namespace UnityEngine.AdaptivePerformance
{
    /// <summary>
    /// Performance bottleneck change event arguments used in the <see cref="PerformanceBottleneckChangeHandler"/>. 
    /// </summary>
    public struct PerformanceBottleneckChangeEventArgs
    {
        /// <summary>
        /// The performance bottleneck reported in the event. 
        /// </summary>
        public PerformanceBottleneck PerformanceBottleneck { get; set; }
    }

    /// <summary>
    /// You can subscribe to the bottleneck event delegate which sends the <see cref="PerformanceBottleneckChangeEventArgs"/> when the bottleneck changes. 
    /// </summary>
    /// <param name="bottleneckEventArgs">The <see cref="PerformanceBottleneckChangeEventArgs"/> describing the performance bottleneck state.</param>
    public delegate void PerformanceBottleneckChangeHandler(PerformanceBottleneckChangeEventArgs bottleneckEventArgs);

    /// <summary>
    /// Performance level change event arguments used in the <see cref="PerformanceLevelChangeHandler"/>. 
    /// </summary>
    public struct PerformanceLevelChangeEventArgs
    {
        /// <summary>
        /// The new CPU level.
        /// </summary>
        public int CpuLevel { get; set; }

        /// <summary>
        /// The difference in CPU levels
        /// 0 in case the previous or new level equals <see cref="Constants.UnknownPerformanceLevel"/>.
        /// </summary>
        public int CpuLevelDelta { get; set; }

        /// <summary>
        /// The new GPU level.
        /// </summary>
        public int GpuLevel { get; set; }

        /// <summary>
        /// The difference in GPU levels.
        /// 0 in case either the previous or the new level equals <see cref="Constants.UnknownPerformanceLevel"/>.
        /// </summary>
        public int GpuLevelDelta { get; set; }

        /// <summary>
        /// The current PerformanceControlMode. See <see cref="IDevicePerformanceControl.PerformanceControlMode"/>.
        /// </summary>
        public PerformanceControlMode PerformanceControlMode { get; set; }

        /// <summary>
        /// `true` if the change was caused by manual adjustments to <see cref="IDevicePerformanceControl.CpuLevel"/> or <see cref="IDevicePerformanceControl.GpuLevel"/> during automatic mode.
        /// </summary>
        public bool ManualOverride { get; set; }
    }

    /// <summary>
    /// You can subscribe to the performance level event delegate which sends the <see cref="PerformanceLevelChangeEventArgs"/> when the performance level changes.
    /// </summary>
    /// <param name="levelChangeEventArgs">The performance level change event is sent to the delegate via the level change event argument.</param>
    public delegate void PerformanceLevelChangeHandler(PerformanceLevelChangeEventArgs levelChangeEventArgs);

    /// <summary>
    /// You can use the performance status interface to obtain performance metrics, frame timing, and subscribe to bottleneck and performance event changes. 
    /// </summary>
    public interface IPerformanceStatus
    {
        /// <summary>
        /// Allows to query the latest performance metrics.
        /// </summary>
        PerformanceMetrics PerformanceMetrics { get; }

        /// <summary>
        /// Allows to query the latest frame timing measurements.
        /// </summary>
        FrameTiming FrameTiming { get; }

        /// <summary>
        /// Subscribe to performance events and get updates when the bottleneck changes.
        /// </summary>
        event PerformanceBottleneckChangeHandler PerformanceBottleneckChangeEvent;

        /// <summary>
        /// Subscribe to events and get updates when the the current CPU/GPU level changes.
        /// </summary>
        event PerformanceLevelChangeHandler PerformanceLevelChangeEvent;
    }

    /// <summary>
    /// PerformanceMetrics store the current bottleneck, CPU, and GPU levels 
    /// </summary>
    public struct PerformanceMetrics
    {
        /// <summary>
        /// Current CPU performance level.
        /// This value updates once per frame when changes are applied to <see cref="IDevicePerformanceControl.CpuLevel"/>.
        /// Value in the range [<see cref="Constants.MinCpuPerformanceLevel"/>, <see cref="IDevicePerformanceControl.MaxCpuPerformanceLevel"/>] or <see cref="Constants.UnknownPerformanceLevel"/>.
        /// </summary>
        /// <value>Current CPU performance level</value>
        public int CurrentCpuLevel { get; set; }

        /// <summary>
        /// Current GPU performance level.
        /// This value updates once per frame when changes are applied to <see cref="IDevicePerformanceControl.GpuLevel"/>.
        /// Value in the range [<see cref="Constants.MinGpuPerformanceLevel"/>, <see cref="IDevicePerformanceControl.MaxGpuPerformanceLevel"/>] or <see cref="Constants.UnknownPerformanceLevel"/>.
        /// </summary>
        /// <value>Current GPU performance level</value>
        public int CurrentGpuLevel { get; set; }

        /// <summary>
        /// Current performance bottleneck which describes if the program is CPU, GPU or `Application.targetFrameRate` bound.
        /// </summary>
        public PerformanceBottleneck PerformanceBottleneck { get; set; }
    }

    /// <summary>
    /// FrameTiming stores timing information about CPU, GPU, and the overall frame time. 
    /// </summary>
    public struct FrameTiming
    {
        /// <summary>
        /// The overall frame time (in seconds).
        /// Returns `-1.0f` if no timing information is available.
        /// This happens for example in the first frame or directly after resume.
        /// </summary>
        /// <value>Frame time in seconds</value>
        public float CurrentFrameTime { get; set; }

        /// <summary>
        /// The overall frame time as an average over the past 100 frames (in seconds).
        /// Returns -1.0f if no timing information is available.
        /// This happens for example in the first frame or directly after resume.
        /// </summary>
        /// <value>Frame time in seconds</value>
        public float AverageFrameTime { get; set; }

        /// <summary>
        /// Returns the GPU time of the last completely rendered frame (in seconds).
        /// Returns `-1.0f` if no timing information is available.
        /// The GPU time only includes the time the GPU spent on rendering a frame.
        /// This happens for example in the first frame or directly after resume.
        /// </summary>
        /// <value>Frame time in seconds</value>
        public float CurrentGpuFrameTime { get; set; }

        /// <summary>
        /// Returns the overall frame time as an average over the past 100 frames (in seconds).
        /// Returns `-1.0f` if no timing information is available.
        /// The GPU time only includes the time the GPU spent on rendering a frame.
        /// This happens for example in the first frame or directly after resume.
        /// </summary>
        /// <value>Frame time value in seconds</value>
        public float AverageGpuFrameTime { get; set; }

        /// <summary>
        /// Returns the main thread CPU time of the last frame (in seconds).
        /// The CPU time includes only time the CPU spent executing Unity's main and/or render threads.
        /// Returns `-1.0f` if no timing information is available.
        /// This happens for example in the first frame or directly after resume.
        /// </summary>
        /// <value>Frame time value in seconds</value>
        public float CurrentCpuFrameTime { get; set; }

        /// <summary>
        /// Returns the main thread CPU time as an average over the past 100 frames (in seconds).
        /// Returns `-1.0f` if this is not available.
        /// The CPU time includes only the time the CPU spent executing Unity's main and/or render threads.
        /// This happens for example in the first frame or directly after resume.
        /// </summary>
        /// <value>Frame time in seconds</value>
        public float AverageCpuFrameTime { get; set; }
    }

    /// <summary>
    /// The performance bottleneck enum is used to describe what the system is currently limited by. 
    /// </summary>
    public enum PerformanceBottleneck
    {
        /// <summary>
        /// Framerate bottleneck is unknown.
        /// </summary>
        Unknown,

        /// <summary>
        /// Framerate is limited by CPU processing.
        /// </summary>
        CPU,

        /// <summary>
        /// Framerate is limited by GPU processing.
        /// </summary>
        GPU,

        /// <summary>
        /// Framerate is limited by `Application.targetFrameRate`.
        /// In this case, the application should consider lowering performance requirements (see <see cref="IDevicePerformanceControl.SetPerformanceRequirements"/>).
        /// </summary>
        TargetFrameRate
    }
}
