using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class FadeOutBullet : MonoBehaviour{
    [SerializeField] public float time=3f;
    [SerializeField] public float timeStartFade=1f;
    [SerializeField] public bool fadeAlphaStartFrom255=false;
    [SerializeField][DisableInEditorMode] public float timer;
    float startAlpha=255f;
    void Start(){timer=time;if(!fadeAlphaStartFrom255)startAlpha=GetComponent<SpriteRenderer>().color.a;}
    void Update(){
        if(timer>0){timer-=Time.deltaTime;}
        if(timer<=timeStartFade){
            Color c=GetComponent<SpriteRenderer>().color;
            GetComponent<SpriteRenderer>().color=new Color(c.r,c.g,c.b,startAlpha*GameAssets.Normalize(timeStartFade-timer,timeStartFade,0f));
            if(GetComponentInChildren<ParticleSystem>()!=null){
                Destroy(GetComponentInChildren<ParticleSystem>().gameObject);
                /*c=GetComponentInChildren<ParticleSystem>().startColor;
                GetComponentInChildren<ParticleSystem>().startColor=new Color(c.r,c.g,c.b,255f*(timeStartFade-timer));*/
            }
        }
        if(timer<=0){Destroy(gameObject);}
    }
}
