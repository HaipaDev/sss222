using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

using AdaptivePerformance = UnityEngine.AdaptivePerformance;

public static class AdaptivePerformanceTestSetup
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Initialize()
    {
        AdaptivePerformance.StartupSettings.Enable = true;
        AdaptivePerformance.StartupSettings.Logging = false;
        AdaptivePerformance.StartupSettings.PreferredSubsystem = AdaptivePerformance.Provider.TestAdaptivePerformanceSubsystem.Initialize();
        AdaptivePerformance.StartupSettings.AutomaticPerformanceControl = false;
    }
}

class AdaptivePerformanceTests
{

    [UnityTest]
    public IEnumerator Applies_Cpu_Level()
    {
        var subsystem = AdaptivePerformance.StartupSettings.PreferredSubsystem as AdaptivePerformance.Provider.TestAdaptivePerformanceSubsystem;
        var ap = AdaptivePerformance.Holder.Instance;

        subsystem.AcceptsPerformanceLevel = true;

        var level = ap.DevicePerformanceControl.MaxCpuPerformanceLevel;

        ap.DevicePerformanceControl.CpuLevel = level;

        yield return null;

        Assert.AreEqual(level, ap.DevicePerformanceControl.CpuLevel);
        Assert.AreEqual(level, ap.PerformanceStatus.PerformanceMetrics.CurrentCpuLevel);
    }

    [UnityTest]
    public IEnumerator Applies_Gpu_Level()
    {
        var subsystem = AdaptivePerformance.StartupSettings.PreferredSubsystem as AdaptivePerformance.Provider.TestAdaptivePerformanceSubsystem;
        var ap = AdaptivePerformance.Holder.Instance;

        subsystem.AcceptsPerformanceLevel = true;

        var level = ap.DevicePerformanceControl.MaxGpuPerformanceLevel;

        ap.DevicePerformanceControl.GpuLevel = level;

        yield return null;

        Assert.AreEqual(level, ap.DevicePerformanceControl.GpuLevel);
        Assert.AreEqual(level, ap.PerformanceStatus.PerformanceMetrics.CurrentGpuLevel);
    }

    [UnityTest]
    public IEnumerator Unknown_GpuLevel_In_Throttling_State()
    {
        var subsystem = AdaptivePerformance.StartupSettings.PreferredSubsystem as AdaptivePerformance.Provider.TestAdaptivePerformanceSubsystem;
        var ap = AdaptivePerformance.Holder.Instance;

        subsystem.AcceptsPerformanceLevel = false;

        ap.DevicePerformanceControl.GpuLevel = ap.DevicePerformanceControl.MaxCpuPerformanceLevel;

        yield return null;

        Assert.AreEqual(AdaptivePerformance.PerformanceControlMode.System, ap.DevicePerformanceControl.PerformanceControlMode);

        Assert.AreEqual(AdaptivePerformance.Constants.UnknownPerformanceLevel, ap.PerformanceStatus.PerformanceMetrics.CurrentGpuLevel);
    }

    [UnityTest]
    public IEnumerator Unknown_CpuLevel_In_Throttling_State()
    {
        var subsystem = AdaptivePerformance.StartupSettings.PreferredSubsystem as AdaptivePerformance.Provider.TestAdaptivePerformanceSubsystem;
        var ap = AdaptivePerformance.Holder.Instance;

        subsystem.AcceptsPerformanceLevel = false;
    
        ap.DevicePerformanceControl.CpuLevel = ap.DevicePerformanceControl.MaxCpuPerformanceLevel;

        yield return null;

        Assert.AreEqual(AdaptivePerformance.PerformanceControlMode.System, ap.DevicePerformanceControl.PerformanceControlMode);
        Assert.AreEqual(AdaptivePerformance.Constants.UnknownPerformanceLevel, ap.PerformanceStatus.PerformanceMetrics.CurrentCpuLevel);
    }

    [UnityTest]
    public IEnumerator Ignores_Invalid_Cpu_Level()
    {
        var subsystem = AdaptivePerformance.StartupSettings.PreferredSubsystem as AdaptivePerformance.Provider.TestAdaptivePerformanceSubsystem;
        var ap = AdaptivePerformance.Holder.Instance;

        subsystem.AcceptsPerformanceLevel = true;
        subsystem.WarningLevel = AdaptivePerformance.WarningLevel.NoWarning;

        ap.DevicePerformanceControl.CpuLevel = 100;

        yield return null;

        Assert.AreEqual(AdaptivePerformance.Constants.UnknownPerformanceLevel, ap.PerformanceStatus.PerformanceMetrics.CurrentCpuLevel);
    }

    [UnityTest]
    public IEnumerator Ignores_Invalid_Gpu_Level()
    {
        var subsystem = AdaptivePerformance.StartupSettings.PreferredSubsystem as AdaptivePerformance.Provider.TestAdaptivePerformanceSubsystem;
        var ap = AdaptivePerformance.Holder.Instance;

        subsystem.AcceptsPerformanceLevel = true;
        subsystem.WarningLevel = AdaptivePerformance.WarningLevel.NoWarning;

        ap.DevicePerformanceControl.GpuLevel = -2;

        yield return null;

        Assert.AreEqual(AdaptivePerformance.Constants.UnknownPerformanceLevel, ap.PerformanceStatus.PerformanceMetrics.CurrentGpuLevel);
    }

    [UnityTest]
    public IEnumerator TemperatureChangeEvent_Values_Are_Applied()
    {
        var subsystem = AdaptivePerformance.StartupSettings.PreferredSubsystem as AdaptivePerformance.Provider.TestAdaptivePerformanceSubsystem;
        var ap = AdaptivePerformance.Holder.Instance;

        subsystem.TemperatureLevel = 0.0f;
        subsystem.TemperatureTrend = 1.0f;

        yield return null;

        Assert.AreEqual(0.0f, ap.ThermalStatus.ThermalMetrics.TemperatureLevel);
        Assert.AreEqual(1.0f, ap.ThermalStatus.ThermalMetrics.TemperatureTrend);
    }

    [UnityTest]
    public IEnumerator WarningLevel_Is_Applied()
    {
        var subsystem = AdaptivePerformance.StartupSettings.PreferredSubsystem as AdaptivePerformance.Provider.TestAdaptivePerformanceSubsystem;
        var ap = AdaptivePerformance.Holder.Instance;

        subsystem.WarningLevel = AdaptivePerformance.WarningLevel.ThrottlingImminent;

        yield return null;

        Assert.AreEqual(AdaptivePerformance.WarningLevel.ThrottlingImminent, ap.ThermalStatus.ThermalMetrics.WarningLevel);

        subsystem.WarningLevel = AdaptivePerformance.WarningLevel.Throttling;

        yield return null;

        Assert.AreEqual(AdaptivePerformance.WarningLevel.Throttling, ap.ThermalStatus.ThermalMetrics.WarningLevel);

        subsystem.WarningLevel = AdaptivePerformance.WarningLevel.NoWarning;

        yield return null;

        Assert.AreEqual(AdaptivePerformance.WarningLevel.NoWarning, ap.ThermalStatus.ThermalMetrics.WarningLevel);
    }

    [UnityTest]
    public IEnumerator Provider_FrameTimes_Work()
    {
        var subsystem = AdaptivePerformance.StartupSettings.PreferredSubsystem as AdaptivePerformance.Provider.TestAdaptivePerformanceSubsystem;
        var ap = AdaptivePerformance.Holder.Instance;

        subsystem.NextGpuFrameTime = 0.033f;
        subsystem.NextCpuFrameTime = 0.015f;
        subsystem.NextOverallFrameTime = 0.042f;

        yield return null;

        var ft = ap.PerformanceStatus.FrameTiming;

        Assert.IsTrue(Mathf.Abs(ft.CurrentFrameTime - subsystem.NextOverallFrameTime) < 0.001f);
        Assert.IsTrue(Mathf.Abs(ft.CurrentCpuFrameTime - subsystem.NextCpuFrameTime) < 0.001f);
        Assert.IsTrue(Mathf.Abs(ft.CurrentGpuFrameTime - subsystem.NextGpuFrameTime) < 0.001f);

    }

    [UnityTest]
    public IEnumerator GpuBound_When_GpuTime_Is_High()
    {
        var subsystem = AdaptivePerformance.StartupSettings.PreferredSubsystem as AdaptivePerformance.Provider.TestAdaptivePerformanceSubsystem;
        var ap = AdaptivePerformance.Holder.Instance;

        for (int i = 0; i < AdaptivePerformance.Constants.DefaultAverageFrameCount; ++i)
        {
            subsystem.NextGpuFrameTime = 0.033f;
            subsystem.NextCpuFrameTime = 0.015f;
            subsystem.NextOverallFrameTime = 0.042f;
            yield return null;
        }

        Assert.AreEqual(AdaptivePerformance.PerformanceBottleneck.GPU, ap.PerformanceStatus.PerformanceMetrics.PerformanceBottleneck);
    }

    [UnityTest]
    public IEnumerator CpuBound_When_CpuTime_Is_High()
    {
        var subsystem = AdaptivePerformance.StartupSettings.PreferredSubsystem as AdaptivePerformance.Provider.TestAdaptivePerformanceSubsystem;
        var ap = AdaptivePerformance.Holder.Instance;

        for (int i = 0; i < AdaptivePerformance.Constants.DefaultAverageFrameCount; ++i)
        {
            subsystem.NextGpuFrameTime = 0.033f;
            subsystem.NextCpuFrameTime = 0.038f;
            subsystem.NextOverallFrameTime = 0.042f;
            yield return null;
        }

        Assert.AreEqual(AdaptivePerformance.PerformanceBottleneck.CPU, ap.PerformanceStatus.PerformanceMetrics.PerformanceBottleneck);
    }

    [UnityTest]
    public IEnumerator Unknown_Bottleneck_When_GpuTime_And_CpuTime_Are_Equal()
    {
        var subsystem = AdaptivePerformance.StartupSettings.PreferredSubsystem as AdaptivePerformance.Provider.TestAdaptivePerformanceSubsystem;
        var ap = AdaptivePerformance.Holder.Instance;

        for (int i = 0; i < AdaptivePerformance.Constants.DefaultAverageFrameCount; ++i)
        {
            subsystem.NextGpuFrameTime = 0.033f;
            subsystem.NextCpuFrameTime = subsystem.NextGpuFrameTime;
            subsystem.NextOverallFrameTime = 0.042f;
            yield return null;
        }

        Assert.AreEqual(AdaptivePerformance.PerformanceBottleneck.Unknown, ap.PerformanceStatus.PerformanceMetrics.PerformanceBottleneck);
    }

    [UnityTest]
    public IEnumerator Bottleneck_TargetFrameRate_Works()
    {
        var subsystem = AdaptivePerformance.StartupSettings.PreferredSubsystem as AdaptivePerformance.Provider.TestAdaptivePerformanceSubsystem;
        var ap = AdaptivePerformance.Holder.Instance;

        for (int i = 0; i < AdaptivePerformance.Constants.DefaultAverageFrameCount; ++i)
        {
            subsystem.NextGpuFrameTime = 0.010f;
            subsystem.NextCpuFrameTime = 0.010f;
            subsystem.NextOverallFrameTime = 0.010f;
            yield return null;
        }

        Assert.AreEqual(AdaptivePerformance.PerformanceBottleneck.TargetFrameRate, ap.PerformanceStatus.PerformanceMetrics.PerformanceBottleneck);
    }

    [UnityTest]
    public IEnumerator PerformanceBottleneckChangeEvent_Works()
    {
        var subsystem = AdaptivePerformance.StartupSettings.PreferredSubsystem as AdaptivePerformance.Provider.TestAdaptivePerformanceSubsystem;
        var ap = AdaptivePerformance.Holder.Instance;

        for (int i = 0; i < AdaptivePerformance.Constants.DefaultAverageFrameCount; ++i)
        {
            subsystem.NextGpuFrameTime = 0.010f;
            subsystem.NextCpuFrameTime = 0.010f;
            subsystem.NextOverallFrameTime = 0.010f;
            yield return null;
        }

        int eventCounter = 0;
        var bottleneck = AdaptivePerformance.PerformanceBottleneck.Unknown;

        AdaptivePerformance.PerformanceBottleneckChangeHandler eventHandler = delegate (AdaptivePerformance.PerformanceBottleneckChangeEventArgs args)
        {
            ++eventCounter;
            bottleneck = args.PerformanceBottleneck;
        };

        ap.PerformanceStatus.PerformanceBottleneckChangeEvent += eventHandler;

        for (int i = 0; i < AdaptivePerformance.Constants.DefaultAverageFrameCount; ++i)
        {
            subsystem.NextGpuFrameTime = 0.050f;
            subsystem.NextCpuFrameTime = 0.010f;
            subsystem.NextOverallFrameTime = 0.050f;
            yield return null;
        }

        Assert.AreEqual(AdaptivePerformance.PerformanceBottleneck.GPU, ap.PerformanceStatus.PerformanceMetrics.PerformanceBottleneck);
        Assert.AreEqual(AdaptivePerformance.PerformanceBottleneck.GPU, bottleneck);
        Assert.AreEqual(1, eventCounter);
    }

    [UnityTest]
    public IEnumerator PerformanceLevelChangeEvent_Works()
    {
        var subsystem = AdaptivePerformance.StartupSettings.PreferredSubsystem as AdaptivePerformance.Provider.TestAdaptivePerformanceSubsystem;
        var ap = AdaptivePerformance.Holder.Instance;

        var ctrl = ap.DevicePerformanceControl;
        ctrl.AutomaticPerformanceControl = false;
        var ps = ap.PerformanceStatus;

        ctrl.CpuLevel = 1;
        ctrl.GpuLevel = 2;

        yield return null;

        Assert.AreEqual(1, ps.PerformanceMetrics.CurrentCpuLevel);
        Assert.AreEqual(2, ps.PerformanceMetrics.CurrentGpuLevel);

        var eventArgs = new AdaptivePerformance.PerformanceLevelChangeEventArgs();
        AdaptivePerformance.PerformanceLevelChangeHandler eventHandler = delegate (AdaptivePerformance.PerformanceLevelChangeEventArgs args)
        {
            eventArgs = args;
        };
        ps.PerformanceLevelChangeEvent += eventHandler;

        ctrl.CpuLevel = 4;
        ctrl.GpuLevel = 0;

        yield return null;

        Assert.AreEqual(4, ps.PerformanceMetrics.CurrentCpuLevel);
        Assert.AreEqual(4, eventArgs.CpuLevel);
        Assert.AreEqual(0, ps.PerformanceMetrics.CurrentGpuLevel);
        Assert.AreEqual(0, eventArgs.GpuLevel);
        Assert.AreEqual(3, eventArgs.CpuLevelDelta);
        Assert.AreEqual(-2, eventArgs.GpuLevelDelta);
        Assert.AreEqual(false, eventArgs.ManualOverride);
        Assert.AreEqual(AdaptivePerformance.PerformanceControlMode.Manual, eventArgs.PerformanceControlMode);
    }

    [UnityTest]
    public IEnumerator ThermalEvent_Works()
    {
        var subsystem = AdaptivePerformance.StartupSettings.PreferredSubsystem as AdaptivePerformance.Provider.TestAdaptivePerformanceSubsystem;
        var ap = AdaptivePerformance.Holder.Instance;
        var thermals = ap.ThermalStatus;
       
        var metrics = new AdaptivePerformance.ThermalMetrics();
        AdaptivePerformance.ThermalEventHandler eventHandler = delegate (AdaptivePerformance.ThermalMetrics args)
        {
            metrics = args;
        };
        thermals.ThermalEvent += eventHandler;

        subsystem.TemperatureLevel = 0.3f;
        subsystem.TemperatureTrend = 0.5f;

        yield return null;

        Assert.AreEqual(0.3f, metrics.TemperatureLevel, 0.0001f);
        Assert.AreEqual(0.5f, metrics.TemperatureTrend, 0.0001f);
    }

    [UnityTest]
    public IEnumerator PerformanceLevels_Are_Reapplied_After_Timeout()
    {
        var subsystem = AdaptivePerformance.StartupSettings.PreferredSubsystem as AdaptivePerformance.Provider.TestAdaptivePerformanceSubsystem;
        var ap = AdaptivePerformance.Holder.Instance;

        subsystem.AcceptsPerformanceLevel = true;

        int gpuLevel = 0;
        int cpuLevel = 0;
        ap.DevicePerformanceControl.CpuLevel = gpuLevel;
        ap.DevicePerformanceControl.GpuLevel = cpuLevel;

        yield return null;

        // Samsung Subsystem would do this when "timeout" happens (setLevels changes levels back to default after 10min)
        subsystem.GpuPerformanceLevel = AdaptivePerformance.Constants.UnknownPerformanceLevel;
        subsystem.CpuPerformanceLevel = AdaptivePerformance.Constants.UnknownPerformanceLevel;

        // Set to some invalid level so that we can check that new levels are requested
        int invalidPerformanceLevel = -2;
        subsystem.LastRequestedCpuLevel = invalidPerformanceLevel;
        subsystem.LastRequestedGpuLevel = invalidPerformanceLevel;

        yield return null;

        // AdaptivePerformance is supposed to reapply the last settings
        Assert.AreEqual(cpuLevel, subsystem.LastRequestedCpuLevel);
        Assert.AreEqual(gpuLevel, subsystem.LastRequestedGpuLevel);
    }
}
