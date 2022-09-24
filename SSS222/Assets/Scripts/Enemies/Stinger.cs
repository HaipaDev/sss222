using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stinger : MonoBehaviour{
    public Vector2 distReq=new Vector2(0.5f,3f);
    public bool flipped;
    void Update(){  if(!GameManager.GlobalTimeIsPaused){
        if(Player.instance!=null){
            if((Mathf.Abs(transform.position.x-Player.instance.transform.position.x)<distReq.x)&&
            ((transform.position.y-Player.instance.transform.position.y<distReq.y)&&
            Player.instance.transform.position.y<transform.position.y//is below
            )){
                flipped=true;
            }else{flipped=false;}
        }else{flipped=false;}
        if(flipped&&transform.rotation.z!=180){transform.rotation=new Quaternion(0,0,180,0);}
        else if(!flipped&&transform.rotation.z!=0){transform.rotation=new Quaternion(0,0,0,0);}
    }}
}
