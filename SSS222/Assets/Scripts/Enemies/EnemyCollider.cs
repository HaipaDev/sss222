﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollider : MonoBehaviour{
    [HideInInspector]public List<colliTypes> collisionTypes=UniCollider.colliTypesForEn;
    void OnTriggerEnter2D(Collider2D other){
        if(!other.CompareTag(tag)){
            if(other==Player.instance){if(Player.instance.dashing==true){GetComponent<Enemy>().Die();}else{return;}}

            float dmg=UniCollider.TriggerCollision(other,transform,collisionTypes);

            if(GetComponent<VortexWheel>()!=null){if(!other.gameObject.name.Contains("Rocket")){dmg/=3;}}//Do it more modular later on, resistance per weapontype etc
            if(Player.instance!=null)dmg*=Player.instance.dmgMulti;

            if(dmg!=0){
                GetComponent<Enemy>().health-=dmg;
                UniCollider.DMG_VFX(0,other,transform,dmg);
            }
        }else if(other.CompareTag(tag)){
            if(other.gameObject.name.Contains(GameAssets.instance.Get("HLaser").name)||other.gameObject.name.Contains(GameAssets.instance.Get("VLaser").name)){GetComponent<Enemy>().giveScore=false;GetComponent<Enemy>().Die();}
        }
    }
    void OnTriggerStay2D(Collider2D other){     if(!other.CompareTag(tag)){
        if(other.GetComponent<Tag_DmgPhaseFreq>()!=null){var dmgPhaseFreq=other.GetComponent<Tag_DmgPhaseFreq>();if(dmgPhaseFreq.phaseTimer<=0){
            if(dmgPhaseFreq.phaseTimer!=-4&&(dmgPhaseFreq.phaseCount<=dmgPhaseFreq.phaseCountLimit||dmgPhaseFreq.phaseCountLimit==0)){
                float dmg=UniCollider.TriggerCollision(other,transform,collisionTypes,true);

                if(Player.instance!=null)dmg*=Player.instance.dmgMulti;
                if(dmg!=0){
                    GetComponent<Enemy>().health-=dmg;
                    UniCollider.DMG_VFX(1,other,transform,dmg);
                }
            }
            dmgPhaseFreq.SetTimer();
        }}
    }}
    void OnTriggerExit2D(Collider2D other){
        if(other.GetComponent<Tag_DmgPhaseFreq>()!=null){other.GetComponent<Tag_DmgPhaseFreq>().ResetTimer();}
    }
}