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
    void OnTriggerEnter2D(Collider2D other){    lastHp=(float)System.Math.Round(player.health,2);
    if(!other.CompareTag(tag)&&(player.collidedId==GetInstanceID()||player.collidedIdChangeTime<=0)){
            float dmg=0;int armorPenetr=0;
            if(player.collidedIdChangeTime<=0){player.collidedId=GetInstanceID();player.collidedIdChangeTime=0.33f;}

            #region//Enemies, World etc
            if(!other.gameObject.CompareTag("Collectibles")){
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
                        if(dmg!=0&&!player._hasStatus("gclover")){player.Damage(dmg,dmgType.normal);AudioManager.instance.Play("ShipHit");ThornsHit();}
                        else if(dmg!=0&&player._hasStatus("gclover")){AudioManager.instance.Play("GCloverHit");}
                        GameAssets.instance.VFX("FlareHit",new Vector2(other.transform.position.x,transform.position.y+0.5f),0.3f);
                        if(dmgVal!=null){if(!dmgVal.phase)if(en!=null)en.Kill(false);}
                    }
                    else if(player._hasStatus("shadowdash")&&player.dashing==true){
                        if(en!=null){if(en.killOnDash){en.Kill(explode:false);}else{float dmgS=UniCollider.GetDmgValAbs("Shadowdash").dmg;en.health-=dmgS;UniCollider.DMG_VFX(0,other,other.transform,dmgS);}}
                   }
                }else{
                    if(dmg!=0&&!player._hasStatus("gclover")){player.Damage(dmg,dmgType.normal);AudioManager.instance.Play("ShipHit");}
                    else if(dmg!=0&&player._hasStatus("gclover")){AudioManager.instance.Play("GCloverHit");}
                }
            }
            #endregion
            #region//Powerups
            else if(other.gameObject.CompareTag("Collectibles")){
                if(other.gameObject.name.Contains(GameAssets.instance.Get("EnBall").name)){
                    player.AddSubEnergy(gr.energyBall_energyGain,true);
                    if(GetComponent<PlayerModules>()!=null){
                        if(GetComponent<PlayerModules>()._isModuleEquipped("EnBurst")){
                            if(!GetComponent<PlayerModules>()._isModuleLvl("EnBurst",2)){
                                if(GameAssets.CheckChance(20f)){player.ShootRadialBullets("EnBurstBall",4,7f);}//Lvl 1
                            }else{
                                if(GameAssets.CheckChance(35f)){player.ShootRadialBullets("EnBurstBall",5,8f);}//Lvl 2+
                            }
                        }
                    }
                }
                if(other.gameObject.name.Contains(GameAssets.instance.Get("Battery").name)){player.AddSubEnergy(gr.battery_energyGain,true);}
                if(other.gameObject.name.Contains(GameAssets.instance.Get("Coin").name)){player.AddSubCoins(gr.crystalGain,true);}
                if(other.gameObject.name.Contains(GameAssets.instance.Get("CoinB").name)){player.AddSubCoins(gr.crystalBigGain,true);}
                if(other.gameObject.name.Contains(GameAssets.instance.Get("PowerCore").name)){player.AddSubCores(gr.coresCollectGain);StatsAchievsManager.instance.CoreCollected();}
                if(other.gameObject.name.Contains(GameAssets.instance.Get("CelestBall").name)){player.AddSubXP(gr.benergyBallGain);}
                if(other.gameObject.name.Contains(GameAssets.instance.Get("CelestVial").name)){player.AddSubXP(gr.benergyVialGain);}
                if(other.gameObject.name.Contains(GameAssets.instance.Get("PlayerHolobody").name)){
                    player.AddSubCoins(other.GetComponent<PlayerHolobody>().crystalsStored,true);
                    HPAdd(GameRules.instance.holobodyHeal);
                    if(other.GetComponent<PlayerHolobody>().powerupStored!=null)player.SetPowerup(other.GetComponent<PlayerHolobody>().powerupStored);
                }
                if(other.GetComponent<Tag_Collectible>().isPowerup){//if((!other.gameObject.name.Contains(enBallName)) && (!other.gameObject.name.Contains(CoinName)) && (!other.gameObject.name.Contains(powercoreName))){
                    spawnReqsMono.AddPwrups(other.gameObject.name);
                    if(!GameSession.instance._isSandboxMode())StatsAchievsManager.instance.AddPowerups(other.gameObject.name);
                    GameSession.instance.AddXP(gr.xp_powerup);//XP For powerups
                }
                if(other.gameObject.name.Contains(GameAssets.instance.Get("LunarGel").name)){if(gr.lunarGel_absorp){HPAbsorp(GameRules.instance.lunarGel_hpGain);}else{HPAdd(GameRules.instance.lunarGel_hpGain);}}
                if(other.gameObject.name.Contains(GameAssets.instance.Get("HealBeam").name)){
                    HealBeam hb=other.GetComponent<HealBeam>();
                    if(hb.absorp)HPAbsorp(hb.value);
                    else HPAddSilent(hb.value);
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
                    if(player._hasStatus("flip")){PwrupEnergyAddDupl();}
                    player.SetStatus("flip",player.flipTime,GameRules.instance.statusCapDefault); 
                }
                if(other.gameObject.name.Contains(GameAssets.instance.Get("inverterPwrup").name)){
                    if(player.energyOn){
                        lastHitDmg=(float)System.Math.Round(player.health,2);
                        var tempHP=player.health; var tempEn=player.energy;
                        player.energy=tempHP; player.health=tempEn;
                    }
                    player.SetStatus("inverter",0);
                }
                if(other.gameObject.name.Contains(GameAssets.instance.Get("magnetPwrup").name)){
                    PwrupEnergyAdd();
                    if(player._hasStatus("magnet")){PwrupEnergyAddDupl();}
                    player.SetStatus("magnet",player.magnetTime,GameRules.instance.statusCapDefault);
                }
                if(other.gameObject.name.Contains(GameAssets.instance.Get("scalerPwrup").name)){
                    PwrupEnergyAdd();
                    if(player._hasStatus("scaler")){PwrupEnergyAddDupl();}
                    player.SetStatus("scaler",player.scalerTime,GameRules.instance.statusCapDefault);
                    player.shipScale=player.shipScaleDefault*player.scalerSizes[UnityEngine.Random.Range(0,player.scalerSizes.Length-1)];
                }
                if(other.gameObject.name.Contains(GameAssets.instance.Get("gcloverPwrup").name)){
                    player.SetStatus("gclover",player.gcloverTime,GameRules.instance.statusCapDefault);
                    GameSession.instance.MultiplyScore(1.25f);
                    player.energy=player.energyMax;
                    GameAssets.instance.VFX("GCloverOutVFX", Vector2.zero,1f);
                }
                if(other.gameObject.name.Contains(GameAssets.instance.Get("shadowdashPwrup").name)){
                    PwrupEnergyAdd();
                    if(player._hasStatus("shadowdash")){PwrupEnergyAddDupl();}
                    player.SetStatus("shadowdash",player.shadowTime,GameRules.instance.statusCapDefault);
                    player.shadowed=true;
                }
                if(other.gameObject.name.Contains(GameAssets.instance.Get("shadowtracesPwrup").name)){
                    if(!player._hasStatus("shadowtracesPwrup")){player.SetSpeedPrev();player.moveSpeedCurrent*=player.shadowtracesSpeed;}
                    player.SetStatus("shadowtraces",player.shadowTime,GameRules.instance.statusCapDefault);
                    player.shadowed=true;
                }
                if(other.gameObject.name.Contains(GameAssets.instance.Get("assassinPwrup").name)){
                    PwrupEnergyAdd();
                    player.Speed(13,GameRules.instance.statusCapDefault,1.4f);
                    player.Power(13,GameRules.instance.statusCapDefault,1.2f);
                    player.Fragile(13,GameRules.instance.statusCapDefault,1.2f);
                }
                if(other.gameObject.name.Contains(GameAssets.instance.Get("tankPwrup").name)){
                    PwrupEnergyAdd();
                    player.Slow(13,GameRules.instance.statusCapDefault,1.4f);
                    player.Armor(13,GameRules.instance.statusCapDefault,1.2f);
                }
                if(other.gameObject.name.Contains(GameAssets.instance.Get("overwritePwrup").name)){
                    PwrupEnergyAdd();
                    player.Hack(13,GameRules.instance.statusCapDefault);
                    player.InfEnergy(13,GameRules.instance.statusCapDefault);
                    player.GetComponent<PlayerModules>().ResetSkillCooldowns();
                }
                if(other.gameObject.name.Contains(GameAssets.instance.Get("matrixPwrup").name)){player.SetStatus("matrix",player.matrixTime);}
                if(other.gameObject.name.Contains(GameAssets.instance.Get("accelPwrup").name)){player.SetStatus("accel",player.accelTime);}
                if(other.gameObject.name.Contains(GameAssets.instance.Get("noHealPwrup").name)){player.SetStatus("noHeal",player.noHealTime);}
                if(other.gameObject.name.Contains(GameAssets.instance.Get("lifeStealPwrup").name)){player.SetStatus("lifeSteal",player.lifeStealTime);}
                if(other.gameObject.name.Contains(GameAssets.instance.Get("thornsPwrup").name)){player.SetStatus("thorns",player.thornsTime);}
                if(other.gameObject.name.Contains(GameAssets.instance.Get("fuelPwrup").name)){player.SetStatus("fuel",player.fuelTime);}
                
                if(other.gameObject.name.Contains(GameAssets.instance.Get("randomizerPwrup").name)){
                    var item=other.GetComponent<LootTable>().GetItem();
                    Instantiate(item.gameObject,new Vector2(other.transform.position.x,other.transform.position.y),Quaternion.identity);
                    Destroy(other.gameObject,0.01f);
                }

                //Weapons
                PowerupItem w=null;w=GameAssets.instance.powerupItems.Find(x=>other.gameObject.name.Contains(GameAssets.instance.Get(x.assetName).name)&&x.powerupType==powerupType.weapon);
                if(w!=null){PowerupCollect(w.name);}
                
                //Other statuses
                if(other.gameObject.name.Contains(GameAssets.instance.Get("onfirePwrup").name)){player.OnFire(10,GameRules.instance.statusCapDefault);}
                if(other.gameObject.name.Contains(GameAssets.instance.Get("decayPwrup").name)){player.Decay(10,GameRules.instance.statusCapDefault);}
                if(other.gameObject.name.Contains(GameAssets.instance.Get("blindPwrup").name)){player.Blind(10,GameRules.instance.statusCapDefault,4);}
                if(other.gameObject.name.Contains(GameAssets.instance.Get("electrcPwrup").name)){player.Electrc(10,GameRules.instance.statusCapDefault);}
                if(other.gameObject.name.Contains(GameAssets.instance.Get("frozenPwrup").name)){player.Freeze(10,GameRules.instance.statusCapDefault);}
                if(other.gameObject.name.Contains(GameAssets.instance.Get("fragilePwrup").name)){player.Fragile(10,GameRules.instance.statusCapDefault);}
                if(other.gameObject.name.Contains(GameAssets.instance.Get("armoredPwrup").name)){player.Armor(10,GameRules.instance.statusCapDefault);}
                if(other.gameObject.name.Contains(GameAssets.instance.Get("powerPwrup").name)){player.Power(10,GameRules.instance.statusCapDefault);}
                if(other.gameObject.name.Contains(GameAssets.instance.Get("weaknsPwrup").name)){player.Weaken(10,GameRules.instance.statusCapDefault);}
                if(other.gameObject.name.Contains(GameAssets.instance.Get("hackedPwrup").name)){player.Hack(10,GameRules.instance.statusCapDefault);}
                if(other.gameObject.name.Contains(GameAssets.instance.Get("infEnergyPwrup").name)){player.InfEnergy(10,GameRules.instance.statusCapDefault);}
                if(other.gameObject.name.Contains(GameAssets.instance.Get("speedPwrup").name)){player.Speed(10,GameRules.instance.statusCapDefault);}
                if(other.gameObject.name.Contains(GameAssets.instance.Get("slowPwrup").name)){player.Slow(10,GameRules.instance.statusCapDefault);}


                void HPAdd(float hp){player.HPAdd(hp);UniCollider.DMG_VFX(2,other,transform,-hp);}
                void HPAddSilent(float hp){player.HPAddSilent(hp);UniCollider.DMG_VFX(2,other,transform,-hp);}
                void HPAbsorp(float hp){player.HPAbsorp(hp);UniCollider.DMG_VFX(4,other,player.transform,hp);}
                

                if(other.gameObject.name.Contains(GameAssets.instance.Get("EnBall").name)){AudioManager.instance.Play("EnergyBall");}
                else if(other.gameObject.name.Contains(GameAssets.instance.Get("Coin").name)){AudioManager.instance.Play("Coin");}
                else if(other.gameObject.name.Contains(GameAssets.instance.Get("PowerCore").name)){AudioManager.instance.Play("CoreCollect");}
                else if(other.gameObject.name.Contains(GameAssets.instance.Get("CelestBall").name)||other.gameObject.name.Contains(GameAssets.instance.Get("CelestVial").name)){AudioManager.instance.Play("CelestBall");}
                else if(other.gameObject.name.Contains(GameAssets.instance.Get("HealBeam").name)){AudioManager.instance.Play("Heal");}
                else if(other.gameObject.name.Contains(GameAssets.instance.Get("PlayerHolobody").name)){AudioManager.instance.Play("HoloCollect");}

                else if(other.gameObject.name.Contains(GameAssets.instance.Get("gcloverPwrup").name)){AudioManager.instance.Play("GClover");}
                else if(other.gameObject.name.Contains(GameAssets.instance.Get("shadowBtPwrup").name)){AudioManager.instance.Play("ShadowGet");}
                else if(other.gameObject.name.Contains(GameAssets.instance.Get("matrixPwrup").name)){AudioManager.instance.Play("MatrixGet");}
                else if(other.gameObject.name.Contains(GameAssets.instance.Get("accelPwrup").name)){AudioManager.instance.Play("AccelGet");}
                else{AudioManager.instance.Play("Powerup");}
                Destroy(other.gameObject, 0.05f);
            }
            #endregion
            string hitName="";
            if((dmg!=0||other.gameObject.name.Contains(GameAssets.instance.Get("inverterPwrup").name)||other.gameObject.name.Contains("Zone_"))&&!player._hasStatus("gclover")){hitName=other.gameObject.name;
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
                if((dmg!=0||other.gameObject.name.Contains("Zone_"))&&!player._hasStatus("gclover")){hitName=other.gameObject.name;
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
        float totalDef=Mathf.Clamp((Mathf.Clamp((def-armorPenetr)*defMulti,0.1f,999)),0,99999f);
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
                        if((!phase||(phase&&fx.onPhase))&&(GameAssets.CheckChance(fx.chance))){
                            var _timerCap=fx.length*2;
                            if(fx.dmgFxType==dmgFxType.fire){player.OnFire(fx.length,_timerCap,fx.power);}
                            if(fx.dmgFxType==dmgFxType.decay){player.Decay(fx.length,_timerCap,fx.power);}
                            if(fx.dmgFxType==dmgFxType.electrc){player.Electrc(fx.length,_timerCap);}
                            if(fx.dmgFxType==dmgFxType.freeze){player.Freeze(fx.length,_timerCap);}
                            if(fx.dmgFxType==dmgFxType.armor){player.Armor(fx.length,_timerCap,fx.power);}
                            if(fx.dmgFxType==dmgFxType.fragile){player.Fragile(fx.length,_timerCap,fx.power);}
                            if(fx.dmgFxType==dmgFxType.power){player.Power(fx.length,_timerCap,fx.power);}
                            if(fx.dmgFxType==dmgFxType.weak){player.Weaken(fx.length,_timerCap,fx.power);}
                            if(fx.dmgFxType==dmgFxType.hack){player.Hack(fx.length,_timerCap);}
                            if(fx.dmgFxType==dmgFxType.blind){player.Blind(fx.length,_timerCap,fx.power);}
                            if(fx.dmgFxType==dmgFxType.speed){player.Speed(fx.length,_timerCap,fx.power);}
                            if(fx.dmgFxType==dmgFxType.slow){player.Slow(fx.length,_timerCap,fx.power);}
                            if(fx.dmgFxType==dmgFxType.infenergy){player.InfEnergy(fx.length,_timerCap);}
                        }
                    }
                }
            }
        }
   }
   void ThornsHit(){player.Thorns();}
   public string _LastHitName(){return lastHitName;}
   public float _LastHp(){return lastHp;}
   public float _LastHitDmg(){return lastHitDmg;}
   public bool _LastHitPhasing(){return lastHitPhasing;}
   public void SetLastHit(string name,float dmg, bool phasing=false){lastHp=player.health;lastHitName=name;lastHitDmg=dmg;lastHitPhasing=phasing;}
}