using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LCrystalDrop : MonoBehaviour{
    public int amnt;
    [SerializeField] int amntS=1;
    [SerializeField] int amntE=10;
    void Start(){
        amnt=Random.Range(amntS,amntE);
        //if(amnt==amntE){
        if(amnt>=amntE/2){
            GetComponent<SpriteRenderer>().sprite=GameAssets.instance.Spr("coinB");
        }
    }
}
