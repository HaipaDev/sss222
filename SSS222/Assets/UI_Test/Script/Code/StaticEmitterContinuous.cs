using UnityEngine;
using System.Collections;

namespace SpriteParticleEmitter
{
/// <summary>
/// Refer to manual for description.
/// </summary>
public class StaticEmitterContinuous : StaticSpriteEmitter
{
    [Header("Emission")]
    [Tooltip("Particles to emit per second")]
    //! Particles to emit per second
    public float EmissionRate = 1000;
    //! Save time to know how many particles to show per frame
    protected float ParticlesToEmitThisFrame;
    //! Will be called when the emitter is ready to play (after caching)
    public override event SimpleEvent OnAvailableToPlay;

    protected override void Update()
    {
        base.Update();
        if (isPlaying && hasCachingEnded)
        {
            Emit();
        }
    }

    /// <summary>
    /// Will cache sprite data needed to emit later.
    /// If a cache is already been done it will be overrided by the new cache. 
    /// Only use this if you have changed the sprite and the cache no longer represent current sprite.
    /// </summary>
    public override void CacheSprite(bool relativeToParent = false)
    {
        base.CacheSprite(false);
        if (OnAvailableToPlay != null)
            OnAvailableToPlay();
    }

    /// <summary>
    /// Emit particles based on EmissionRate.
    /// </summary>
    protected void Emit()
    {
        //safe check
        if (!hasCachingEnded)
            return;

        ParticlesToEmitThisFrame += EmissionRate * Time.deltaTime;

        //getting sprite source as gameobject for pos rot and scale
        Vector3 transformPos = spriteRenderer.gameObject.transform.position;
        Quaternion transformRot = spriteRenderer.gameObject.transform.rotation;
        Vector3 transformScale = spriteRenderer.gameObject.transform.lossyScale;
        ParticleSystemSimulationSpace currentSimulationSpace = SimulationSpace;

        int pCount = particlesCacheCount;
        float pStartSize = particleStartSize;
        int EmissionCount = (int)ParticlesToEmitThisFrame;
        if (particlesCacheCount <= 0)
            return;

        //faster access
        Color[] colorCache = particleInitColorCache;
        Vector3[] posCache = particleInitPositionsCache;

        Vector3 tempV = Vector3.zero;

        for (int i = 0; i < EmissionCount; i++)
        {
            int rnd = Random.Range(0, pCount);
            ParticleSystem.EmitParams em = new ParticleSystem.EmitParams();
            if (UsePixelSourceColor)
                em.startColor = colorCache[rnd];
            em.startSize = pStartSize;

            //if particles are set to World we must remove original particle calculation and apply the new transform modifiers.
            if (currentSimulationSpace == ParticleSystemSimulationSpace.World)
            {
                Vector3 origPos = posCache[rnd];

                tempV.x = origPos.x * transformScale.x;
                tempV.y = origPos.y * transformScale.y;

                em.position = transformRot * tempV + transformPos;
                particlesSystem.Emit(em, 1);
            }
            else
            {
                em.position = posCache[rnd];
                particlesSystem.Emit(em, 1);
            }
        }

        //sustract integer particles emitted and leave the float bit
        ParticlesToEmitThisFrame -= EmissionCount;
    }

    public override void Play()
    {
        if (!isPlaying)
            particlesSystem.Play();
        isPlaying = true;
    }

    public override void Stop()
    {
        isPlaying = false;
    }

    public override void Pause()
    {
        if (isPlaying)
            particlesSystem.Pause();
        isPlaying = false;
    }
}
}
