using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Sirenix.OdinInspector;

public class GameRules : MonoBehaviour{     [ES3NonSerializable]public static GameRules instance;
    public bool _isAdventure(){return this.cfgName.Contains("Adventure");}
    public bool _isAdventureNotSubZone(){return this.cfgName.Contains("Adventure")&&!_isAdventureSubZone;}
#region//Values
#region//Global values
[Title("Preset Settings", titleAlignment: TitleAlignments.Centered)]
    [ES3NonSerializable]public string cfgName;
    ///*[ShowIf("@this._isAdventureSubZone]*/[ShowIf("@this.cfgName.Contains(\"Adventure\")")][ES3NonSerializable]public int adventureZoneID;
    [ShowIf("@this._isAdventure()")][DisableIf("@this._isAdventureBossZone||this._isAdventureTravelZone")][ES3NonSerializable]public bool _isAdventureSubZone;
    [ShowIf("@this._isAdventureSubZone&&this._isAdventureBossZone==false")][ES3NonSerializable]public bool _isAdventureTravelZone;
    [ShowIf("@this._isAdventureSubZone&&this._isAdventureTravelZone==false")][ES3NonSerializable]public bool _isAdventureBossZone;
    [ES3NonSerializable][HideIf("@this._isAdventureSubZone")][MultiLineProperty]public string cfgDesc;
    [ES3NonSerializable][HideIf("@this._isAdventureSubZone&&this._isAdventureBossZone==false")]public string cfgIconAssetName;
    [HideIf("@this._isAdventureSubZone&&this._isAdventureBossZone==false")][InfoBox("Place a special GameObject with multiple icons here:")][AssetsOnly,ES3NonSerializable]public GameObject cfgIconsGo;
    [ES3NonSerializable][HideIf("@this._isAdventureSubZone")]public ShaderMatProps cfgIconShaderMatProps;
    [HideIf("@this._isAdventureTravelZone||this.cfgName.Contains(\"Adventure\")&&this._isAdventureSubZone==false")]public ShaderMatProps bgMaterial;
[Title("Global", titleAlignment: TitleAlignments.Centered)]
    //[HideIfGroup("Global",Condition="_isAdventureZone")]
    [HideIf("@this._isAdventureNotSubZone()")][FoldoutGroup("Global",false)][Range(0.1f,10f)]public float defaultGameSpeed=1f;
    [HideIf("@this._isAdventureNotSubZone()")][FoldoutGroup("Global")]public scoreDisplay scoreDisplay=scoreDisplay.score;
    [ShowIf(nameof(scoreDisplay), scoreDisplay.sessionTimeAsDistance)][FoldoutGroup("Global")]public int secondToDistanceRatio=100;
    [ShowIf("@this._isAdventureTravelZone")][FoldoutGroup("Global")][ES3NonSerializable]public float travelTimeToDistanceRatio=1;
    [ShowIf("@this._isAdventureTravelZone")][FoldoutGroup("Global")][ES3NonSerializable]public float travelTimeToAddOnDeath=0;
    [ShowIf("@this._isAdventureTravelZone")][FoldoutGroup("Global")][ES3NonSerializable]public float killSubTravelTime=3;
    [ShowIf("@this._isAdventureNotSubZone()")][FoldoutGroup("Global")][ES3NonSerializable]public float holodeathCrystalsRatio=1;
    [ShowIf("@this._isAdventureNotSubZone()")][FoldoutGroup("Global")][ES3NonSerializable]public float holodeathTimeRatio=1;
    [ShowIf("@this._isAdventureNotSubZone()")][FoldoutGroup("Global")][ES3NonSerializable]public float holobodyHeal=15;
    [HideIf("_isAdventureSubZone")][FoldoutGroup("Global")]public bool crystalsOn=true;
    [HideIf("_isAdventureSubZone")][FoldoutGroup("Global")]public bool xpOn=true;
    [HideIf("_isAdventureSubZone")][FoldoutGroup("Global")]public bool coresOn=true;
    [HideIf("_isAdventureSubZone")][FoldoutGroup("Global")]public bool shopOn=true;
    [HideIf("_isAdventureSubZone")][FoldoutGroup("Global")][DisableIf("@this.shopOn==false")]public bool shopCargoOn=true;
    [HideIf("_isAdventureSubZone")][FoldoutGroup("Global")][DisableIf("@this.xpOn==false")]public bool levelingOn=true;
    [HideIf("_isAdventureSubZone")][FoldoutGroup("Global")][DisableIf("@this.levelingOn==false||this.xpOn==false")]public bool forceAutoAscend=false;
    [HideIf("_isAdventureSubZone")][FoldoutGroup("Global")]public bool modulesOn=true;
    [HideIf("_isAdventureSubZone")][FoldoutGroup("Global")]public bool statUpgOn=false;
    //[HideIf("_isAdventureSubZone")][FoldoutGroup("Global")]public bool iteminvOn=true;
    [HideIf("_isAdventureSubZone")][FoldoutGroup("Global")]public bool barrierOn=false;
    [HideIf("_isAdventureSubZone")][FoldoutGroup("Global")]public bool breakEncounter=false;

    [HideIf("_isAdventureSubZone")][FoldoutGroup("Global")]public bool instaPause=true;
    [HideIf("_isAdventureSubZone")][FoldoutGroup("Global")]public bool musicSlowdownOnPause=true;
    [HideIf("_isAdventureSubZone")][FoldoutGroup("Global")]public bool musicSlowdownOnPaceChange=true;
    //public float upgradeMenuOpenGameSpeed=0;
    //public float shopOpenGameSpeed=0;

    [HideIf("_isAdventureSubZone")][FoldoutGroup("Global")]public float scoreMulti=1;
    [HideIf("_isAdventureSubZone")][FoldoutGroup("Global")]public float luckMulti=1;
#endregion
    [HideIf("_isAdventureSubZone")][Searchable]public List<DamageValues> dmgValues;
#region//Player
[Title("Player", titleAlignment: TitleAlignments.Centered)]
    [FoldoutGroup("Player",false,VisibleIf="@this._isAdventureSubZone==false")]public Vector2 startingPosPlayer=new Vector2(0.36f,-6.24f);
    [FoldoutGroup("Player")]public bool autoShootPlayer=false;
    [FoldoutGroup("Player")]public bool playerTimeIndependentFromDelta=true;
    [FoldoutGroup("Player")]public bool moveX=true;
    [FoldoutGroup("Player")]public bool moveY=true;
    [FoldoutGroup("Player")]public Vector2 playfieldPadding=new Vector2(-0.125f,0.45f);
    [FoldoutGroup("Player")]public float moveSpeedPlayer=5f;
    [FoldoutGroup("Player")]public ShaderMatProps playerShaderMatProps;
    [FoldoutGroup("Player")]public float healthPlayer=150;
    [FoldoutGroup("Player")]public float healthMaxPlayer=150;
    [FoldoutGroup("Player")]public int defensePlayer=0;
    [FoldoutGroup("Player")]public bool energyOnPlayer=true;
    [FoldoutGroup("Player")]public float energyPlayer=180;
    [FoldoutGroup("Player")]public float energyMaxPlayer=180;
    [FoldoutGroup("Player")]public int hpAbsorpFractionCap=4;
    [FoldoutGroup("Player")]public int enAbsorpFractionCap=4;
    [FoldoutGroup("Player")]public List<Powerup> powerupsStarting;//={new Powerup(name:"laser")};
    [FoldoutGroup("Player")][Range(1,10)]public int powerupsCapacity=5;
    /*public int powerupsCapacityStarting=2;
    public int powerupsCapacityMax=5;*/
    [FoldoutGroup("Player")]public string powerupDefault;
    [FoldoutGroup("Player")]public bool displayCurrentPowerup=true;
    [FoldoutGroup("Player")]public bool weaponsLimited;
    [FoldoutGroup("Player")]public bool losePwrupOutOfEn;
    [FoldoutGroup("Player")]public bool losePwrupOutOfAmmo;
    [FoldoutGroup("Player")]public bool slottablePowerupItems=true;
    [FoldoutGroup("Player")]public PowerupItemSettings[] powerupItemSettings;//={new Powerup(name:"medkit",max="5")};
    //public float armorMultiPlayer=1f;
    [FoldoutGroup("Player")]public float dmgMultiPlayer=1f;
    [FoldoutGroup("Player")]public float shootMultiPlayer=1f;
    [FoldoutGroup("Player")]public float shipScaleDefault=0.89f;
    [FoldoutGroup("Player")]public bool bulletResize;
    [FoldoutGroup("Player")]public bool overheatOnPlayer=true;
    [FoldoutGroup("Player")]public bool overheatShaderIdentif=true;
    [FoldoutGroup("Player")]public float overheatTimerMax = 8.66f;
    [FoldoutGroup("Player")]public float overheatCooldown = 0.65f;
    [FoldoutGroup("Player")]public float overheatedTime=3;
    [FoldoutGroup("Player")]public bool recoilOnPlayer=true;
    [FoldoutGroup("Player")]public float critChancePlayer=4f;
    [FoldoutGroup("Player")]public bool playerWeaponsFade=true;
    [FoldoutGroup("Player")]public List<WeaponProperties> weaponProperties;
[Header("State Defaults")]
    [FoldoutGroup("Player")]public List<StatusFx> statusesStart;
    [FoldoutGroup("Player")]public float statusCapDefault=60f;
    [FoldoutGroup("Player")]public bool addToStatusTimer=true;
    [FoldoutGroup("Player")]public float flipTime = 7f;
    [FoldoutGroup("Player")]public float gcloverTime = 6f;
    [FoldoutGroup("Player")]public float shadowTime = 10f;
    [FoldoutGroup("Player")]public float shadowLength=0.33f;
    [FoldoutGroup("Player")]public float shadowtracesSpeed=2f;
    [FoldoutGroup("Player")]public float shadowCost=5f;
    [FoldoutGroup("Player")]public float dashSpeed=10f;
    [FoldoutGroup("Player")]public float startDashTime=0.2f;
    [FoldoutGroup("Player")]public float inverterTimeMax=10f;
    [FoldoutGroup("Player")]public float magnetTime=15f;
    [FoldoutGroup("Player")]public float scalerTime=15f;
    [FoldoutGroup("Player")]public float[] scalerSizes={0.45f,0.75f,1.2f,1.5f,1.75f,2f,2.5f};
    [FoldoutGroup("Player")]public float matrixLimit=0.05f;
    [FoldoutGroup("Player")]public float accelLimit=-2f;
    //public float shipScaleMin=0.45f;
    //public float shipScaleMax=2.5f;
    [FoldoutGroup("Player")]public float matrixTime=7f;
    [FoldoutGroup("Player")]public float accelTime=7f;
    [FoldoutGroup("Player")]public float onfireTickrate = 0.38f;
    [FoldoutGroup("Player")]public float onfireDmg = 1f;
    [FoldoutGroup("Player")]public float decayTickrate = 0.5f;
    [FoldoutGroup("Player")]public float decayDmg = 0.5f;
    [FoldoutGroup("Player")]public float fuelDrainAmnt=0.1f;
    [FoldoutGroup("Player")]public float fuelDrainFreq=0.5f;
[Header("Collectibles")]//Collectibles
    [FoldoutGroup("Player")]public float energyBall_energyGain=6f;
    [FoldoutGroup("Player")]public float battery_energyGain=11f;
    [FoldoutGroup("Player")]public float benergyBallGain=1;
    [FoldoutGroup("Player")]public float benergyVialGain=5;
    [FoldoutGroup("Player")]public int crystalGain=2;
    [FoldoutGroup("Player")]public int crystalBigGain=6;
    [FoldoutGroup("Player")]public float medkit_energyGain=26f;
    [FoldoutGroup("Player")]public float medkit_hpGain=25f;
    [FoldoutGroup("Player")]public float lunarGel_hpGain=10f;
    [FoldoutGroup("Player")]public bool lunarGel_absorp=true;
    [FoldoutGroup("Player")]public float powerups_energyGain=36f;
    [FoldoutGroup("Player")]public float powerups_energyNeeded=25f;
    [FoldoutGroup("Player")]public float powerups_energyDupl=42f;
    [FoldoutGroup("Player")]public int coresCollectGain=1;
#endregion
#region//Spawns - Waves, Disrupters, Powerups
[Title("Spawns - Waves, Disrupters, Powerups", titleAlignment: TitleAlignments.Centered)]
//[Header("Waves")]
    [FoldoutGroup("Spawns",false,VisibleIf="@this._isAdventure()==false||(this._isAdventureSubZone&&this._isAdventureBossZone==false)")][OnValueChanged("VaildateWaveSpawnReqs")][SerializeField]public spawnReqsType waveSpawnReqsType=spawnReqsType.score;
    #region//VaildateWaveSpawn
    [FoldoutGroup("Spawns")][Button("VaildateWaveSpawnReqs")][ContextMenu("VaildateWaveSpawnReqs")]void VaildateWaveSpawnReqs(){spawnReqsMono.Validate(ref waveSpawnReqs, ref waveSpawnReqsType);}
    #endregion
    [FoldoutGroup("Spawns")][SerializeReference]public spawnReqs waveSpawnReqs=new spawnScore();
    [FoldoutGroup("Spawns")][Searchable]public List<LootTableEntryWaves> waveList;
    [FoldoutGroup("Spawns")][ReadOnly]public float wavesWeightsSumTotal;
    [FoldoutGroup("Spawns")]public int startingWave=0;
    [FoldoutGroup("Spawns")]public bool startingWaveRandom=false;
    [FoldoutGroup("Spawns")]public bool uniqueWaves=true;
[Header("Disrupters")]
    [FoldoutGroup("Spawns")][Searchable]public List<DisrupterConfig> disrupterList;
[Header("Powerups")]
    [FoldoutGroup("Spawns")][Searchable]public List<PowerupsSpawnerGR> powerupSpawners;
    #region//VaildatePowerupsSpawn
    [FoldoutGroup("Spawns")][Button("VaildatePowerupsSpawnReqs")][ContextMenu("VaildatePowerupsSpawnReqs")]void VaildatePowerupsSpawnReqs(){foreach(PowerupsSpawnerGR p in powerupSpawners){
        spawnReqsMono.Validate(ref p.spawnReqs, ref p.spawnReqsType);}}
    #endregion
[Title("Boss", titleAlignment: TitleAlignments.Centered)]
    [BoxGroup("Boss Info",false,VisibleIf="@this._isAdventureBossZone")]public BossClass bossInfo;
    [BoxGroup("Boss Info")]public float shipScaleBoss=0.66f;
[Title("Enemies", titleAlignment: TitleAlignments.Centered)]
    [FoldoutGroup("Enemies",false,VisibleIf="@this._isAdventureSubZone==false")]public bool enemyDefenseHit=true;
    [FoldoutGroup("Enemies")]public bool enemyDefensePhase=true;
    [FoldoutGroup("Enemies")]public float enemyDefenseFloor=0.1f;
    [FoldoutGroup("Enemies")][Searchable]public EnemyClass[] enemies;
    [FoldoutGroup("Enemies")]public CometSettings cometSettings;
    [FoldoutGroup("Enemies")]public EnCombatantSettings enCombatantSettings;
    [FoldoutGroup("Enemies")]public EnShipSettings enShipSettings;
    [FoldoutGroup("Enemies")]public MechaLeechSettings mechaLeechSettings;
    [FoldoutGroup("Enemies")]public HealingDroneSettings healingDroneSettings;
    [FoldoutGroup("Enemies")]public VortexWheelSettings vortexWheelSettings;
    [FoldoutGroup("Enemies")]public GlareDevilSettings glareDevilSettings;
    [FoldoutGroup("Enemies")]public GoblinBossSettings goblinBossSettings;
    [FoldoutGroup("Enemies")]public HLaserSettings vlaserSettings;
    [FoldoutGroup("Enemies")]public HLaserSettings hlaserSettings;
#endregion
#region//Shop
[Title("Trading", titleAlignment: TitleAlignments.Centered)]
    [FoldoutGroup("Trading",false,VisibleIf="@this._isAdventureSubZone==false&&this.shopOn")][OnValueChanged("VaildateShopSpawnReqs")]public spawnReqsType shopSpawnReqsType=spawnReqsType.score;
    [FoldoutGroup("Trading")][Button("VaildateShopSpawnReqs")][ContextMenu("VaildateShopSpawnReqs")]void VaildateShopSpawnReqs(){spawnReqsMono.Validate(ref shopSpawnReqs, ref shopSpawnReqsType);}
    [FoldoutGroup("Trading")][SerializeReference]public spawnReqs shopSpawnReqs=new spawnScore();
    [FoldoutGroup("Trading")]public List<LootTableEntryShop> shopList;
    [FoldoutGroup("Trading")][EnableIf("shopCargoOn")]public float cargoSpeed=2;
    [FoldoutGroup("Trading")][EnableIf("shopCargoOn")]public float cargoHealth=44;
    [FoldoutGroup("Trading")][EnableIf("shopCargoOn")] public int[] repMinusCargoHit=new int[2]{1,3};
    [FoldoutGroup("Trading")][EnableIf("shopCargoOn")] public int repMinusCargoKill=7;
    [FoldoutGroup("Trading")][EnableIf("shopCargoOn")] public int cargoDeathCoinsB=5;
    [FoldoutGroup("Trading")]public bool repEnabled=true;
    [FoldoutGroup("Trading")]public const int repLength=4;
    [FoldoutGroup("Trading")]public int[] reputationThresh=new int[repLength];
    [FoldoutGroup("Trading")]public bool shopTimeLimitEnabled=true;
    [FoldoutGroup("Trading")][EnableIf("shopTimeLimitEnabled")]public float shopTimeLimit=10;
    [FoldoutGroup("Trading")]public float shopOpenGameSpeed=0;
#endregion
#region//Leveling
[Title("Leveling", titleAlignment: TitleAlignments.Centered)]
    [FoldoutGroup("Leveling",false,VisibleIf="@this._isAdventureSubZone==false")]public float xpMax=100f;
    [FoldoutGroup("Leveling")]public float xpMaxOvefillMult=1.5f;
    [FoldoutGroup("Leveling")]public float xp_wave=20;
    [FoldoutGroup("Leveling")]public float xp_shop=3;
    [FoldoutGroup("Leveling")]public float xp_powerup=1;
    [FoldoutGroup("Leveling")]public float xp_flying=7;
    [FoldoutGroup("Leveling")]public float flyingTimeReq=25;
    [FoldoutGroup("Leveling")]public float xp_staying=-2;
    [FoldoutGroup("Leveling")]public float stayingTimeReq=4;
    [FoldoutGroup("Leveling")]public List<ShipLvlFractionsValues> shipLvlFractionsValues;
    //[FoldoutGroup("Leveling")][ShowIf("@this._isAdventure()")]public int savePointsFromLvl=5;
[Header("Changes per level")]
    [FoldoutGroup("Leveling")]public List<ListEvents> lvlEvents;
#endregion
#region//Modules, Skills & Stats
[Title("Modules, Skills & Stats", titleAlignment: TitleAlignments.Centered)]
    [FoldoutGroup("Modules, Skills & Stats",false,VisibleIf="@this._isAdventureSubZone==false")]public List<ModulePropertiesGR> modulesPlayer;
    [FoldoutGroup("Modules, Skills & Stats")]public List<SkillPropertiesGR> skillsPlayer;
    [FoldoutGroup("Modules, Skills & Stats")]public int playerModulesCapacity=4;
    //[ES3NonSerializable]public int playerSkillsCapacity=2;
    [FoldoutGroup("Modules, Skills & Stats")]public float timeTeleport=3;
    [FoldoutGroup("Modules, Skills & Stats")]public float timeDetemined=4;
    [FoldoutGroup("Modules, Skills & Stats")]public float timeGiveItToMe=8;
    [FoldoutGroup("Modules, Skills & Stats")]public bool playerExhaustROF=true;
    [FoldoutGroup("Modules, Skills & Stats")]public int crystalMend_refillCost=2;
    [FoldoutGroup("Modules, Skills & Stats")]public float energyDiss_refillCost=3.3f;
    //public int[] unlockableSkills;
    [FoldoutGroup("Modules, Skills & Stats")][HideIf("@this._isAdventure()")]public int accumulateCelestPointsFromLvl=2;
    [FoldoutGroup("Modules, Skills & Stats")]public int bodyUpgrade_price=2;
    [FoldoutGroup("Modules, Skills & Stats")]public int engineUpgrade_price=1;
    [FoldoutGroup("Modules, Skills & Stats")]public int blastersUpgrade_price=1;
    [FoldoutGroup("Modules, Skills & Stats")]public int bodyUpgrade_defense=1;
    [FoldoutGroup("Modules, Skills & Stats")]public int bodyUpgrade_powerupCapacity=1;
    [FoldoutGroup("Modules, Skills & Stats")]public float engineUpgrade_moveSpeed=0.25f;
    [FoldoutGroup("Modules, Skills & Stats")]public float engineUpgrade_energyRegen=0.5f;
    [FoldoutGroup("Modules, Skills & Stats")]public float engineUpgrade_energyRegenFreqMinus=0.075f;
    [FoldoutGroup("Modules, Skills & Stats")]public float blastersUpgrade_shootMulti=0.05f;
    [FoldoutGroup("Modules, Skills & Stats")]public float blastersUpgrade_critChance=0.2f;

    [FoldoutGroup("Modules, Skills & Stats")]public float upgradeMenuOpenGameSpeed=0;
#endregion
#region//Break Encounter
    [FoldoutGroup("Break Encounter",false,VisibleIf="@this._isAdventureSubZone==false&&this.breakEncounter")]public bool breakEncounterAscendReq=true;
    [FoldoutGroup("Break Encounter")][DisableIf("@this.breakEncounterAscendReq==false")]public bool breakEncounterCountWavesPostAscend=false;
    [FoldoutGroup("Break Encounter")]public int breakEncounterWavesReq=5;
    [FoldoutGroup("Break Encounter")]public bool breakEncounterQuitWhenPlayerUp=true;
    [FoldoutGroup("Break Encounter")]public bool breakEncounterPauseMusic=false;
#endregion
#endregion

#region//Voids
    //public void ReplaceGameRules(GameRules gr){Destroy(this,0.01f);var _gr=gameObject.AddComponent<GameRules>();_gr=gr;}
    public void ReplaceAdventureZoneInfo(GameRules gr, bool boss=false){
        //Global
        bgMaterial=gr.bgMaterial;
        defaultGameSpeed=gr.defaultGameSpeed;
        scoreDisplay=gr.scoreDisplay;
        _isAdventureSubZone=gr._isAdventureSubZone;
        _isAdventureTravelZone=gr._isAdventureTravelZone;
        _isAdventureBossZone=gr._isAdventureBossZone;

        if(!boss){
            //Spawns
            waveSpawnReqs=gr.waveSpawnReqs;
            waveList=gr.waveList;
            startingWave=gr.startingWave;
            startingWaveRandom=gr.startingWaveRandom;
            uniqueWaves=gr.uniqueWaves;
            disrupterList=gr.disrupterList;
            powerupSpawners=gr.powerupSpawners;
            bossInfo=null;
            //Some info changed by bosses
            cometSettings=gr.cometSettings;
        }else{
            //clear Spawns
            waveSpawnReqs=new spawnReqs{timer=-5};
            waveList=null;
            disrupterList=null;
            powerupSpawners=null;
            //Boss info
            bossInfo=gr.bossInfo;
            shipScaleBoss=gr.shipScaleBoss;
        }
    }
    void Awake(){if(GameRules.instance!=null&&this!=GameRules.instance){Destroy(gameObject);}else{DontDestroyOnLoad(gameObject);instance=this;}}
    IEnumerator Start(){
        if(gameObject.name.Contains("(Clone)")){gameObject.name.Replace("(Clone)","");}
        if(_isAdventure()){GameManager.instance.gamemodeSelected=-1;if(GameManager.instance.zoneSelected==-1)GameManager.instance.zoneSelected=0;}
        //Set gameModeSelected if artificially turned on gamemode etc
        yield return new WaitForSecondsRealtime(0.05f);
        if(!GameManager.instance.CheckGamemodeSelected(cfgName)){
            GameManager.instance.SetGamemodeSelectedStr(cfgName);
            if(SceneManager.GetActiveScene().name=="Game")EnterGameScene();
        }
        yield return new WaitForSecondsRealtime(0.02f);

        SumUpWavesWeightsTotal();
        SumUpAllPowerupSpawnersWeightsTotal();
    }
    IEnumerator enterGameCor;
    public void EnterGameScene(){if(enterGameCor==null){enterGameCor=EnterGameSceneI();StartCoroutine(EnterGameSceneI());}else{StopCoroutine(enterGameCor);enterGameCor=null;EnterGameScene();}}
    IEnumerator EnterGameSceneI(){
        yield return new WaitForSeconds(0.02f);
        StartCoroutine(CreateSpawners());
        yield return new WaitForSeconds(1f);
        if(_isAdventureBossZone&&FindObjectOfType<BossAI>()==null&&!SaveSerial.instance.advD.defeatedBosses.Contains(bossInfo.name)){AssetsManager.instance.Make(bossInfo.name,bossInfo.spawnPos);Debug.Log("Spawned: "+bossInfo.name);}
        enterGameCor=null;
    }
    IEnumerator CreateSpawners(){
        //Set/Create WaveSpawner
        if(waveList!=null){if(waveList.Count>0){
            Waves ws;
            if(FindObjectOfType<Waves>()==null){
                ws=Instantiate(AssetsManager.instance.waveSpawnerPrefab).GetComponent<Waves>();
                ws.name="Waves";
            }else{ws=FindObjectOfType<Waves>();}
            yield return new WaitForSecondsRealtime(0.005f);
            ws.startingWave=startingWave;
            ws.GetComponent<LootTableWaves>().itemList=waveList;
            ws.startingWaveRandom=startingWaveRandom;
            ws.uniqueWaves=uniqueWaves;
            SumUpWavesWeightsTotal();
        }}

        //Set/Create DisruptersSpawner
        if(disrupterList!=null){if(disrupterList.Count>0){
            DisruptersSpawner ds;
            if(FindObjectOfType<DisruptersSpawner>()==null){
                ds=Instantiate(AssetsManager.instance.disrupterSpawnerPrefab).GetComponent<DisruptersSpawner>();
                ds.name="DisruptersSpawner";
            }else{ds=FindObjectOfType<DisruptersSpawner>();}
            yield return new WaitForSecondsRealtime(0.005f);
            ds.disruptersList=disrupterList;
        }}
        
        //Set/Create PowerupSpawners
        if(powerupSpawners!=null){if(powerupSpawners.Count>0){
            List<PowerupsSpawner> ps=new List<PowerupsSpawner>();
            if(FindObjectsOfType<PowerupsSpawner>()!=null){
                foreach(PowerupsSpawner ps1 in FindObjectsOfType<PowerupsSpawner>()){ps.Add(ps1);}
            }for(int i=FindObjectsOfType<PowerupsSpawner>().Length;i<powerupSpawners.Count;i++)ps.Add(Instantiate(AssetsManager.instance.powerupSpawnerPrefab).GetComponent<PowerupsSpawner>());
            yield return new WaitForSecondsRealtime(0.005f);
            for(int i=0;i<powerupSpawners.Count;i++){   if(powerupSpawners[i].powerupList.Count>0){
                ps[i].GetComponent<LootTablePowerups>().itemList=powerupSpawners[i].powerupList;
                ps[i].spawnReqsType=powerupSpawners[i].spawnReqsType;
                ps[i].spawnReqs=powerupSpawners[i].spawnReqs;
                ps[i].powerupSpawnPosRange=powerupSpawners[i].powerupSpawnPosRange;
            }}
            SumUpAllPowerupSpawnersWeightsTotal();
        }}
    }
    [NonSerialized][ES3NonSerializable]Player p;
    void Update(){
        if(Player.instance!=null&&p!=Player.instance){p=Player.instance;}
        if(!(SceneManager.GetActiveScene().name=="Game"||SceneManager.GetActiveScene().name=="InfoGameMode"||SceneManager.GetActiveScene().name=="AdventureZones"||SceneManager.GetActiveScene().name=="SandboxMode")){
            Destroy(gameObject);}
        CapToMaxValues();
        if(FindObjectOfType<BossAI>()!=null){if(scoreDisplay!=scoreDisplay.bossHealth){scoreDisplay=scoreDisplay.bossHealth;}
            /*foreach(Waves w in FindObjectsOfType<Waves>())Destroy(w.gameObject);
            foreach(DisruptersSpawner ds in FindObjectsOfType<DisruptersSpawner>())Destroy(ds.gameObject);
            foreach(PowerupsSpawner ps in FindObjectsOfType<PowerupsSpawner>())Destroy(ps.gameObject);*/
        }
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

        if(_isAdventureBossZone)for(int i=0;i<bossInfo.phasesInfo.Count;i++){bossInfo.phasesInfo[i].name="Phase "+(i+1);}
        CapToMaxValues();
        SumUpWavesWeightsTotal();
        SumUpAllPowerupSpawnersWeightsTotal();
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
    public void SumUpWavesWeightsTotal(){
        wavesWeightsSumTotal=0;
        if(waveList!=null)foreach(LootTableEntryWaves w in waveList){wavesWeightsSumTotal+=w.dropChance;}
    }
    public void SumUpAllPowerupSpawnersWeightsTotal(){
        if(powerupSpawners!=null)foreach(PowerupsSpawnerGR ps in powerupSpawners){ps.SumUpPowerupsWeightsTotal();}
    }
    #region//Custom Events
    public void MultiplyhealthMax(float amnt){p.healthMax*=amnt;}
    public void MultiplyenergyMax(float amnt){p.energyMax*=amnt;}
    //public void ArmorMultiAdd(float amnt){p.armorMultiInit+=amnt;}
    public void DmgMultiAdd(float amnt){p.dmgMultiInit+=amnt;}
    public void ShootMultiAdd(float amnt){p.shootMulti+=amnt;}
    public void LaserShootSpeed(float amnt){if(p.GetWeaponProperty("laser")!=null){var wp=(weaponTypeBullet)p.GetWeaponProperty("laser").weaponTypeProperties;wp.shootDelay=amnt;}}
    public void MLaserBulletAmnt(int amnt){if(p.GetWeaponProperty("mlaser")!=null){var wp=(weaponTypeBullet)p.GetWeaponProperty("mlaser").weaponTypeProperties;wp.bulletAmount=amnt;}}
    public void ChangeMaxXP(int amnt){GameManager.instance.xpMax=amnt;}
    public void MaxHPAdd(float amnt){
        Player.instance.healthMax+=amnt;
        if(!GameManager.instance._lvlEventsLoading){
            Player.instance.healthStart+=(amnt/2f);
            Player.instance.health+=amnt;
        }else{if(Player.instance.healthStart==healthPlayer)Player.instance.healthStart+=(amnt/2f);}
    }
    public void UpgradeBody(){Player.instance.GetComponent<PlayerModules>().bodyUpgraded++;}
    public void UpgradeEngine(){Player.instance.GetComponent<PlayerModules>().engineUpgraded++;}
    public void UpgradeBlasters(){Player.instance.GetComponent<PlayerModules>().blastersUpgraded++;}
    #endregion
#endregion
#region//Return functions
    public GameRules ShallowCopy(){return (GameRules)this.MemberwiseClone();}
    public PowerupItemSettings GetItemSettings(string name){PowerupItemSettings p=null;p=Array.Find(powerupItemSettings,x=>x.name==name);return p;}
    public bool CheckWaveStarting(string name){bool b=false;if(waveList.Find(x=>x.lootItem.name==name)!=null){if(startingWave==waveList.FindIndex(x=>x.lootItem.name==name)){b=true;}}return b;}
#endregion
}
#region//Custom classes
[System.Serializable]
public class PowerupsSpawnerGR{
    public string name;
    public string sprAssetName;
    [Searchable]public List<LootTableEntryPowerup> powerupList;
    [ReadOnly]public float sum;
    public void SumUpPowerupsWeightsTotal(){
        sum=0;
        foreach(LootTableEntryPowerup w in powerupList){sum+=w.dropChance;}
    }
    [OnValueChanged("VaildatePowerupsSpawnReqs")]public spawnReqsType spawnReqsType;
    [SerializeReference] public spawnReqs spawnReqs;
    void VaildatePowerupsSpawnReqs(){spawnReqsMono.Validate(ref spawnReqs, ref spawnReqsType);}
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
    public ShaderMatProps sprMatProps;
    public float healthStart=25;
    public float healthMax=25;
    public int defense=0;
    public bool healthBySize=false;
    public bool shooting = false;
    [HideIf("@this.shooting==false")]public Vector2 shootTime=new Vector2(1.75f,2.8f);
    [HideIf("@this.shooting==false")]public string bulletAssetName;
    [HideIf("@this.shooting==false")]public float bulletSpeed = 8f;
    [HideIf("@this.shooting==false")]public bool DBullets = false;
    [HideIf("@this.shooting==false")]public float bulletDist=0.35f;
    public bool randomizeWaveDeath = false;
    public bool flyOff = false;
    public bool killOnDash = true;
    public bool destroyOut = true;
[Header("Drops & Points")]
    public bool giveScore = true;
    [ShowIf("@this.giveScore")]public Vector2 scoreValue=new Vector2(1,10);
    public float xpChance = 100f;
    [EnableIf("@this.xpChance>0")]public float xpAmnt = 0f;
    [EnableIf("@this.xpChance>0")]public bool accumulateXp = true;
    public List<LootTableEntryDrops> drops;
}
[System.Serializable]
public class BossClass{
    public string codeName;
    public string name;
    public enemyType type;
    public float healthStart=25;
    public float healthMax=25;

    public Vector2 spawnPos;
    public bool scaleUpOnSpawn=true;
    public AudioClip ost;
    public float deathLength=3f;
    public string playerKillQuip="MoonOfLunacy-Laugh2";
    public Sprite deathSprite;
    public string deathStartAudio="MoonOfLunacy-Death";
    public string deathStartVFX="ExplosionsLong";
    public string deathEndAudio="Explosion";
    public string deathEndVFX="ExplosionBig";
    public float deathShakeStrength=4f;
    public float deathShakeSpeed=0.3f;
[Header("Drops & PhaseInfo")]
    public float xpAmnt = 100f;
    public float xpChance = 100f;
    public List<LootTableEntryDrops> drops;
    public List<BossPhaseInfo> phasesInfo;
    public Sprite bossTitleSprite;
}
[System.Serializable]
public class BossPhaseInfo{
    [Title("$name", titleAlignment: TitleAlignments.Centered)]
    [ReadOnly]public string name;
    public int defense;
    public Vector2 size=Vector2.one;
    public WaveConfig pathingInfo;
    public ShaderMatProps sprMatProps;
    public List<SimpleAnim> anims;
[Header("Transformation")]
    public float delay=1f;
    public string audioOnChangeStartAsset;
    public string vfxOnChangeStartAsset;
    public string audioOnChangeEndAsset;
    public string vfxOnChangeEndAsset;
    public float camShakeStrength=2f;
    public float camShakeSpeed=0.2f;
    public bool pauseOstOnPhaseChange=true;
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
    public string healPelletAssetName;
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
    public Vector2 timeToDieSet=new Vector2(4f,6f);
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
    public string transformVfxAssetName="DarkEnergy";
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

[System.Serializable]public class ModulePropertiesGR{
    public ModuleProperties item;
    //public costType costType;
    //public costTypeProperties costTypeProperties;
    public List<ModuleSkillLvlVals> lvlVals=new List<ModuleSkillLvlVals>(1);
    [ValidateInput("@this.lvlValsContainsHigherThanLvlExpire()==false||lvlExpire==0","lvlExpire cant be lower or equals any of the lvlReqs")]public int lvlExpire=0;
    bool lvlValsContainsHigherThanLvlExpire(){return lvlVals.FindIndex(x=>x.lvlReq>=lvlExpire)!=-1;}
    [ValidateInput("@this.lessLvlValsThanUnlockedLvl()==false&&unlockedLvl>=0","unlockedLvl cant be higher than the amount of lvlVals")]public int unlockedLvl=0;
    bool lessLvlValsThanUnlockedLvl(){return lvlVals.Count<unlockedLvl;}
    [DisableIf("@this.unlockedLvl<=0")]public bool equipped=false;
}
[System.Serializable]public class SkillPropertiesGR{
    public SkillProperties item;
    public costType costType;
    public costTypeProperties costTypeProperties;
    public float cooldown;
    public List<ModuleSkillLvlVals> lvlVals=new List<ModuleSkillLvlVals>(1);
    [ValidateInput("@this.lvlValsContainsHigherThanLvlExpire()==false||lvlExpire==0","lvlExpire cant be lower or equals any of the lvlReqs")]public int lvlExpire=0;
    bool lvlValsContainsHigherThanLvlExpire(){return lvlVals.FindIndex(x=>x.lvlReq>=lvlExpire)!=-1;}
    [ValidateInput("@this.lessLvlValsThanUnlockedLvl()==false&&unlockedLvl>=0","unlockedLvl cant be higher than the amount of lvlVals")]public int unlockedLvl=0;
    bool lessLvlValsThanUnlockedLvl(){return lvlVals.Count<unlockedLvl;}
    [DisableIf("@this.unlockedLvl<=0")]public bool equipped=false;
}
[System.Serializable]public class ModuleSkillLvlVals{
    public int coreCost=1;
    public int lvlReq=1;
}

#endregion


[System.Serializable]
public class AdventureZoneData{
    [Title("$name", titleAlignment: TitleAlignments.Centered)]
    public bool enabled=true;
    public string name="1";
    public GameRules gameRules;
    [EnableIf("enabled")]public Vector2 pos;
    [EnableIf("enabled")]public int lvlReq;
    [EnableIf("enabled")]public bool isBoss;
    [EnableIf("enabled")][ShowIf("isBoss")]public bool bossBlackOutImg=true;
}

public enum scoreDisplay{score,sessionTime,timeLeft,bossHealth,sessionTimeAsDistance}