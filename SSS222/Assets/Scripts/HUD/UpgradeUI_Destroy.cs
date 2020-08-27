using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeUI_Destroy : MonoBehaviour{
    void Start(){
        StartCoroutine(Check());
    }
    IEnumerator Check(){
        yield return new WaitForSeconds(0.07f);
        if(GameSession.instance!=null){if(!GameSession.instance.upgradesOn)Destroy(gameObject);}
        Destroy(this);
    }
}
