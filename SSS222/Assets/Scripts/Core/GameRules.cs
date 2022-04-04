﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Sirenix.OdinInspector;

public class GameRules : MonoBehaviour{     public static GameRules instance;
#region//Values
#region//Global values
[Title("Global", titleAlignment: TitleAlignments.Centered)]
    public string cfgName;
    public float defaultGameSpeed=1f;
    public Material bgMaterial;
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
    public float scoreMulti=1;
    public float luckMulti=1;
#endregion
#region//Player
[Title("Player", titleAlignment: TitleAlignments.Centered)]
    public Vector2 startingPosPlayer=new Vector2(0.36f,-6.24f);
    public bool autoShootPlayer=false;
    public bool moveX=true;
    public bool moveY=true;
    public float paddingX=-0.125f;
    public float paddingY=0.45f;
    public float moveSpeedPlayer=5f;
    public float healthPlayer=150;
    public float healthMaxPlayer=150;
    public int defensePlayer=0;
    public bool energyOnPlayer=true;
    public float energyPlayer=180;
    public float energyMaxPlayer=180;
    public bool ammoOn=false;
    public bool fuelOn=false;
    public float fuelDrainAmnt=0.1f;
    public float fuelDrainFreq=0.5f;
    public List<Powerup> powerupsStarting;//={new Powerup(name:"laser")};
    [Range(1,10)]public int powerupsCapacity=5;
    /*public int powerupsCapacityStarting=2;
    public int powerupsCapacityMax=5;*/
    public string powerupDefault;
    public bool displayCurrentPowerup=true;
    public bool weaponsLimited;
    public bool losePwrupOutOfEn;
    public bool losePwrupOutOfAmmo;
    public bool slottablePowerupItems=true;
    public PowerupItemSettings[] powerupItemSettings;//={new Powerup(name:"medkit",max="5")};
    //public float armorMultiPlayer=1f;
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
    public float critChancePlayer=4f;
    public bool playerWeaponsFade=true;
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
[Header("Collectibles")]//Collectibles
    public float energyBallGet=6f;
    public float energyBatteryGet=11f;
    public float medkitEnergyGet=26f;
    public float medkitHpAmnt=25f;
    public float microMedkitHpAmnt=10f;
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
[Title("Waves & Powerups", titleAlignment: TitleAlignments.Centered)]
    public List<PowerupsSpawnerGR> powerupSpawners;
[Header("Waves & Disrupters")]
    [SerializeField]public spawnReqsType waveSpawnReqsType=spawnReqsType.score;
    #region//VaildateWaveSpawn
    [Button("VaildateWaveSpawnReqs")][ContextMenu("VaildateWaveSpawnReqs")]void VaildateWaveSpawnReqs(){spawnReqsMono.Validate(ref waveSpawnReqs, ref waveSpawnReqsType);}
    #endregion
    [SerializeReference]public spawnReqs waveSpawnReqs=new spawnScore();
    public List<LootTableEntryWaves> waveList;
    public int startingWave=0;
    public bool startingWaveRandom=false;
    public bool uniqueWaves=true;
    public List<DisrupterConfig> disrupterList;
[Title("Enemies", titleAlignment: TitleAlignments.Centered)]
    public bool enemyDefenseHit=true;
    public bool enemyDefensePhase=true;
    public EnemyClass[] enemies;
    public CometSettings cometSettings;
    public EnCombatantSettings enCombatantSettings;
    public EnShipSettings enShipSettings;
    public MechaLeechSettings mechaLeechSettings;
    public HealingDroneSettings healingDroneSettings;
    public VortexWheelSettings vortexWheelSettings;
    public GlareDevilSettings glareDevilSettings;
    public GoblinBossSettings goblinBossSettings;
    public HLaserSettings vlaserSettings;
    public HLaserSettings hlaserSettings;
#endregion
#region//Damage Values
[Title("DMG Values", titleAlignment: TitleAlignments.Centered)]
    public List<DamageValues> dmgValues;
#endregion
#region//Shop
[Title("Shop", titleAlignment: TitleAlignments.Centered)]
    public spawnReqsType shopSpawnReqsType=spawnReqsType.score;
    [Button("VaildateShopSpawnReqs")][ContextMenu("VaildateShopSpawnReqs")]void VaildateShopSpawnReqs(){spawnReqsMono.Validate(ref shopSpawnReqs, ref shopSpawnReqsType);}
    [SerializeReference]public spawnReqs shopSpawnReqs=new spawnScore();
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
[Title("Leveling", titleAlignment: TitleAlignments.Centered)]
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
[Title("Upgrades", titleAlignment: TitleAlignments.Centered)]
    [ShowIf("@this.cfgName.Contains(\"Adventure\")")]public int saveBarsFromLvl=5;
    public int total_UpgradesCountMax=5;
    public int other_UpgradesCountMax=10;
    public float healthMax_UpgradeAmnt=5f;
    public bool hpStat_enabled=true;
    public int healthMax_UpgradeCost=1;
    public int healthMax_UpgradesCountMax=5;
    public bool energyStat_enabled=true;
    public float energyMax_UpgradeAmnt=5f;
    public int energyMax_UpgradeCost=1;
    public int energyMax_UpgradesCountMax=4;
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
    void Awake(){if(GameRules.instance!=null){Destroy(gameObject);}else{DontDestroyOnLoad(gameObject);instance=this;}}
    IEnumerator Start(){
        if(gameObject.name.Contains("(Clone)")){gameObject.name.Replace("(Clone)","");}
        //Set gameModeSelected if artificially turned on gamemode etc
        yield return new WaitForSecondsRealtime(0.05f);
        if(!GameSession.instance.CheckGamemodeSelected(cfgName)){
            GameSession.instance.SetGamemodeSelectedStr(cfgName);}
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
        if(waveList.Count>0){
            Waves ws;
            if(FindObjectOfType<Waves>()==null){
                ws=Instantiate(GameAssets.instance.waveSpawnerPrefab).GetComponent<Waves>();
                ws.name="Waves";
            }else{ws=FindObjectOfType<Waves>();}
            yield return new WaitForSecondsRealtime(0.005f);
            ws.startingWave=startingWave;
            ws.GetComponent<LootTableWaves>().itemList=waveList;
            ws.startingWaveRandom=startingWaveRandom;
            ws.uniqueWaves=uniqueWaves;
        }

        //Set/Create DisruptersSpawner
        if(disrupterList.Count>0){
            DisruptersSpawner ds;
            if(FindObjectOfType<DisruptersSpawner>()==null){
                ds=Instantiate(GameAssets.instance.disrupterSpawnerPrefab).GetComponent<DisruptersSpawner>();
                ds.name="DisruptersSpawner";
            }else{ds=FindObjectOfType<DisruptersSpawner>();}
            yield return new WaitForSecondsRealtime(0.005f);
            ds.disruptersList=disrupterList;
        }
        
        //Set/Create PowerupSpawners
        if(powerupSpawners.Count>0){
            List<PowerupsSpawner> ps=new List<PowerupsSpawner>();
            if(FindObjectsOfType<PowerupsSpawner>()!=null){
                foreach(PowerupsSpawner ps1 in FindObjectsOfType<PowerupsSpawner>()){ps.Add(ps1);}
            }for(int i=FindObjectsOfType<PowerupsSpawner>().Length;i<powerupSpawners.Count;i++)ps.Add(Instantiate(GameAssets.instance.powerupSpawnerPrefab).GetComponent<PowerupsSpawner>());
            yield return new WaitForSecondsRealtime(0.005f);
            for(int i=0;i<powerupSpawners.Count;i++){if(powerupSpawners[i].powerupList.Count>0){
                ps[i].GetComponent<LootTablePowerups>().itemList=powerupSpawners[i].powerupList;
                ps[i].powerupsSpawner=powerupSpawners[i].psConfig;
                ps[i].powerupSpawnPosRange=powerupSpawners[i].powerupSpawnPosRange;
            }}
        }
    }
    Player p;
    void Update(){
        if(Player.instance!=null&&p!=Player.instance){p=Player.instance;}
        if(!(SceneManager.GetActiveScene().name=="Game"||SceneManager.GetActiveScene().name=="InfoGameMode"||SceneManager.GetActiveScene().name=="AdventureZones"||SceneManager.GetActiveScene().name=="SandboxMode")){
            Destroy(gameObject);}
        CapToMaxValues();
    }
    void OnValidate(){
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

        /*foreach(DamageValues dmgVal in dmgValues){
            foreach(colliEventsClass co in dmgVal.colliEvents){
                if(co.colliEventsType==colliEventsType.vfx){co.colliEvents=new colliEvent_VFX();}
                if(co.colliEventsType==colliEventsType.playerDmg){co.colliEvents=new colliEvent_PlayerDmg();}
            }
        }*/
        CapToMaxValues();
    }
    void CapToMaxValues(){
        healthPlayer=Mathf.Clamp(healthPlayer,0,healthMaxPlayer);
        //if(healthMaxPlayer<=0){healthMaxPlayer=0.1f;}
        energyPlayer=Mathf.Clamp(energyPlayer,0,energyMaxPlayer);
        if(!energyOnPlayer){energyPlayer=0;energyMaxPlayer=0;}
        //if(energyMaxPlayer<=0){energyMaxPlayer=0.1f;}

        foreach(EnemyClass en in enemies){
            en.healthStart=Mathf.Clamp(en.healthStart,0,en.healthMax);
        }

        if(!shopOn)shopCargoOn=false;
        if(!xpOn)levelingOn=false;
    }
    #region//Custom Events
    public void MultiplyhealthMax(float amnt){p.healthMax*=amnt;}
    public void MultiplyenergyMax(float amnt){p.energyMax*=amnt;}
    //public void ArmorMultiAdd(float amnt){p.armorMultiInit+=amnt;}
    public void DmgMultiAdd(float amnt){p.dmgMultiInit+=amnt;}
    public void ShootMultiAdd(float amnt){p.shootMulti+=amnt;}
    public void LaserShootSpeed(float amnt){if(p.GetWeaponProperty("laser")!=null){var wp=(weaponTypeBullet)p.GetWeaponProperty("laser").weaponTypeProperties;wp.shootDelay=amnt;}}
    public void MLaserBulletAmnt(int amnt){if(p.GetWeaponProperty("mlaser")!=null){var wp=(weaponTypeBullet)p.GetWeaponProperty("mlaser").weaponTypeProperties;wp.bulletAmount=amnt;}}
    public void ChangeMaxXP(int amnt){GameSession.instance.xpMax=amnt;}
    #endregion
#endregion
#region//Return functions
    public PowerupItemSettings GetItemSettings(string name){PowerupItemSettings p=null;p=Array.Find(powerupItemSettings,x=>x.name==name);return p;}
#endregion
}
#region//Custom classes
[System.Serializable]
public class PowerupsSpawnerGR{
    public List<LootTableEntryPowerup> powerupList;
    public PowerupsSpawnerConfig psConfig;
    public Vector2 powerupSpawnPosRange=new Vector2(-3f,3f);
}

[System.Serializable]
public class ListEvents{
    [HideInInspector]public string name;
    public UnityEvent events=new UnityEvent();
    public Vector2 lvls;
    public bool skipRe;
}



[System.Serializable]
public class DamageValues{
    public string name;
    public colliTypes colliType=colliTypes.playerWeapons;
    public float dmg=1f;
    public bool dmgBySize=false;
    public bool dmgBySpeed=false;
    public int armorPenetr=0;
    public bool phase=false;
    [HideIf("@this.phase == false")]public float dmgPhase=0.5f;
    [HideIf("@this.phase == false")]public float phaseFreqFirst=0f;
    [HideIf("@this.phase == false")]public float phaseFreq=0.33f;
    [HideIf("@this.phase == false")]public int phaseCountLimit=0;
    [HideIf("@this.colliType==colliTypes.player || this.colliType==colliTypes.playerWeapons")]public bool dmgFx=false;
    [HideIf("@this.dmgFx==false || (this.colliType==colliTypes.player || this.colliType==colliTypes.playerWeapons)")]
    public DmgFxValues[] dmgFxValues;
    public string sound="EnemyHit";
    public string soundPhase="";
    public bool dispDmgCount=false;
    public colliEvents[] colliEvents;
    //public colliEventsClass[] colliEvents;
}
public enum dmgFxType{fire,decay,electrc,freeze,armor,fragile,power,weak,hack,blind,speed,slow,infenergy}
[System.Serializable]
public class DmgFxValues{
    public dmgFxType dmgFxType;
    //public dmgFxReqs[] dmgFxReqs;
    public float length=1f;
    public float power=1f;
    public bool onPhase=false;
    public float chance=100f;
}
[System.Serializable]
public class colliEvents{
    public bool onPhase=false;
    public string vfx="";
    public Vector2 vfxPos=Vector2.zero;
    public float dmgPlayer=0f;
    public dmgType dmgPlayerType=dmgType.silent;
    public float healBeamPlayer=0f;
    public string assetMake="";
    public Vector2 assetPos=Vector2.zero;
}
/*[System.Serializable]
public class colliEventsClass{
    public colliEventsType colliEventsType;
    public colliEvents colliEvents;
}
public enum colliEventsType{vfx,playerDmg}
[System.Serializable]
public class colliEvents{
    public bool onPhase=false;
}
[System.Serializable]
public class colliEvent_VFX:colliEvents{
    public string name="ExplosionSmall";
    public Vector2 pos;
}
[System.Serializable]
public class colliEvent_PlayerDmg:colliEvents{
    public float dmg=1;
}*/


/*public class dmgFxReqs{}
public class dmgFxReqs_angle:dmgFxReqs{
    public float angleP;
}*/


[System.Serializable]
public class Powerup{
    public string name;
    public int ammo=-5;//-5 is infinite, -6 protects it from being replaced
    public float timer=-4;//-4 is basically off, -5 is infinite
}
[System.Serializable]
public class PowerupItemSettings{
    public string name;
    public int max=5;
}


[System.Serializable]
public class EnemyClass{
    public string name;
    public enemyType type;
    public Vector2 size = Vector2.one;
    public Sprite spr;
    public Material sprMat;
    public float healthStart=25;
    public float healthMax=25;
    public int defense=0;
    public bool healthBySize=false;
    public bool shooting = false;
    public Vector2 shootTime=new Vector2(1.75f,2.8f);
    [AssetsOnly]public GameObject bullet;
    public float bulletSpeed = 8f;
    public bool DBullets = false;
    public float bulletDist=0.35f;
    public bool randomizeWaveDeath = false;
    public bool flyOff = false;
    public bool killOnDash=true;
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
    public bool scoreBySize=false;
    public CometScoreSize[] scoreSizes;
    [AssetsOnly]public Sprite[] sprites;
[Header("Lunar")]
    public Vector2 sizeMultLunar=new Vector2(0.88f,1.55f);
    public int lunarCometChance=10;
    public float lunarHealthMulti=2.5f;
    public float lunarSpeedMulti=0.415f;
    public Vector2 lunarScore;
    public List<LootTableEntryDrops> lunarDrops;
    [AssetsOnly]public Sprite[] spritesLunar;
    [AssetsOnly]public string lunarPart="BFlameLunar";
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
    public float radiusBlind=3f;
    public float timerBlindMax=3.3f;
    public Vector2 efxBlind=new Vector2(4,4);
}
[System.Serializable]
public class GoblinBossSettings{
    public Sprite goblinBossSprite;
    public float goblinbossHealth=50f;
    public List<LootTableEntryDrops> goblinBossDrops;
}
[System.Serializable]
public class HLaserSettings{
    public float timerWarn=0.8f;
    public float timerCharging=1f;
    public float timerStay=3.3f;
    public RuntimeAnimatorController chargingAnimation;
    public RuntimeAnimatorController hlaserAnimation;
}


#endregion