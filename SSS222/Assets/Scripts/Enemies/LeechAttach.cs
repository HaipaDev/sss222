using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeechAttach : MonoBehaviour{
    [HeaderAttribute("Properties")]
    [SerializeField] float catch_distance=1.5f;
    [SerializeField] public float shake_distance = 0.05f;
    [SerializeField] public int count_max = 3;
    [SerializeField] float fallSpeed = 6f;
    [HeaderAttribute("Current")]
    public bool attached;
    public bool detached;
    public int stage = 0;
    public int count = 0;
    public float dist;

    Follow follow;
    Rigidbody2D rb;
    void Awake(){
    //Set Values
    var i=GameRules.instance;
    if(i!=null){
        var e=i.mechaLeechSettings;
        catch_distance=e.catch_distance;
        shake_distance=e.shake_distance;
        count_max=e.count_max;
        fallSpeed=e.fallSpeed;
    }
    }
    void Start(){
        follow=GetComponent<Follow>();
        rb=GetComponent<Rigidbody2D>();
    }

    void Update(){
        if(follow.targetObj!=null){
            dist=Vector2.Distance(follow.targetPos,follow.selfPos);
            if(dist<catch_distance&&detached==false){
                if(attached==false){
                    AudioManager.instance.Play("LeechAttach");
                    attached=true;
                }
            }else{attached=false;}

            if(attached==true){
                if(count<count_max){
                    if(follow.selfPos.x>follow.targetPos.x+shake_distance){
                        if(stage==0)stage=1;
                    }
                    else if(follow.selfPos.x<follow.targetPos.x-shake_distance){
                        if (stage==1)stage=2;
                    }

                    if(stage==2){
                        count+=1;
                        stage=0;
                    }
                }else{
                    if(follow.selfPos.x<follow.targetPos.x-shake_distance){
                        rb.velocity=new Vector2(fallSpeed,-fallSpeed);
                        follow.enabled=false;
                        detached=true;
                        attached=false;
                    }
                }
            }
        }else{rb.velocity=new Vector2(fallSpeed,-fallSpeed);}
    }
}
