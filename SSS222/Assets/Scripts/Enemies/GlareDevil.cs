using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlareDevil : MonoBehaviour{
    [SerializeField]float timerMax;
    float timer=-4;
    EnemyPathing path;
    void Start(){
        timer=timerMax;
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
        if(timer<=0){FindObjectOfType<Player>().Blind(4,4);GetComponent<AudioSource>().Play();timer=timerMax;}
    }
}
