using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatchUI : MonoBehaviour{
    RectTransform rt;
    void Start(){
        StartCoroutine(Check());
        if(GetComponent<RectTransform>()!=null){rt=GetComponent<RectTransform>();}
    }
    IEnumerator Check(){
        yield return new WaitForSeconds(0.15f);
        if(gameObject.name=="PauseButton"&&!GameRules.instance.upgradesOn){
        if(!GameRules.instance.shopOn){rt.position=new Vector2(rt.position.x,rt.position.y+(180/3));}
        else if(GameRules.instance.shopOn){transform.position=new Vector2(transform.position.x,transform.position.y+(100/3));}
        Destroy(this);
        }
        if(gameObject.name=="CoinsIMG"&&!GameRules.instance.upgradesOn&&GameRules.instance.shopOn){transform.position=new Vector2(transform.position.x+1,transform.position.y);}
        if(gameObject.name=="CoinsText"&&!GameRules.instance.upgradesOn&&GameRules.instance.shopOn){rt.position=new Vector2(rt.position.x+(124/3),rt.position.y);}
        if(!(GameRules.instance.xpOn)&&(gameObject.name=="CoinsIMG"||gameObject.name=="CoresIMG")){transform.position=new Vector2(transform.position.x,transform.position.y+0.1f);}
        if(!(GameRules.instance.xpOn)&&(gameObject.name=="CoinsText"||gameObject.name=="CoresText")){rt.position=new Vector2(rt.position.x,rt.position.y+(20f/3));}
        //yield return new WaitForSeconds(0.025f);
        Destroy(this);
    }
}
