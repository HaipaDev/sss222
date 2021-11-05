using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecificUI_Destroy : MonoBehaviour{
    [SerializeField] specifiUI_type type;
    IEnumerator Start(){
        yield return new WaitForSeconds(0.1f);
        if(type!=specifiUI_type.energy&&!GameRules.instance.energyOnPlayer)Destroy(gameObject);
        if(type!=specifiUI_type.crystals&&!GameSession.instance.crystalsOn)Destroy(gameObject);
        if(type!=specifiUI_type.xp&&!GameSession.instance.xpOn)Destroy(gameObject);
        if(type!=specifiUI_type.shop&&!GameSession.instance.shopOn)Destroy(gameObject);
        if(type!=specifiUI_type.upgrades&&!GameSession.instance.upgradesOn)Destroy(gameObject);
    }
}

enum specifiUI_type{
    energy,crystals,xp,shop,upgrades
}