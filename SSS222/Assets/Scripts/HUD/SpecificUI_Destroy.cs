using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecificUI_Destroy : MonoBehaviour{
    [SerializeField] specifiUI_type type;
    void Start(){
        StartCoroutine(Check());
    }

    IEnumerator Check(){
        if(type==specifiUI_type.energy&&!GameRules.instance.energyOnPlayer)Destroy(gameObject);
        else if(type==specifiUI_type.crystals&&!GameSession.instance.crystalsOn)Destroy(gameObject);
        else if(type==specifiUI_type.xp&&!GameSession.instance.xpOn)Destroy(gameObject);
        else if(type==specifiUI_type.shop&&!GameSession.instance.shopOn)Destroy(gameObject);
        else if(type==specifiUI_type.modules&&!GameSession.instance.modulesOn)Destroy(gameObject);
        else if(type==specifiUI_type.statUpgs&&!GameSession.instance.statUpgOn)Destroy(gameObject);
        else if(type==specifiUI_type.inventory&&!GameSession.instance.iteminvOn)Destroy(gameObject);
        else if(type==specifiUI_type.leveling&&!GameSession.instance.levelingOn)Destroy(gameObject);
        else if(type==specifiUI_type.cores&&!GameSession.instance.coresOn)Destroy(gameObject);
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(Check());
    }
}

enum specifiUI_type{
    energy,crystals,xp,shop,modules,statUpgs,inventory,leveling,cores
}