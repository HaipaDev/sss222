using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI Particle System
/// </summary>
public class UIParticleSystem : MonoBehaviour {

    /// <summary>
    /// Particle Image
    /// </summary>
    public Sprite Particle;

    /// <summary>
    /// Play Duration
    /// </summary>
    public float Duration = 5f;

    /// <summary>
    /// Loop Emission
    /// </summary>
    public bool Looping = true;

    /// <summary>
    /// Play Lifetime (if not loopable)
    /// </summary>
    public float Lifetime = 5f;

    /// <summary>
    /// Particle Emission Speed
    /// </summary>
    public float Speed = 5f;

    /// <summary>
    /// Particle Size (will be multiplied with the size over lifetime)
    /// </summary>
    public float Size = 1f;

    /// <summary>
    /// Particle Rotation per Second
    /// </summary>
    public float Rotation = 0f;

    /// <summary>
    /// Play Particle Effect On Awake
    /// </summary>
    public bool PlayOnAwake = true;

    /// <summary>
    /// Gravity
    /// </summary>
    public float Gravity = -9.81f;

    /// <summary>
    /// Emission Per Second
    /// </summary>
    public float EmissionsPerSecond = 10f;

    /// <summary>
    /// Initial Direction
    /// </summary>
    public Vector2 EmissionDirection = new Vector2(0,1f);

    /// <summary>
    /// Random Range where particles are emitted
    /// </summary>
    public float EmissionAngle = 90f;

    /// <summary>
    /// Color Over Lifetime
    /// </summary>
    public Gradient ColorOverLifetime;

    /// <summary>
    /// Size Over Lifetime
    /// </summary>
    public AnimationCurve SizeOverLifetime;

    /// <summary>
    /// Speed Over Lifetime
    /// </summary>
    public AnimationCurve SpeedOverLifetime;

    [HideInInspector]
    public bool IsPlaying { get; protected set; }

    protected float Playtime = 0f;
    protected Image[] ParticlePool;
    protected int ParticlePoolPointer;


	// Use this for initialization
	void Start () {    
    }

    void Awake()
    {
        if (ParticlePool == null)
            Init();
        if (PlayOnAwake)
            Play();
    }

    private void Init()
    {
        ParticlePoolPointer = 0;
        ParticlePool = new Image[(int)(Lifetime * EmissionsPerSecond * 1.1f + 1)];
        for (int i = 0; i < ParticlePool.Length; i++)
        {

            var gameObject = new GameObject("Particle");
            gameObject.transform.SetParent(transform);
            gameObject.SetActive(false);
            ParticlePool[i] = gameObject.AddComponent<Image>();
            ParticlePool[i].transform.localRotation = Quaternion.identity;
            ParticlePool[i].transform.localPosition = Vector3.zero;
            ParticlePool[i].sprite = Particle;
        }
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void Play()
    {
        IsPlaying = true;
        StartCoroutine(CoPlay());
    }

    private IEnumerator CoPlay()
    {
        Playtime = 0f;
        var particleTimer = 0f;
        while (IsPlaying && (Playtime < Duration || Looping))
        {
            Playtime += Time.deltaTime;
            particleTimer += Time.deltaTime;
            while (particleTimer > 1f / EmissionsPerSecond)
            {
                particleTimer -= 1f / EmissionsPerSecond;
                ParticlePoolPointer = (ParticlePoolPointer + 1) % ParticlePool.Length;
                if(!ParticlePool[ParticlePool.Length - 1 - ParticlePoolPointer].gameObject.activeSelf)
                    StartCoroutine(CoParticleFly(ParticlePool[ParticlePool.Length - 1 - ParticlePoolPointer]));
            }
            
            yield return new WaitForEndOfFrame();
        }
        IsPlaying = false;
    }

    private IEnumerator CoParticleFly(Image particle)
    {
        particle.gameObject.SetActive(true);
        particle.transform.localPosition = Vector3.zero;
        var particleLifetime = 0f;

        //get default velocity
        var emissonAngle = new Vector3(EmissionDirection.x,EmissionDirection.y,0f);
        //apply angle
        emissonAngle = Quaternion.AngleAxis(Random.Range(-EmissionAngle / 2f, EmissionAngle / 2f), Vector3.forward) * emissonAngle;
        //normalize
        emissonAngle.Normalize();

        var gravityForce = Vector3.zero;

        while (particleLifetime < Lifetime)
        {
            particleLifetime += Time.deltaTime;

            //apply gravity
            gravityForce = Vector3.up * Gravity * particleLifetime;

            //set position
            particle.transform.position += emissonAngle * SpeedOverLifetime.Evaluate(particleLifetime / Lifetime) * Speed + gravityForce;

            //set scale
            particle.transform.localScale = Vector3.one * SizeOverLifetime.Evaluate(particleLifetime / Lifetime) * Size;

            //set rortaion
            particle.transform.localRotation = Quaternion.AngleAxis(Rotation * particleLifetime, Vector3.forward);

            //set color
            particle.color = ColorOverLifetime.Evaluate(particleLifetime / Lifetime);

            yield return new WaitForEndOfFrame();
        }

        particle.gameObject.SetActive(false);
    }
}
