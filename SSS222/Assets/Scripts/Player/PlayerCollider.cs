using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PlayerCollider : MonoBehaviour{
    [HeaderAttribute("Powerups")]
    GameObject CoinPrefab;
    GameObject enBallPrefab;
    GameObject powercorePrefab;
    GameObject armorPwrupPrefab;
    GameObject armorUPwrupPrefab;
    GameObject laser2PwrupPrefab;
    GameObject laser3PwrupPrefab;
    GameObject phaserPwrupPrefab;
    GameObject hrocketPwrupPrefab;
    GameObject mlaserPwrupPrefab;
    GameObject lsaberPwrupPrefab;
    GameObject lclawsPwrupPrefab;
    GameObject flipPwrupPrefab;
    GameObject gcloverPwrupPrefab;
    GameObject shadowPwrupPrefab;
    GameObject shadowBTPwrupPrefab;
    GameObject qrocketPwrupPrefab;
    GameObject procketPwrupPrefab;
    GameObject cstreamPwrupPrefab;
    GameObject inverterPwrupPrefab;
    GameObject magnetPwrupPrefab;
    GameObject scalerPwrupPrefab;
    GameObject matrixPwrupPrefab;
    GameObject pmultiPwrupPrefab;
    GameObject randomizerPwrupPrefab;
    #region
    [HeaderAttribute("Damage Dealers")]
    GameObject cometPrefab;
    GameObject batPrefab;
    GameObject enShip1Prefab;
    GameObject enCombatantPrefab;
    GameObject enSaberPrefab;
    GameObject soundwavePrefab;
    GameObject EBtPrefab;
    GameObject leechPrefab;
    GameObject hlaserPrefab;
    GameObject goblinPrefab;
    GameObject hdronePrefab;
    GameObject vortexPrefab;
    [HeaderAttribute("Other")]
    [SerializeField] public GameObject dmgPopupPrefab;
    [SerializeField] float dmgFreq=0.38f;
    public float dmgTimer;

    Player player;
    GameSession gameSession;
    // Start is called before the first frame update
    #endregion
    void Start()
    {
        player = FindObjectOfType<Player>().GetComponent<Player>();
        gameSession = FindObjectOfType<GameSession>();

        SetPrefabs();
    }

    void SetPrefabs(){
        CoinPrefab=GameAssets.instance.Get("Coin");
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
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        SetPrefabs();
        if (!other.CompareTag(tag))
        {
            #region//Enemies
            if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("EnemyBullet"))
            {
                DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
                if (!damageDealer) { return; }
                bool en = false;
                bool destroy = true;
                var dmg = damageDealer.GetDamage();

                var cometName = cometPrefab.name; var cometName1 = cometPrefab.name + "(Clone)";
                if (other.gameObject.name == cometName || other.gameObject.name == cometName1) { dmg = (float)System.Math.Round(damageDealer.GetDamageComet()*other.transform.localScale.x,1); en = true; }
                var batName = batPrefab.name; var batName1 = batPrefab.name + "(Clone)";
                if (other.gameObject.name == batName || other.gameObject.name == batName1) { dmg = damageDealer.GetDamageBat(); en = true; }
                var enShip1Name = enShip1Prefab.name; var enShip1Name1 = enShip1Prefab.name + "(Clone)";
                if (other.gameObject.name == enShip1Name || other.gameObject.name == enShip1Name1) { dmg = damageDealer.GetDamageEnemyShip1(); en = true; }

                var enCombatantName = enCombatantPrefab.name; var enCombatantName1 = enCombatantPrefab.name + "(Clone)";
                if (other.gameObject.name == enCombatantName || other.gameObject.name == enCombatantName1) { en = true; destroy=false; }
                var enSaberName = enSaberPrefab.name; var enSaberName1 = enSaberPrefab.name + "(Clone)";
                if (other.gameObject.name == enSaberName || other.gameObject.name == enSaberName1) { dmg = damageDealer.GetDamageEnSaber(); en = false; destroy=false; }

                var goblinName = goblinPrefab.name; var goblinName1 = goblinPrefab.name + "(Clone)";
                if (other.gameObject.name == goblinName || other.gameObject.name == goblinName1) { dmg = damageDealer.GetDamageGoblin(); en = true; }
                var hdroneName = hdronePrefab.name; var hdroneName1 = hdronePrefab.name + "(Clone)";
                if (other.gameObject.name == hdroneName || other.gameObject.name == hdroneName1) { dmg = damageDealer.GetDamageHealDrone(); en = true; }
                var vortexName = vortexPrefab.name; var vortexName1 = vortexPrefab.name + "(Clone)";
                if (other.gameObject.name == vortexName || other.gameObject.name == vortexName1) { dmg = damageDealer.GetDamageVortex(); en = true; }


                var Sname = soundwavePrefab.name; var Sname1 = soundwavePrefab.name + "(Clone)";
                if (other.gameObject.name == Sname || other.gameObject.name == Sname1) { dmg = damageDealer.GetDamageSoundwave(); AudioManager.instance.Play("SoundwaveHit"); }
                var EBtname = EBtPrefab.name; var EBtname1 = EBtPrefab.name + "(Clone)";
                if (other.gameObject.name == EBtname || other.gameObject.name == EBtname1) { dmg = damageDealer.GetDamageEBt();}


                var leechName = leechPrefab.name; var leechName1 = leechPrefab.name + "(Clone)";
                if (other.gameObject.name == leechName || other.gameObject.name == leechName1) { en = true;  destroy = false; }
        
                var hlaserName = hlaserPrefab.name; var hlaserName1 = hlaserPrefab.name + "(Clone)";
                if (other.gameObject.name == hlaserName || other.gameObject.name == hlaserName1) { destroy = false; }

                if (other.gameObject.name != hlaserName && other.gameObject.name != hlaserName1)
                {
                    if (player.dashing == false)
                    {
                        player.health -= dmg;
                        if (destroy == true)
                        {
                            if (en != true) { Destroy(other.gameObject, 0.05f); }
                            else { other.GetComponent<Enemy>().givePts = false; other.GetComponent<Enemy>().health = -1; other.GetComponent<Enemy>().Die(); }
                        }
                        else { }
                        player.damaged = true;
                        AudioManager.instance.Play("ShipHit");
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
                    player.health -= dmg;
                    player.damaged = true;
                    AudioManager.instance.Play("ShipHit");
                }
                if(gameSession.dmgPopups==true){
                    GameObject dmgpopup=CreateOnUI.CreateOnUIFunc(dmgPopupPrefab,transform.position);
                    dmgpopup.GetComponentInChildren<TMPro.TextMeshProUGUI>().color=Color.red;
                    dmgpopup.transform.localScale=new Vector2(2,2);
                    dmgpopup.GetComponentInChildren<TMPro.TextMeshProUGUI>().text=dmg.ToString();
                }
                DMGPopUpHud(dmg);
            }
            #endregion
            #region//Powerups
            else if (other.gameObject.CompareTag("Powerups"))
            {
                var enBallName = enBallPrefab.name; var enBallName1 = enBallPrefab.name + "(Clone)";
                if (other.gameObject.name == enBallName || other.gameObject.name == enBallName1) { player.energy += player.energyBallGet; EnergyPopUpHUDPlus(player.energyBallGet);}

                var CoinName = CoinPrefab.name; var CoinName1 = CoinPrefab.name + "(Clone)";
                if (other.gameObject.name == CoinName || other.gameObject.name == CoinName1) { gameSession.coins += 1; }

                var powercoreName = powercorePrefab.name; var powercoreName1 = powercorePrefab.name + "(Clone)";
                if (other.gameObject.name == powercoreName || other.gameObject.name == powercoreName1) { gameSession.cores += 1; }

                if((other.gameObject.name != enBallName && other.gameObject.name != enBallName1) && (other.gameObject.name != CoinName && other.gameObject.name != CoinName1) && (other.gameObject.name != powercoreName && other.gameObject.name != powercoreName1)){
                    gameSession.AddXP(gameSession.xp_powerup);}//XP For powerups

                var armorName = armorPwrupPrefab.name; var armorName1 = armorPwrupPrefab.name + "(Clone)";
                if (other.gameObject.name == armorName || other.gameObject.name == armorName1) { if(player.health>(player.maxHP-25)){gameSession.AddToScoreNoEV(Mathf.RoundToInt(player.maxHP - player.health)*2);} HPAdd(); player.energy += player.medkitEnergyGet; EnergyPopUpHUDPlus(player.medkitEnergyGet); player.healed = true; }
                var armorUName = armorUPwrupPrefab.name; var armorUName1 = armorUPwrupPrefab.name + "(Clone)";
                if (other.gameObject.name == armorUName || other.gameObject.name == armorUName1) { if (player.health>(player.maxHP-30)) {gameSession.AddToScoreNoEV(Mathf.RoundToInt(player.maxHP - player.health)*2);} HPAddU(); player.energy += player.medkitUEnergyGet; EnergyPopUpHUDPlus(player.medkitUEnergyGet); player.healed = true; }

                void HPAdd(){
                    player.health += player.medkitHpAmnt;
                    HPPopUpHUD(player.medkitHpAmnt);
                }void HPAddU(){
                    player.health += player.medkitUHpAmnt;
                    HPPopUpHUD(player.medkitUHpAmnt);
                }

                var flipName = flipPwrupPrefab.name; var flipName1 = flipPwrupPrefab.name + "(Clone)";
                if (other.gameObject.name == flipName || other.gameObject.name == flipName1) {
                    if(player.energy<=player.enForPwrupRefill){EnergyAdd();}
                    if(player.flip==true){EnergyAddDupl();}
                    player.SetStatus("flip"); 
                }
                
                var inverterName = inverterPwrupPrefab.name; var inverterName1 = inverterPwrupPrefab.name + "(Clone)";
                if (other.gameObject.name == inverterName || other.gameObject.name == inverterName1) {
                var tempHP = player.health; var tempEn = player.energy;
                player.energy=tempHP; player.health=tempEn;
                player.SetStatus("inverter"); player.inverterTimer = 0; }

                var magnetName = magnetPwrupPrefab.name; var magnetName1 = magnetPwrupPrefab.name + "(Clone)";
                if (other.gameObject.name == magnetName || other.gameObject.name == magnetName1) {
                    if(player.energy<=player.enForPwrupRefill){EnergyAdd();}
                    if(player.magnet==true){EnergyAddDupl();}
                    player.SetStatus("magnet");
                    }
                
                var scalerName = scalerPwrupPrefab.name; var scalerName1 = scalerPwrupPrefab.name + "(Clone)";
                if (other.gameObject.name == scalerName || other.gameObject.name == scalerName1) {
                    if(player.energy<=player.enForPwrupRefill){EnergyAdd();}
                    if(player.scaler==true){EnergyAddDupl();}
                    player.SetStatus("scaler");
                    }

                var gcloverName = gcloverPwrupPrefab.name; var gcloverName1 = gcloverPwrupPrefab.name + "(Clone)";
                if (other.gameObject.name == gcloverName || other.gameObject.name == gcloverName1)
                {
                    player.SetStatus("gclover");
                    gameSession.MultiplyScore(1.25f);
                    player.energy = player.maxEnergy;
                    //GameObject gcloverexVFX = Instantiate(gcloverVFX, new Vector2(0, 0), Quaternion.identity);
                    GameObject gcloverexOVFX = Instantiate(player.gcloverOVFX, new Vector2(0, 0), Quaternion.identity);
                    //Destroy(gcloverexVFX, 1f);
                    Destroy(gcloverexOVFX, 1f);
                }
                var shadowName = shadowPwrupPrefab.name; var shadowName1 = shadowPwrupPrefab.name + "(Clone)";
                if (other.gameObject.name == shadowName || other.gameObject.name == shadowName1)
                {
                    if(player.energy<=player.enForPwrupRefill){player.energy += player.pwrupEnergyGet;}
                    if(player.shadow==true){player.energy+=player.enPwrupDuplicate;EnergyAddDupl();}
                    player.SetStatus("shadow");
                    player.shadowed = true;
                    //GameObject gcloverexVFX = Instantiate(gcloverVFX, new Vector2(0, 0), Quaternion.identity);
                    //GameObject gcloverexOVFX = Instantiate(shadowEVFX, new Vector2(0, 0), Quaternion.identity);
                    //Destroy(gcloverexVFX, 1f);
                    //Destroy(gcloverexOVFX, 1f);
                }
                var matrixName = matrixPwrupPrefab.name; var matrixName1 = matrixPwrupPrefab.name + "(Clone)";
                if (other.gameObject.name == matrixName || other.gameObject.name == matrixName1) { player.SetStatus("matrix"); }
                var pmultiName = pmultiPwrupPrefab.name; var pmultiName1 = pmultiPwrupPrefab.name + "(Clone)";
                if (other.gameObject.name == pmultiName || other.gameObject.name == pmultiName1) { player.SetStatus("pmulti");if(player.pmultiTimer<0){player.pmultiTimer=0;}player.pmultiTimer += player.pmultiTime; gameSession.scoreMulti=2f;}
                
                var randomizerName = randomizerPwrupPrefab.name; var randomizerName1 = randomizerPwrupPrefab.name + "(Clone)";
                if (other.gameObject.name == randomizerName || other.gameObject.name == randomizerName1) { 
                    var item = other.GetComponent<LootTable>().GetItem();
                    Instantiate(item.gameObject,new Vector2(other.transform.position.x,other.transform.position.y),Quaternion.identity);
                    Destroy(other.gameObject,0.01f);
                 }

                var laser2Name = laser2PwrupPrefab.name; var laser2Name1 = laser2PwrupPrefab.name + "(Clone)";
                if (other.gameObject.name == laser2Name || other.gameObject.name == laser2Name1) { PowerupCollect("laser2"); }

                var laser3Name = laser3PwrupPrefab.name; var laser3Name1 = laser3PwrupPrefab.name + "(Clone)";
                if (other.gameObject.name == laser3Name || other.gameObject.name == laser3Name1) { PowerupCollect("laser3"); }

                var phaserName = phaserPwrupPrefab.name; var phaserName1 = phaserPwrupPrefab.name + "(Clone)";
                if (other.gameObject.name == phaserName || other.gameObject.name == phaserName1) { PowerupCollect("phaser"); }

                var hrocketName = hrocketPwrupPrefab.name; var hrocketName1 = hrocketPwrupPrefab.name + "(Clone)";
                if (other.gameObject.name == hrocketName || other.gameObject.name == hrocketName1) { PowerupCollect("hrocket"); }

                var minilaserName = mlaserPwrupPrefab.name; var minilaserName1 = mlaserPwrupPrefab.name + "(Clone)";
                if (other.gameObject.name == minilaserName || other.gameObject.name == minilaserName1) { PowerupCollect("mlaser"); }

                var lsaberWName1 = player.lsaberPrefab.name;
                var lclawsWName1 = player.lclawsPrefab.name;
                var lsaberName = lsaberPwrupPrefab.name; var lsaberName1 = lsaberPwrupPrefab.name + "(Clone)";
                if (other.gameObject.name == lsaberName || other.gameObject.name == lsaberName1) { PowerupCollect("lsaber"); GameObject.Find(lclawsWName1); }
                
                var lclawsName = lclawsPwrupPrefab.name; var lclawsName1 = lclawsPwrupPrefab.name + "(Clone)";
                if (other.gameObject.name == lclawsName || other.gameObject.name == lclawsName1) { PowerupCollect("lclaws"); GameObject.Find(lsaberWName1); }

                var shadowbtName = shadowBTPwrupPrefab.name; var shadowbtName1 = shadowBTPwrupPrefab.name + "(Clone)";
                if (other.gameObject.name == shadowbtName || other.gameObject.name == shadowbtName1) { PowerupCollect("shadowbt"); }

                var qrocketName = qrocketPwrupPrefab.name; var qrocketName1 = qrocketPwrupPrefab.name + "(Clone)";
                if (other.gameObject.name == qrocketName || other.gameObject.name == qrocketName1) { PowerupCollect("qrocket"); }
                var procketName = procketPwrupPrefab.name; var procketName1 = procketPwrupPrefab.name + "(Clone)";
                if (other.gameObject.name == procketName || other.gameObject.name == procketName1) { PowerupCollect("procket"); }

                var cstreamName = cstreamPwrupPrefab.name; var cstreamName1 = cstreamPwrupPrefab.name + "(Clone)";
                if (other.gameObject.name == cstreamName || other.gameObject.name == cstreamName1) { PowerupCollect("cstream"); }


                if (other.gameObject.name == enBallName || other.gameObject.name == enBallName1)
                {
                    AudioManager.instance.Play("EnergyBall");
                }
                else if (other.gameObject.name == CoinName || other.gameObject.name == CoinName1)
                {
                    AudioManager.instance.Play("Coin");
                }else if (other.gameObject.name == powercoreName || other.gameObject.name == powercoreName1)
                {
                    AudioManager.instance.Play("LvlUp");
                }
                else if (other.gameObject.name == gcloverName || other.gameObject.name == gcloverName1)
                {
                    AudioManager.instance.Play("GClover");
                }
                else if (other.gameObject.name == shadowbtName || other.gameObject.name == shadowbtName1)
                {
                    AudioManager.instance.Play("ShadowGet");
                }else if (other.gameObject.name == matrixName || other.gameObject.name == matrixName1)
                {
                    AudioManager.instance.Play("MatrixGet");
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
        if(player.energy<=player.enForPwrupRefill){EnergyAdd();} if(player.powerup==name){EnergyAddDupl();} player.powerup = name;
    }
    void EnergyAdd(){
        player.energy += player.pwrupEnergyGet;EnergyPopUpHUDPlus(player.pwrupEnergyGet);
    }void EnergyAddDupl(){
        player.energy += player.enPwrupDuplicate;EnergyPopUpHUDPlus(player.enPwrupDuplicate);
    }
    private void OnTriggerStay2D(Collider2D other){
        if (!other.CompareTag(tag))
        {
            if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("EnemyBullet"))
            {
                DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
                if (!damageDealer) { return; }
                //bool en = false;
                var dmg = damageDealer.GetDamage();

                var leechName = leechPrefab.name; var leechName1 = leechPrefab.name + "(Clone)";
                if (other.gameObject.name == leechName || other.gameObject.name == leechName1) { dmg = damageDealer.GetDamageLeech(); }

                var hlaserName = hlaserPrefab.name; var hlaserName1 = hlaserPrefab.name + "(Clone)";
                if (other.gameObject.name == hlaserName || other.gameObject.name == hlaserName1) { dmg = damageDealer.GetDamageHLaser(); }

                var enSaberName = enSaberPrefab.name; var enSaberName1 = enSaberPrefab.name + "(Clone)";
                if (other.gameObject.name == enSaberName || other.gameObject.name == enSaberName1) { dmg = damageDealer.GetDamageEnSaber(); }

                if (dmgTimer<=0){
                    player.health -= dmg;
                    player.damaged = true;
                    if (other.gameObject.name == leechName || other.gameObject.name == leechName1){AudioManager.instance.Play("LeechBite");}
                    //var flare = Instantiate(player.flareHitVFX, new Vector2(other.transform.position.x, transform.position.y + 0.5f), Quaternion.identity);
                    //Destroy(flare.gameObject, 0.3f);
                    if(gameSession.dmgPopups==true){
                        GameObject dmgpopup=CreateOnUI.CreateOnUIFunc(dmgPopupPrefab,transform.position);
                        dmgpopup.GetComponentInChildren<TMPro.TextMeshProUGUI>().color=Color.red;
                        dmgpopup.transform.localScale=new Vector2(2,2);
                        dmgpopup.GetComponentInChildren<TMPro.TextMeshProUGUI>().text=dmg.ToString();
                    }
                    DMGPopUpHud(dmg);
                    dmgTimer = dmgFreq;
                }else{ dmgTimer -= Time.deltaTime; }
            }
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
    public GameObject GetRandomizerPwrup(){return randomizerPwrupPrefab;}

    public void DMGPopUpHud(float dmg){
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
    }
}
