using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class EnemyCollider : MonoBehaviour{
    public List<colliTypes> collisionTypes=UniCollider.colliTypesForEn;
    Enemy en;
    void Start(){en=GetComponent<Enemy>();}
    void OnTriggerEnter2D(Collider2D other){  if(!other.CompareTag(tag)){if(other.GetComponent<Player>()==null&&other.GetComponent<Tag_Collectible>()==null&&other.GetComponent<Shredder>()==null){
            float dmg=0f;int armorPenetr=0;bool crit=false;
            DamageValues dmgVal=UniCollider.GetDmgVal(other.gameObject.name);
            if(dmgVal!=null){
                dmg=UniCollider.TriggerCollision(other,transform,collisionTypes);
                armorPenetr=UniCollider.GetArmorPenetr(dmgVal.armorPenetr,en.defense);
            }

            if(other.gameObject.name.Contains(AssetsManager.instance.Get("VLaser").name)||other.gameObject.name.Contains(AssetsManager.instance.Get("HLaser").name)){GetComponent<Enemy>().Kill(false);return;}

            
            if(dmgVal!=null){
                if(dmgVal.colliType==colliTypes.playerWeapons||dmgVal.colliType==colliTypes.player){
                    if(Player.instance!=null){dmg*=Player.instance.dmgMulti;if(Random.Range(0,100)<=Player.instance.critChance&&Player.instance.critChance>0)crit=true;}}
                if(dmg!=0){
                    if(!en._dmgHeals){if(dmg>0)dmg=CalculateDmg(dmgVal,dmg,armorPenetr,crit,false);}
                    else{dmg*=-1;}

                    en.Damage(dmg);
                    if(crit){UniCollider.DMG_VFX(0,other,transform,dmg,crit);}
                    else UniCollider.DMG_VFX(0,other,transform,dmg);
                }
                if(en._healable()){
                    if(Player.instance!=null){if(Player.instance._hasStatus("lifeSteal")){
                        HealBeam hb=Instantiate(AssetsManager.instance.Get("HealBeam"),(Vector2)transform.position,Quaternion.identity).GetComponent<HealBeam>();
                        hb.value=dmg/100*(Player.instance.GetStatus("lifeSteal").strength);
                        hb.absorp=false;
                    }}
                }
            }
            if(Player.instance!=null){if(Player.instance._costOnHitMelee)Player.instance.meleeCostTimer=0;}
    }}}
    void OnTriggerStay2D(Collider2D other){  if(!other.CompareTag(tag)){if(other.GetComponent<Player>()==null&&other.GetComponent<Tag_Collectible>()==null&&other.GetComponent<Shredder>()==null){
        if(other.GetComponent<Tag_DmgPhaseFreq>()!=null){var dmgPhaseFreq=other.GetComponent<Tag_DmgPhaseFreq>();if(dmgPhaseFreq.phaseTimer<=0){
            if(dmgPhaseFreq.phaseTimer!=-4&&(dmgPhaseFreq.phaseCount<=dmgPhaseFreq.phaseCountLimit||dmgPhaseFreq.phaseCountLimit==0)){
                float dmg=0f;int armorPenetr=0;bool crit=false;
                DamageValues dmgVal=UniCollider.GetDmgVal(other.gameObject.name);
                if(dmgVal!=null){
                    dmg=UniCollider.TriggerCollision(other,transform,collisionTypes,true);
                    armorPenetr=UniCollider.GetArmorPenetr(dmgVal.armorPenetr,en.defense);
                }

                if(other.gameObject.name.Contains(AssetsManager.instance.Get("VLaser").name)||other.gameObject.name.Contains(AssetsManager.instance.Get("HLaser").name)){GetComponent<Enemy>().Kill(false);return;}

            if(dmgVal!=null){
                if(dmgVal.colliType==colliTypes.playerWeapons||dmgVal.colliType==colliTypes.player){
                    if(Player.instance!=null){dmg*=Player.instance.dmgMulti;if(Random.Range(0,100)<=Player.instance.critChance&&Player.instance.critChance>0)crit=true;}}}
                if(dmg!=0){
                    if(dmg>0)dmg=CalculateDmg(dmgVal,dmg,armorPenetr,crit,true);
                    en.Damage(dmg);
                    if(crit){UniCollider.DMG_VFX(1,other,transform,dmg,crit);}
                    else UniCollider.DMG_VFX(1,other,transform,dmg);
                }
            }
            dmgPhaseFreq.SetTimer();
            if(Player.instance!=null){if(Player.instance._costOnPhaseMelee)Player.instance.meleeCostTimer=0;}
        }}
    }}}
    void OnTriggerExit2D(Collider2D other){
        if(other.GetComponent<Tag_DmgPhaseFreq>()!=null){other.GetComponent<Tag_DmgPhaseFreq>().ResetTimer();}
    }

    float CalculateDmg(DamageValues dmgVal,float _dmg,int armorPenetrVal,bool crit,bool phase=false){
        float dmg=_dmg;
        int def=en.defense;int armorPenetr=armorPenetrVal;float defMulti=0.5f;
        if(!phase){if(!GameRules.instance.enemyDefenseHit){def=0;armorPenetr=0;}}
        else{if(!GameRules.instance.enemyDefensePhase||!en.defenseOnPhase){def=0;armorPenetr=0;}defMulti=0.2f;}
        float totalDef=Mathf.Clamp((Mathf.Clamp((def-armorPenetr)*defMulti,0,999)),0,99999f);
        dmg=Mathf.Clamp(dmg-=totalDef,GameRules.instance.enemyDefenseFloor,999999f);
        if(def==-1){dmg=0;}
        if(def<-1){dmg/=Mathf.Abs(def);}
        if(crit){dmg*=2f;}
        return (float)System.Math.Round(dmg,2);
    }
}