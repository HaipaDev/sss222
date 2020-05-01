using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour{
    float dmg = 10;
    float dmgLaser = 5f;
    float dmgPhaser = 0.1f;
    float dmgHRocket = 13.5f;
    float dmgMiniLaser = 0.32f;
    float dmgLSaber = 1.55f;
    float dmgLClaws = 26f;
    float dmgShadowBT = 40.5f;
    float dmgQRocket = 14.5f;
    float dmgPRocket = 0f;
    float dmgPRocketExpl = 0.5f;
    float dmgCBullet = 1f;

    float dmgComet = 47;
    float dmgBat = 36;
    float dmgEnemyShip1 = 80;
    float dmgSoundwave = 16;
    float dmgEBt = 24;
    float dmgLeech = 4f;
    float dmgHLaser = 46f;

    public float GetDamage(){return dmg;}
    public float GetDamageLaser(){return dmgLaser;}
    public float GetDamagePhaser(){return dmgPhaser;}
    public float GetDamageHRocket(){return dmgHRocket; }
    public float GetDamageMiniLaser(){return dmgMiniLaser; }
    public float GetDamageLSaber(){return dmgLSaber; }
    public float GetDamageLClaws(){return dmgLClaws; }
    public float GetDamageShadowBT(){return dmgShadowBT; }
    public float GetDamageQRocket(){return dmgQRocket; }
    public float GetDamagePRocket(){return dmgPRocket; }
    public float GetDamagePRocketExpl(){return dmgPRocketExpl; }
    public float GetDamageCBullet(){return dmgCBullet; }


    public float GetDamageComet(){return dmgComet;}
    public float GetDamageBat(){return dmgBat;}
    public float GetDamageEnemyShip1(){return dmgEnemyShip1;}
    public float GetDamageLeech(){return dmgLeech; }
    public float GetDamageHLaser(){return dmgHLaser; }
    

    public float GetDamageSoundwave(){return dmgSoundwave;}
    public float GetDamageEBt(){return dmgEBt;}

    

    public void Hit(){
        Destroy(gameObject);
    }
}
