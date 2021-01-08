using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PlayerCollider : MonoBehaviour{
    #region
    [Header("Other")]
    [SerializeField] public GameObject dmgPopupPrefab;
    [SerializeField] float dmgFreq=0.38f;
    public float dmgTimer;
    public string lastHitObj;
    public float lastHitDmg;

    Player player;
    GameSession gameSession;
    #endregion
    void Start()
    {
        player = FindObjectOfType<Player>();
        gameSession = FindObjectOfType<GameSession>();

        SetPrefabs();
    }

    void SetPrefabs(){
        /*CoinPrefab=GameAssets.instance.Get("Coin");
        enBallPrefab=GameAssets.instance.Get("EnBall");
        powercorePrefab=GameAssets.instance.Get("PowerCore");
        armorPwrupPrefab=GameAssets.instance.Get("ArmorPwrup");
        armorUPwrupPrefab=GameAssets.instance.Get("ArmorUPwrup");
        laser2PwrupPrefab=GameAssets.instance.Get("Laser2Pwrup");
        laser3PwrupPrefab=GameAssets.instance.Get("Laser3Pwrup");
        phaserPwrupPrefab=GameAssets.instance.Get("PhaserPwrup");
        hrocketPwrupPrefab=GameAssets.instance.Get("HRocketPwrup");
        mlaserPwrupPrefab=GameAssets.instance.Get("MLaserPwrup");
        lsaberPwrupPrefab=GameAssets.instance.Get("LSaberPwrup");
        lclawsPwrupPrefab=GameAssets.instance.Get("LClawsPwrup");
        flipPwrupPrefab=GameAssets.instance.Get("FlipPwrup");
        gcloverPwrupPrefab=GameAssets.instance.Get("GCloverPwrup");
        shadowPwrupPrefab=GameAssets.instance.Get("ShadowPwrup");
        shadowBTPwrupPrefab=GameAssets.instance.Get("ShadowBtPwrup");
        qrocketPwrupPrefab=GameAssets.instance.Get("QRocketPwrup");
        procketPwrupPrefab=GameAssets.instance.Get("PRocketPwrup");
        cstreamPwrupPrefab=GameAssets.instance.Get("CStreamPwrup");
        inverterPwrupPrefab=GameAssets.instance.Get("InverterPwrup");
        magnetPwrupPrefab=GameAssets.instance.Get("MagnetPwrup");
        scalerPwrupPrefab=GameAssets.instance.Get("ScalerPwrup");
        matrixPwrupPrefab=GameAssets.instance.Get("MatrixPwrup");
        pmultiPwrupPrefab=GameAssets.instance.Get("PMultiPwrup");
        randomizerPwrupPrefab=GameAssets.instance.Get("RandomizerPwrup");
        acceleratorPwrupPrefab=GameAssets.instance.Get("AcceleratorPwrup");
        plaserPwrupPrefab=GameAssets.instance.Get("PLaserPwrup");

        cometPrefab=GameAssets.instance.Get("Comet");
        batPrefab=GameAssets.instance.Get("Bat");
        soundwavePrefab=GameAssets.instance.Get("Soundwave");
        enShip1Prefab=GameAssets.instance.Get("EnShip");
        EBtPrefab=GameAssets.instance.Get("EnBt");
        enCombatantPrefab=GameAssets.instance.Get("EnComb");
        enSaberPrefab=GameAssets.instance.Get("EnSaber");
        leechPrefab=GameAssets.instance.Get("Leech");
        hlaserPrefab=GameAssets.instance.Get("HLaser");
        goblinPrefab=GameAssets.instance.Get("Goblin");
        hdronePrefab=GameAssets.instance.Get("HDrone");
        vortexPrefab=GameAssets.instance.Get("Vortex");
        stingerPrefab=GameAssets.instance.Get("Stinger");
        goblinbtPrefab=GameAssets.instance.Get("GoblinBt");
        glaredevPrefab=GameAssets.instance.Get("GlareDevil");*/
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        SetPrefabs();
        if (!other.CompareTag(tag))
        {
            DamageDealer damageDealer = other.GetComponent<DamageDealer>();
            DamageValues damageValues=DamageValues.instance;
            //ifif(!damageDealer||!damageValues){Debug.LogWarning("No DamageDealer component or DamageValues instance");return;}

            if(other.GetComponent<Tag_OutsideZone>()!=null){player.Hack(1f);player.Damage(damageValues.GetDmgZone(),dmgType.silent);}
            #region//Enemies
            if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("EnemyBullet"))
            {
                bool en = false;
                bool destroy = true;
                float dmg = 0;

                if (other.gameObject.name.Contains(GameAssets.instance.Get("Comet").name)) { if(other.GetComponent<CometRandomProperties>()!=null){if(other.GetComponent<CometRandomProperties>().damageBySpeedSize){dmg=(float)System.Math.Round(damageValues.GetDmgComet()*Mathf.Abs(other.GetComponent<Rigidbody2D>().velocity.y)*other.transform.localScale.x,1);}else{dmg=damageValues.GetDmgComet();}}else{dmg=damageValues.GetDmgComet();} en = true; }

                if (other.gameObject.name.Contains(GameAssets.instance.Get("Bat").name)) { dmg = damageValues.GetDmgBat(); en = true; }
                if (other.gameObject.name.Contains(GameAssets.instance.Get("Soundwave").name)) { dmg = damageValues.GetDmgSoundwave(); AudioManager.instance.Play("SoundwaveHit"); }

                if (other.gameObject.name.Contains(GameAssets.instance.Get("EnShip").name)) { dmg = damageValues.GetDmgEnemyShip1(); en = true; }
                if (other.gameObject.name.Contains(GameAssets.instance.Get("EnBt").name)) { dmg = damageValues.GetDmgEBt();}

                if (other.gameObject.name.Contains(GameAssets.instance.Get("EnComb").name)) { en = true; destroy=false; }
                if (other.gameObject.name.Contains(GameAssets.instance.Get("EnSaber").name)) { dmg = damageValues.GetDmgEnSaber(); en = false; destroy=false; }

                if (other.gameObject.name.Contains(GameAssets.instance.Get("Goblin").name)) { dmg = damageValues.GetDmgGoblin(); en = true; }

                if (other.gameObject.name.Contains(GameAssets.instance.Get("HDrone").name)) { dmg = damageValues.GetDmgHealDrone(); en = true; }

                if (other.gameObject.name.Contains(GameAssets.instance.Get("Vortex").name)) { dmg = damageValues.GetDmgVortex(); en = true; }
                if(other.gameObject.name.Contains("StickBomb")) { dmg=0; en = false; destroy=false; }

                if (other.gameObject.name.Contains(GameAssets.instance.Get("Stinger").name)) { dmg = damageValues.GetDmgStinger(); player.Weaken(damageValues.GetEfxStinger().x,damageValues.GetEfxStinger().y); en = true; }

                if (other.gameObject.name.Contains(GameAssets.instance.Get("Leech").name)) { en = true;  destroy = false; }
        
                if (other.gameObject.name.Contains(GameAssets.instance.Get("HLaser").name)) { destroy = false; }

                if (other.gameObject.name.Contains(GameAssets.instance.Get("GoblinBt").name)) { dmg=damageValues.GetDmgGoblinBt(); /*player.Blind(3,2);*/player.Fragile(damageValues.GetEfxGoblinBt().x,damageValues.GetEfxGoblinBt().y); player.Hack(damageValues.GetEfxGoblinBt().x*0.9f); AudioManager.instance.Play("GoblinBtHit");}
                
                if (other.gameObject.name.Contains(GameAssets.instance.Get("GlareDevil").name)) { en=true; dmg=damageValues.GetDmgGoblin(); player.Fragile(damageValues.GetEfxGlareDev().x,damageValues.GetEfxGlareDev().y); player.Weaken(damageValues.GetEfxGlareDev().x,damageValues.GetEfxGlareDev().y); }

                if (!other.gameObject.name.Contains(GameAssets.instance.Get("HLaser").name))
                {
                    if (player.dashing == false)
                    {
                        if(dmg!=0&&!player.gclover){player.Damage(dmg,dmgType.normal);}
                        //else if(dmg!=0&&player.gclover){AudioManager.instance.Play("GCloverHit");}
                        if (destroy == true)
                        {
                            if (en != true) { Destroy(other.gameObject, 0.05f); }
                            else { other.GetComponent<Enemy>().givePts = false; other.GetComponent<Enemy>().health = -1; other.GetComponent<Enemy>().Die(); }
                        }
                        else { }
                        var flare = Instantiate(player.flareHitVFX, new Vector2(other.transform.position.x, transform.position.y + 0.5f), Quaternion.identity);
                        Destroy(flare.gameObject, 0.3f);
                    }
                    else if (player.shadow == true && player.dashing == true)
                    {
                        //if (destroy == true){
                        if (en != true) { Destroy(other.gameObject, 0.05f); }
                        else { other.GetComponent<Enemy>().health = -1; other.GetComponent<Enemy>().Die(); }
                        //}else{ }
                    }
                }else{
                    if(dmg!=0&&!player.gclover){player.Damage(dmg,dmgType.normal);}
                    //else if(dmg!=0&&player.gclover){AudioManager.instance.Play("GCloverHit");}
                }
                var name=other.gameObject.name.Split('(')[0];lastHitObj=name;lastHitDmg=dmg;
                if(gameSession.dmgPopups==true&&dmg!=0&&!player.gclover){
                    GameObject dmgpopup=CreateOnUI.CreateOnUIFunc(dmgPopupPrefab,transform.position);
                    dmgpopup.GetComponentInChildren<TMPro.TextMeshProUGUI>().color=Color.red;
                    dmgpopup.transform.localScale=new Vector2(2,2);
                    dmgpopup.GetComponentInChildren<TMPro.TextMeshProUGUI>().text=System.Math.Round(dmg/player.armorMulti,2).ToString();
                }
                //DMGPopUpHud(dmg);
            }
            #endregion
            #region//Powerups
            else if (other.gameObject.CompareTag("Powerups"))
            {
                if (other.gameObject.name.Contains(GameAssets.instance.Get("EnBall").name)) { player.AddSubEnergy(player.energyBallGet,true);}

                if (other.gameObject.name.Contains(GameAssets.instance.Get("Coin").name)) { player.AddSubCoins(other.GetComponent<LCrystalDrop>().amnt,true);}//gameSession.coins += 1; }

                if (other.gameObject.name.Contains(GameAssets.instance.Get("PowerCore").name)) { player.AddSubCores(1,true); }

                if(other.GetComponent<Tag_Collectible>().isPowerup){//if((!other.gameObject.name.Contains(enBallName)) && (!other.gameObject.name.Contains(CoinName)) && (!other.gameObject.name.Contains(powercoreName))){
                    if(FindObjectOfType<DisruptersSpawner>()!=null)FindObjectOfType<DisruptersSpawner>().powerupsGoblin++;
                    gameSession.AddXP(gameSession.xp_powerup);}//XP For powerups

                if (other.gameObject.name.Contains(GameAssets.instance.Get("ArmorPwrup").name)) { if(player.health>(player.maxHP-25)){gameSession.AddToScoreNoEV(Mathf.RoundToInt(player.maxHP - player.health)*2);} HPAdd(); player.AddSubEnergy(player.medkitEnergyGet,true); }//EnergyPopUpHUDPlus(player.medkitEnergyGet); player.healed = true; }
                if (other.gameObject.name.Contains(GameAssets.instance.Get("ArmorUPwrup").name)) { if (player.health>(player.maxHP-30)) {gameSession.AddToScoreNoEV(Mathf.RoundToInt(player.maxHP - player.health)*2);} HPAddU(); player.AddSubEnergy(player.medkitUEnergyGet,true); }//EnergyPopUpHUDPlus(player.medkitUEnergyGet); player.healed = true; }

                void HPAdd(){
                    player.Damage(player.medkitHpAmnt,dmgType.heal);
                    //player.health += player.medkitHpAmnt;
                    //HPPopUpHUD(player.medkitHpAmnt);
                }void HPAddU(){
                    player.Damage(player.medkitUHpAmnt,dmgType.heal);
                    //player.health += player.medkitUHpAmnt;
                    //HPPopUpHUD(player.medkitUHpAmnt);
                }

                if (other.gameObject.name.Contains(GameAssets.instance.Get("FlipPwrup").name)) {
                    if(player.energy<=player.enForPwrupRefill){EnergyAdd();}
                    if(player.flip==true){EnergyAddDupl();}
                    player.SetStatus("flip"); 
                }
                
                if (other.gameObject.name.Contains(GameAssets.instance.Get("InverterPwrup").name)){
                if(player.energyOn){
                    var tempHP = player.health; var tempEn = player.energy;
                    player.energy=tempHP; player.health=tempEn;
                }
                player.SetStatus("inverter"); player.inverterTimer = 0; }

                if (other.gameObject.name.Contains(GameAssets.instance.Get("MagnetPwrup").name)) {
                    if(player.energy<=player.enForPwrupRefill){EnergyAdd();}
                    if(player.magnet==true){EnergyAddDupl();}
                    player.SetStatus("magnet");
                    }
                
                if (other.gameObject.name.Contains(GameAssets.instance.Get("ScalerPwrup").name)) {
                    if(player.energy<=player.enForPwrupRefill){EnergyAdd();}
                    if(player.scaler==true){EnergyAddDupl();}
                    player.SetStatus("scaler");
                    player.shipScale=UnityEngine.Random.Range(player.shipScaleMin,player.shipScaleMax);
                    }

                if (other.gameObject.name.Contains(GameAssets.instance.Get("GCloverPwrup").name))
                {
                    player.SetStatus("gclover");
                    gameSession.MultiplyScore(1.25f);
                    player.energy = player.maxEnergy;
                    //GameObject gcloverexVFX = Instantiate(gcloverVFX, new Vector2(0, 0), Quaternion.identity);
                    GameObject gcloverexOVFX = Instantiate(player.gcloverOVFX, new Vector2(0, 0), Quaternion.identity);
                    //Destroy(gcloverexVFX, 1f);
                    Destroy(gcloverexOVFX, 1f);
                }
                if (other.gameObject.name.Contains(GameAssets.instance.Get("ShadowPwrup").name))
                {
                    if(player.energy<=player.enForPwrupRefill){player.AddSubEnergy(player.pwrupEnergyGet,true);}
                    if(player.shadow==true){EnergyAddDupl();}
                    player.SetStatus("shadow");
                    player.shadowed = true;
                    //GameObject gcloverexVFX = Instantiate(gcloverVFX, new Vector2(0, 0), Quaternion.identity);
                    //GameObject gcloverexOVFX = Instantiate(shadowEVFX, new Vector2(0, 0), Quaternion.identity);
                    //Destroy(gcloverexVFX, 1f);
                    //Destroy(gcloverexOVFX, 1f);
                }
                if (other.gameObject.name.Contains(GameAssets.instance.Get("MatrixPwrup").name)) { player.SetStatus("matrix"); }
                if (other.gameObject.name.Contains(GameAssets.instance.Get("AccelPwrup").name)) { player.SetStatus("accel"); }
                if (other.gameObject.name.Contains(GameAssets.instance.Get("PMultiPwrup").name)) { player.SetStatus("pmulti");if(player.pmultiTimer<0){player.pmultiTimer=0;}player.pmultiTimer += player.pmultiTime; gameSession.scoreMulti=2f;}
                
                if (other.gameObject.name.Contains(GameAssets.instance.Get("RandomizerPwrup").name)) { 
                    var item = other.GetComponent<LootTable>().GetItem();
                    Instantiate(item.gameObject,new Vector2(other.transform.position.x,other.transform.position.y),Quaternion.identity);
                    Destroy(other.gameObject,0.01f);
                 }

                if (other.gameObject.name.Contains(GameAssets.instance.Get("Laser2Pwrup").name)) { PowerupCollect("laser2"); }
                if (other.gameObject.name.Contains(GameAssets.instance.Get("Laser3Pwrup").name)) { PowerupCollect("laser3"); }
                if (other.gameObject.name.Contains(GameAssets.instance.Get("PhaserPwrup").name)) { PowerupCollect("phaser"); }
                if (other.gameObject.name.Contains(GameAssets.instance.Get("HRocketPwrup").name)) { PowerupCollect("hrocket"); }
                if (other.gameObject.name.Contains(GameAssets.instance.Get("MLaserPwrup").name)) { PowerupCollect("mlaser"); }
                if (other.gameObject.name.Contains(GameAssets.instance.Get("LSaberPwrup").name)) { PowerupCollect("lsaber"); }
                if (other.gameObject.name.Contains(GameAssets.instance.Get("LClawsPwrup").name)) { PowerupCollect("lclaws"); }
                if (other.gameObject.name.Contains(GameAssets.instance.Get("ShadowBtPwrup").name)) { PowerupCollect("shadowbt"); }
                if (other.gameObject.name.Contains(GameAssets.instance.Get("QRocketPwrup").name)) { PowerupCollect("qrocket"); }
                if (other.gameObject.name.Contains(GameAssets.instance.Get("PRocketPwrup").name)) { PowerupCollect("procket"); }
                if (other.gameObject.name.Contains(GameAssets.instance.Get("CStreamPwrup").name)) { PowerupCollect("cstream"); }
                if (other.gameObject.name.Contains(GameAssets.instance.Get("PLaserPwrup").name)) { PowerupCollect("plaser"); }
                if (other.gameObject.name.Contains(GameAssets.instance.Get("Laser1Pwrup").name)) { PowerupCollect("laser"); }
                if (other.gameObject.name.Contains(GameAssets.instance.Get("FirePwrup").name)) { player.OnFire(10,1); }
                if (other.gameObject.name.Contains(GameAssets.instance.Get("DecayPwrup").name)) { player.Decay(10,1); }
                if (other.gameObject.name.Contains(GameAssets.instance.Get("BlindPwrup").name)) { player.Blind(10,4); }
                if (other.gameObject.name.Contains(GameAssets.instance.Get("ElectrcPwrup").name)) { player.Electrc(10); }
                if (other.gameObject.name.Contains(GameAssets.instance.Get("FrozenPwrup").name)) { player.Freeze(10); }
                if (other.gameObject.name.Contains(GameAssets.instance.Get("FragilePwrup").name)) { player.Fragile(10,1); }
                if (other.gameObject.name.Contains(GameAssets.instance.Get("ArmoredPwrup").name)) { player.Armor(10,1); }
                if (other.gameObject.name.Contains(GameAssets.instance.Get("PowerPwrup").name)) { player.Power(10,1); }
                if (other.gameObject.name.Contains(GameAssets.instance.Get("WeaknsPwrup").name)) { player.Weaken(10,1); }
                if (other.gameObject.name.Contains(GameAssets.instance.Get("HackedPwrup").name)) { player.Hack(10); }
                if (other.gameObject.name.Contains(GameAssets.instance.Get("InfEnergyPwrup").name)) { player.InfEnergy(10); }


                if (other.gameObject.name.Contains(GameAssets.instance.Get("EnBall").name))
                {
                    AudioManager.instance.Play("EnergyBall");
                }
                else if (other.gameObject.name.Contains(GameAssets.instance.Get("Coin").name))
                {
                    AudioManager.instance.Play("Coin");
                }else if (other.gameObject.name.Contains(GameAssets.instance.Get("PowerCore").name))
                {
                    AudioManager.instance.Play("LvlUp");
                }
                else if (other.gameObject.name.Contains(GameAssets.instance.Get("GCloverPwrup").name))
                {
                    AudioManager.instance.Play("GClover");
                }
                else if (other.gameObject.name.Contains(GameAssets.instance.Get("ShadowBtPwrup").name))
                {
                    AudioManager.instance.Play("ShadowGet");
                }else if (other.gameObject.name.Contains(GameAssets.instance.Get("MatrixPwrup").name))
                {
                    AudioManager.instance.Play("MatrixGet");
                }else if (other.gameObject.name.Contains(GameAssets.instance.Get("AccelPwrup").name))
                {
                    AudioManager.instance.Play("AccelGet");
                }
                else
                {
                    //SoundManager.PlaySound(SoundManager.Sound.powerupSFX);
                    AudioManager.instance.Play("Powerup");
                }
                Destroy(other.gameObject, 0.05f);
            }
            #endregion
        }
    }
    void PowerupCollect(string name){
        if(player.energy<=player.enForPwrupRefill){EnergyAdd();}
        var w=player.GetWeaponProperty(name);
        if(w.costType==costType.energy){player.ammo=-4;if(player.powerup==name){EnergyAddDupl();}}
        else if(w.costType==costType.ammo){if(player.powerup==name){AmmoAddDupl(w);}else{AmmoAdd(w);}}
        player.SetPowerup(name);
    }
    void EnergyAdd(){
        player.AddSubEnergy(player.pwrupEnergyGet,true);//EnergyPopUpHUDPlus(player.pwrupEnergyGet);
    }void EnergyAddDupl(){
        player.AddSubEnergy(player.enPwrupDuplicate,true);//EnergyPopUpHUDPlus(player.enPwrupDuplicate);
    }void AmmoAdd(WeaponProperties w){
        player.AddSubAmmo(w.ammoSize,true);//EnergyPopUpHUDPlus(player.pwrupEnergyGet);
    }void AmmoAddDupl(WeaponProperties w){
        player.AddSubAmmo(w.ammoSize,true);//EnergyPopUpHUDPlus(player.enPwrupDuplicate);
    }
    private void OnTriggerStay2D(Collider2D other){
        if (!other.CompareTag(tag))
        {
        if (dmgTimer<=0){
            DamageDealer damageDealer=other.GetComponent<DamageDealer>();
            DamageValues damageValues=DamageValues.instance;
            //if(!damageDealer||!damageValues){Debug.LogWarning("No DamageDealer component or DamageValues instance");return;}
            //bool en = false;
            float dmg = 0;
            if(other.GetComponent<Tag_OutsideZone>()!=null){player.Hack(1f);dmg=damageValues.GetDmgZone();}
        if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("EnemyBullet")){
            
            if (other.gameObject.name.Contains(GameAssets.instance.Get("Leech").name)) { dmg = damageValues.GetDmgLeech(); AudioManager.instance.Play("LeechBite");}

            if (other.gameObject.name.Contains(GameAssets.instance.Get("HLaser").name)) { dmg = damageValues.GetDmgHLaser(); }

            if (other.gameObject.name.Contains(GameAssets.instance.Get("EnSaber").name)) { dmg = damageValues.GetDmgEnSaber(); }

            if(other.gameObject.name.Contains("StickBomb")) { dmg=0; }
            }
        //if (dmgTimer<=0){
                //if (other.gameObject.name.Contains(leechName)){}
                //var flare = Instantiate(player.flareHitVFX, new Vector2(other.transform.position.x, transform.position.y + 0.5f), Quaternion.identity);
                //Destroy(flare.gameObject, 0.3f);
                if(gameSession.dmgPopups==true&&dmg!=0){
                    GameObject dmgpopup=CreateOnUI.CreateOnUIFunc(dmgPopupPrefab,transform.position);
                    dmgpopup.GetComponentInChildren<TMPro.TextMeshProUGUI>().color=Color.red;
                    dmgpopup.transform.localScale=new Vector2(2,2);
                    dmgpopup.GetComponentInChildren<TMPro.TextMeshProUGUI>().text=System.Math.Round(dmg/player.armorMulti,2).ToString();
                }
                if(dmg!=0)player.Damage(dmg,dmgType.silent);
                dmgTimer = dmgFreq;
            }else{ dmgTimer -= Time.deltaTime; }
        }
    }
    private void OnTriggerExit2D(Collider2D other){
        if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("EnemyBullet"))
        {
            dmgTimer = dmgFreq;
        }
        //GameObject dmgpopupHud=GameObject.Find("HPDiffParrent");
        //dmgpopupHud.GetComponent<AnimationOn>().AnimationSet(true);
    }
    public GameObject GetRandomizerPwrup(){return GameAssets.instance.Get("RandomizerPwrup");}

    /*public void DMGPopUpHud(float dmg){
        GameObject dmgpopupHud=GameObject.Find("HPDiffParrent");
        dmgpopupHud.GetComponent<AnimationOn>().AnimationSet(true);
        //dmgpupupHud.GetComponent<Animator>().SetTrigger(0);
        dmgpopupHud.GetComponentInChildren<TMPro.TextMeshProUGUI>().text="-"+dmg.ToString();
    }public void HPPopUpHUD(float dmg){
        GameObject dmgpopupHud=GameObject.Find("HPDiffParrent");
        dmgpopupHud.GetComponent<AnimationOn>().AnimationSet(true);
        //dmgpupupHud.GetComponent<Animator>().SetTrigger(0);
        dmgpopupHud.GetComponentInChildren<TMPro.TextMeshProUGUI>().text="+"+dmg.ToString();
    }
    public void EnergyPopUpHUD(float en){
        GameObject enpopupHud=GameObject.Find("EnergyDiffParrent");
        enpopupHud.GetComponent<AnimationOn>().AnimationSet(true);
        //enpupupHud.GetComponent<Animator>().SetTrigger(0);
        enpopupHud.GetComponentInChildren<TMPro.TextMeshProUGUI>().text="-"+en.ToString();
    }public void EnergyPopUpHUDPlus(float en){
        GameObject enpopupHud=GameObject.Find("EnergyDiffParrent");
        enpopupHud.GetComponent<AnimationOn>().AnimationSet(true);
        //enpupupHud.GetComponent<Animator>().SetTrigger(0);
        enpopupHud.GetComponentInChildren<TMPro.TextMeshProUGUI>().text="+"+en.ToString();
    }*/
}
