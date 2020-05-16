using System;
using UnityEngine;
using UnityEngine.Scripting;
using System.Collections.Generic;

#if UNITY_2018_3_OR_NEWER
    [assembly: AlwaysLinkAssembly]
#endif

namespace UnityEngine.AdaptivePerformance.Provider
{

#if UNITY_2018_3_OR_NEWER
    #if UNITY_2019_2_OR_NEWER
        using AdaptivePerformanceSubsystemDescriptorBase = UnityEngine.SubsystemDescriptor<AdaptivePerformanceSubsystem>;
    #else
        using AdaptivePerformanceSubsystemDescriptorBase = UnityEngine.Experimental.SubsystemDescriptor<AdaptivePerformanceSubsystem>;
        using SubsystemManager = UnityEngine.Experimental.SubsystemManager;
    #endif

    [Preserve]
    internal static class AdaptivePerformanceSubsystemRegistry
    {
        /// <summary>
        /// Only for internal use.
        /// </summary>
        /// <param name="cinfo"></param>
        /// <returns></returns>
        public static AdaptivePerformanceSubsystemDescriptor RegisterDescriptor(AdaptivePerformanceSubsystemDescriptor.Cinfo cinfo)
        {
            var desc = new AdaptivePerformanceSubsystemDescriptor(cinfo);
            if (SubsystemRegistration.CreateDescriptor(desc))
            {
                return desc;
            }
            else
            {
                var registeredDescriptors = GetRegisteredDescriptors();
                foreach (var d in registeredDescriptors)
                {
                    if (d.subsystemImplementationType == cinfo.subsystemImplementationType)
                        return d;
                }
            }
            return null;
        }

        /// <summary>
        /// Only for internal use.
        /// </summary>
        /// <returns></returns>
        public static List<AdaptivePerformanceSubsystemDescriptor> GetRegisteredDescriptors()
        {
            var perfDescriptors = new List<AdaptivePerformanceSubsystemDescriptor>();
            SubsystemManager.GetSubsystemDescriptors<AdaptivePerformanceSubsystemDescriptor>(perfDescriptors);
            return perfDescriptors;
        }
    }

#else
    /// <summary>
    /// Used as Subsystem base class for backwards comptibility. 
    /// </summary>
    [Preserve]
    public class AdaptivePerformanceSubsystemDescriptorBase
    {
        /// <summary>
        /// Creates a subsystem and registers it in Unity used for backwards comptibility. 
        /// </summary>
        /// <returns></returns>
        public AdaptivePerformanceSubsystem Create()
        {
            return Activator.CreateInstance(subsystemImplementationType) as AdaptivePerformanceSubsystem;
        }

        /// <summary>
        /// ID of the subsystem in human readable form. 
        /// </summary>
        /// <returns></returns>
        public string id { get; set; }

        /// <summary>
        /// Implementation type of the subsystem. 
        /// </summary>
        /// <returns></returns>
        public Type subsystemImplementationType { get; set; }
    }


    [Preserve]
    internal static class AdaptivePerformanceSubsystemRegistry
    {
        private static List<AdaptivePerformanceSubsystemDescriptor> SubsystemDescriptors = new List<AdaptivePerformanceSubsystemDescriptor>();

        public static AdaptivePerformanceSubsystemDescriptor RegisterDescriptor(AdaptivePerformanceSubsystemDescriptor.Cinfo cinfo)
        {
            foreach (var d in SubsystemDescriptors)
            {
                if (d.subsystemImplementationType == cinfo.subsystemImplementationType)
                    return d;
            }

            var desc = new AdaptivePerformanceSubsystemDescriptor(cinfo);
            SubsystemDescriptors.Add(desc);
            return desc;
        }

        public static List<AdaptivePerformanceSubsystemDescriptor> GetRegisteredDescriptors()
        {
            return SubsystemDescriptors;
        }
    }

#endif
    /// <summary>
    /// The Adaptive Performance Subsystem Descriptor is used for describing the subsystem so it can be picked up by the subsystem management system. 
    /// </summary>
    [Preserve]
    public sealed class AdaptivePerformanceSubsystemDescriptor : AdaptivePerformanceSubsystemDescriptorBase
    {
        /// <summary>
        /// Cinfo stores the ID and subsystem implementation type which is used to identify the subsystem during initialization of the subsystem. 
        /// </summary>
        public struct Cinfo
        {
            /// <summary>
            /// The ID stores the name of the subsystem used to identify it in the subsystem registry. 
            /// </summary>
            public string id { get; set; }
            /// <summary>
            /// The subsystem implementation type stores the the type used for initialization in the subsystem registry. 
            /// </summary>
            public Type subsystemImplementationType { get; set; }
        }

        /// <summary>
        /// Constructor to fill the subsystem descriptor with all information to register the subsystem successfully.
        /// </summary>
        /// <param name="cinfo">Pass in the information about the subsystem.</param>
        public AdaptivePerformanceSubsystemDescriptor(Cinfo cinfo)
        {
            id = cinfo.id;
            subsystemImplementationType = cinfo.subsystemImplementationType;
        }

        /// <summary>
        /// Register the subsystem with the subsystem registry and make it available to use during runtime.
        /// </summary>
        /// <param name="cinfo">Pass in the information about the subsystem.</param>
        /// <returns>Returns an active subsytem descriptor.</returns>
        public static AdaptivePerformanceSubsystemDescriptor RegisterDescriptor(Cinfo cinfo)
        {
            return AdaptivePerformanceSubsystemRegistry.RegisterDescriptor(cinfo);
        }
    }
}
