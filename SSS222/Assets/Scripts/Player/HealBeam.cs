using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealBeam : MonoBehaviour{
    public float value=0.125f;
    public bool absorp=true;
    Color colorDef;
    [SerializeField] Color colorHeal=Color.red;
    void Start(){
        colorDef=GetComponent<SpriteRenderer>().color;
        if(!absorp){GetComponent<SpriteRenderer>().color=colorHeal;}
    }
}