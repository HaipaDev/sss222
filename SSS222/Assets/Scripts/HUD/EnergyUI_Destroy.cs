using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyUI_Destroy : MonoBehaviour{
    void Start(){
        StartCoroutine(Check());
    }
    IEnumerator Check(){
        yield return new WaitForSecondsRealtime(0.07f);
        if(GameRules.instance!=null){if(!GameRules.instance.energyOnPlayer)Destroy(gameObject);}
        Destroy(this);
    }
}
