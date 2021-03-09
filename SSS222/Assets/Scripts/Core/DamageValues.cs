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
    heal,
    healSilent
}
public class DamageValues : MonoBehaviour{
    public static DamageValues instance;
    float dmg = 5;
    float dmgFlame = 1;
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
    float dmgPlaser = 6.78f;
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
    float dmgVLaser = 90f;
    float dmgHLaser = 16f;
    float dmgStinger = 33.3f;
    Vector2 efxStinger = new Vector2(20,1);
    float dmgGoblinBt = 7f;
    Vector2 efxGoblinBt = new Vector2(6,0.8f);
    Vector2 efxGlareDev = new Vector2(1.5f,2f);

#region//Set Values
private void Awake(){
instance=this;
StartCoroutine(SetValues());}
public IEnumerator SetValues(){
yield return new WaitForSeconds(0.08f);
var i=GameRules.instance;
if(i!=null){
    dmg=i.dmg;
    //dmgFlame=i.dmgFlame;
    dmgZone=i.dmgZone;
    dmgLaser=i.dmgLaser;
    dmgPhaser=i.dmgPhaser;
    dmgHRocket=i.dmgHRocket;
    dmgMiniLaser=i.dmgMiniLaser;
    dmgLSaber=i.dmgLSaber;
    dmgLClaws=i.dmgLClaws;
    dmgShadowBT=i.dmgShadowBT;
    dmgQRocket=i.dmgQRocket;
    dmgPRocket=i.dmgPRocket;
    dmgPRocketExpl=i.dmgPRocketExpl;
    dmgCBullet=i.dmgCBullet;
    dmgPlaser=i.dmgPlaser;
    dmgMPulse=i.dmgMPulse;

    dmgComet=i.dmgComet;
    dmgBat=i.dmgBat;
    dmgSoundwave=i.dmgSoundwave;
    dmgEnemyShip1=i.dmgEnemyShip1;
    dmgEBt=i.dmgEBt;
    dmgEnemySaber=i.dmgEnemySaber;    
    dmgGoblin=i.dmgGoblin;
    dmgHealDrone=i.dmgHealDrone;
    dmgVortex=i.dmgVortex;
    dmgLeech=i.dmgLeech;
    dmgVLaser=i.dmgVLaser;
    dmgHLaser=i.dmgHLaser;
    dmgStinger=i.dmgStinger;
    efxStinger=i.efxStinger;
    dmgGoblinBt=i.dmgGoblinBt;
    efxGoblinBt=i.efxGoblinBt;
    efxGlareDev=i.efxGlareDev;
}
}
#endregion
#region//Get Functions
    public float GetDmg(){return dmg;}
    public float GetDmgFlame(){return dmgFlame;}
    public float GetDmgZone(){return dmgZone;}

    public float GetDmgLaser(){return dmgLaser;}
    public float GetDmgPhaser(){return dmgPhaser;}
    public float GetDmgHRocket(){return dmgHRocket;}
    public float GetDmgMiniLaser(){return dmgMiniLaser;}
    public float GetDmgLSaber(){return dmgLSaber;}
    public float GetDmgLClaws(){return dmgLClaws;}
    public float GetDmgShadowBT(){return dmgShadowBT;}
    public float GetDmgQRocket(){return dmgQRocket;}
    public float GetDmgPRocket(){return dmgPRocket;}
    public float GetDmgPRocketExpl(){return dmgPRocketExpl;}
    public float GetDmgCBullet(){return dmgCBullet;}
    public float GetPLaserDMG(){return dmgPlaser;}
    
    public float GetDmgMPulse(){return dmgMPulse;}


    public float GetDmgComet(){return dmgComet;}
    public float GetDmgBat(){return dmgBat;}
    public float GetDmgSoundwave(){return dmgSoundwave;}
    public float GetDmgEnemyShip1(){return dmgEnemyShip1;}
    public float GetDmgEBt(){return dmgEBt;}
    public float GetDmgEnSaber(){return dmgEnemySaber;}
    public float GetDmgGoblin(){return dmgGoblin;}
    public float GetDmgHealDrone(){return dmgHealDrone;}
    public float GetDmgVortex(){return dmgVortex;}
    public float GetDmgLeech(){return dmgLeech;}
    public float GetDmgVLaser(){return dmgVLaser;}
    public float GetDmgHLaser(){return dmgHLaser;}
    public float GetDmgStinger(){return dmgStinger;}
    public Vector2 GetEfxStinger(){return efxStinger;}
    public float GetDmgGoblinBt(){return dmgGoblinBt;}
    public Vector2 GetEfxGoblinBt(){return efxGoblinBt;}
    public Vector2 GetEfxGlareDev(){return efxGlareDev;}
#endregion

}
