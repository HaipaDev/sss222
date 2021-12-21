using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class EnemyCollider : MonoBehaviour{
    [DisableInPlayMode]public List<colliTypes> collisionTypes=UniCollider.colliTypesForEn;
    void OnTriggerEnter2D(Collider2D other){
        if(!other.CompareTag(tag)){
            float dmg=0;
            DamageValues dmgVal=UniCollider.GetDmgVal(other.gameObject.name);
            if(dmgVal!=null)dmg=UniCollider.TriggerCollision(other,transform,collisionTypes);

            if(other.gameObject.name.Contains(GameAssets.instance.Get("VLaser").name)||other.gameObject.name.Contains(GameAssets.instance.Get("HLaser").name)){GetComponent<Enemy>().Kill(false);return;}

            if(GetComponent<VortexWheel>()!=null){if(!other.gameObject.name.Contains("Rocket")){dmg/=3;}}//Do it more modular later on, resistance per weapontype etc
            if(dmgVal!=null)if(dmgVal.colliType==colliTypes.playerWeapons||dmgVal.colliType==colliTypes.player)if(Player.instance!=null)dmg*=Player.instance.dmgMulti;
            if(dmg!=0){
                GetComponent<Enemy>().health-=dmg;
                UniCollider.DMG_VFX(0,other,transform,dmg);
            }
            
        }
    }
    void OnTriggerStay2D(Collider2D other){     if(!other.CompareTag(tag)){
        if(other.GetComponent<Tag_DmgPhaseFreq>()!=null){var dmgPhaseFreq=other.GetComponent<Tag_DmgPhaseFreq>();if(dmgPhaseFreq.phaseTimer<=0){
            if(dmgPhaseFreq.phaseTimer!=-4&&(dmgPhaseFreq.phaseCount<=dmgPhaseFreq.phaseCountLimit||dmgPhaseFreq.phaseCountLimit==0)){
                float dmg=0;
                DamageValues dmgVal=UniCollider.GetDmgVal(other.gameObject.name);
                if(dmgVal!=null)dmg=UniCollider.TriggerCollision(other,transform,collisionTypes,true);

                if(other.gameObject.name.Contains(GameAssets.instance.Get("VLaser").name)||other.gameObject.name.Contains(GameAssets.instance.Get("HLaser").name)){GetComponent<Enemy>().Kill(false);return;}

                if(dmgVal!=null)if(dmgVal.colliType==colliTypes.playerWeapons||dmgVal.colliType==colliTypes.player)if(Player.instance!=null)dmg*=Player.instance.dmgMulti;
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