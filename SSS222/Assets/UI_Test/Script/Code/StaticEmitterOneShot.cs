using UnityEngine;
using System.Collections;

namespace SpriteParticleEmitter
{
/// <summary>
/// Read the manual for a description of this component.
/// </summary>
public class StaticEmitterOneShot : StaticSpriteEmitter
{
    [Tooltip("Must the script disable referenced spriteRenderer component?")]
    //! Must the script disable referenced spriteRenderer component?
    public bool HideOriginalSpriteOnPlay = true;

    [Header("Silent Emission")]
    [Tooltip("Should start Silent Emitting as soon as has cache ended? (Refer to manual for further explanation)")]
    //! Should start Silent Emitting as soon as has cache ended? (Refer to manual for further explanation)
    public bool SilentEmitOnAwake = true;
    [Tooltip("Silent emission can be expensive. This defines the lower limit fps can go before continue silent emission on next frame (Refer to manual for further explanation)")]
    //! Silent emission can be expensive. This defines the lower limit fps can go before continue silent emission on next frame (Refer to manual for further explanation)
    public float WantedFPSDuringSilentEmission = 60;

    //! is system ready to play One Shot?
    protected bool SilentEmissionEnded;
    //! When a silent emission has been shot it must be reset to be able to shoot again.
    protected bool hasSilentEmissionAlreadyBeenShot;

    //! Event called when the emitter is ready to start
    public override event SimpleEvent OnAvailableToPlay;

    protected override void Awake()
    {
        base.Awake();
        SilentEmissionEnded = false;
        if (SilentEmitOnAwake)
            EmitSilently();
    }

    public override void CacheSprite(bool relativeToParent = false)
    {
        base.CacheSprite(SimulationSpace == ParticleSystemSimulationSpace.World);

        //set max particles as particles count to show
        #if UNITY_5_5_OR_NEWER
        if (mainModule.maxParticles < particlesCacheCount)
            mainModule.maxParticles = Mathf.CeilToInt(particlesCacheCount);
        #else
            if (particlesCacheCount > particlesSystem.maxParticles)
                particlesSystem.maxParticles = particlesCacheCount;
        #endif

        SilentEmissionEnded = false;
        hasSilentEmissionAlreadyBeenShot = false;
    }

    /// <summary>
    /// This will start emittin particles in a silent way, once all particles are emitted the system will be ready to play. 
    /// </summary>
    public void EmitSilently()
    {
        StartCoroutine(EmitParticlesSilently());
    }

    /// <summary>
    /// It works as coroutine, it will emit particles until it reachs the end or fps are lower than WantedFPSDuringSilentEmission variable.
    /// Calls OnAvailableToPlay event when everything is ok.
    /// </summary>
    private IEnumerator EmitParticlesSilently()
    {
        hasSilentEmissionAlreadyBeenShot = false;
        SilentEmissionEnded = false;
        isPlaying = false;

        float time = Time.realtimeSinceStartup;
        float LastTimeSaved = Time.realtimeSinceStartup;
        float waitTimeMax = 1000 / WantedFPSDuringSilentEmission;

        particlesSystem.Clear();
        particlesSystem.Pause();

        //faster access
        Color[] colorCache = particleInitColorCache;
        Vector3[] posCache = particleInitPositionsCache;
        float pStartSize = particleStartSize;
        int length = particlesCacheCount;
        ParticleSystem ps = particlesSystem;

        //basically what this for does is keep emitting particles until fps drop to a certain point, then continues emitting in the next frame. Does this until it ends emitting the wanted amount of particles
        for (int i = 0; i < length; i++)
        {
            if (i % 3 == 0) //Minimizing calls to Time.realtimeSinceStartup because it takes some time, isn't it ironic?
                LastTimeSaved = Time.realtimeSinceStartup;

            ParticleSystem.EmitParams em = new ParticleSystem.EmitParams();
            if (UsePixelSourceColor)
                em.startColor = colorCache[i];
            em.startSize = pStartSize;
            em.position = posCache[i];
            ps.Emit(em, 1);
            if (LastTimeSaved - time > waitTimeMax)
            {
                //Always pause particle system after emitting a particle.
                particlesSystem.Pause();
                time = LastTimeSaved;
                yield return null;
            }
        }
        particlesSystem.Pause();
        SilentEmissionEnded = true;

        if (OnAvailableToPlay != null)
            OnAvailableToPlay();
    }

    /// <summary>
    /// Define whether the Sprite component should be disabled on play or not.
    /// </summary>
    /// <param name="hideOriginalSprite">Must it disable referenced spriteRenderer on play? </param>
    public void SetHideSpriteOnPlay(bool hideOriginalSprite)
    {
        HideOriginalSpriteOnPlay = hideOriginalSprite;
    }

    /// <summary>
    /// Start playing the whole particle system.
    /// </summary>
    /// <returns>Has emitted particles or found an error</returns>
    private bool PlayOneShot()
    {
        if (HideOriginalSpriteOnPlay)
            spriteRenderer.enabled = false;

        if (!SilentEmissionEnded)
        {
            Debug.LogError("Silent particles haven't been emitted yet. Particles need to be emitted silently first for PlayOneShot to work (Please Refer to manual)");
            return false;
        }
        particlesSystem.Play();
        isPlaying = true;
        hasSilentEmissionAlreadyBeenShot = true;
        return true;
    }

    /// <summary>
    /// Play if available. If has already been shot it will resume. For reemit Reset() must be called.
    /// </summary>
    public override void Play()
    {
        if (!IsAvailableToPlay())
            return;

        if (!hasSilentEmissionAlreadyBeenShot)
        {
            if (!isPlaying)
                PlayOneShot();
        }
        else
        {
            if (!isPlaying)
            {
                particlesSystem.Play();
                isPlaying = true;
            }
        }
    }

    /// <summary>
    /// In this emitter Stop does the same as Pause()
    /// </summary>
    public override void Stop()
    {
        if (isPlaying)
            particlesSystem.Pause();
        isPlaying = false;
    }

    public override void Pause()
    {
        if (isPlaying)
            particlesSystem.Pause();
        isPlaying = false;
    }

    /// <summary>
    /// Reemit particles silently. It will Clear previously emitted particles.
    /// </summary>
    public void Reset()
    {
        EmitSilently();
    }

    /// <summary>
    /// Has ended everything it needs to do to play the system?
    /// </summary>
    public override bool IsAvailableToPlay()
    {
        return hasCachingEnded && !isPlaying && SilentEmissionEnded;
    }
}
}