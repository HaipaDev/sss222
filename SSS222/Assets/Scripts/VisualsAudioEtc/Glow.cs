using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glow : MonoBehaviour{
    [SerializeField] GameObject glowVFX;
    ParticleSystem ps;
    public Color colorGlow = Color.red;
    [SerializeField] float sizeGlow = 1f;
    [SerializeField] float alphaGlow = 0.5f;
    [SerializeField] float emissionSpeed = 7.63f;
    //[SerializeField] float speed=
    [SerializeField] float xx = 0;
    [SerializeField] float yy = 0;
    void Start(){
        var go=Instantiate(glowVFX,new Vector2(transform.position.x+xx,transform.position.y+yy),Quaternion.identity);
        go.transform.parent=transform;
        //transform.position=new Vector3(transform.position.x,transform.position.y,transform.position.z-0.01f);
        ps=go.GetComponent<ParticleSystem>();
        var col=ps.colorOverLifetime;
        col.enabled=true;
        colorGlow.a=alphaGlow;
        col.color=colorGlow;
        var size=ps.sizeOverLifetime;
        size.enabled=true;
        size.size=sizeGlow;
        var emission=ps.emission;
        emission.rateOverTime=emissionSpeed;
    }

    void Update(){
        if(!SaveSerial.instance.settingsData.particles&&ps.isPlaying){ps.Stop();}
        if(SaveSerial.instance.settingsData.particles&&ps.isStopped){ps.Play();}
        if(sizeGlow==-1){
            sizeGlow=(transform.localScale.x+transform.localScale.y)/2;
        }
    }
    public float GetXX(){return xx;}
    public float GetYY(){return yy;}
}
