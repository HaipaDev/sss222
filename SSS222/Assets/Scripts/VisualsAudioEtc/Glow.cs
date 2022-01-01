using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glow : MonoBehaviour{
    [SerializeField] string assetName="GlowAuto";
    [SerializeField] bool materialClone=true;
    [SerializeField] public Color color=Color.white;
    [SerializeField] Vector2 size=Vector2.zero;
    [SerializeField] float emissionSpeed=0f;
    [SerializeField] int maxParticles=0;
    [SerializeField] public Vector2 offset;
    [SerializeField] public bool alignToDirection;
    
    ParticleSystem ps;
    Material mat;
    void Start(){
        GameObject go=Instantiate(GameAssets.instance.GetVFX(assetName),transform);
        go.transform.localPosition=offset;

        if(go!=null){ps=go.GetComponent<ParticleSystem>();}else{Debug.LogWarning("No particle created for "+gameObject.name);}
        if(ps!=null){
            if(materialClone){
                mat=ps.GetComponent<Renderer>().material=Instantiate(ps.GetComponent<Renderer>().material);
                mat.SetTexture("_MainTex",GetComponent<SpriteRenderer>().sprite.texture);
            }
            ps.startColor=color;
            var main=ps.main;
            if(size!=Vector2.zero){
                main.startSize3D=true;
                if(size.x!=0)main.startSizeXMultiplier=size.x;
                if(size.y!=0)main.startSizeYMultiplier=size.y;
            }
            if(alignToDirection){var shape=ps.shape;shape.alignToDirection=true;}
            var emission=ps.emission;
            if(maxParticles!=0)ps.maxParticles=maxParticles;
            if(emissionSpeed!=0)emission.rateOverTime=emissionSpeed;
        }
    }
    void Update(){
        if(ps!=null){
            if(!SaveSerial.instance.settingsData.particles&&ps.isPlaying){ps.Stop();}
            if(SaveSerial.instance.settingsData.particles&&ps.isStopped){ps.Play();}
        }
    }
    void OnDestroy(){Destroy(mat);}
}
