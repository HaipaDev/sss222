using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Sirenix.OdinInspector;

public class GameRules : MonoBehaviour{
#region//Values
public static GameRules instance;
#region//Global values
[Header("Global")]
    public string cfgName;
    public float defaultGameSpeed=1f;
    public bool crystalsOn=true;
    public bool xpOn=true;
    public bool coresOn=true;
    public bool shopOn=true;
    public bool shopCargoOn=true;
    public bool levelingOn=true;
    public bool modulesOn=true;
    public bool statUpgOn=false;
    public bool iteminvOn=true;
    public bool barrierOn=false;
    public Vector2Int EVscoreMax=new Vector2Int(30,50);
    public Vector2Int shopScoreMax=new Vector2Int(200,450);
    public float scoreMulti=1;
    public float luckMulti=1;
#endregion
#region//Player
[Header("Player")]
    public Vector2 startingPosPlayer=new Vector2(0.36f,-6.24f);
    public bool autoShootPlayer=false;
    public bool moveX=true;
    public bool moveY=true;
    public float paddingX=-0.125f;
    public float paddingY=0.45f;
    public float moveSpeedPlayer=5f;
    public float healthPlayer=150;
    public float maxHPPlayer=150;
    public bool energyOnPlayer=true;
    public float energyPlayer=180;
    public float maxEnergyPlayer=180;
    public bool ammoOn=false;
    public bool fuelOn=false;
    public float fuelDrainAmnt=0.1f;
    public float fuelDrainFreq=0.5f;
    public string powerupStarting="laser";
    public string powerupDefault="laser";
    public bool weaponsLimited;
    public bool losePwrupOutOfEn;
    public bool losePwrupOutOfAmmo;
    public float armorMultiPlayer=1f;
    public float dmgMultiPlayer=1f;
    public float shootMultiPlayer=1f;
    public float shipScaleDefault=0.89f;
    public bool bulletResize;
    public int bflameDmgTillLvl=1;
    public bool overheatOnPlayer=true;
    public float overheatTimerMax = 8.66f;
    public float overheatCooldown = 0.65f;
    public float overheatedTime=3;
    public bool recoilOnPlayer=true;
    public List<WeaponProperties> weaponProperties;
[Header("State Defaults")]
    public float flipTime = 7f;
    public float gcloverTime = 6f;
    public bool dashingEnabled = true;
    public float shadowTime = 10f;
    public float shadowLength=0.33f;
    public float shadowtracesSpeed=1.3f;
    public float shadowCost=5f;
    public float dashSpeed=10f;
    public float startDashTime=0.2f;
    public float inverterTime=10f;
    public float magnetTime=15f;
    public float scalerTime=15f;
    public float[] scalerSizes={0.45f,0.75f,1.2f,1.5f,1.75f,2f,2.5f};
    //public float shipScaleMin=0.45f;
    //public float shipScaleMax=2.5f;
    public float matrixTime=7f;
    public float pmultiTime=24f;
    public float accelTime=7f;
    public float onfireTickrate = 0.38f;
    public float onfireDmg = 1f;
    public float decayTickrate = 0.5f;
    public float decayDmg = 0.5f;
[Header("Energy Gains")]//Collectibles
    public float energyBallGet=6f;
    public float energyBatteryGet=11f;
    public float medkitEnergyGet=26f;
    public float microMedkitHpAmnt=10f;
    public float medkitHpAmnt=25f;
    public float pwrupEnergyGet=36f;
    public float enForPwrupRefill=25f;
    public float enPwrupDuplicate=42f;
    public int crystalGet=2;
    public int crystalBGet=6;
[Header("Skills")]
    public Skill[] skillsPlayer;
    public float timeOverhaul=10;
#endregion
#region//Waves & Powerups
[Header("Powerup Spawns")]
    public List<PowerupsSpawnerGR> powerupSpawners;
[Header("Waves & Disrupters")]
    public List<LootTableEntryWaves> waveList;
    public int startingWave=0;
    public bool startingWaveRandom=false;
    public bool uniqueWaves=true;
    public List<DisrupterConfig> disrupterList;
[Header("Enemies")]
    public EnemyClass[] enemies;
    public CometSettings cometSettings;
    public EnCombatantSettings enCombatantSettings;
    public EnShipSettings enShipSettings;
    public MechaLeechSettings mechaLeechSettings;
    public HealingDroneSettings healingDroneSettings;
    public VortexWheelSettings vortexWheelSettings;
    public GlareDevilSettings glareDevilSettings;
    public GoblinBossSettings goblinBossSettings;
#endregion
#region//Damage Values
[Header("Damage Values")]
    public float dmgZone=2;
    public float dmgShipFlame=1;
    public float dmgLaser=5f;
    public float dmgPhaser=0.5f;
    public float dmgHRocket=13.5f;
    public float dmgMiniLaser=0.32f;
    public float dmgLSaber=0.86f;
    public float dmgLSaberIni=7.77f;
    public float dmgLClaws=7f;
    public float dmgLClawsTouch=0.23f;
    public float dmgGloomyScythes=40.5f;
    public float dmgGloomyScythes_player=1f;
    public float dmgShadowBT=40.5f;
    public float dmgShadowBT_player=0f;
    public float dmgQRocket=14.5f;
    public float dmgPRocket=0f;
    public float dmgPRocketExpl=0.5f;
    public float dmgCBullet=2f;
    public float dmgPlaser=6.78f;
    public float dmgMPulse=130f;
    //
    public float dmgBlueFlame=0.2f;
    public float dmgComet=10f;
    public float dmgBat=36f;
    public float dmgSoundwave=16.5f;
    public float dmgEnemyShip1=80f;
    public float dmgEBt=24.5f;
    public float dmgEnSaber=2.5f;
    public float dmgGoblin=16f;
    public float dmgHealDrone=75f;
    public float dmgVortex=70f;
    public float dmgLeech=4f;
    public float dmgVLaser=90f;
    public float dmgHLaser=16f;
    public float dmgStinger=33.3f;
    public Vector2 efxStinger=new Vector2(20,1);
    public float dmgGoblinBt=7f;
    public Vector2 efxGoblinBt=new Vector2(6,0.8f);
    public float dmgGlareDev = 8f;
    public Vector2 efxGlareDev=new Vector2(1.5f,2f);
#endregion
#region//Shop
[Header("Shop")]
    public List<LootTableEntryShop> shopList;
    public float cargoSpeed=2;
    public float cargoHealth=44;
    [SerializeField] public int[] repMinusCargoHit=new int[2]{1,3};
    [SerializeField] public int repMinusCargoKill=7;
    public bool repEnabled=true;
    public const int repLength=4;
    public int[] reputationThresh=new int[repLength];
    public bool shopTimeLimitEnabled=true;
    public float shopTimeLimit=10;
#endregion
#region//Leveling
[Header("Leveling")]
    public float xpMax=100f;
    public float xp_wave=20f;
    public float xp_shop=3f;
    public float xp_powerup=1f;
    public float xp_flying=7f;
    public float flyingTimeReq=25f;
    public float xp_staying=-2f;
    public float stayingTimeReq=4f;
[Header("Changes per level")]
    public List<ListEvents> lvlEvents;
#endregion
#region//Upgrades
[Header("Upgrades")]
    public int saveBarsFromLvl=5;
    public int total_UpgradesCountMax=5;
    public int other_UpgradesCountMax=10;
    public float maxHealth_UpgradeAmnt=5f;
    public bool hpStat_enabled=true;
    public int maxHealth_UpgradeCost=1;
    public int maxHealth_UpgradesCountMax=5;
    public bool energyStat_enabled=true;
    public float maxEnergy_UpgradeAmnt=5f;
    public int maxEnergy_UpgradeCost=1;
    public int maxEnergy_UpgradesCountMax=4;
    public bool speedStat_enabled=true;
    public float speed_UpgradeAmnt=0.1f;
    public int speed_UpgradeCost=1;
    public int speed_UpgradesCountMax=5;
    public bool luckStat_enabled=true;
    public float luck_UpgradeAmnt=0.05f;
    public int luck_UpgradeCost=1;
    public int luck_UpgradesCountMax=5;
    public int defaultPowerup_upgradeCost1=1;
    public int defaultPowerup_upgradeCost2=1;
    public int defaultPowerup_upgradeCost3=4;
    //public int energyRefill_upgradeCost=2;
    //public int energyRefill_upgradeCost2=3;
    public bool mPulse_enabled=true;
    public int mPulse_upgradeCost=3;
    public int mPulse_lvlReq=2;
    public bool postMortem_enabled=true;
    public int postMortem_upgradeCost=0;
    public int postMortem_lvlReq=5;
    public bool teleport_enabled=true;
    public int teleport_upgradeCost=2;
    public int teleport_lvlReq=3;
    public bool overhaul_enabled=false;
    public int overhaul_upgradeCost=3;
    public int overhaul_lvlReq=3;
    public bool crMend_enabled=true;
    public int crMend_upgradeCost=5;
    public int crMend_lvlReq=5;
    public bool enDiss_enabled=true;
    public int enDiss_upgradeCost=4;
    public int enDiss_lvlReq=4;
    //public int[] unlockableSkills;
#endregion
#endregion
#region//Voids
    void Awake(){SetupSingleton();}
    void SetupSingleton(){
        if(GameRules.instance!=null||!(SceneManager.GetActiveScene().name=="Game"||SceneManager.GetActiveScene().name=="InfoGameMode")){Destroy(gameObject);}
        else{DontDestroyOnLoad(gameObject);instance=this;}
    }
    IEnumerator Start(){
        //Set gameModeSelected if artificially turned on gamemode etc
        yield return new WaitForSecondsRealtime(0.05f);
        if(!GameSession.instance.CheckGameModeSelected(cfgName)){
            GameSession.instance.SetGameModeSelectedStr(cfgName);}
        yield return new WaitForSecondsRealtime(0.02f);    
        if(SceneManager.GetActiveScene().name=="Game")EnterGameScene();
    }
    public void EnterGameScene(){StartCoroutine(EnterGameSceneI());}
    IEnumerator EnterGameSceneI(){
        yield return new WaitForSecondsRealtime(0.02f);
        StartCoroutine(CreateSpawners());
    }
    IEnumerator CreateSpawners(){
        //Set/Create WaveSpawner
        /*Waves ws;
        if(FindObjectOfType<Waves>()==null){
            ws=Instantiate(GameAssets.instance.waveSpawnerPrefab).GetComponent<Waves>();
            ws.name="Waves";
        }else{ws=FindObjectOfType<Waves>();}
        ws.startingWave=startingWave;
        ws.GetComponent<LootTableWaves>().itemList=waveList;
        ws.startingWaveRandom=startingWaveRandom;
        ws.uniqueWaves=uniqueWaves;

        //Set/Create DisruptersSpawner
        DisruptersSpawner ds;
        if(FindObjectOfType<DisruptersSpawner>()==null){
            ds=Instantiate(GameAssets.instance.disrupterSpawnerPrefab).GetComponent<DisruptersSpawner>();
            ds.name="DisruptersSpawner";
        }else{ds=FindObjectOfType<DisruptersSpawner>();}
        ds.disruptersList=disrupterList;
        */
        //Set/Create PowerupSpawners
        List<PowerupsSpawner> ps=new List<PowerupsSpawner>();
        if(FindObjectsOfType<PowerupsSpawner>()!=null){
            foreach(PowerupsSpawner ps1 in FindObjectsOfType<PowerupsSpawner>()){ps.Add(ps1);}
        }for(int i=FindObjectsOfType<PowerupsSpawner>().Length;i<powerupSpawners.Count;i++)ps.Add(Instantiate(GameAssets.instance.powerupSpawnerPrefab).GetComponent<PowerupsSpawner>());
        yield return new WaitForSecondsRealtime(0.005f);
        for(int i=0;i<powerupSpawners.Count;i++){
            ps[i].GetComponent<LootTablePowerups>().itemList=powerupSpawners[i].powerupList;
            ps[i].powerupsSpawner=powerupSpawners[i].psConfig;
        }
    }
    Player p;
    void Update(){
        if(Player.instance!=null&&p!=Player.instance){p=Player.instance;}
        if(!(SceneManager.GetActiveScene().name=="Game"||SceneManager.GetActiveScene().name=="InfoGameMode")){Destroy(gameObject);}
    }
    void OnValidate(){
        /*foreach(var p in powerupsSpawners){
            if(p.powerupSpawnerType==powerupSpawnerType.time){p.powerupSpawner=new powerupSpawnerTime();}
            if(p.powerupSpawnerType==powerupSpawnerType.kills){p.powerupSpawner=new powerupSpawnerKills();}
        }*/

        if(!shopOn)shopCargoOn=false;
        foreach(ListEvents le in lvlEvents){le.name="Levels: "+le.lvls.x+"-"+le.lvls.y;}
        foreach(EnemyClass e in enemies){
            //e.drops=new List<LootTableEntryDrops>();//Restart list if bugged
            e.drops[0].name="EnBall";e.drops[1].name="Coin";e.drops[2].name="PowerCore";
            if(e.drops.Count==0){
                var obj=new LootTableEntryDrops(){name="EnBall",ammount=new Vector2(1,1),dropChance=30};e.drops.Add(obj);
                obj=new LootTableEntryDrops(){name="Coin",ammount=new Vector2(1,1),dropChance=3};e.drops.Add(obj);
                obj=new LootTableEntryDrops(){name="PowerCore",ammount=new Vector2(1,1),dropChance=0};e.drops.Add(obj);
            }
        }
        cometSettings.lunarDrops[0].name="Coin";
        if(cometSettings.lunarDrops.Count==0)cometSettings.lunarDrops.Add(new LootTableEntryDrops(){name="Coin",ammount=new Vector2(6,12),dropChance=101});
    }
    #region//Custom Events
    public void MultiplyMaxHealth(float amnt){p.maxHP*=amnt;}
    public void MultiplyMaxEnergy(float amnt){p.maxEnergy*=amnt;}
    public void ShootMultiAdd(float amnt){p.shootMulti+=amnt;}
    public void ArmorMultiAdd(float amnt){p.armorMultiInit+=amnt;}
    public void LaserShootSpeed(float amnt){if(p.GetWeaponProperty("laser")!=null){var wp=(weaponTypeBullet)p.GetWeaponProperty("laser").weaponTypeProperties;wp.shootDelay=amnt;}}
    public void MLaserBulletAmnt(int amnt){if(p.GetWeaponProperty("mlaser")!=null){var wp=(weaponTypeBullet)p.GetWeaponProperty("mlaser").weaponTypeProperties;wp.bulletAmount=amnt;}}
    public void ChangeMaxXP(int amnt){GameSession.instance.xpMax=amnt;}
    #endregion
}
#endregion

#region//Custom classes
[System.Serializable]
public class PowerupsSpawnerGR{
    public List<LootTableEntryPowerup> powerupList;
    public PowerupsSpawnerConfig psConfig;
}

[System.Serializable]
public class ListEvents{
    [HideInInspector]public string name;
    public UnityEvent events=new UnityEvent();
    public Vector2 lvls;
    public bool skipRe;
}



[System.Serializable]
public class EnemyClass{
    public string name;
    public Vector2 size = Vector2.one;
    public Sprite spr;
    public float health=100;
    public bool shooting = false;
    public Vector2 shootTime=new Vector2(1.75f,2.8f);
    [AssetsOnly]public GameObject bullet;
    public float bulletSpeed = 8f;
    public bool DBullets = false;
    public float bulletDist=0.35f;
    public bool randomizeWaveDeath = false;
    public bool flyOff = false;
[Header("Drops & Points")]
    public bool giveScore = true;
    public Vector2 scoreValue=new Vector2(1,10);
    public float xpAmnt = 0f;
    public float xpChance = 100f;
    public List<LootTableEntryDrops> drops;
}
[System.Serializable]
public class CometSettings{
[Header("Basic")]
    public Vector2 sizes=new Vector2(0.4f,1.4f);
    public bool healthBySize=true;
    public bool damageBySpeedSize=true;
    public bool scoreBySize=false;
    public CometScoreSize[] scoreSizes;
    [AssetsOnly]public Sprite[] sprites;
    [AssetsOnly]public GameObject bflamePart;
[Header("Lunar")]
    public Vector2 sizeMultLunar=new Vector2(0.88f,1.55f);
    public int lunarCometChance=10;
    public float lunarHealthMulti=2.5f;
    public float lunarSpeedMulti=0.415f;
    public Vector2 lunarScore;
    public List<LootTableEntryDrops> lunarDrops;
    [AssetsOnly]public Sprite[] spritesLunar;
    [AssetsOnly]public GameObject lunarPart;
}
[System.Serializable]
public class EnCombatantSettings{
    public float speedFollowX = 3.5f;
    public float speedFollowY = 4f;
    public float vspeed = 0.1f;
    public float distY = 1.3f;
    public float distX = 0.3f;
    public float distYPlayer = 1.5f;
    [AssetsOnly]public GameObject saberPrefab;
}
[System.Serializable]
public class EnShipSettings{
    public float speedFollow = 2f;
    public float vspeed = 0.1f;
    public float distY = 1.3f;
    public float distX = 0.3f;
    public bool getClose = false;
}
[System.Serializable]
public class MechaLeechSettings{
    public float catch_distance=1.5f;
    public float shake_distance = 0.05f;
    public int count_max = 3;
    public float fallSpeed = 6f;
}
[System.Serializable]
public class HealingDroneSettings{
    [AssetsOnly]public GameObject healBallPrefab;
    public float shootFrequency=0.2f;
    public float speedBullet=4f;
[Header("Dodge")]
    public float distMin=1.6f;
    public float dodgeSpeed=2f;
    public float dodgeTime=0.5f;
}
/*[System.Serializable]
public class GoblinThiefSettings{
    
}*/
[System.Serializable]
public class VortexWheelSettings{
    public float startTimer=3f;
    public float timeToDieMin=8f;
    public float timeToDieMax=13f;
    public float chargeMultip=0.8f;
    public float chargeMultipS=1.3f;
    Sprite[] sprites;
[Header("Bullet")]
	[AssetsOnly]public GameObject projectile;
	public int numberOfProjectiles=4;
	public float radius=5;
	public float moveSpeed=5;
}
[System.Serializable]
public class GlareDevilSettings{
    public float radius=3f;
    public float timerMax=3.3f;
    public Vector2 efxBlind=new Vector2(4,4);
}

[System.Serializable]
public class GoblinBossSettings{
    public Sprite goblinBossSprite;
    public float goblinBossHP=50f;
    public List<LootTableEntryDrops> goblinBossDrops;
}


#endregion