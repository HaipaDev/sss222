using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tag_MagnetAffected : MonoBehaviour{
    float distReq,speedFollow;
    [SerializeField] float distReqMulti=6,speedFollowMulti=5;
    float distReqCalc,speedFollowCalc;
    void Start() {
        if(GetComponent<Follow>()!=null){
            distReq=GetComponent<Follow>().distReq;
            speedFollow=GetComponent<Follow>().speedFollow;
        }
        distReqCalc=distReq*distReqMulti;
        speedFollowCalc=speedFollow*speedFollowMulti;
    }
    public float GetDistReq(){return distReqCalc;}
    public float GetSpeedFollow(){return speedFollowCalc;}
}