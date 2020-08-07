using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum dmgType{
    normal,
    silent,
    flame,
    shadow,
    decay,
    electr,
    heal
}
public class DamageDealer : MonoBehaviour{
    public static DamageDealer instance;
    float dmg = 5;
    float dmgZone = 2;
    float dmgLaser = 5f;
    float dmgPhaser = 0.5f;
    float dmgHRocket = 13.5f;
    float dmgMiniLaser = 0.32f;
    float dmgLSaber = 0.86f;
    float dmgLClaws = 7f;
    float dmgShadowBT = 40.5f;
    float dmgQRocket = 14.5f;
    float dmgPRocket = 0f;
    float dmgPRocketExpl = 0.5f;
    float dmgCBullet = 2f;
    //PLaser damage is inside the obj
    float dmgMPulse = 130f;

    float dmgComet = 10f;
    float dmgBat = 36f;
    float dmgSoundwave = 16.5f;
    float dmgEnemyShip1 = 80f;
    float dmgEBt = 24.5f;
    float dmgEnemySaber = 2.5f;    
    float dmgGoblin = 16f;
    float dmgHealDrone = 75f;
    float dmgVortex = 70f;
    float dmgLeech = 4f;
    float dmgHLaser = 90f;
    float dmgStinger = 33.3f;
    float dmgGoblinBt = 7f;

    public float GetDmg(){return dmg;}
    public float GetDmgZone(){return dmgZone;}

    public float GetDmgLaser(){return dmgLaser;}
    public float GetDmgPhaser(){return dmgPhaser;}
    public float GetDmgHRocket(){return dmgHRocket; }
    public float GetDmgMiniLaser(){return dmgMiniLaser; }
    public float GetDmgLSaber(){return dmgLSaber; }
    public float GetDmgLClaws(){return dmgLClaws; }
    public float GetDmgShadowBT(){return dmgShadowBT; }
    public float GetDmgQRocket(){return dmgQRocket; }
    public float GetDmgPRocket(){return dmgPRocket; }
    public float GetDmgPRocketExpl(){return dmgPRocketExpl; }
    public float GetDmgCBullet(){return dmgCBullet; }
    
    public float GetDmgMPulse(){return dmgMPulse; }


    public float GetDmgComet(){return dmgComet;}
    public float GetDmgBat(){return dmgBat;}
    public float GetDmgSoundwave(){return dmgSoundwave;}
    public float GetDmgEnemyShip1(){return dmgEnemyShip1;}
    public float GetDmgEBt(){return dmgEBt;}
    public float GetDmgEnSaber(){return dmgEnemySaber;}
    public float GetDmgGoblin(){return dmgGoblin;}
    public float GetDmgHealDrone(){return dmgHealDrone;}
    public float GetDmgVortex(){return dmgVortex;}
    public float GetDmgLeech(){return dmgLeech; }
    public float GetDmgHLaser(){return dmgHLaser; }
    public float GetDmgStinger(){return dmgStinger; } 
    public float GetDmgGoblinBt(){return dmgGoblinBt; } 

    public void Hit(){
        Destroy(gameObject);
    }
}
