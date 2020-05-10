using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PlayerCollider : MonoBehaviour{
    [HeaderAttribute("Powerups")]
    [SerializeField] GameObject CoinPrefab;
    [SerializeField] GameObject enBallPrefab;
    [SerializeField] GameObject powercorePrefab;
    [SerializeField] GameObject armorPwrupPrefab;
    [SerializeField] GameObject armorUPwrupPrefab;
    [SerializeField] GameObject laser2PwrupPrefab;
    [SerializeField] GameObject laser3PwrupPrefab;
    [SerializeField] GameObject phaserPwrupPrefab;
    [SerializeField] GameObject hrocketPwrupPrefab;
    [SerializeField] GameObject mlaserPwrupPrefab;
    [SerializeField] GameObject lsaberPwrupPrefab;
    [SerializeField] GameObject lclawsPwrupPrefab;
    [SerializeField] GameObject flipPwrupPrefab;
    [SerializeField] GameObject gcloverPwrupPrefab;
    [SerializeField] GameObject shadowPwrupPrefab;
    [SerializeField] GameObject shadowBTPwrupPrefab;
    [SerializeField] GameObject qrocketPwrupPrefab;
    [SerializeField] GameObject procketPwrupPrefab;
    [SerializeField] GameObject cstreamPwrupPrefab;
    [SerializeField] GameObject inverterPwrupPrefab;
    [SerializeField] GameObject magnetPwrupPrefab;
    [SerializeField] GameObject scalerPwrupPrefab;
    [SerializeField] GameObject matrixPwrupPrefab;
    [SerializeField] GameObject pmultiPwrupPrefab;
    [SerializeField] GameObject randomizerPwrupPrefab;
    [HeaderAttribute("Damage Dealers")]
    [SerializeField] GameObject cometPrefab;
    [SerializeField] GameObject batPrefab;
    [SerializeField] GameObject enShip1Prefab;
    [SerializeField] GameObject soundwavePrefab;
    [SerializeField] GameObject EBtPrefab;
    [SerializeField] GameObject leechPrefab;
    [SerializeField] GameObject hlaserPrefab;
    [SerializeField] GameObject goblinPrefab;
    [SerializeField] GameObject hdronePrefab;
    [HeaderAttribute("Other")]
    [SerializeField] float dmgFreq=0.38f;
    public float dmgTimer;

    Player player;
    GameSession gameSession;
    AudioSource myAudioSource;
    AudioMixer mixer;
    AudioSource PlayClipAt(AudioClip clip, Vector2 pos)
    {
        GameObject tempGO = new GameObject("TempAudio"); // create the temp object
        tempGO.transform.position = pos; // set its position
        AudioSource aSource = tempGO.AddComponent<AudioSource>(); // add an audio source
        aSource.clip = clip; // define the clip
                             // set other aSource properties here, if desired
        _OutputMixer = "SoundVolume";
        aSource.outputAudioMixerGroup = myAudioSource.outputAudioMixerGroup;
        aSource.Play(); // start the sound
        MonoBehaviour.Destroy(tempGO, aSource.clip.length); // destroy object after clip duration (this will not account for whether it is set to loop)
        return aSource; // return the AudioSource reference
    }
    string _OutputMixer;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>().GetComponent<Player>();
        gameSession = FindObjectOfType<GameSession>();
        myAudioSource = GetComponent<AudioSource>();
        mixer = Resources.Load("MainMixer") as AudioMixer;
        _OutputMixer = "SoundVolume";
        //GetComponent<AudioSource>().outputAudioMixerGroup = mixer.FindMatchingGroups(_OutputMixer)[0];
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

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

                var goblinName = goblinPrefab.name; var goblinName1 = goblinPrefab.name + "(Clone)";
                if (other.gameObject.name == goblinName || other.gameObject.name == goblinName1) { dmg = damageDealer.GetDamageGoblin(); en = true; }
                var hdroneName = hdronePrefab.name; var hdroneName1 = hdronePrefab.name + "(Clone)";
                if (other.gameObject.name == hdroneName || other.gameObject.name == hdroneName1) { dmg = damageDealer.GetDamageHealDrone(); en = true; }


                var Sname = soundwavePrefab.name; var Sname1 = soundwavePrefab.name + "(Clone)";
                if (other.gameObject.name == Sname || other.gameObject.name == Sname1) { dmg = damageDealer.GetDamageSoundwave(); PlayClipAt(player.soundwaveHitSFX, new Vector2(transform.position.x, transform.position.y)); }
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
                        PlayClipAt(player.shipHitSFX, new Vector2(transform.position.x, transform.position.y));
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
                    PlayClipAt(player.shipHitSFX, new Vector2(transform.position.x, transform.position.y));
                }

                
            }
            #endregion
            #region//Powerups
            else if (other.gameObject.CompareTag("Powerups"))
            {
                var enBallName = enBallPrefab.name; var enBallName1 = enBallPrefab.name + "(Clone)";
                if (other.gameObject.name == enBallName || other.gameObject.name == enBallName1) { player.energy += player.energyBallGet; }

                var CoinName = CoinPrefab.name; var CoinName1 = CoinPrefab.name + "(Clone)";
                if (other.gameObject.name == CoinName || other.gameObject.name == CoinName1) { gameSession.coins += 1; }

                var powercoreName = powercorePrefab.name; var powercoreName1 = powercorePrefab.name + "(Clone)";
                if (other.gameObject.name == powercoreName || other.gameObject.name == powercoreName1) { gameSession.cores += 1; }

                if((other.gameObject.name != enBallName && other.gameObject.name != enBallName1) && (other.gameObject.name != CoinName && other.gameObject.name != CoinName1) && (other.gameObject.name != powercoreName && other.gameObject.name != powercoreName1)){
                    gameSession.AddXP(gameSession.xp_powerup);}//XP For powerups

                var armorName = armorPwrupPrefab.name; var armorName1 = armorPwrupPrefab.name + "(Clone)";
                if (other.gameObject.name == armorName || other.gameObject.name == armorName1) { if(player.health>(player.maxHP-25)){gameSession.AddToScoreNoEV(Mathf.RoundToInt(player.maxHP - player.health)*2);} player.health += player.medkitHpAmnt; player.energy += player.medkitEnergyGet; player.healed = true; }
                var armorUName = armorUPwrupPrefab.name; var armorUName1 = armorUPwrupPrefab.name + "(Clone)";
                if (other.gameObject.name == armorUName || other.gameObject.name == armorUName1) { if (player.health>(player.maxHP-30)) {gameSession.AddToScoreNoEV(Mathf.RoundToInt(player.maxHP - player.health)*2);} player.health += player.medkitUHpAmnt; player.energy += player.medkitUEnergyGet; player.healed = true; }

                var flipName = flipPwrupPrefab.name; var flipName1 = flipPwrupPrefab.name + "(Clone)";
                if (other.gameObject.name == flipName || other.gameObject.name == flipName1) { player.flip = true; player.flipTimer = player.flipTime; }
                
                var inverterName = inverterPwrupPrefab.name; var inverterName1 = inverterPwrupPrefab.name + "(Clone)";
                if (other.gameObject.name == inverterName || other.gameObject.name == inverterName1) {
                var tempHP = player.health; var tempEn = player.energy;
                player.energy=tempHP; player.health=tempEn;
                player.inverted=true; player.inverterTimer = 0; }

                var magnetName = magnetPwrupPrefab.name; var magnetName1 = magnetPwrupPrefab.name + "(Clone)";
                if (other.gameObject.name == magnetName || other.gameObject.name == magnetName1) {
                    if(player.energy<=player.enForPwrupRefill){player.energy += player.pwrupEnergyGet;}
                    if(player.magnetized==true){player.energy+=player.enPwrupDuplicate;}
                    player.magnetized = true; player.magnetTimer = player.magnetTime; 
                    
                    }
                
                var scalerName = scalerPwrupPrefab.name; var scalerName1 = scalerPwrupPrefab.name + "(Clone)";
                if (other.gameObject.name == scalerName || other.gameObject.name == scalerName1) {
                    if(player.energy<=player.enForPwrupRefill){player.energy += player.pwrupEnergyGet;}
                    if(player.scaler==true){player.energy+=player.enPwrupDuplicate;}
                    player.scaler = true; player.scalerTimer = player.scalerTime;
                    }

                var gcloverName = gcloverPwrupPrefab.name; var gcloverName1 = gcloverPwrupPrefab.name + "(Clone)";
                if (other.gameObject.name == gcloverName || other.gameObject.name == gcloverName1)
                {
                    player.gclover = true; player.gcloverTimer = player.gcloverTime;
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
                    if(player.shadow==true){player.energy+=player.enPwrupDuplicate;}
                    player.shadow = true;
                    player.shadowTimer = player.shadowTime;
                    player.shadowed = true;
                    //GameObject gcloverexVFX = Instantiate(gcloverVFX, new Vector2(0, 0), Quaternion.identity);
                    //GameObject gcloverexOVFX = Instantiate(shadowEVFX, new Vector2(0, 0), Quaternion.identity);
                    //Destroy(gcloverexVFX, 1f);
                    //Destroy(gcloverexOVFX, 1f);
                }
                var matrixName = matrixPwrupPrefab.name; var matrixName1 = matrixPwrupPrefab.name + "(Clone)";
                if (other.gameObject.name == matrixName || other.gameObject.name == matrixName1) { player.matrix = true; player.matrixTimer = player.matrixTime; }
                var pmultiName = pmultiPwrupPrefab.name; var pmultiName1 = pmultiPwrupPrefab.name + "(Clone)";
                if (other.gameObject.name == pmultiName || other.gameObject.name == pmultiName1) { if(player.pmultiTimer<0){player.pmultiTimer=0;}player.pmultiTimer += player.pmultiTime; gameSession.scoreMulti=2f;}
                
                var randomizerName = randomizerPwrupPrefab.name; var randomizerName1 = randomizerPwrupPrefab.name + "(Clone)";
                if (other.gameObject.name == randomizerName || other.gameObject.name == randomizerName1) { 
                    var item = other.GetComponent<LootTable>().GetItem();
                    Instantiate(item.gameObject,new Vector2(other.transform.position.x,other.transform.position.y),Quaternion.identity);
                    Destroy(other.gameObject,0.01f);
                 }


                var laser2Name = laser2PwrupPrefab.name; var laser2Name1 = laser2PwrupPrefab.name + "(Clone)";
                if (other.gameObject.name == laser2Name || other.gameObject.name == laser2Name1) { if(player.energy<=player.enForPwrupRefill){player.energy += player.pwrupEnergyGet;} if(player.powerup=="laser2"){player.energy+=player.enPwrupDuplicate;} player.powerup = "laser2"; }

                var laser3Name = laser3PwrupPrefab.name; var laser3Name1 = laser3PwrupPrefab.name + "(Clone)";
                if (other.gameObject.name == laser3Name || other.gameObject.name == laser3Name1) { if(player.energy<=player.enForPwrupRefill){player.energy += player.pwrupEnergyGet;} if(player.powerup=="laser3"){player.energy+=player.enPwrupDuplicate;} player.powerup = "laser3"; }

                var phaserName = phaserPwrupPrefab.name; var phaserName1 = phaserPwrupPrefab.name + "(Clone)";
                if (other.gameObject.name == phaserName || other.gameObject.name == phaserName1) { if(player.energy<=player.enForPwrupRefill){player.energy += player.pwrupEnergyGet;} if(player.powerup=="phaser"){player.energy+=player.enPwrupDuplicate;} player.powerup = "phaser"; }

                var hrocketName = hrocketPwrupPrefab.name; var hrocketName1 = hrocketPwrupPrefab.name + "(Clone)";
                if (other.gameObject.name == hrocketName || other.gameObject.name == hrocketName1) { if(player.energy<=player.enForPwrupRefill){player.energy += player.pwrupEnergyGet;} if(player.powerup=="hrockets"){player.energy+=player.enPwrupDuplicate;} player.powerup = "hrockets"; }

                var minilaserName = mlaserPwrupPrefab.name; var minilaserName1 = mlaserPwrupPrefab.name + "(Clone)";
                if (other.gameObject.name == minilaserName || other.gameObject.name == minilaserName1) { if(player.energy<=player.enForPwrupRefill){player.energy += player.pwrupEnergyGet;} if(player.powerup=="mlaser"){player.energy+=player.enPwrupDuplicate;} player.powerup = "mlaser"; }

                var lsaberWName1 = player.lsaberPrefab.name;
                var lclawsWName1 = player.lclawsPrefab.name;
                var lsaberName = lsaberPwrupPrefab.name; var lsaberName1 = lsaberPwrupPrefab.name + "(Clone)";
                if (other.gameObject.name == lsaberName || other.gameObject.name == lsaberName1) { if(player.energy<=player.enForPwrupRefill){player.energy += player.pwrupEnergyGet;} Destroy(GameObject.Find(lclawsWName1)); if(player.powerup=="lsaber"){player.energy+=player.enPwrupDuplicate;} player.powerup = "lsaber";  }
                
                var lclawsName = lclawsPwrupPrefab.name; var lclawsName1 = lclawsPwrupPrefab.name + "(Clone)";
                if (other.gameObject.name == lclawsName || other.gameObject.name == lclawsName1) { if(player.energy<=player.enForPwrupRefill){player.energy += player.pwrupEnergyGet;} Destroy(GameObject.Find(lsaberWName1)); if(player.powerup=="lclaws"){player.energy+=player.enPwrupDuplicate;} player.powerup = "lclaws"; }

                var shadowbtName = shadowBTPwrupPrefab.name; var shadowbtName1 = shadowBTPwrupPrefab.name + "(Clone)";
                if (other.gameObject.name == shadowbtName || other.gameObject.name == shadowbtName1) { if(player.energy<=player.enForPwrupRefill){player.energy += player.pwrupEnergyGet;} if(player.powerup=="shadowbt"){player.energy+=player.enPwrupDuplicate;} player.powerup = "shadowbt"; }

                var qrocketName = qrocketPwrupPrefab.name; var qrocketName1 = qrocketPwrupPrefab.name + "(Clone)";
                if (other.gameObject.name == qrocketName || other.gameObject.name == qrocketName1) { if(player.energy<=player.enForPwrupRefill){player.energy += player.pwrupEnergyGet;} if(player.powerup=="qrockets"){player.energy+=player.enPwrupDuplicate;} player.powerup = "qrockets"; }
                var procketName = procketPwrupPrefab.name; var procketName1 = procketPwrupPrefab.name + "(Clone)";
                if (other.gameObject.name == procketName || other.gameObject.name == procketName1) { if(player.energy<=player.enForPwrupRefill){player.energy += player.pwrupEnergyGet;} if(player.powerup=="prockets"){player.energy+=player.enPwrupDuplicate;} player.powerup = "prockets"; }

                var cstreamName = cstreamPwrupPrefab.name; var cstreamName1 = cstreamPwrupPrefab.name + "(Clone)";
                if (other.gameObject.name == cstreamName || other.gameObject.name == cstreamName1) { if(player.energy<=player.enForPwrupRefill){player.energy += player.pwrupEnergyGet;} if(player.powerup=="cstream"){player.energy+=player.enPwrupDuplicate;} player.powerup = "cstream"; }


                if (other.gameObject.name == enBallName || other.gameObject.name == enBallName1)
                {
                    PlayClipAt(player.energyBallSFX, new Vector2(transform.position.x, transform.position.y));
                }
                else if (other.gameObject.name == CoinName || other.gameObject.name == CoinName1)
                {
                    PlayClipAt(player.coinSFX, new Vector2(transform.position.x, transform.position.y));
                }else if (other.gameObject.name == powercoreName || other.gameObject.name == powercoreName1)
                {
                    PlayClipAt(gameSession.lvlUpSFX, new Vector2(transform.position.x, transform.position.y));
                }
                else if (other.gameObject.name == gcloverName || other.gameObject.name == gcloverName1)
                {
                    PlayClipAt(player.gcloverSFX, new Vector2(transform.position.x, transform.position.y));
                }
                else if (other.gameObject.name == shadowbtName || other.gameObject.name == shadowbtName1)
                {
                    PlayClipAt(player.shadowbtPwrupSFX, new Vector2(transform.position.x, transform.position.y));
                }else if (other.gameObject.name == matrixName || other.gameObject.name == matrixName1)
                {
                    PlayClipAt(player.matrixGetSFX, new Vector2(transform.position.x, transform.position.y));
                }
                else
                {
                    //SoundManager.PlaySound(SoundManager.Sound.powerupSFX);
                    PlayClipAt(player.powerupSFX, new Vector2(transform.position.x, transform.position.y));
                }
                Destroy(other.gameObject, 0.05f);
            }
            #endregion
        }
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

                if (dmgTimer<=0){
                    player.health -= dmg;
                    player.damaged = true;
                    if (other.gameObject.name == leechName || other.gameObject.name == leechName1){PlayClipAt(player.leechBiteSFX, new Vector2(transform.position.x, transform.position.y));}
                    //var flare = Instantiate(player.flareHitVFX, new Vector2(other.transform.position.x, transform.position.y + 0.5f), Quaternion.identity);
                    //Destroy(flare.gameObject, 0.3f);
                    dmgTimer = dmgFreq;
                }else{ dmgTimer -= Time.deltaTime; }
            }
        }
    }
    private void OnCollisionExit(Collision other){
        if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("EnemyBullet"))
        {
            dmgTimer = dmgFreq;
        }
    }
    public GameObject GetRandomizerPwrup(){return randomizerPwrupPrefab;}
}
