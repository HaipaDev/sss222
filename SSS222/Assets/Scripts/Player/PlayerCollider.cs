using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class PlayerCollider : MonoBehaviour{
    [Header("Other")]
    public float dmgTimer;
    public string lastHitObj;
    public float lastHitDmg;
    public List<colliTypes> colliTypes=UniCollider.colliTypesForPl;

    Player player;
    void Start(){player=Player.instance;}
    void OnTriggerEnter2D(Collider2D other){
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
                if(other.gameObject.name.Contains(GameAssets.instance.Get("EnBall").name)){player.AddSubEnergy(player.energyBallGet,true);}
                if(other.gameObject.name.Contains(GameAssets.instance.Get("Coin").name)){player.AddSubCoins(player.crystalGet,true);}
                if(other.gameObject.name.Contains(GameAssets.instance.Get("CoinB").name)){player.AddSubCoins(player.crystalBGet,true);}
                if(other.gameObject.name.Contains(GameAssets.instance.Get("PowerCore").name)){player.AddSubCores(1);}
                if(other.gameObject.name.Contains(GameAssets.instance.Get("CelestBall").name)){player.AddSubXP(1);}
                if(other.gameObject.name.Contains(GameAssets.instance.Get("CelestVial").name)){player.AddSubXP(5);}
                if(other.gameObject.name.Contains(GameAssets.instance.Get("Battery").name)){player.AddSubEnergy(player.energyBatteryGet,true);}
                if(other.GetComponent<Tag_Collectible>().isPowerup){//if((!other.gameObject.name.Contains(enBallName)) && (!other.gameObject.name.Contains(CoinName)) && (!other.gameObject.name.Contains(powercoreName))){
                    spawnReqsMono.AddPwrups(other.gameObject.name);
                    StatsAchievsManager.instance.AddPowerups();
                    GameSession.instance.AddXP(GameRules.instance.xp_powerup);//XP For powerups
                }
                if(other.gameObject.name.Contains(GameAssets.instance.Get("LunarGel").name)){HPAbsorp(player.microMedkitHpAmnt);}
                if(other.gameObject.name.Contains(GameAssets.instance.Get("HealBeam").name)){
                    HealBeam hb=other.GetComponent<HealBeam>();
                    if(hb.absorp)HPAbsorp(hb.value);
                    else HPAdd(hb.value);
                }

                if(other.gameObject.name.Contains(GameAssets.instance.Get("MedkitPwrup").name)){
                    if(player.health>=player.healthMax){GameSession.instance.AddToScoreNoEV(Mathf.RoundToInt(player.medkitHpAmnt));}
                    else if(player.health!=player.healthMax&&player.health>(player.healthMax-player.medkitHpAmnt)){
                        int val=Mathf.RoundToInt(player.medkitHpAmnt-(player.healthMax-player.health));
                        if(val>0)GameSession.instance.AddToScoreNoEV(val);}
                    HPAdd(player.medkitHpAmnt);
                    player.AddSubEnergy(player.medkitEnergyGet,true);
                }
                if(other.gameObject.name.Contains(GameAssets.instance.Get("MedkitCPwrup").name)){
                    if(player.health>=player.healthMax){GameSession.instance.AddToScoreNoEV(25);}
                    else{HPAdd(player.medkitHpAmnt);}
                }
                
                if(other.gameObject.name.Contains(GameAssets.instance.Get("FlipPwrup").name)) {
                    if(player.energy<=player.enForPwrupRefill){EnergyAdd();}
                    if(player.flip==true){EnergyAddDupl();}
                    player.SetStatus("flip"); 
                }
                if(other.gameObject.name.Contains(GameAssets.instance.Get("InverterPwrup").name)){
                    if(player.energyOn){
                        lastHitDmg=player.health;
                        var tempHP=player.health; var tempEn=player.energy;
                        player.energy=tempHP; player.health=tempEn;
                    }
                    player.SetStatus("inverter"); player.inverterTimer=0;
                }
                if(other.gameObject.name.Contains(GameAssets.instance.Get("MagnetPwrup").name)){
                    if(player.energy<=player.enForPwrupRefill){EnergyAdd();}
                    if(player.magnet==true){EnergyAddDupl();}
                    player.SetStatus("magnet");
                }
                if(other.gameObject.name.Contains(GameAssets.instance.Get("ScalerPwrup").name)){
                    if(player.energy<=player.enForPwrupRefill){EnergyAdd();}
                    if(player.scaler==true){EnergyAddDupl();}
                    player.SetStatus("scaler");
                    player.shipScale=player.shipScaleDefault*player.scalerSizes[UnityEngine.Random.Range(0,player.scalerSizes.Length-1)];
                }
                if(other.gameObject.name.Contains(GameAssets.instance.Get("GCloverPwrup").name)){
                    player.SetStatus("gclover");
                    GameSession.instance.MultiplyScore(1.25f);
                    player.energy=player.energyMax;
                    GameAssets.instance.VFX("GCloverOutVFX", Vector2.zero,1f);
                }
                if(other.gameObject.name.Contains(GameAssets.instance.Get("ShadowPwrup").name)||other.gameObject.name.Contains(GameAssets.instance.Get("ShadowtracesPwrup").name)){
                    if(other.gameObject.name.Contains(GameAssets.instance.Get("ShadowtracesPwrup").name)){
                        if(!player.shadow){player.SetSpeedPrev();player.moveSpeedCurrent*=player.shadowtracesSpeed;}
                    }
                    if(player.energy<=player.enForPwrupRefill){player.AddSubEnergy(player.pwrupEnergyGet,true);}
                    if(player.shadow==true){EnergyAddDupl();}
                    player.SetStatus("shadow");
                    player.shadowed=true;
                    
                }
                if(other.gameObject.name.Contains(GameAssets.instance.Get("AssassinPwrup").name)){
                    if(player.energy<=player.enForPwrupRefill){player.AddSubEnergy(player.pwrupEnergyGet,true);}
                    player.Speed(13,1.4f);
                    player.Power(13,1.2f);
                    player.Fragile(13,1.2f);
                }
                if(other.gameObject.name.Contains(GameAssets.instance.Get("TankPwrup").name)){
                    if(player.energy<=player.enForPwrupRefill){player.AddSubEnergy(player.pwrupEnergyGet,true);}
                    player.Slow(13,1.4f);
                    player.Armor(13,1.2f);
                }
                if(other.gameObject.name.Contains(GameAssets.instance.Get("OverwritePwrup").name)){
                    if(player.energy<=player.enForPwrupRefill){player.AddSubEnergy(player.pwrupEnergyGet,true);}
                    player.Hack(13);
                    player.InfEnergy(13);
                    player.GetComponent<PlayerSkills>().ResetSkillCooldowns();
                }
                if(other.gameObject.name.Contains(GameAssets.instance.Get("MatrixPwrup").name)){player.SetStatus("matrix");}
                if(other.gameObject.name.Contains(GameAssets.instance.Get("AccelPwrup").name)){player.SetStatus("accel");}
                if(other.gameObject.name.Contains(GameAssets.instance.Get("PMultiPwrup").name)){player.SetStatus("pmulti");if(player.pmultiTimer<0){player.pmultiTimer=0;}player.pmultiTimer += player.pmultiTime; GameSession.instance.scoreMulti=2f;}
                
                if(other.gameObject.name.Contains(GameAssets.instance.Get("RandomizerPwrup").name)){
                    var item=other.GetComponent<LootTable>().GetItem();
                    Instantiate(item.gameObject,new Vector2(other.transform.position.x,other.transform.position.y),Quaternion.identity);
                    Destroy(other.gameObject,0.01f);
                }

                if(other.gameObject.name.Contains(GameAssets.instance.Get("Laser2Pwrup").name)){PowerupCollect("laser2");}
                if(other.gameObject.name.Contains(GameAssets.instance.Get("Laser3Pwrup").name)){PowerupCollect("laser3");}
                if(other.gameObject.name.Contains(GameAssets.instance.Get("PhaserPwrup").name)){PowerupCollect("phaser");}
                if(other.gameObject.name.Contains(GameAssets.instance.Get("HRocketPwrup").name)){PowerupCollect("hrocket");}
                if(other.gameObject.name.Contains(GameAssets.instance.Get("MLaserPwrup").name)){PowerupCollect("mlaser");}
                if(other.gameObject.name.Contains(GameAssets.instance.Get("LSaberPwrup").name)){PowerupCollect("lsaber");}
                if(other.gameObject.name.Contains(GameAssets.instance.Get("GloomyScythesPwrup").name)){PowerupCollect("gloomyScythes");}
                if(other.gameObject.name.Contains(GameAssets.instance.Get("ShadowBtPwrup").name)){PowerupCollect("shadowbt");}
                if(other.gameObject.name.Contains(GameAssets.instance.Get("QRocketPwrup").name)){PowerupCollect("qrocket");}
                if(other.gameObject.name.Contains(GameAssets.instance.Get("PRocketPwrup").name)){PowerupCollect("procket");}
                if(other.gameObject.name.Contains(GameAssets.instance.Get("CStreamPwrup").name)){PowerupCollect("cstream");}
                if(other.gameObject.name.Contains(GameAssets.instance.Get("PLaserPwrup").name)){PowerupCollect("plaser");}
                if(other.gameObject.name.Contains(GameAssets.instance.Get("Laser1Pwrup").name)){PowerupCollect("laser");}
                
                if(other.gameObject.name.Contains(GameAssets.instance.Get("FirePwrup").name)){player.OnFire(10);}
                if(other.gameObject.name.Contains(GameAssets.instance.Get("DecayPwrup").name)){player.Decay(10);}
                if(other.gameObject.name.Contains(GameAssets.instance.Get("BlindPwrup").name)){player.Blind(10,4);}
                if(other.gameObject.name.Contains(GameAssets.instance.Get("ElectrcPwrup").name)){player.Electrc(10);}
                if(other.gameObject.name.Contains(GameAssets.instance.Get("FrozenPwrup").name)){player.Freeze(10);}
                if(other.gameObject.name.Contains(GameAssets.instance.Get("FragilePwrup").name)){player.Fragile(10);}
                if(other.gameObject.name.Contains(GameAssets.instance.Get("ArmoredPwrup").name)){player.Armor(10);}
                if(other.gameObject.name.Contains(GameAssets.instance.Get("PowerPwrup").name)){player.Power(10);}
                if(other.gameObject.name.Contains(GameAssets.instance.Get("WeaknsPwrup").name)){player.Weaken(10);}
                if(other.gameObject.name.Contains(GameAssets.instance.Get("HackedPwrup").name)){player.Hack(10);}
                if(other.gameObject.name.Contains(GameAssets.instance.Get("InfEnergyPwrup").name)){player.InfEnergy(10);}
                if(other.gameObject.name.Contains(GameAssets.instance.Get("SpeedPwrup").name)){player.Speed(10);}
                if(other.gameObject.name.Contains(GameAssets.instance.Get("SlowPwrup").name)){player.Slow(10);}


                void HPAdd(float hp){player.Damage(hp,dmgType.heal);UniCollider.DMG_VFX(2,other,transform,-hp);}
                void HPAbsorp(float hp){player.HPAbsorp(hp);UniCollider.DMG_VFX(4,other,transform,hp);}
                

                if(other.gameObject.name.Contains(GameAssets.instance.Get("EnBall").name)){AudioManager.instance.Play("EnergyBall");}
                else if(other.gameObject.name.Contains(GameAssets.instance.Get("Coin").name)){AudioManager.instance.Play("Coin");}
                else if(other.gameObject.name.Contains(GameAssets.instance.Get("PowerCore").name)){AudioManager.instance.Play("CoreCollect");}
                else if(other.gameObject.name.Contains(GameAssets.instance.Get("CelestBall").name)||other.gameObject.name.Contains(GameAssets.instance.Get("CelestVial").name)){AudioManager.instance.Play("CelestBall");}
                else if(other.gameObject.name.Contains(GameAssets.instance.Get("HealBeam").name)){AudioManager.instance.Play("Heal");}
                else if(other.gameObject.name.Contains(GameAssets.instance.Get("GCloverPwrup").name)){AudioManager.instance.Play("GClover");}
                else if(other.gameObject.name.Contains(GameAssets.instance.Get("ShadowBtPwrup").name)){AudioManager.instance.Play("ShadowGet");}
                else if(other.gameObject.name.Contains(GameAssets.instance.Get("MatrixPwrup").name)){AudioManager.instance.Play("MatrixGet");}
                else if(other.gameObject.name.Contains(GameAssets.instance.Get("AccelPwrup").name)){AudioManager.instance.Play("AccelGet");}
                else{AudioManager.instance.Play("Powerup");}
                Destroy(other.gameObject, 0.05f);
            }
            #endregion
            if((dmg!=0||other.gameObject.name.Contains(GameAssets.instance.Get("InverterPwrup").name))&&!player.gclover){var name=other.gameObject.name.Split('(')[0];lastHitObj=name;lastHitDmg=dmg;}
            UniCollider.DMG_VFX(2,other,transform,dmg);
    }
    }
    public void PowerupCollect(string name){
        if(player.energy<=player.enForPwrupRefill){EnergyAdd();}
        var w=player.GetWeaponProperty(name);
        if(w!=null){
            if(w.costType==costType.energy){player.ammo=-4;if(player.ContainsPowerup(name)){EnergyAddDupl();}}
            else if(w.costType==costType.ammo){if(player.ContainsPowerup(name)){AmmoAddDupl(w);}else{player.ammo=0;AmmoAdd(w);}}
       }else{Debug.LogWarning("WeaponProperty by name "+name+" does not exist");}
        player.SetPowerupStr(name);
    }
    void EnergyAdd(){player.AddSubEnergy(player.pwrupEnergyGet,true);}
    void EnergyAddDupl(){player.AddSubEnergy(player.enPwrupDuplicate,true);}
    void AmmoAdd(WeaponProperties w){costTypeAmmo wc=(costTypeAmmo)w.costTypeProperties;player.AddSubAmmo(wc.ammoSize,true);}
    void AmmoAddDupl(WeaponProperties w){costTypeAmmo wc=(costTypeAmmo)w.costTypeProperties;player.AddSubAmmo(wc.ammoSize,true);}


    void OnTriggerStay2D(Collider2D other){
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
            }
            dmgPhaseFreq.SetTimer();
        }}
    }}

    private void OnTriggerExit2D(Collider2D other){
        if(other.GetComponent<Tag_DmgPhaseFreq>()!=null){other.GetComponent<Tag_DmgPhaseFreq>().ResetTimer();}
    }
    float CalculateDmg(float dmgVal,int armorPenetrVal,bool phase=false){
        float dmg=dmgVal;
        int def=player.defense;int armorPenetr=armorPenetrVal;float defMulti=0.5f;
        if(phase){defMulti=0.2f;}
        float totalDef=Mathf.Clamp((Mathf.Clamp((def-armorPenetr)*defMulti,0,999)),0,99999f);
        dmg=Mathf.Clamp(dmg-=totalDef,0f,999999f);
        if(def==-1){dmg/=2;}
        if(def==-2){dmg/=4;}
        if(def==-3){dmg/=8;}
        if(def==-4){dmg/=16;}
        if(def==-99){dmg=0;}
        return (float)System.Math.Round(dmg,2);
    }
   void PlayerEffects(string goName,bool phase=false){
        DamageValues dmgVal=UniCollider.GetDmgVal(goName);
        if(dmgVal!=null){
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
}