using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopUI_Destroy : MonoBehaviour{
    void Start(){
        StartCoroutine(Check());
    }
    IEnumerator Check(){
        yield return new WaitForSeconds(0.1f);
        if(GameSession.instance!=null){if(!GameSession.instance.shopOn)Destroy(gameObject);}
        Destroy(this);
    }
}
