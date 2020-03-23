using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour{
    float dmg = 10;
    float dmgLaser = 5f;
    float dmgPhaser = 0.1f;
    float dmgHRocket = 7f;
    float dmgMiniLaser = 0.75f;
    float dmgLSaber = 3.5f;
    float dmgShadowBT = 40.5f;
    float dmgComet = 37;
    float dmgBat = 26;
    float dmgEnemyShip1 = 80;
    float dmgSoundwave = 8;
    float dmgEBt = 20;
    float dmgLeech = 4f;

    public float GetDamage(){return dmg;}
    public float GetDamageLaser(){return dmgLaser;}
    public float GetDamagePhaser(){return dmgPhaser;}
    public float GetDamageHRocket(){return dmgHRocket; }
    public float GetDamageMiniLaser(){return dmgMiniLaser; }
    public float GetDamageLSaber(){return dmgLSaber; }
    public float GetDamageShadowBT(){return dmgShadowBT; }


    public float GetDamageComet(){return dmgComet;}
    public float GetDamageBat(){return dmgBat;}
    public float GetDamageEnemyShip1(){return dmgEnemyShip1;}
    public float GetDamageLeech(){return dmgLeech; }
    

    public float GetDamageSoundwave(){return dmgSoundwave;}
    public float GetDamageEBt(){return dmgEBt;}

    

    public void Hit(){
        Destroy(gameObject);
    }
}
