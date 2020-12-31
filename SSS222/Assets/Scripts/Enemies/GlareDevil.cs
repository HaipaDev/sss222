using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlareDevil : MonoBehaviour{
    [SerializeField]float timerStart=1f;
    [SerializeField]float timerMax=3.3f;
    [SerializeField]Vector2 efxBlind=new Vector2(4,4);

    float timer=-4;
    EnemyPathing path;
    void Awake(){
    //Set Values
    var i=GameRules.instance;
    if(i!=null){
        var e=i.glareDevilSettings;
        timerStart=e.timerStart;
        timerMax=e.timerMax;
        efxBlind=e.efxBlind;
    }
    }
    void Start(){
        timer=timerStart;
        path=GetComponent<EnemyPathing>();
    }

    void Update(){
        if(FindObjectOfType<Player>()!=null){
            if(path.waypointIndex==path.waypointsL.Count-1){GetComponent<SpriteRenderer>().flipX=false;transform.GetChild(0).position=new Vector3(transform.position.x+(-GetComponent<Glow>().GetXX()),transform.position.y+GetComponent<Glow>().GetYY(),0.01f);}
            if(path.waypointIndex==1){GetComponent<SpriteRenderer>().flipX=true;transform.GetChild(0).position=new Vector3(transform.position.x+GetComponent<Glow>().GetXX(),transform.position.y+GetComponent<Glow>().GetYY(),0.01f);}
            //transform.GetChild(0).GetComponent<ParticleSystem>().Play();
            if(Vector2.Distance(FindObjectOfType<Player>().transform.position,transform.position)<3f){Blind();}
        }
        if(timer>0)timer-=Time.deltaTime;
    }
    void Blind(){
        if(timer<=0){FindObjectOfType<Player>().Blind(efxBlind.x,efxBlind.y);GetComponent<AudioSource>().Play();timer=timerMax;}
    }
}
