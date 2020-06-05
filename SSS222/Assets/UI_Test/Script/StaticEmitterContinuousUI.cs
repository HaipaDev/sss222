using UnityEngine;
using System.Collections;

namespace SpriteParticleEmitter
{
/// <summary>
/// Refer to manual for description.
/// </summary>
public class StaticEmitterContinuousUI : StaticUIImageEmitter
{
    [Header("Emission")]
    [Tooltip("Particles to emit per second")]
    //! Particles to emit per second
    public float EmissionRate = 1000;
    //! Save time to know how many particles to show per frame
    protected float ParticlesToEmitThisFrame;
    //! Will be called when the emitter is ready to play (after caching)
    public override event SimpleEvent OnAvailableToPlay;

    protected override void Awake()
    {
        base.Awake();
        currentRectTransform = GetComponent<RectTransform>();
        targetRectTransform = imageRenderer.GetComponent<RectTransform>();
    }

    [Tooltip("Should the transform match target Image Renderer Position?")]
    //! Should the transform match target Image Renderer Position?
    public bool matchImageRendererPostionData = true;
    [Tooltip("Should the transform match target Image Renderer Scale?")]
    //! Should the RectTransform match target Image Renderer Position?
    public bool matchImageRendererScale = true;
    
    //! The target Image Renderer's RectTransform
    private RectTransform targetRectTransform;
    //! This RectTransform
    private RectTransform currentRectTransform;
    
    protected Vector2 offsetXY;
    //! Multiplier used with texture's Pixels per unit
    protected float wMult = 100;
    //! Multiplier used with texture's Pixels per unit
    protected float hMult = 100;

    protected override void Update()
    {
        base.Update();

        if (isPlaying && hasCachingEnded)
        {
            ProcessPositionAndScale();
            Emit();
        }
    }

    void ProcessPositionAndScale()
    {
        //match current RectTransform's data with target RectTransform
        if (matchImageRendererPostionData)
            currentRectTransform.position = new Vector3(targetRectTransform.position.x,
                targetRectTransform.position.y, targetRectTransform.position.z);
        currentRectTransform.pivot = targetRectTransform.pivot;
        if (matchImageRendererPostionData)
        {
            currentRectTransform.anchoredPosition = targetRectTransform.anchoredPosition;
            currentRectTransform.anchorMin = targetRectTransform.anchorMin;
            currentRectTransform.anchorMax = targetRectTransform.anchorMax;
            currentRectTransform.offsetMin = targetRectTransform.offsetMin;
            currentRectTransform.offsetMax = targetRectTransform.offsetMax;
        }
        if (matchImageRendererScale)
            currentRectTransform.localScale = targetRectTransform.localScale;
        currentRectTransform.rotation = targetRectTransform.rotation;

        currentRectTransform.sizeDelta = new Vector2(targetRectTransform.rect.width, targetRectTransform.rect.height);

        //Calculate position multipliers based on pixels per unit
        float offsetX = (1 - currentRectTransform.pivot.x) * (currentRectTransform.rect.width) - currentRectTransform.rect.width / 2;
        float offsetY = (1 - currentRectTransform.pivot.y) * (-currentRectTransform.rect.height) + currentRectTransform.rect.height / 2;
        offsetXY = new Vector2(offsetX, offsetY);
        Sprite sprite = imageRenderer.sprite;
        wMult = sprite.pixelsPerUnit * (currentRectTransform.rect.width / sprite.rect.size.x);
        hMult = sprite.pixelsPerUnit * (currentRectTransform.rect.height / sprite.rect.size.y);
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
        Vector3 transformPos = currentRectTransform.position;
        Quaternion transformRot = currentRectTransform.rotation;
        Vector3 transformScale = currentRectTransform.localScale;
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

            Vector3 origPos = posCache[rnd];

            //if particles are set to World we must remove original particle calculation and apply the new transform modifiers.
            if (currentSimulationSpace == ParticleSystemSimulationSpace.World)
            {
                tempV.x = (origPos.x * wMult) * transformScale.x + offsetXY.x;
                tempV.y = (origPos.y * hMult) * transformScale.y - offsetXY.y;
                em.position = transformRot * tempV + transformPos;
                particlesSystem.Emit(em, 1);
            }
            else
            {
                tempV.x = (origPos.x * wMult) + offsetXY.x;
                tempV.y = (origPos.y * hMult) - offsetXY.y;
                em.position = tempV;
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
