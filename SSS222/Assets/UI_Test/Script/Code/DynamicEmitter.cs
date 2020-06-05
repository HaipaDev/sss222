using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Assertions.Comparers;
using UnityEngine.Profiling;


namespace SpriteParticleEmitter
{
/// <summary>
/// Refer to manual for description.
/// </summary>
public class DynamicEmitter : EmitterBase
{
    [Tooltip("Start emitting as soon as able")]
    //! Start emitting as soon as able?
    public bool PlayOnAwake = true;

    [Header("Emission")]
    [Tooltip("Particles to emit per second")]
    //! Particles to emit per second
    public float EmissionRate = 1000;
    //! Save time to know how many particles to show per frame
    protected float ParticlesToEmitThisFrame;

    [Tooltip("Should the system cache sprites data? (Refer to manual for further explanation)")]
    //! Should the system cache sprites data? (Refer to manual for further explanation)
    public bool CacheSprites = true;

    //! Emiting from color needs to cycle all pixels in the sprite to know where the color is and later emition needs to randomize the emitting position so a look up table cache is needed
    //! Made it private but not local to the scope for reusing in next frames
    private Color[] colorCache = new Color[1];
    //! Emiting from color needs to cycle all pixels in the sprite to know where the color is and later emition needs to randomize the emitting position so a look up table cache is needed
    //! Made it private but not local to the scope for reusing in next frames
    private int[] indexCache = new int[1];

    //! Dictionary containing all sprites data so far for not asking texture.GetPixels() every frame, which would be slow.
    protected Dictionary<Sprite, Color[]> spritesSoFar = new Dictionary<Sprite, Color[]>();

    protected override void Awake()
    {
        base.Awake();
        if (PlayOnAwake)
            isPlaying = true;

        #if UNITY_5_5_OR_NEWER
        if (mainModule.maxParticles < EmissionRate)
            mainModule.maxParticles = Mathf.CeilToInt(EmissionRate);
        #endif
    }

    /// <summary>
    /// When playing it emits particles based on EmissionRate.
    /// </summary>
    protected void Update()
    {
        if (isPlaying)
        {
            ParticlesToEmitThisFrame += EmissionRate*Time.deltaTime;

            int EmissionCount = (int) ParticlesToEmitThisFrame;
            //don't even call the method if no particle would be emitted
            if (EmissionCount > 0)
                Emit(EmissionCount);
            ParticlesToEmitThisFrame -= EmissionCount;
        }
    }

    /// <summary>
    /// Randomly emit particles sprite.
    /// </summary>
    /// <param name="emitCount">Number of particles to emit</param>
    public void Emit(int emitCount)
    {
        Sprite sprite = spriteRenderer.sprite;

        float colorR = EmitFromColor.r;
        float colorG = EmitFromColor.g;
        float colorB = EmitFromColor.b;

        Vector3 transformPos = spriteRenderer.gameObject.transform.position;
        Quaternion transformRot = spriteRenderer.gameObject.transform.rotation;
        Vector3 transformScale = spriteRenderer.gameObject.transform.lossyScale;
        
        //if Particle system is using Local Space discard transform modifiers.
        if (SimulationSpace == ParticleSystemSimulationSpace.Local)
        {
            transformPos = Vector3.zero;
            transformScale = Vector3.one;
            transformRot = Quaternion.identity;
        }

        bool flipX = spriteRenderer.flipX;
        bool flipY = spriteRenderer.flipY;

        float PixelsPerUnit = sprite.pixelsPerUnit;

        float width = (int) sprite.rect.size.x;
        float height = (int) sprite.rect.size.y;

        //set particle size based on sprite Pixels per unit and particle system prefered size
        #if UNITY_5_5_OR_NEWER
        float startSize = 1 / (PixelsPerUnit);
        startSize *= mainModule.startSize.constant; //TODO ability to process different sizes coming in next update
        #else
            float startSize = 1/(PixelsPerUnit);
            startSize *= particlesSystem.startSize;
        #endif

        //calculate sprite offset position in texture
        float offsetX = sprite.pivot.x/PixelsPerUnit;
        float offsetY = sprite.pivot.y/PixelsPerUnit;

        //if the sprite raw data is cached use that one, if not ask for it to the texture.
        Color[] pix;
        if (CacheSprites)
        {
            if (spritesSoFar.ContainsKey(sprite))
                pix = spritesSoFar[sprite];
            else
            {
                pix = sprite.texture.GetPixels((int) sprite.rect.position.x, (int) sprite.rect.position.y, (int) width, (int) height); 
                spritesSoFar.Add(sprite, pix);
            }
        }
        else
        {
            pix = sprite.texture.GetPixels((int) sprite.rect.position.x, (int) sprite.rect.position.y, (int) width, (int) height);
        }

        float toleranceR = RedTolerance;
        float toleranceG = GreenTolerance;
        float toleranceB = BlueTolerance;

        float widthByHeight = width*height;

        Color[] cCache = colorCache;
        int[] iCache = indexCache;

        if (cCache.Length < widthByHeight)
        {
            colorCache = new Color[(int) widthByHeight];
            indexCache = new int[(int) widthByHeight];
            cCache = colorCache;
            iCache = indexCache;
        }

        //Profiler.BeginSample("Part ONe");
        //find available pixels to emit from
        int matchesCount = 0;
        for (int i = 0; i < widthByHeight; i++)
        {
            //Profiler.BeginSample("Color access");
            Color c = pix[i];
            //Profiler.EndSample();
            //skip pixels with alpha 0
            if (c.a <= 0)
                continue;

            //Profiler.BeginSample("Color comparer");
            //Skip unwanted colors when using Emission from color.
            if (UseEmissionFromColor) 
                if(!FloatComparer.AreEqual(colorR, c.r, toleranceR) ||
                 !FloatComparer.AreEqual(colorG, c.g, toleranceG) ||
                 !FloatComparer.AreEqual(colorB, c.b, toleranceB))
                continue;
            //Profiler.EndSample();

            //Profiler.BeginSample("Assignation");
            cCache[matchesCount] = c;
            iCache[matchesCount] = i;
            matchesCount++;
            //Profiler.EndSample();
        }
        //Profiler.EndSample();

        //no colors were matched, stop
        if (matchesCount <= 0)
            return;
        
        Vector3 tempV = Vector3.zero;

        //Profiler.BeginSample("Part Two");
        //emit needed particle count
        for (int k = 0; k < emitCount; k++)
        {
            int index = Random.Range(0, matchesCount - 1);
            int i = iCache[index];

            //get pixel position in texture
            float posX = ((i%width)/PixelsPerUnit) - offsetX;
            float posY = ((i/width)/PixelsPerUnit) - offsetY;

            //handle sprite renderer fliping
            if (flipX)
                posX = width/PixelsPerUnit - posX - offsetX*2;
            if (flipY)
                posY = height/PixelsPerUnit - posY - offsetY*2;

            tempV.x = posX * transformScale.x;
            tempV.y = posY * transformScale.y;

            ParticleSystem.EmitParams em = new ParticleSystem.EmitParams();
            // define new particle start position based on Sprite pixel position in texture, this game object's rotation and position.
            em.position = transformRot * tempV + transformPos;
            if (UsePixelSourceColor)
                em.startColor = cCache[index];

            em.startSize = startSize;
            particlesSystem.Emit(em, 1);
        }

        //Profiler.EndSample();
    }

    /// <summary>
    /// Will emit one particle from every pixel in the sprite, or from every pixel in the found color if UseEmissionFromColor is set to true
    /// </summary>
    /// <param name="hideSprite">Must it disable referenced spriteRenderer</param>
    public void EmitAll(bool hideSprite = true)
    {
        if (hideSprite)
            spriteRenderer.enabled = false;

        Sprite sprite = spriteRenderer.sprite;

        float colorR = EmitFromColor.r;
        float colorG = EmitFromColor.g;
        float colorB = EmitFromColor.b;

        Vector3 transformPos = spriteRenderer.gameObject.transform.position;
        Quaternion transformRot = spriteRenderer.gameObject.transform.rotation;
        Vector3 transformScale = spriteRenderer.gameObject.transform.lossyScale;
        //if Particle system is using Local Space discard transform modifiers.
        if (SimulationSpace == ParticleSystemSimulationSpace.Local)
        {
            transformPos = Vector3.zero;
            transformScale = Vector3.one;
            transformRot = Quaternion.identity;
        }

        bool flipX = spriteRenderer.flipX;
        bool flipY = spriteRenderer.flipY;

        float PixelsPerUnit = sprite.pixelsPerUnit;

        float width = (int) sprite.rect.size.x;
        float height = (int) sprite.rect.size.y;

        //set particle size based on sprite Pixels per unit and particle system prefered size
        #if UNITY_5_5_OR_NEWER
        float startSize = 1 / (PixelsPerUnit);
        startSize *= mainModule.startSize.constant; //TODO ability to process different sizes coming in next update
        #else
            float startSize = 1/(PixelsPerUnit);
            startSize *= particlesSystem.startSize;
        #endif

        //calculate sprite offset position in texture
        float offsetX = sprite.pivot.x/PixelsPerUnit;
        float offsetY = sprite.pivot.y/PixelsPerUnit;

        //if the sprite raw data is cached use that one, if not ask for it to the texture.
        Color[] pix;
        if (CacheSprites)
        {
            if (spritesSoFar.ContainsKey(sprite))
                pix = spritesSoFar[sprite];
            else
            {
                pix = sprite.texture.GetPixels((int)sprite.rect.position.x, (int)sprite.rect.position.y, (int)width, (int)height);
                spritesSoFar.Add(sprite, pix);
            }
        }
        else
        {
            pix = sprite.texture.GetPixels((int)sprite.rect.position.x, (int)sprite.rect.position.y, (int)width, (int)height);
        }

        float toleranceR = RedTolerance;
        float toleranceG = GreenTolerance;
        float toleranceB = BlueTolerance;

        float widthByHeight = width*height;

        Vector3 tempV = Vector3.zero;

        for (int i = 0; i < widthByHeight; i++)
        {
            Color c = pix[i];
            //skip pixels with alpha 0
            if (c.a <= 0)
                continue;

            //Skip unwanted colors when using Emission from color.
            if (UseEmissionFromColor)
                if (!FloatComparer.AreEqual(colorR, c.r, toleranceR) ||
                 !FloatComparer.AreEqual(colorG, c.g, toleranceG) ||
                 !FloatComparer.AreEqual(colorB, c.b, toleranceB))
                    continue;

            //get pixel position in texture
            float posX = ((i%width)/PixelsPerUnit) - offsetX;
            float posY = ((i/width)/PixelsPerUnit) - offsetY;

            //handle sprite renderer fliping
            if (flipX)
                posX = width/PixelsPerUnit - posX - offsetX*2;
            if (flipY)
                posY = height/PixelsPerUnit - posY - offsetY*2;

            tempV.x = posX * transformScale.x;
            tempV.y = posY * transformScale.y;

            ParticleSystem.EmitParams em = new ParticleSystem.EmitParams();
            // define new particle start position based on Sprite pixel position in texture, this game object's rotation and position.
            em.position = transformRot * tempV + transformPos;
            if (UsePixelSourceColor)
                em.startColor = c;

            em.startSize = startSize;
            particlesSystem.Emit(em, 1);
        }
    }

    /// <summary>
    /// Enable spriteRenderer if it was disabled.
    /// </summary>
    public void RestoreSprite()
    {
        spriteRenderer.enabled = true;
    }

    public override void Play()
    {
        if (!isPlaying)
            particlesSystem.Play();
        isPlaying = true;
    }

    public override void Pause()
    {
        if (isPlaying)
            particlesSystem.Pause();
        isPlaying = false;
    }

    public override void Stop()
    {
        isPlaying = false;
    }

    public override bool IsPlaying()
    {
        return isPlaying;
    }

    public override bool IsAvailableToPlay()
    {
        return true;
    }

    /// <summary>
    /// Clears the sprites cache
    /// </summary>
    public void ClearCachedSprites()
    {
        spritesSoFar = new Dictionary<Sprite, Color[]>();
    }
}
}