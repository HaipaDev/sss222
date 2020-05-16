namespace UnityEngine.AdaptivePerformance
{
    /// <summary>
    /// Changes to the startup settings are only respected when those are made before Adaptive Performance starts, for instance, from a method with the attribute [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]).
    /// </summary>
    public static class StartupSettings
    {
        /// <summary>
        /// Default Settings are applied. 
        /// </summary>
        static StartupSettings()
        {
            Logging = false;
            StatsLoggingFrequencyInFrames = 50;
            Enable = true;
            PreferredSubsystem = null;
            AutomaticPerformanceControl = true;
        }

        /// <summary>
        ///  Control debug logging.
        ///  This setting only affects development builds. All logging is disabled in release builds.
        ///  This setting can also be controlled after startup using <see cref="IDevelopmentSettings.Logging"/>.
        ///  Logging is disabled by default.
        /// </summary>
        /// <value>`true` to enable debug logging, `false` to disable it (default: `false`)</value>
        static public bool Logging { get; set; }

        /// <summary>
        /// Adjust the frequency in frames at which the application logs frame statistics to the console.
        /// This is only relevant when logging is enabled. See <see cref="Logging"/>.
        /// This setting can also be controlled after startup using <see cref="IDevelopmentSettings.StatsLoggingFrequencyInFrames"/>.
        /// </summary>
        /// <value>Logging frequency in frames (default: 50)</value>
        static public int StatsLoggingFrequencyInFrames { get; set; }

        /// <summary>
        /// Enable Adaptive Performance.
        /// </summary>
        /// <value>`true` to enable debug Adaptive Performance, `false` to disable it (default: `true`)</value>
        static public bool Enable { get; set; }

        /// <summary>
        /// The Initial value of <see cref="IDevicePerformanceControl.AutomaticPerformanceControl"/>.
        /// </summary>
        static public bool AutomaticPerformanceControl { get; set; }

        /// <summary>
        /// You can use this property to override the automatic selection of an Adaptive Performance subsystem.
        /// You should use this primarily for testing.
        /// </summary>
        static public Provider.AdaptivePerformanceSubsystem PreferredSubsystem { get; set; }
    }
}
