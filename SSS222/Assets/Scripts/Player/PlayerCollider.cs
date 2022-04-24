using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class PlayerCollider : MonoBehaviour{
    [Header("Other")]
    public float dmgTimer;
    public List<colliTypes> colliTypes=UniCollider.colliTypesForPl;
    
    string lastHitName;
    float lastHp;
    float lastHitDmg;
    bool lastHitPhasing;

    Player player;
    GameRules gr;
    void Start(){player=Player.instance;gr=GameRules.instance;}
    void OnTriggerEnter2D(Collider2D other){    lastHp=player.health;
    if(!other.CompareTag(tag)&&(player.collidedId==GetInstanceID()||player.collidedIdChangeTime<=0)){
            float dmg=0;int armorPenetr=0;
            if(player.collidedIdChangeTime<=0){player.collidedId=GetInstanceID();player.collidedIdChangeTime=0.33f;}

            #region//Enemies, World etc
            if(!other.gameObject.CompareTag("Powerups")){
                DamageValues dmgVal=UniCollider.GetDmgVal(other.gameObject.name);
                if(dmgVal!=null){
                    dmg=UniCollider.TriggerCollision(other,transform,colliTypes);
                    armorPenetr=UniCollider.GetArmorPenetr(dmgVal.armorPenetr,player.defense);
                    if(player.dashing==false)PlayerEffects(other.gameObject.name);
                }
                if(dmg>0){dmg=CalculateDmg(dmg,armorPenetr);}
                else if(dmg<0){player.Damage(dmg,dmgType.heal);}
                if(!other.gameObject.name.Contains(GameAssets.instance.Get("VLaser").name)&&!other.gameObject.name.Contains(GameAssets.instance.Get("HLaser").name)){
                    Enemy en=other.GetComponent<Enemy>();
                    if(player.dashing==false){
                        if(dmg!=0&&!player.gclover){player.Damage(dmg,dmgType.normal);AudioManager.instance.Play("ShipHit");}
                        else if(dmg!=0&&player.gclover){AudioManager.instance.Play("GCloverHit");}
                        GameAssets.instance.VFX("FlareHit",new Vector2(other.transform.position.x,transform.position.y+0.5f),0.3f);
                        if(dmgVal!=null){if(!dmgVal.phase)if(en!=null)en.Kill(false);}
                    }
                    else if(player.shadow==true&&player.dashing==true){
                        if(en!=null){if(en.killOnDash){en.Kill();}else{float dmgS=UniCollider.GetDmgValAbs("Shadowdash").dmg;en.health-=dmgS;UniCollider.DMG_VFX(0,other,other.transform,dmgS);}}
                   }
                }else{
                    if(dmg!=0&&!player.gclover){player.Damage(dmg,dmgType.normal);AudioManager.instance.Play("ShipHit");}
                    else if(dmg!=0&&player.gclover){AudioManager.instance.Play("GCloverHit");}
                }
            }
            #endregion
            #region//Powerups
            else if(other.gameObject.CompareTag("Powerups")){
                if(other.gameObject.name.Contains(GameAssets.instance.Get("EnBall").name)){player.AddSubEnergy(gr.energyBall_energyGain,true);}
                if(other.gameObject.name.Contains(GameAssets.instance.Get("Battery").name)){player.AddSubEnergy(gr.battery_energyGain,true);}
                if(other.gameObject.name.Contains(GameAssets.instance.Get("Coin").name)){player.AddSubCoins(gr.crystalGain,true);}
                if(other.gameObject.name.Contains(GameAssets.instance.Get("CoinB").name)){player.AddSubCoins(gr.crystalBigGain,true);}
                if(other.gameObject.name.Contains(GameAssets.instance.Get("PowerCore").name)){player.AddSubCores(gr.coresCollectGain);}
                if(other.gameObject.name.Contains(GameAssets.instance.Get("CelestBall").name)){player.AddSubXP(gr.benergyBallGain);}
                if(other.gameObject.name.Contains(GameAssets.instance.Get("CelestVial").name)){player.AddSubXP(gr.benergyVialGain);}
                if(other.GetComponent<Tag_Collectible>().isPowerup){//if((!other.gameObject.name.Contains(enBallName)) && (!other.gameObject.name.Contains(CoinName)) && (!other.gameObject.name.Contains(powercoreName))){
                    spawnReqsMono.AddPwrups(other.gameObject.name);
                    StatsAchievsManager.instance.AddPowerups();
                    GameSession.instance.AddXP(gr.xp_powerup);//XP For powerups
                }
                if(other.gameObject.name.Contains(GameAssets.instance.Get("LunarGel").name)){if(gr.lunarGel_absorp){HPAbsorp(GameRules.instance.lunarGel_hpGain);}else{HPAdd(GameRules.instance.lunarGel_hpGain);}}
                if(other.gameObject.name.Contains(GameAssets.instance.Get("HealBeam").name)){
                    HealBeam hb=other.GetComponent<HealBeam>();
                    if(hb.absorp)HPAbsorp(hb.value);
                    else HPAdd(hb.value);
                }

                if(other.gameObject.name.Contains(GameAssets.instance.Get("medkitPwrup").name)){
                    if(!SaveSerial.instance.settingsData.autoUseMedkitsIfLow){player.AddItem("medkit");}
                    else if(SaveSerial.instance.settingsData.autoUseMedkitsIfLow&&player.health<=(player.healthMax-GameRules.instance.medkit_hpGain))player.MedkitUse();
                }
                if(other.gameObject.name.Contains(GameAssets.instance.Get("medkitCPwrup").name)){
                    if(player.health>=player.healthMax){GameSession.instance.AddToScoreNoEV(25);}
                    else{HPAdd(GameRules.instance.medkit_hpGain);}
                }
                
                if(other.gameObject.name.Contains(GameAssets.instance.Get("flipPwrup").name)) {
                    PwrupEnergyAdd();
                    if(player.flip==true){PwrupEnergyAddDupl();}
                    player.SetStatus("flip"); 
                }
                if(other.gameObject.name.Contains(GameAssets.instance.Get("inverterPwrup").name)){
                    if(player.energyOn){
                        lastHitDmg=player.health;
                        var tempHP=player.health; var tempEn=player.energy;
                        player.energy=tempHP; player.health=tempEn;
                    }
                    player.SetStatus("inverter"); player.inverterTimer=0;
                }
                if(other.gameObject.name.Contains(GameAssets.instance.Get("magnetPwrup").name)){
                    PwrupEnergyAdd();
                    if(player.magnet==true){PwrupEnergyAddDupl();}
                    player.SetStatus("magnet");
                }
                if(other.gameObject.name.Contains(GameAssets.instance.Get("scalerPwrup").name)){
                    PwrupEnergyAdd();
                    if(player.scaler==true){PwrupEnergyAddDupl();}
                    player.SetStatus("scaler");
                    player.shipScale=player.shipScaleDefault*player.scalerSizes[UnityEngine.Random.Range(0,player.scalerSizes.Length-1)];
                }
                if(other.gameObject.name.Contains(GameAssets.instance.Get("gcloverPwrup").name)){
                    player.SetStatus("gclover");
                    GameSession.instance.MultiplyScore(1.25f);
                    player.energy=player.energyMax;
                    GameAssets.instance.VFX("GCloverOutVFX", Vector2.zero,1f);
                }
                if(other.gameObject.name.Contains(GameAssets.instance.Get("shadowdashPwrup").name)||other.gameObject.name.Contains(GameAssets.instance.Get("shadowtracesPwrup").name)){
                    if(other.gameObject.name.Contains(GameAssets.instance.Get("shadowtracesPwrup").name)){
                        if(!player.shadow){player.SetSpeedPrev();player.moveSpeedCurrent*=player.shadowtracesSpeed;}
                    }
                    PwrupEnergyAdd();
                    if(player.shadow==true){PwrupEnergyAddDupl();}
                    player.SetStatus("shadow");
                    player.shadowed=true;
                }
                if(other.gameObject.name.Contains(GameAssets.instance.Get("assassinPwrup").name)){
                    PwrupEnergyAdd();
                    player.Speed(13,1.4f);
                    player.Power(13,1.2f);
                    player.Fragile(13,1.2f);
                }
                if(other.gameObject.name.Contains(GameAssets.instance.Get("tankPwrup").name)){
                    PwrupEnergyAdd();
                    player.Slow(13,1.4f);
                    player.Armor(13,1.2f);
                }
                if(other.gameObject.name.Contains(GameAssets.instance.Get("overwritePwrup").name)){
                    PwrupEnergyAdd();
                    player.Hack(13);
                    player.InfEnergy(13);
                    player.GetComponent<PlayerSkills>().ResetSkillCooldowns();
                }
                if(other.gameObject.name.Contains(GameAssets.instance.Get("matrixPwrup").name)){player.SetStatus("matrix");}
                if(other.gameObject.name.Contains(GameAssets.instance.Get("accelPwrup").name)){player.SetStatus("accel");}
                
                if(other.gameObject.name.Contains(GameAssets.instance.Get("randomizerPwrup").name)){
                    var item=other.GetComponent<LootTable>().GetItem();
                    Instantiate(item.gameObject,new Vector2(other.transform.position.x,other.transform.position.y),Quaternion.identity);
                    Destroy(other.gameObject,0.01f);
                }

                //Weapons
                PowerupItem w=null;w=System.Array.Find(GameAssets.instance.powerupItems,x=>other.gameObject.name.Contains(GameAssets.instance.Get(x.assetName).name)&&x.powerupType==powerupType.weapon);
                if(w!=null){PowerupCollect(w.name);}
                
                //Other statuses
                if(other.gameObject.name.Contains(GameAssets.instance.Get("firePwrup").name)){player.OnFire(10);}
                if(other.gameObject.name.Contains(GameAssets.instance.Get("decayPwrup").name)){player.Decay(10);}
                if(other.gameObject.name.Contains(GameAssets.instance.Get("blindPwrup").name)){player.Blind(10,4);}
                if(other.gameObject.name.Contains(GameAssets.instance.Get("electrcPwrup").name)){player.Electrc(10);}
                if(other.gameObject.name.Contains(GameAssets.instance.Get("frozenPwrup").name)){player.Freeze(10);}
                if(other.gameObject.name.Contains(GameAssets.instance.Get("fragilePwrup").name)){player.Fragile(10);}
                if(other.gameObject.name.Contains(GameAssets.instance.Get("armoredPwrup").name)){player.Armor(10);}
                if(other.gameObject.name.Contains(GameAssets.instance.Get("powerPwrup").name)){player.Power(10);}
                if(other.gameObject.name.Contains(GameAssets.instance.Get("weaknsPwrup").name)){player.Weaken(10);}
                if(other.gameObject.name.Contains(GameAssets.instance.Get("hackedPwrup").name)){player.Hack(10);}
                if(other.gameObject.name.Contains(GameAssets.instance.Get("infEnergyPwrup").name)){player.InfEnergy(10);}
                if(other.gameObject.name.Contains(GameAssets.instance.Get("speedPwrup").name)){player.Speed(10);}
                if(other.gameObject.name.Contains(GameAssets.instance.Get("slowPwrup").name)){player.Slow(10);}


                void HPAdd(float hp){player.HPAdd(hp);UniCollider.DMG_VFX(2,other,transform,-hp);}
                void HPAbsorp(float hp){player.HPAbsorp(hp);UniCollider.DMG_VFX(4,other,player.transform,hp);}
                

                if(other.gameObject.name.Contains(GameAssets.instance.Get("EnBall").name)){AudioManager.instance.Play("EnergyBall");}
                else if(other.gameObject.name.Contains(GameAssets.instance.Get("Coin").name)){AudioManager.instance.Play("Coin");}
                else if(other.gameObject.name.Contains(GameAssets.instance.Get("PowerCore").name)){AudioManager.instance.Play("CoreCollect");}
                else if(other.gameObject.name.Contains(GameAssets.instance.Get("CelestBall").name)||other.gameObject.name.Contains(GameAssets.instance.Get("CelestVial").name)){AudioManager.instance.Play("CelestBall");}
                else if(other.gameObject.name.Contains(GameAssets.instance.Get("HealBeam").name)){AudioManager.instance.Play("Heal");}

                else if(other.gameObject.name.Contains(GameAssets.instance.Get("gcloverPwrup").name)){AudioManager.instance.Play("GClover");}
                else if(other.gameObject.name.Contains(GameAssets.instance.Get("shadowBtPwrup").name)){AudioManager.instance.Play("ShadowGet");}
                else if(other.gameObject.name.Contains(GameAssets.instance.Get("matrixPwrup").name)){AudioManager.instance.Play("MatrixGet");}
                else if(other.gameObject.name.Contains(GameAssets.instance.Get("accelPwrup").name)){AudioManager.instance.Play("AccelGet");}
                else{AudioManager.instance.Play("Powerup");}
                Destroy(other.gameObject, 0.05f);
            }
            #endregion
            string hitName="";
            if((dmg!=0||other.gameObject.name.Contains(GameAssets.instance.Get("inverterPwrup").name)||other.gameObject.name.Contains("Zone_"))&&!player.gclover){hitName=other.gameObject.name;
                if(hitName.Contains("(Clone)"))hitName=hitName.Replace("(Clone)","");lastHitName=hitName;lastHitDmg=dmg;lastHitPhasing=false;}
            if(hitName.Contains("Zone_")){hitName=hitName.Split('_')[0];lastHitName=hitName;}
            UniCollider.DMG_VFX(2,other,transform,dmg);
    }
    }
    public void PowerupCollect(string name){
        player.SetPowerupStr(name);
        PwrupEnergyAdd();
        var w=player.GetWeaponProperty(name);
        if(w!=null){
            if(w.costType==costType.energy){if(player.ContainsPowerup(name)){PwrupEnergyAddDupl();}}
            else if(w.costType==costType.ammo){AmmoAdd(w);}
       }else{Debug.LogWarning("WeaponProperty by name "+name+" does not exist");}
    }
    void PwrupEnergyAdd(){if(player.energy<=GameRules.instance.powerups_energyNeeded)player.AddSubEnergy(GameRules.instance.powerups_energyGain,true);}
    void PwrupEnergyAddDupl(){player.AddSubEnergy(GameRules.instance.powerups_energyDupl,true);}
    void AmmoAdd(WeaponProperties w){costTypeAmmo wc=(costTypeAmmo)w.costTypeProperties;player.AddSubAmmo(wc.ammoSize,w.name,true);}


    void OnTriggerStay2D(Collider2D other){     lastHp=player.health;
    if(!other.CompareTag(tag)&&(player.collidedId==GetInstanceID()||player.collidedIdChangeTime<=0)){
        if(other.GetComponent<Tag_DmgPhaseFreq>()!=null){var dmgPhaseFreq=other.GetComponent<Tag_DmgPhaseFreq>();if(dmgPhaseFreq.phaseTimer<=0){
            if(dmgPhaseFreq.phaseTimer!=-4&&(dmgPhaseFreq.phaseCount<=dmgPhaseFreq.phaseCountLimit||dmgPhaseFreq.phaseCountLimit==0)){
                if(player.collidedIdChangeTime<=0){player.collidedId=GetInstanceID();player.collidedIdChangeTime=0.33f;}
                float dmg=0;int armorPenetr=0;
                DamageValues dmgVal=UniCollider.GetDmgVal(other.gameObject.name);
                if(dmgVal!=null){
                    dmg=UniCollider.TriggerCollision(other,transform,colliTypes,true);
                    armorPenetr=UniCollider.GetArmorPenetr(dmgVal.armorPenetr,player.defense);
                    PlayerEffects(other.gameObject.name,true);
                }
                
                if(dmg>0){dmg=CalculateDmg(dmg,armorPenetr,true);player.Damage(dmg,dmgType.silent);}
                else if(dmg<0){player.Damage(dmg,dmgType.heal);}//?
                UniCollider.DMG_VFX(3,other,transform,dmg);

                string hitName="";
                if((dmg!=0||other.gameObject.name.Contains("Zone_"))&&!player.gclover){hitName=other.gameObject.name;
                    if(hitName.Contains("(Clone)"))hitName=hitName.Replace("(Clone)","");lastHitName=hitName;lastHitDmg=dmg;lastHitPhasing=true;}
                if(hitName.Contains("Zone_")){hitName=hitName.Split('_')[0];lastHitName=hitName;}
            }
            dmgPhaseFreq.SetTimer();
        }}
    }}

    void OnTriggerExit2D(Collider2D other){if(other.GetComponent<Tag_DmgPhaseFreq>()!=null){other.GetComponent<Tag_DmgPhaseFreq>().ResetTimer();}}
    float CalculateDmg(float dmgVal,int armorPenetrVal,bool phase=false){
        float dmg=dmgVal;
        int def=player.defense;int armorPenetr=armorPenetrVal;float defMulti=0.5f;
        if(phase){defMulti=0.2f;}
        float totalDef=Mathf.Clamp((Mathf.Clamp((def-armorPenetr)*defMulti,0,999)),0,99999f);
        dmg=Mathf.Clamp(dmg-=totalDef,0f,999999f);
        if(def==-1){dmg=0;}
        if(def<-1){dmg/=Mathf.Abs(def);}
        return (float)System.Math.Round(dmg,2);
    }
   void PlayerEffects(string goName,bool phase=false){
        DamageValues dmgVal=UniCollider.GetDmgVal(goName);  if(dmgVal!=null){
            if(colliTypes.Contains(dmgVal.colliType)){
                if(dmgVal.dmgFx){
                    foreach(DmgFxValues fx in dmgVal.dmgFxValues){
                        if((!phase||(phase&&fx.onPhase))&&(fx.chance>=Random.Range(0f,100f))){
                            if(fx.dmgFxType==dmgFxType.fire){player.OnFire(fx.length,fx.power);}
                            if(fx.dmgFxType==dmgFxType.decay){player.Decay(fx.length,fx.power);}
                            if(fx.dmgFxType==dmgFxType.electrc){player.Electrc(fx.length);}
                            if(fx.dmgFxType==dmgFxType.freeze){player.Freeze(fx.length);}
                            if(fx.dmgFxType==dmgFxType.armor){player.Armor(fx.length,fx.power);}
                            if(fx.dmgFxType==dmgFxType.fragile){player.Fragile(fx.length,fx.power);}
                            if(fx.dmgFxType==dmgFxType.power){player.Power(fx.length,fx.power);}
                            if(fx.dmgFxType==dmgFxType.weak){player.Weaken(fx.length,fx.power);}
                            if(fx.dmgFxType==dmgFxType.hack){player.Hack(fx.length);}
                            if(fx.dmgFxType==dmgFxType.blind){player.Blind(fx.length,fx.power);}
                            if(fx.dmgFxType==dmgFxType.speed){player.Speed(fx.length,fx.power);}
                            if(fx.dmgFxType==dmgFxType.slow){player.Slow(fx.length,fx.power);}
                            if(fx.dmgFxType==dmgFxType.infenergy){player.InfEnergy(fx.length);}
                        }
                    }
                }
            }
        }
   }
   public string _LastHitName(){return lastHitName;}
   public float _LastHp(){return lastHp;}
   public float _LastHitDmg(){return lastHitDmg;}
   public bool _LastHitPhasing(){return lastHitPhasing;}
}