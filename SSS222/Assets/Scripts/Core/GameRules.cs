using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRules : MonoBehaviour{
    public static GameRules instance;
    [Header("Global")]
    public float defaultGameSpeed=1f;
    public bool barrierOn=false;
    public bool shopOn=true;
    public bool xpOn=true;
    public bool upgradesOn=true;
    public int EVscoreMax=30;
    public int shopScoreMax=200;
    public int shopScoreMaxS=200;
    public int shopScoreMaxE=450;
    public float scoreMulti=1;
    public float luckMulti=1;
    [Header("Leveling")]
    public float xp_forCore=100f;
    public float xp_wave=20f;
    public float xp_shop=3f;
    public float xp_powerup=1f;
    public float xp_flying=7f;
    public float flyingTimeReq=25f;
    public float xp_staying=-2f;
    public float stayingTimeReq=4f;
    [Header("Player")]
    public Vector2 startingPosPlayer=new Vector2(0.36f,-6.24f);
    public bool autoShootPlayer=false;
    public float healthPlayer=150;
    public bool energyOnPlayer=true;
    public float energyPlayer=180;
    public string powerupStarting="laser";
    public string powerupDefault="laser";
    public bool moveX=true;
    public bool moveY=true;
    public float paddingX=-0.125f;
    public float paddingY=0.45f;
    public float moveSpeedPlayer=5f;
    public float armorMultiPlayer=1f;
    public float dmgMultiPlayer=1f;
    public float shootMultiPlayer=1f;
    public float shipScaleDefault=0.89f;
    [Header("Player Weapons")]
    
    public float laserSpeed=9f;
    public float laserShootPeriod=0.34f;
    public float laser2Speed=8.9f;
    public float laser2ShootPeriod=0.35f;
    public float laser3Speed=8.8f;
    public float laser3ShootPeriod=0.36f;
    public float phaserSpeed=10.5f;
    public float phaserShootPeriod=0.2f;
    public float hrocketSpeed=6.5f;
    public float hrocketShootPeriod=0.3f;
    public float mlaserSpeedS=8.5f;
    public float mlaserSpeedE=10f;
    public float mlaserShootPeriod=0.1f;
    public int mlaserBulletsAmmount=10;
    public float shadowBTSpeed=4f;
    public float shadowBTShootPeriod=0.65f;
    public float qrocketSpeed=9.5f;
    public float qrocketShootPeriod=0.3f;
    public float procketSpeedS=9.5f;
    public float procketSpeedE=10.5f;
    public float procketShootPeriod=0.5f;
    public int procketsBulletsAmmount=10;
    public float cbulletSpeed=8.25f;
    public float cbulletShootPeriod=0.15f;
    public float plaserSpeed=9.5f;
    public float plaserShootPeriod=0.7f;
    [Header("Energy Costs")]
    //Weapons
    public float laserEn=0.3f;
    public float laser2En=1.25f;
    public float laser3En=2.5f;
    public float phaserEn=1.5f;
    public float mlaserEn=0.075f;
    public float lsaberEn=0.4f;
    public float lsaberEnPeriod=0.1f;
    public float lclawsEn=6.3f;
    public float shadowEn=5f;
    public float shadowBTEn=10f;
    public float cbulletEn=1.3f;
    public float plaserEn=7f;
    //Rockets
    public float hrocketOh=0.9f;//2.6
    public float qrocketOh=2.23f;//5.5
    public float procketEn=0.86f;//0.26
    public float procketOh=0.09f;
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
    public List<LootTableEntryWaves> waveList;
    public bool spawnLeech=true;
    public float mSTimeSpawnsLeech=55f;
    public float mETimeSpawnsLeech=80f;
    public bool spawnHlaser=true;
    public float mSTimeSpawnsHlaser = 30f;
    public float mETimeSpawnsHlaser = 60f;
    public bool spawnGoblin=true;
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
    public EnemyClass[] enemies;
    private void Awake(){
        instance=this;
    }
    private void OnValidate(){
        foreach(LootTableEntryPowerup entry in pwrupStatusList){
        entry.name=entry.lootItem.name;
        entry.rarity=entry.lootItem.rarity;
        entry.dropChance=entry.lootItem.dropChance;
        entry.levelReq=entry.lootItem.levelReq;
        }
        foreach(LootTableEntryPowerup entry in pwrupWeaponList){
        entry.name=entry.lootItem.name;
        entry.rarity=entry.lootItem.rarity;
        entry.dropChance=entry.lootItem.dropChance;
        entry.levelReq=entry.lootItem.levelReq;
        }
        foreach(LootTableEntryWaves entry in waveList){
        entry.name=entry.lootItem.name;
        //entry.dropChance=entry.lootItem.dropChance;
        //entry.levelReq=entry.lootItem.levelReq;
        }
    }
}


[System.Serializable]
public class EnemyClass{
    public string name;
    //[Header("Basic")]
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
    [Header("Drops & Points")]
    public bool givePts = true;
    public int scoreValueStart = 1;
    public int scoreValueEnd = 10;
    public float enBallChanceInit = 30f;
    public float coinChanceInit = 3f;
    public float powercoreChanceInit = 0f;
    public float xpAmnt = 0f;
}