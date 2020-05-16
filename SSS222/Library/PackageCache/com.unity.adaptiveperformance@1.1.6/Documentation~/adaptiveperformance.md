**_Adaptive Performance Guide_**

# About Adaptive Performance

Adaptive Performance allows you to get feedback about the thermal and power state of your mobile device, and react appropriately. For example, you can create applications that react to temperature trends and events on the device, to ensure constant frame rates over a longer period of time and prevent thermal throttling. 

# Installing Adaptive Performance

Unity automatically installs this package as a dependency if you install an Adaptive Performance subsystem. See the list of subsystems below, and refer to install instructions in each subsystem’s documentation. 

To use Adaptive Performance, you must have at least one subsystem installed. This version of Adaptive Performance supports the following subsystems:

* [Samsung (Android)](https://docs.unity3d.com/Packages/com.unity.adaptiveperformance.samsung.android@latest/index.html)

At least one subsystem is required to use Adaptive Performance.

# Using Adaptive Performance

When you install the Adaptive Performance package, Unity automatically creates a GameObject that implements `IAdaptivePerformance` in your Project at run time. To access the instance, use `UnityEngine.AdaptivePerformance.Holder.Instance`.

To check if your device supports Adaptive Performance, use the `Instance.Active` property. To get detailed information during runtime, enable debug logging with the `UnityEngine.AdaptivePerformance.StartupSettings.Logging` flag:

```
static class AdaptivePerformanceConfig
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Setup()
	{
		UnityEngine.AdaptivePerformance.StartupSettings.Logging = true;
    }
}
```

Unity enables AdaptivePerformance by default once you install the package and if it finds a suitable subsystem. To disable Adaptive Performance, use the `UnityEngine.AdaptivePerformance.StartupSettings.Logging` flag:

```
static class AdaptivePerformanceConfig
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Setup()
	{
		UnityEngine.AdaptivePerformance.StartupSettings.Enable = false;
    }
}
```

For a description of the detailed startup behavior of a subsystem please read the subsystem documentation.   

## Performance Status

Adaptive Performance tracks several performance metrics and updates them every frame. To access these metrics, use the `Instance.PerformanceStatus`property.

### Frame timing

Adaptive Performance always tracks the average GPU, CPU, and overall frame times, and updates them every frame. You can access the latest timing data using the `PerformanceStatus.FrameTiming` property.

The overall frame time is the time difference between frames. You can use it to calculate the current framerate of the application.
The CPU time only includes the time the CPU is actually executing Unity's main thread and the render thread. It doesn’t include the times when Unity might be blocked by the operating system, or when Unity needs to wait for the GPU to catch up with rendering. 
The GPU time is the time the GPU is actively processing data to render a frame. It doesn’t include the time when the GPU has to wait for Unity to provide data to render.

### Performance bottleneck

Adaptive Performance uses the currently configured target frame rate (see `UnityEngine.Application.targetFrameRate` and `QualitySettings`) and the information that `FrameTiming`provides to calculate what is limiting the frame rate of the application. If the application isn’t performing at the desired target framerate, it might be bound by either CPU or GPU processing. You can subscribe with a delegate function to the `PerformanceStatus.PerformanceBottleneckChangeEvent` event to get a notification whenever the current performance bottleneck of the application changes.

You can use the information about the current performance bottleneck to make targeted adjustments to the game content at run time. For example, in a GPU-bound application, lowering the rendering resolution often improves the frame rate significantly, but the same change might not make a big difference for a CPU-bound application.

## Device thermal state feedback

The Adaptive Performance API gives you access to the current thermal warning level of the device (`Instance.ThermalStatus.ThermalMetrics.WarningLevel`) and a more detailed temperature level (`Instance.ThermalStatus.ThermalMetrics.TemperatureLevel`). The application can make modifications based on these values to avoid the operating system throttling it.
The following example shows the implementation of a Unity component that uses Adaptive Performance feedback to adjust the global LOD bias:

```
using UnityEngine;  
using UnityEngine.AdaptivePerformance;  
  
public class AdaptiveLOD : MonoBehaviour  
{  
    private IAdaptivePerformance ap = null;  
  
    void Start() {  
        ap = Holder.Instance;  
  		if (!ap.Active)  
            return;  
  
        QualitySettings.lodBias = 1.0f;  
        ap.ThermalStatus.ThermalEvent += OnThermalEvent;  
    }  
  
    void OnThermalEvent(ThermalMetrics ev) {  
        switch (ev.WarningLevel) {  
            case WarningLevel.NoWarning:  
                QualitySettings.lodBias = 1;  
                break;  
            case WarningLevel.ThrottlingImminent:
				if (ev.temperatureLevel > 0.8f)
					QualitySettings.lodBias = 0.75f;
				else
					QualitySettings.lodBias = 1.0f;
                break;  
            case WarningLevel.Throttling:  
                QualitySettings.lodBias = 0.5f;  
                break;  
        }  
    }  
} 
```

## Configuring CPU and GPU performance levels

The CPU and GPU consume the most power on a mobile device, especially when running a game. Typically, the operating system decides which clock speeds to use for the CPU and GPU. CPU cores and GPUs are less efficient when running at their maximum clock speed. When they run at high clock speeds, the mobile device overheats, and the operating system throttles CPU and GPU frequency to cool down the device.
By default, Adaptive Performance automatically configures CPU and GPU performance levels based on the current performance bottleneck.
Alternatively, you can switch to `Manual` mode; to do this, set  `Instance.DevicePerformanceControl.AutomaticPerformanceControl` to `false`. In `Manual` mode, you can use the `Instance.DevicePerformanceControl.CpuLevel` and `Instance.DevicePerformanceControl.GpuLevel` properties to optimize CPU and GPU performance. To check which mode you are currently in, use `Instance.DevicePerformanceControl.PerformanceControlMode`. 

The application can configure these properties based on the thermal feedback and the frame time data that the Adaptive Performance API provides. It also uses these questions about its current performance requirements:
- Did the application reach the target frame rate in the previous frames?
- Is the application in an in-game scene, a loading screen, or a menu?
- Are device temperatures rising?
- Is the device close to thermal throttling?
- Is the device GPU or CPU bound?

*Note:* Changing GPU and GPU levels only has an effect as long as the device is not in thermal throttling state (`Instance.WarningLevel` equals `PerformanceWarningLevel.Throttling`).
In some situations, the device might take control over the CPU and GPU levels. This changes the value of `Instance.DevicePerformanceControl.PerformanceControlMode`
to `PerformanceControlMode.System`.

The following example shows how to configure performance levels based on the current Scene type:

```
public void EnterMenu()
{   
    if (!ap.Active)  
        return;   
  
    var ctrl = ap.DevicePerformanceControl;
  
    // Set low CPU and GPU level in menu  
    ctrl.CpuLevel = 0;  
    ctrl.GpuLevel = 0;
    // Set low target FPS  
    Application.targetFrameRate = 15;  
}  
  
public void ExitMenu()
{   
    var ctrl = ap.DevicePerformanceControl;
    // Set higher CPU and GPU level when going back into the game  
    ctrl.cpuLevel = ctrl.MaxCpuPerformanceLevel;  
    ctrl.gpuLevel = ctrl.MaxGpuPerformanceLevel;  
} 
```

# Technical details
## Requirements

This version of Adaptive Performance is compatible with Unity Editor versions 2018 LTS and later (2019 LTS and later recommended).
To use Adaptive Performance, you must have at least one subsystem installed. See the [Installing Adaptive Performance](#installing-adaptive-performance) section in this documentation for more details.

## Document revision history
This section includes the revision history of the document. The revision history tracks creation, edits, and updates to the document. If you create or update a document, you must add a new row describing the revision. The Documentation Team also uses this table to track when a document is edited and its editing level.
 
|Date|Reason|
|---|---|
|December 13, 2019|Update of the section: Using Adaptive Performance.|
|July 04, 2019|Chief Technical Editor reviewed.|
|June 21, 2019|Technical writer reviewed.|
|June 17, 2019|Work in progress for 1.0 release.|
|March 14, 2019|Document created. Work in progress for initial release.|
