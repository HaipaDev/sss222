using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpreadObjects : MonoBehaviour{
    public static void SpreadGO(GameObject go, Vector2 pos, int amnt=2, float rangeX=0.5f, float rangeY=0.5f){
        for(var i=0;i<amnt;i++){
            var rndmX=Random.Range(-rangeX,rangeX);
            var rndmY=Random.Range(-rangeY,rangeY);
            var poss=pos+new Vector2(rndmX,rndmY);
            Instantiate(go,poss,Quaternion.identity);
        }
    }
}
