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
    int dmgComet = 37;
    int dmgBat = 26;
    int dmgEnemyShip1 = 80;
    int dmgSoundwave = 8;
    int dmgEBt = 20;

    public float GetDamage(){return dmg;}
    public float GetDamageLaser(){return dmgLaser;}
    public float GetDamagePhaser(){return dmgPhaser;}
    public float GetDamageHRocket(){return dmgHRocket; }
    public float GetDamageMiniLaser(){return dmgMiniLaser; }
    public float GetDamageLSaber(){return dmgLSaber; }
    public float GetDamageShadowBT(){return dmgShadowBT; }


    public int GetDamageComet(){return dmgComet;}
    public int GetDamageBat(){return dmgBat;}
    public int GetDamageEnemyShip1(){return dmgEnemyShip1;}
    

    public int GetDamageSoundwave(){return dmgSoundwave;}
    public int GetDamageEBt(){return dmgEBt;}

    

    public void Hit(){
        Destroy(gameObject);
    }
}
