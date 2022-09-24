using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LCrystalDrop : MonoBehaviour{
    public int amnt;
    [SerializeField] int amntS=4;
    [SerializeField] int amntE=14;
    /*private void Awake() {
        //Set values
        var i=GameRules.instance;
        if(i!=null){
            amntS=i.crystalDropS;
            amntS=i.crystalDropE;
        }
    }*/
    void Start(){
        amnt=Random.Range(amntS,amntE);
        if(amnt>=amntS+amntE/2)GetComponent<SpriteRenderer>().sprite=AssetsManager.instance.Spr("coinB");
    }
}
