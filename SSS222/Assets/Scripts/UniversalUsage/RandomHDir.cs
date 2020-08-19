using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomHDir : MonoBehaviour{
    [SerializeField] float chanceForR=50;
    [SerializeField] float hspeed = 1f;
    [SerializeField] bool randomSpeed=false;
    [SerializeField] float hspeedS = 0.8f;
    [SerializeField] float hspeedE = 1.2f;
    [SerializeField] bool dynamicSpeed=false;
    
    [SerializeField] float timeToChangeSpeed=0.25f;
    [SerializeField] bool randomChangeTime=false;
    [SerializeField] float timeToChangeSpeedS=0.15f;
    [SerializeField] float timeToChangeSpeedE=0.45f;
    public float timerSpeed;
    public int dir = 1;

    Rigidbody2D rb;
    
    void Start(){
        timerSpeed=timeToChangeSpeed;
        rb = GetComponent<Rigidbody2D>();

        
        if(Random.Range(0,100)<chanceForR) { dir = -1; }
        if(randomSpeed==true){
            hspeed=Random.Range(hspeedS, hspeedE);
        }
        rb.velocity = new Vector2(dir * hspeed, rb.velocity.y);
    }

    void Update(){
        if(dynamicSpeed==true){
            timerSpeed-=Time.deltaTime;
            if(timerSpeed<=0){
                dir*=-1;
                hspeed=Random.Range(hspeedS, hspeedE);
                rb.velocity=new Vector2(dir*hspeed,rb.velocity.y);

                if(randomChangeTime==true){timerSpeed=Random.Range(timeToChangeSpeedS,timeToChangeSpeedE);}
                else{timerSpeed=timeToChangeSpeed;}
            }
        }
    }
}
