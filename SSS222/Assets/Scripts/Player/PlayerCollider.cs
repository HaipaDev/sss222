using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PlayerCollider : MonoBehaviour{
    [Header("Other")]
    //[SerializeField] float dmgFreq=0.38f;
    public float dmgTimer;
    public string lastHitObj;
    public float lastHitDmg;
    public List<colliTypes> collisionTypes=UniCollider.colliTypesForPl;

    Player player;
    void Start(){player=Player.instance;}
    private void OnTriggerEnter2D(Collider2D other){
    if(!other.CompareTag(tag)&&(player.collidedId==GetInstanceID()||player.collidedIdChangeTime<=0)){
            DamageDealer damageDealer=other.GetComponent<DamageDealer>();
            DamageValues damageValues=DamageValues.instance;
            float dmg=0;
            if(player.collidedIdChangeTime<=0){player.collidedId=GetInstanceID();player.collidedIdChangeTime=0.33f;}
            //ifif(!damageDealer||!damageValues){Debug.LogWarning("No DamageDealer component or DamageValues instance");return;}

            //if(other.GetComponent<Tag_OutsideZone>()!=null){player.Hack(1f);player.Damage(damageValues.dmgZone,dmgType.silent);}
            #region//Enemies
            if(other.gameObject.CompareTag("Enemy")||other.gameObject.CompareTag("EnemyBullet")){
                bool en=true;
                bool destroy=true;
                dmg=UniCollider.TriggerEnter(other,transform,collisionTypes)[0];
                if(UniCollider.TriggerEnter(other,transform,collisionTypes)[1]==0)destroy=false;
                if(UniCollider.TriggerEnter(other,transform,collisionTypes)[2]==0)en=false;

                if(!other.gameObject.name.Contains(GameAssets.instance.Get("HLaser").name)&&!other.gameObject.name.Contains(GameAssets.instance.Get("VLaser").name)){
                    if(player.dashing==false){
                        if(dmg!=0&&!player.gclover){player.Damage(dmg,dmgType.normal);}
                        //else if(dmg!=0&&player.gclover){AudioManager.instance.Play("GCloverHit");}
                        if(destroy==true){
                            if(en!=true){Destroy(other.gameObject,0.05f);}
                            else{other.GetComponent<Enemy>().giveScore=false;other.GetComponent<Enemy>().health=-1;other.GetComponent<Enemy>().Die();}
                        }
                        GameAssets.instance.VFX("FlareHit",new Vector2(other.transform.position.x,transform.position.y+0.5f),0.3f);
                    }
                    else if(player.shadow==true&&player.dashing==true){
                        //if(destroy == true){
                        if(en!=true){Destroy(other.gameObject,0.05f);}
                        else{other.GetComponent<Enemy>().health=-1;other.GetComponent<Enemy>().Die();}
                        //}else{}
                   }
                }else{
                    if(dmg!=0&&!player.gclover){player.Damage(dmg,dmgType.normal);}
                    //else if(dmg!=0&&player.gclover){AudioManager.instance.Play("GCloverHit");}
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
                    if(FindObjectOfType<DisruptersSpawner>()!=null)FindObjectOfType<DisruptersSpawner>().AddPwrups(1);//powerupsGoblin++;
                    GameSession.instance.AddXP(GameSession.instance.xp_powerup);//XP For powerups
                }
                if(other.gameObject.name.Contains(GameAssets.instance.Get("MicroMedkit").name)){player.hpAbsorpAmnt+=player.microMedkitHpAmnt;}
                if(other.gameObject.name.Contains(GameAssets.instance.Get("ArmorPwrup").name)){
                    if(player.health>=player.maxHP){GameSession.instance.AddToScoreNoEV(Mathf.RoundToInt(player.medkitHpAmnt));}
                    else if(player.health!=player.maxHP&&player.health>(player.maxHP-player.medkitHpAmnt)){
                        int val=Mathf.RoundToInt(player.medkitHpAmnt-(player.maxHP-player.health));
                        if(val>0)GameSession.instance.AddToScoreNoEV(val);}
                    HPAdd(player.medkitHpAmnt);
                    player.AddSubEnergy(player.medkitEnergyGet,true);
                }
                if(other.gameObject.name.Contains(GameAssets.instance.Get("ArmorCPwrup").name)){
                    if(player.health>=player.maxHP){GameSession.instance.AddToScoreNoEV(25);}
                    else{HPAdd(player.medkitHpAmnt);}
                }
                void HPAdd(float hp){player.Damage(hp,dmgType.heal);}
                
                if(other.gameObject.name.Contains(GameAssets.instance.Get("FlipPwrup").name)) {
                    if(player.energy<=player.enForPwrupRefill){EnergyAdd();}
                    if(player.flip==true){EnergyAddDupl();}
                    player.SetStatus("flip"); 
                }
                if(other.gameObject.name.Contains(GameAssets.instance.Get("InverterPwrup").name)){
                    if(player.energyOn){
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
                    player.energy=player.maxEnergy;
                    GameAssets.instance.VFX("GCloverOutVFX", Vector2.zero,1f);
                }
                if(other.gameObject.name.Contains(GameAssets.instance.Get("ShadowPwrup").name)||other.gameObject.name.Contains(GameAssets.instance.Get("ShadowtracesPwrup").name)){
                    if(other.gameObject.name.Contains(GameAssets.instance.Get("ShadowtracesPwrup").name)){
                        if(!player.shadow){player.SetSpeedPrev();player.moveSpeedCurrent*=1.3f;}
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
                if(other.gameObject.name.Contains(GameAssets.instance.Get("LClawsPwrup").name)){PowerupCollect("lclaws");}
                if(other.gameObject.name.Contains(GameAssets.instance.Get("GloomyScythesPwrup").name)){PowerupCollect("gloomyScythes");}
                if(other.gameObject.name.Contains(GameAssets.instance.Get("ShadowBtPwrup").name)){PowerupCollect("shadowbt");}
                if(other.gameObject.name.Contains(GameAssets.instance.Get("QRocketPwrup").name)){PowerupCollect("qrocket");}
                if(other.gameObject.name.Contains(GameAssets.instance.Get("PRocketPwrup").name)){PowerupCollect("procket");}
                if(other.gameObject.name.Contains(GameAssets.instance.Get("CStreamPwrup").name)){PowerupCollect("cstream");}
                if(other.gameObject.name.Contains(GameAssets.instance.Get("PLaserPwrup").name)){PowerupCollect("plaser");}
                if(other.gameObject.name.Contains(GameAssets.instance.Get("Laser1Pwrup").name)){PowerupCollect("laser");}
                if(other.gameObject.name.Contains(GameAssets.instance.Get("FirePwrup").name)){player.OnFire(10,1);}
                if(other.gameObject.name.Contains(GameAssets.instance.Get("DecayPwrup").name)){player.Decay(10,1);}
                if(other.gameObject.name.Contains(GameAssets.instance.Get("BlindPwrup").name)){player.Blind(10,4);}
                if(other.gameObject.name.Contains(GameAssets.instance.Get("ElectrcPwrup").name)){player.Electrc(10);}
                if(other.gameObject.name.Contains(GameAssets.instance.Get("FrozenPwrup").name)){player.Freeze(10);}
                if(other.gameObject.name.Contains(GameAssets.instance.Get("FragilePwrup").name)){player.Fragile(10,1);}
                if(other.gameObject.name.Contains(GameAssets.instance.Get("ArmoredPwrup").name)){player.Armor(10,1);}
                if(other.gameObject.name.Contains(GameAssets.instance.Get("PowerPwrup").name)){player.Power(10,1);}
                if(other.gameObject.name.Contains(GameAssets.instance.Get("WeaknsPwrup").name)){player.Weaken(10,1);}
                if(other.gameObject.name.Contains(GameAssets.instance.Get("HackedPwrup").name)){player.Hack(10);}
                if(other.gameObject.name.Contains(GameAssets.instance.Get("InfEnergyPwrup").name)){player.InfEnergy(10);}
                if(other.gameObject.name.Contains(GameAssets.instance.Get("SpeedPwrup").name)){player.Speed(10,1);}
                if(other.gameObject.name.Contains(GameAssets.instance.Get("SlowPwrup").name)){player.Slow(10,1);}


                if(other.gameObject.name.Contains(GameAssets.instance.Get("EnBall").name)){AudioManager.instance.Play("EnergyBall");}
                else if(other.gameObject.name.Contains(GameAssets.instance.Get("Coin").name)){AudioManager.instance.Play("Coin");}
                else if(other.gameObject.name.Contains(GameAssets.instance.Get("PowerCore").name)){AudioManager.instance.Play("CoreCollect");}
                else if(other.gameObject.name.Contains(GameAssets.instance.Get("CelestBall").name)||other.gameObject.name.Contains(GameAssets.instance.Get("CelestVial").name)){AudioManager.instance.Play("CelestBall");}
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
            if(w.costType==costType.energy){player.ammo=-4;if(player.powerup==name){EnergyAddDupl();}}
            else if(w.costType==costType.ammo){if(player.powerup==name){AmmoAddDupl(w);}else{AmmoAdd(w);}}
       }else{Debug.LogWarning("WeaponProperty by name "+name+" does not exist");}
        player.SetPowerup(name);
    }
    void EnergyAdd(){player.AddSubEnergy(player.pwrupEnergyGet,true);}
    void EnergyAddDupl(){player.AddSubEnergy(player.enPwrupDuplicate,true);}
    void AmmoAdd(WeaponProperties w){costTypeAmmo wc=(costTypeAmmo)w.costTypeProperties;player.AddSubAmmo(wc.ammoSize,true);}
    void AmmoAddDupl(WeaponProperties w){costTypeAmmo wc=(costTypeAmmo)w.costTypeProperties;player.AddSubAmmo(wc.ammoSize,true);}
    private void OnTriggerStay2D(Collider2D other){
    if(!other.CompareTag(tag)&&(player.collidedId==GetInstanceID()||player.collidedIdChangeTime<=0)){if(dmgTimer<=0){
        DamageDealer damageDealer=other.GetComponent<DamageDealer>();
        DamageValues damageValues=DamageValues.instance;
        //if(!damageDealer||!damageValues){Debug.LogWarning("No DamageDealer component or DamageValues instance");return;}
        if(player.collidedIdChangeTime<=0){player.collidedId=GetInstanceID();player.collidedIdChangeTime=0.33f;}
        float dmg=UniCollider.TriggerStay(other,transform,collisionTypes);
        if(other.GetComponent<Tag_OutsideZone>()!=null){player.Hack(1f);dmg=damageValues.dmgZone;}
        //if(other.gameObject.CompareTag("Enemy")||other.gameObject.CompareTag("EnemyBullet")){
            
        //}
        UniCollider.DMG_VFX(3,other,transform,dmg);
        if(dmg>0)player.Damage(dmg,dmgType.silent);
        if(other.GetComponent<Tag_DmgPhaseFreq>()!=null)dmgTimer=other.GetComponent<Tag_DmgPhaseFreq>().dmgFreq;
    }else{dmgTimer-=Time.deltaTime;}}
    }
    private void OnTriggerExit2D(Collider2D other){
        if(other.gameObject.CompareTag("Enemy")||other.gameObject.CompareTag("EnemyBullet")){if(other.GetComponent<Tag_DmgPhaseFreq>()!=null)dmgTimer=other.GetComponent<Tag_DmgPhaseFreq>().dmgFreq;}
        //GameObject dmgpopupHud=GameObject.Find("HPDiffParrent");
        //dmgpopupHud.GetComponent<AnimationOn>().AnimationSet(true);
   }
    public GameObject GetRandomizerPwrup(){return GameAssets.instance.Get("RandomizerPwrup");}
}