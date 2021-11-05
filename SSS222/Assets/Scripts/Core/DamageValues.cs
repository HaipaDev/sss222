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
    public float dmgFlame = 1;
    public float dmgZone = 2;
    public float dmgLaser = 5f;
    public float dmgPhaser = 0.5f;
    public float dmgHRocket = 13.5f;
    public float dmgMiniLaser = 0.32f;
    public float dmgLSaber = 0.86f;
    public float dmgLSaberIni = 7.77f;
    public float dmgLClaws = 7f;
    public float dmgLClawsTouch = 0.23f;
    public float dmgGloomyScythes=40.5f;
    public float dmgGloomyScythes_player=1f;
    public float dmgShadowBT = 40.5f;
    public float dmgShadowBT_player = 1f;
    public float dmgQRocket = 14.5f;
    public float dmgPRocket = 0f;
    public float dmgPRocketExpl = 0.5f;
    public float dmgCBullet = 2f;
    public float dmgPlaser = 6.78f;
    //PLaser damage is inside the obj
    public float dmgMPulse = 130f;

    public float dmgComet = 10f;
    public float dmgBlueFlame = 0.2f;
    public float dmgBat = 36f;
    public float dmgSoundwave = 16.5f;
    public float dmgEnemyShip1 = 80f;
    public float dmgEBt = 24.5f;
    public float dmgEnSaber = 2.5f;    
    public float dmgGoblin = 16f;
    public float dmgHealDrone = 75f;
    public float dmgVortex = 70f;
    public float dmgLeech = 4f;
    public float dmgVLaser = 90f;
    public float dmgHLaser = 16f;
    public float dmgStinger = 33.3f;
    public Vector2 efxStinger = new Vector2(20,1);
    public float dmgGoblinBt = 7f;
    public Vector2 efxGoblinBt = new Vector2(6,0.8f);
    public float dmgGlareDev = 8f;
    public Vector2 efxGlareDev = new Vector2(1.5f,2f);

#region//Set Values
private void Awake(){
instance=this;
StartCoroutine(SetValues());}
public IEnumerator SetValues(){
yield return new WaitForSeconds(0.08f);
var i=GameRules.instance;
if(i!=null){
    //dmgFlame=i.dmgFlame;
    dmgZone=i.dmgZone;
    dmgLaser=i.dmgLaser;
    dmgPhaser=i.dmgPhaser;
    dmgHRocket=i.dmgHRocket;
    dmgMiniLaser=i.dmgMiniLaser;
    dmgLSaber=i.dmgLSaber;
    dmgLSaberIni=i.dmgLSaberIni;
    dmgLClaws=i.dmgLClaws;
    dmgLClawsTouch=i.dmgLClawsTouch;
    dmgGloomyScythes=i.dmgGloomyScythes;
    dmgGloomyScythes_player=i.dmgGloomyScythes_player;
    dmgShadowBT=i.dmgShadowBT;
    dmgShadowBT_player=i.dmgShadowBT_player;
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
    dmgEnSaber=i.dmgEnSaber;    
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
    dmgGlareDev=i.dmgGlareDev;
    efxGlareDev=i.efxGlareDev;
}
#endregion
}
}