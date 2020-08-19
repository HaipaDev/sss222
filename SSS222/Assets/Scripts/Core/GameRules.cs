using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRules : MonoBehaviour{
    public static GameRules instance;
    [HeaderAttribute("Global")]
    public bool barrierOn=false;
    public bool shopOn=true;
    public bool xpOn=true;
    public bool upgradesOn=true;
    [HeaderAttribute("Player")]
    public float healthPlayer=150;
    public bool energyOnPlayer=true;
    public float energyPlayer=180;
    public string powerupStarting="laser";
    public string powerupDefault="laser";
    public bool moveX=true;
    public bool moveY=true;
    public float paddingX = -0.125f;
    public float paddingY = 0.45f;
    public float moveSpeedPlayer = 5f;
    public float armorMultiPlayer = 1f;
    public float dmgMultiPlayer = 1f;
    public float shootMultiPlayer = 1f;
    public float shipScaleDefault=0.89f;
    [HeaderAttribute("Player Weapons")]
    public float laserSpeed = 9f;
    public float laserShootPeriod = 0.34f;
    public float phaserSpeed = 10.5f;
    public float phaserShootPeriod = 0.2f;
    public float hrocketSpeed = 6.5f;
    public float hrocketShootPeriod = 0.3f;
    public float mlaserSpeedS = 8.5f;
    public float mlaserSpeedE = 10f;
    public float mlaserShootPeriod = 0.1f;
    public int mlaserBulletsAmmount = 10;
    public float lsaberEnPeriod = 0.1f;
    public float shadowBTSpeed = 4f;
    public float shadowBTShootPeriod = 0.65f;
    public float qrocketSpeed = 9.5f;
    public float qrocketShootPeriod = 0.3f;
    public float procketSpeedS = 9.5f;
    public float procketSpeedE = 10.5f;
    public float procketShootPeriod = 0.5f;
    public int procketsBulletsAmmount = 10;
    public float cbulletSpeed = 8.25f;
    public float cbulletShootPeriod = 0.15f;
    public float plaserSpeed = 9.5f;
    public float plaserShootPeriod = 0.7f;
    [HeaderAttribute("Energy Costs")]
    //Weapons
    public float laserEn = 0.3f;
    public float laser2En = 1.25f;
    public float laser3En = 2.5f;
    public float phaserEn = 1.5f;
    public float mlaserEn = 0.075f;
    public float lsaberEn = 0.375f;
    public float lclawsEn = 6.3f;
    public float shadowEn = 5f;
    public float shadowBTEn = 10f;
    public float cbulletEn = 1.3f;
    public float plaserEn = 7f;
    //Rockets
    public float hrocketOh = 0.9f;//2.6
    public float qrocketOh = 2.23f;//5.5
    public float procketEn = 0.86f;//0.26
    public float procketOh = 0.09f;
    [HeaderAttribute("Energy Gains")]//Collectibles
    public float energyBallGet = 9f;
    public float medkitEnergyGet = 26f;
    public float medkitUEnergyGet = 40f;
    public float medkitHpAmnt = 25f;
    public float medkitUHpAmnt = 62f;
    public float pwrupEnergyGet = 36f;
    public float enForPwrupRefill = 25f;
    public float enPwrupDuplicate = 42f;
    public float refillEnergyAmnt=110f;
    private void Awake() {
        instance=this;
    }
    void Start(){
        
    }

    void Update(){
        
    }
}
