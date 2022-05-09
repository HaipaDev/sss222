using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tag_BarrierColor : MonoBehaviour{
    ParticleSystem ps;
    Color color;
    void Start(){
        ps=GetComponent<ParticleSystem>();
        color=ps.main.startColor.colorMin;
    }
    public void SetColor(Color c){color=c;}
    public Color GetColor(){return color;}
}