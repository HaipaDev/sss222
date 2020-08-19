using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinBullet : MonoBehaviour{
    float hspeed=0.01f;
    float xLim=0.3f;
    bool goLeft;
    float xPosStart;
    float xPos;
    float xMin;
    float xMax;
    void Start(){
        xPosStart=transform.position.x;
        xPos=xPosStart;
        xMin=(xPosStart+xMin);
        xMax=(xPosStart+xMax);
        if(FindObjectOfType<GoblinBullet>().transform.position.x<transform.position.x&&FindObjectOfType<GoblinBullet>().transform.position.y==transform.position.y){goLeft=true;xMin=0;xMax=xLim;}else{goLeft=false;xMin=-xLim;xMax=0;}
    }

    void Update(){
        if(Random.value>0.5){goLeft=true;}else{goLeft=false;}
        var curSpeed=hspeed*Time.timeScale;
        if(!goLeft&&xPos<xMax){xPos+=curSpeed;}
        if(xPos>=xMax){goLeft=true;}
        if(goLeft&&xPos>xMin){xPos-=curSpeed;}
        if(xPos<=xMax){goLeft=false;}
        transform.position=new Vector2(xPos,transform.position.y);
    }
}
