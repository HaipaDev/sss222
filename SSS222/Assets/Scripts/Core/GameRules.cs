using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class GameRules : MonoBehaviour{
#region//Values
public static GameRules instance;
#region//Global values
    [Header("Global")]
    public string cfgName;
    public float defaultGameSpeed=1f;
    public bool shopOn=true;
    public bool shopCargoOn=true;
    public bool upgradesOn=true;
    public bool xpOn=true;
    public bool barrierOn=false;
    public int EVscoreMax=30;
    public int shopScoreMax=200;
    public int shopScoreMaxS=200;
    public int shopScoreMaxE=450;
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
    public bool losePwrupOutOfEn;
    public bool losePwrupOutOfAmmo;
    public float armorMultiPlayer=1f;
    public float dmgMultiPlayer=1f;
    public float shootMultiPlayer=1f;
    public float shipScaleDefault=0.89f;
    public bool overheatOnPlayer=true;
    public float overheatTimerMax = 8.66f;
    public float overheatCooldown = 0.65f;
    public float overheatedTime=3;
    public bool recoilOnPlayer=true;
    public List<WeaponProperties> weaponProperties;
    [Header("State Defaults")]
    public float flipTime = 7f;
    public float gcloverTime = 6f;
    public float shadowTime = 10f;
    public float shadowLength=0.33f;
    public float shadowCost=5f;
    public float dashSpeed=10f;
    public float startDashTime=0.2f;
    public float inverterTime=10f;
    public float magnetTime=15f;
    public float scalerTime=15f;
    public float shipScaleMin=0.45f;
    public float shipScaleMax=2.5f;
    public float matrixTime=7f;
    public float pmultiTime=24f;
    public float accelTime=7f;
    public float onfireTickrate = 0.38f;
    public float onfireDmg = 1f;
    public float decayTickrate = 0.5f;
    public float decayDmg = 0.5f;
    public float armoredMulti = 2f;
    public float fragileMulti = 0.7f;
    public float powerMulti = 1.6f;
    public float weaknsMulti = 0.66f;
    [Header("Energy Gains")]//Collectibles
    public float energyBallGet=9f;
    public float medkitEnergyGet=26f;
    public float medkitUEnergyGet=40f;
    public float medkitHpAmnt=25f;
    public float medkitUHpAmnt=62f;
    public float pwrupEnergyGet=36f;
    public float enForPwrupRefill=25f;
    public float enPwrupDuplicate=42f;
    public float refillEnergyAmnt=110f;
    [Header("Skills")]
    public Skill[] skillsPlayer;
    public float timeOverhaul=10;
#endregion
#region//Powerup Spawns
    [Header("Powerup/Waves Spawns")]
    public List<LootTableEntryPowerup> pwrupStatusList;
    public float mTimePowerupStatusSpawns=-1;
    public float mTimePowerupStatusSpawnsS=9f;
    public float mTimePowerupStatusSpawnsE=16f;
    public float firstPowerupStatusSpawn=8;
    public int enemiesPowerupStatusCountReq=-1;
    public List<LootTableEntryPowerup> pwrupWeaponList;
    public float mTimePowerupWeaponsSpawns=-1;
    public float mTimePowerupWeaponsSpawnsS=-1;
    public float mTimePowerupWeaponsSpawnsE=-1;
    public float firstPowerupWeaponsSpawn=-4;
    public int enemiesPowerupWeaponsCountReq=20;
#endregion
#region//Waves Spawns & Enemies
[Header("Waves & Disrupters")]
    public List<LootTableEntryWaves> waveList;
    public int startingWave=0;
    public bool startingWaveRandom=false;
    public bool uniqueWaves=true;
    public bool spawnLeech=true;
    public float mSTimeSpawnsLeech=55f;
    public float mETimeSpawnsLeech=80f;
    public bool spawnHlaser=true;
    public float mSTimeSpawnsHlaser = 30f;
    public float mETimeSpawnsHlaser = 60f;
    public bool spawnGoblin=true;
    public int mPowerupsGoblin=2;
    public float mSTimeSpawnsGoblin=40f;
    public float mETimeSpawnsGoblin=50f;
    public bool spawnHealDrone=true;
    public int mEnemiesCountHealDrone=50;
    public float mSTimeSpawnsHealDrone=40f;
    public float mETimeSpawnsHealDrone=50f;
    public bool spawnVortexWheel=true;
    public float mEnergyCountVortexWheel=200;
    public float EnergyCountVortexWheel=0;
    public float mSTimeSpawnsVortexWheel=40f;
    public float mETimeSpawnsVortexWheel=50f;
    public bool spawnGlareDevil=true;
    public float mEnergyCountGlareDevil=20;
    public float mSTimeSpawnsGlareDevil=40f;
    public float mETimeSpawnsGlareDevil=50f;
    [Header("Enemies")]
    public EnemyClass[] enemies;
    public CometSettings cometSettings;
    public EnCombatantSettings enCombatantSettings;
    public EnShipSettings enShipSettings;
    public MechaLeechSettings mechaLeechSettings;
    public HealingDroneSettings healingDroneSettings;
    public VortexWheelSettings vortexWheelSettings;
    public GlareDevilSettings glareDevilSettings;
    public float goblinBossHP=50;
#endregion
#region//Damage Values
[Header("Damage Values")]
    public float dmg=5;
    public float dmgZone=2;
    public float dmgLaser=5f;
    public float dmgPhaser=0.5f;
    public float dmgHRocket=13.5f;
    public float dmgMiniLaser=0.32f;
    public float dmgLSaber=0.86f;
    public float dmgLClaws=7f;
    public float dmgShadowBT=40.5f;
    public float dmgQRocket=14.5f;
    public float dmgPRocket=0f;
    public float dmgPRocketExpl=0.5f;
    public float dmgCBullet=2f;
    public float dmgPlaser=6.78f;
    public float dmgMPulse=130f;
    //
    public float dmgComet=10f;
    public float dmgBat=36f;
    public float dmgSoundwave=16.5f;
    public float dmgEnemyShip1=80f;
    public float dmgEBt=24.5f;
    public float dmgEnemySaber=2.5f;    
    public float dmgGoblin=16f;
    public float dmgHealDrone=75f;
    public float dmgVortex=70f;
    public float dmgLeech=4f;
    public float dmgHLaser=90f;
    public float dmgStinger=33.3f;
    public Vector2 efxStinger=new Vector2(20,1);
    public float dmgGoblinBt=7f;
    public Vector2 efxGoblinBt=new Vector2(6,0.8f);
    public Vector2 efxGlareDev=new Vector2(1.5f,2f);
#endregion
#region//Shop
[Header("Shop")]
    public ShopItem[] shopList;
    public int defSlots=1;
    public bool repEnabled=true;
     public const int repLength=4;
    public int[] reputationThresh=new int[repLength];
    public bool shopTimeLimitEnabled=true;
    public float shopTimeLimit=10;
    public int crystalDropS=4;
    public int crystalDropE=14;
#endregion
#region//Leveling
    [Header("Leveling")]
    public float xp_forCore=100f;
    public float xp_wave=20f;
    public float xp_shop=3f;
    public float xp_powerup=1f;
    public float xp_flying=7f;
    public float flyingTimeReq=25f;
    public float xp_staying=-2f;
    public float stayingTimeReq=4f;
    [Header("Changes per level")]
    public UnityEvent[] lvlEvents;
    public int[] lvlEventsIDs;
#endregion
#region//Upgrades
[Header("Upgrades")]
    public int total_UpgradesCountMax=5;
    public int other_UpgradesCountMax=10;
    public float maxHealth_UpgradeAmnt=5f;
    public int maxHealth_UpgradeCost=1;
    public int maxHealth_UpgradesCountMax=5;
    public float maxEnergy_UpgradeAmnt=5f;
    public int maxEnergy_UpgradeCost=1;
    public int maxEnergy_UpgradesCountMax=4;
    public float speed_UpgradeAmnt=0.1f;
    public int speed_UpgradeCost=1;
    public int speed_UpgradesCountMax=5;
    public float hpRegen_UpgradeAmnt=0.2f;
    public int hpRegen_UpgradeCost=1;
    public int hpRegen_UpgradesCountMax=2;
    public float enRegen_UpgradeAmnt=1;
    public int enRegen_UpgradeCost=1;
    public int enRegen_UpgradesCountMax=2;
    public float luck_UpgradeAmnt=0.05f;
    public int luck_UpgradeCost=1;
    public int luck_UpgradesCountMax=5;
    public int defaultPowerup_upgradeCost1=1;
    public int defaultPowerup_upgradeCost2=1;
    public int defaultPowerup_upgradeCost3=4;
    public int energyRefill_upgradeCost=2;
    public int energyRefill_upgradeCost2=3;
    public int mPulse_upgradeCost=3;
    public int postMortem_upgradeCost=1;
    public int teleport_upgradeCost=2;
    public int overhaul_upgradeCost=3;
    public int[] unlockableSkills;
#endregion
#endregion
#region//Voids
    private void Awake(){
        SetupSingleton();
    }
    private void SetupSingleton(){
        int numberOfObj = FindObjectsOfType<GameRules>().Length;
        if(numberOfObj>1||!(SceneManager.GetActiveScene().name=="Game"||SceneManager.GetActiveScene().name=="InfoGameMode")){
            Destroy(gameObject);
        }else{
            DontDestroyOnLoad(gameObject);
            instance=this;
        }
    }
    private void Update() {
        if(!(SceneManager.GetActiveScene().name=="Game"||SceneManager.GetActiveScene().name=="InfoGameMode")){Destroy(gameObject);}
    }
    private void OnValidate(){
        if(!shopOn)shopCargoOn=false;
    }
    #region//Custom Events
    public void AdventureLvlEach(){
        var p=FindObjectOfType<Player>();
        p.shootMulti+=0.01f;
        p.armorMultiInit+=0.005f;
    }
    public void AdventureLvl1(){
        var p=FindObjectOfType<Player>();
        if(p.GetWeaponProperty("laser")!=null){var wp=(weaponTypeBullet)p.GetWeaponProperty("laser").weaponTypeProperties;wp.shootDelay=0.30f;}
        if(p.GetWeaponProperty("mlaser")!=null){var wp=(weaponTypeBullet)p.GetWeaponProperty("mlaser").weaponTypeProperties;wp.bulletAmount=5;}
    }
    public void AdventureLvl2(){
        var p=FindObjectOfType<Player>();
        if(p.GetWeaponProperty("laser")!=null){var wp=(weaponTypeBullet)p.GetWeaponProperty("laser").weaponTypeProperties;wp.shootDelay=0.26f;}
        if(p.GetWeaponProperty("mlaser")!=null){var wp=(weaponTypeBullet)p.GetWeaponProperty("mlaser").weaponTypeProperties;wp.bulletAmount=7;}
    }
    public void AdventureLvl4(){
        var p=FindObjectOfType<Player>();
        if(p.GetWeaponProperty("laser")!=null){var wp=(weaponTypeBullet)p.GetWeaponProperty("laser").weaponTypeProperties;wp.shootDelay=0.22f;}
        if(p.GetWeaponProperty("mlaser")!=null){var wp=(weaponTypeBullet)p.GetWeaponProperty("mlaser").weaponTypeProperties;wp.bulletAmount=10;}
    }


    public void ArcadeLvlEach(){
        var p=FindObjectOfType<Player>();
        p.shootMulti+=0.02f;
        p.armorMultiInit+=0.01f;
    }
    public void ArcadeLvl1(){
        var p=FindObjectOfType<Player>();
        p.maxEnergy*=2;
    }
    #endregion
}
#endregion

#region//Custom classes
[System.Serializable]
public class EnemyClass{
    public string name;
    public Vector2 size = Vector2.one;
    public Sprite spr;
    public float health=100;
    public bool shooting = false;
    public float minTimeBtwnShots=0.2f;
    public float maxTimeBtwnShots=1f;
    public GameObject bullet;
    public float bulletSpeed = 8f;
    public bool DBullets = false;
    public float bulletDist=0.35f;
    public bool randomizeWaveDeath = false;
    public bool flyOff = false;
    public float freezefxTime = 0;
    [Header("Drops & Points")]
    public bool givePts = true;
    public int scoreValueStart = 1;
    public int scoreValueEnd = 10;
    public float enBallChanceInit = 30f;
    public float coinChanceInit = 3f;
    public float powercoreChanceInit = 0f;
    public float xpAmnt = 0f;
    public GameObject specialDrop;
}
[System.Serializable]
public class CometSettings{
    [Header("Basic")]
    public float sizeMin=0.4f;
    public float sizeMax=1.4f;
    public bool healthBySize=true;
    public bool damageBySpeedSize=true;
    public bool scoreBySize=false;
    public CometScoreSize[] scoreSizes;
    public Sprite[] sprites;
    public GameObject bflamePart;
    [Header("Lunar")]
    public float sizeMinLunar=0.88f;
    public float sizeMaxLunar=1.55f;
    public int lunarCometChance=10;
    public float lunarHealthMulti=2.5f;
    public float lunarSpeedMulti=0.415f;
    public int lunarScore=-1;
    public int lunarScoreS=0;
    public int lunarScoreE=0;
    public GameObject lunarDrop;
    public int lunarDropChance=0;
    public Sprite[] spritesLunar;
    public GameObject lunarPart;
}
[System.Serializable]
public class EnCombatantSettings{
    public float speedFollowX = 3.5f;
    public float speedFollowY = 4f;
    public float vspeed = 0.1f;
    public float distY = 1.3f;
    public float distX = 0.3f;
    public float distYPlayer = 1.5f;
    public GameObject saberPrefab;
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
    public GameObject healBallPrefab;
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
	public GameObject projectile;
	public int numberOfProjectiles=4;
	public float radius=5;
	public float moveSpeed=5;
}
[System.Serializable]
public class GlareDevilSettings{
    public float timerMax=3.3f;
    public Vector2 efxBlind=new Vector2(4,4);
}


#endregion