using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XpUI_Destroy : MonoBehaviour{
    void Start(){
        StartCoroutine(Check());
    }
    IEnumerator Check(){
        yield return new WaitForSeconds(0.1f);
        if(GameSession.instance!=null){if(!GameSession.instance.xpOn)Destroy(gameObject);}
        Destroy(this);
    }
}
