using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glow : MonoBehaviour{
    [SerializeField] string assetName="GlowAuto";
    [SerializeField] bool materialClone=true;
    [SerializeField] public Color color=Color.white;
    [SerializeField] float size=0f;
    //[SerializeField] float alpha=0.5f;
    [SerializeField] float emissionSpeed=0f;
    [SerializeField] int maxParticles=0;
    [SerializeField] public Vector2 offset;
    
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
            var col=ps.colorOverLifetime;
            col.enabled=true;
            //color.a=alpha;
            col.color=color;
            var sizePs=ps.sizeOverLifetime;
            sizePs.enabled=true;
            if(size!=0)sizePs.size=size;
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

        if(size==-1){size=(transform.localScale.x+transform.localScale.y)/2;}
    }
    void OnDestroy(){Destroy(mat);}
}
