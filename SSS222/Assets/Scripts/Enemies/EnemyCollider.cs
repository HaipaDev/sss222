using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class EnemyCollider : MonoBehaviour{
    public List<colliTypes> collisionTypes=UniCollider.colliTypesForEn;
    void OnTriggerEnter2D(Collider2D other){Enemy en=GetComponent<Enemy>();  if(!other.CompareTag(tag)){if(other.GetComponent<Player>()==null&&other.GetComponent<Tag_Collectible>()==null&&other.GetComponent<Shredder>()==null){
            float dmg=0f;int armorPenetr=0;
            DamageValues dmgVal=UniCollider.GetDmgVal(other.gameObject.name);
            if(dmgVal!=null){
                dmg=UniCollider.TriggerCollision(other,transform,collisionTypes);
                armorPenetr=UniCollider.GetArmorPenetr(dmgVal.armorPenetr,en.defense);
            }

            if(other.gameObject.name.Contains(GameAssets.instance.Get("VLaser").name)||other.gameObject.name.Contains(GameAssets.instance.Get("HLaser").name)){GetComponent<Enemy>().Kill(false);return;}

            if(GetComponent<VortexWheel>()!=null){if(!other.gameObject.name.Contains("Rocket")){dmg/=3;}}//Do it more modular later on, resistance per weapontype etc
            if(dmgVal!=null)if(dmgVal.colliType==colliTypes.playerWeapons||dmgVal.colliType==colliTypes.player)if(Player.instance!=null)dmg*=Player.instance.dmgMulti;
            if(dmg!=0){
                if(dmg>0)dmg=CalculateDmg(dmg,armorPenetr);
                en.Damage(dmg);
                UniCollider.DMG_VFX(0,other,transform,dmg);
            }
    }}}
    void OnTriggerStay2D(Collider2D other){Enemy en=GetComponent<Enemy>();  if(!other.CompareTag(tag)){if(other.GetComponent<Player>()==null&&other.GetComponent<Tag_Collectible>()==null&&other.GetComponent<Shredder>()==null){
        if(other.GetComponent<Tag_DmgPhaseFreq>()!=null){var dmgPhaseFreq=other.GetComponent<Tag_DmgPhaseFreq>();if(dmgPhaseFreq.phaseTimer<=0){
            if(dmgPhaseFreq.phaseTimer!=-4&&(dmgPhaseFreq.phaseCount<=dmgPhaseFreq.phaseCountLimit||dmgPhaseFreq.phaseCountLimit==0)){
                float dmg=0f;int armorPenetr=0;
                DamageValues dmgVal=UniCollider.GetDmgVal(other.gameObject.name);
                if(dmgVal!=null){
                    dmg=UniCollider.TriggerCollision(other,transform,collisionTypes,true);
                    armorPenetr=UniCollider.GetArmorPenetr(dmgVal.armorPenetr,en.defense);
                }

                if(other.gameObject.name.Contains(GameAssets.instance.Get("VLaser").name)||other.gameObject.name.Contains(GameAssets.instance.Get("HLaser").name)){GetComponent<Enemy>().Kill(false);return;}

                if(dmgVal!=null)if(dmgVal.colliType==colliTypes.playerWeapons||dmgVal.colliType==colliTypes.player)if(Player.instance!=null)dmg*=Player.instance.dmgMulti;
                if(dmg!=0){
                    if(dmg>0)dmg=CalculateDmg(dmg,armorPenetr,true);
                    en.Damage(dmg);
                    UniCollider.DMG_VFX(1,other,transform,dmg);
                }
            }
            dmgPhaseFreq.SetTimer();
        }}
    }}}
    void OnTriggerExit2D(Collider2D other){
        if(other.GetComponent<Tag_DmgPhaseFreq>()!=null){other.GetComponent<Tag_DmgPhaseFreq>().ResetTimer();}
    }

    float CalculateDmg(float dmgVal,int armorPenetrVal,bool phase=false){
        Enemy en=GetComponent<Enemy>();float dmg=dmgVal;
        int def=en.defense;int armorPenetr=armorPenetrVal;float defMulti=0.5f;
        if(!phase){if(!GameRules.instance.enemyDefenseHit){def=0;armorPenetr=0;}}
        else{if(!GameRules.instance.enemyDefensePhase||!en.defenseOnPhase){def=0;armorPenetr=0;}defMulti=0.2f;}
        float totalDef=Mathf.Clamp((Mathf.Clamp((def-armorPenetr)*defMulti,0,999)),0,99999f);
        if(dmg>totalDef)dmg-=totalDef;
        return dmg;
    }
}