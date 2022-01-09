using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecificUI_Destroy : MonoBehaviour{
    [SerializeField] specifiUI_type type;
    void Start(){
        if(GameRules.instance!=null)StartCoroutine(Check());
        else Destroy(this);
    }

    IEnumerator Check(){
        if(type==specifiUI_type.energy&&!GameRules.instance.energyOnPlayer)Destroy(gameObject);
        else if(type==specifiUI_type.crystals&&!GameRules.instance.crystalsOn)Destroy(gameObject);
        else if(type==specifiUI_type.xp&&!GameRules.instance.xpOn)Destroy(gameObject);
        else if(type==specifiUI_type.shop&&!GameRules.instance.shopOn)Destroy(gameObject);
        else if(type==specifiUI_type.modules&&!GameRules.instance.modulesOn)Destroy(gameObject);
        else if(type==specifiUI_type.statUpgs&&!GameRules.instance.statUpgOn)Destroy(gameObject);
        else if(type==specifiUI_type.inventory&&!GameRules.instance.iteminvOn)Destroy(gameObject);
        else if(type==specifiUI_type.leveling&&!GameRules.instance.levelingOn)Destroy(gameObject);
        else if(type==specifiUI_type.cores&&!GameRules.instance.coresOn)Destroy(gameObject);
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(Check());
    }
}

enum specifiUI_type{
    energy,crystals,xp,shop,modules,statUpgs,inventory,leveling,cores
}