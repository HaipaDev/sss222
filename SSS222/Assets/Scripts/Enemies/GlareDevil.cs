using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlareDevil : MonoBehaviour{
    [SerializeField]float radiusBlind=3f;
    [SerializeField]float timerBlindMax=3.3f;
    [SerializeField]Vector2 efxBlind=new Vector2(4,4);

    float timerBlind=-4;
    PointPathing path;

    void Awake(){
        var i=GameRules.instance;
        if(i!=null){
            var e=i.glareDevilSettings;
            radiusBlind=e.radiusBlind;
            timerBlindMax=e.timerBlindMax;
            efxBlind=e.efxBlind;
        }
        timerBlind=timerBlindMax;
        path=GetComponent<PointPathing>();
    }
    void Update(){  if(!GameSession.GlobalTimeIsPaused){
        if(path.waypointIndex==path.waypointsL.Count-1){GetComponent<SpriteRenderer>().flipX=false;transform.GetChild(0).position=new Vector3(transform.position.x+(-GetComponent<Glow>().offset.x),transform.position.y+GetComponent<Glow>().offset.y,0.01f);}
        if(path.waypointIndex==1){GetComponent<SpriteRenderer>().flipX=true;transform.GetChild(0).position=new Vector3(transform.position.x+GetComponent<Glow>().offset.x,transform.position.y+GetComponent<Glow>().offset.y,0.01f);}
        if(Player.instance!=null)if(Vector2.Distance(Player.instance.transform.position,transform.position)<radiusBlind){Blind();}
        if(timerBlind>0)timerBlind-=Time.deltaTime;
    }}
    void Blind(){if(timerBlind<=0&&timerBlind!=-4){Player.instance.Blind(efxBlind.x,efxBlind.y);AudioManager.instance.Play("GlareDevil");timerBlind=timerBlindMax;}}
}
