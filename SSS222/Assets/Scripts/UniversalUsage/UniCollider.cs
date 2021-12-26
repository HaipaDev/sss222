using System;
//using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniCollider : MonoBehaviour{
    public static List<colliTypes> colliTypesForEn=new List<colliTypes>{colliTypes.player,colliTypes.playerWeapons,colliTypes.world};
    public static List<colliTypes> colliTypesForPl=new List<colliTypes>{colliTypes.enemies,colliTypes.enemyWeapons,colliTypes.world,colliTypes.zone};

    public static float TriggerCollision(Collider2D other, Transform transform, List<colliTypes> collis, bool triggerStay=false){
        float dmg=0;
        if(other.GetComponent<Player>()==null&&other.GetComponent<Tag_Collectible>()==null&&other.GetComponent<Shredder>()==null){
            DamageValues dmgVal;
            dmgVal=GetDmgVal(other.gameObject.name);
            if(dmgVal==null){dmgVal=GetDmgValAbs(other.gameObject.name.Split('_')[0]);}
            if(dmgVal!=null){if(collis.Contains(dmgVal.colliType)){
                dmg=dmgVal.dmg;if(triggerStay)dmg=dmgVal.dmgPhase;
                if(dmgVal.dmgBySize&&!dmgVal.dmgBySpeed){dmg*=((other.gameObject.transform.localScale.x+other.gameObject.transform.localScale.y)/2);}
                else if(!dmgVal.dmgBySize&&dmgVal.dmgBySpeed){dmg*=Mathf.Abs(other.GetComponent<Rigidbody2D>().velocity.magnitude);}
                else if(dmgVal.dmgBySize&&dmgVal.dmgBySpeed){dmg*=((other.gameObject.transform.localScale.x+other.gameObject.transform.localScale.y)/2)*Mathf.Abs(other.GetComponent<Rigidbody2D>().velocity.magnitude);}
                
                foreach(colliEvents co in dmgVal.colliEvents){
                    if(!String.IsNullOrEmpty(co.vfx)){if(GameAssets.instance.GetVFX(co.vfx)!=null)Instantiate(GameAssets.instance.GetVFX(co.vfx),(Vector2)transform.position+co.vfxPos,Quaternion.identity);}
                    if(co.dmgPlayer!=0){if(Player.instance!=null){Player.instance.Damage(co.dmgPlayer,co.dmgPlayerType);}}
                }
                
                if(!dmgVal.phase){Destroy(other.gameObject,0.01f);}
                else{var dmgPhaseFreq=other.GetComponent<Tag_DmgPhaseFreq>();if(dmgPhaseFreq==null){dmgPhaseFreq=other.gameObject.AddComponent<Tag_DmgPhaseFreq>();}
                    dmgPhaseFreq.phaseFreqFirst=dmgVal.phaseFreqFirst;
                    dmgPhaseFreq.phaseFreq=dmgVal.phaseFreq;
                    dmgPhaseFreq.phaseCountLimit=dmgVal.phaseCountLimit;
                    dmgPhaseFreq.soundPhase=dmgVal.soundPhase;
                    if(dmgVal.soundPhase=="-"){dmgPhaseFreq.soundPhase=dmgVal.sound;}
                    else if(dmgVal.soundPhase=="."){dmgPhaseFreq.soundPhase="EnemyHit";}
                }
                if(!triggerStay)if(!String.IsNullOrEmpty(dmgVal.sound))AudioManager.instance.Play(dmgVal.sound);
            }
            }//else{Debug.LogWarning("DamageValues not defined for "+other.gameObject.name);}
        }
        #region //Old
        /*if(collis.Contains(colliTypes.playerWeapons)){
        #region//Player Weapons
            if(other.gameObject.name.Contains(GameAssets.instance.Get("Laser").name)){dmg=dmgVal.dmgLaser;AudioManager.instance.Play("EnemyHit");}
            if(other.gameObject.name.Contains(GameAssets.instance.Get("MLaser").name)){
                    AudioManager.instance.Play("MLaserHit");
                    /*var mlaserHitSound=other.GetComponent<RandomSound>().sound;
                    if(other.GetComponent<RandomSound>().playLimitForThis==true){mlaserHitSound=other.GetComponent<RandomSound>().sound2;}
                    AudioSource.PlayClipAtPoint(mlaserHitSound, new Vector2(transform.position.x, transform.position.y));*/
            /*    dmg=dmgVal.dmgMiniLaser;
            }
            if (other.gameObject.name.Contains(GameAssets.instance.Get("HRocket").name)){dmg=dmgVal.dmgHRocket;AudioManager.instance.Play("HRocketHit");
                GameAssets.instance.VFX("ExplosionSmall", new Vector2(transform.position.x,transform.position.y-0.5f),0.3f);
            }
            if(other.gameObject.name.Contains(GameAssets.instance.Get("LSaber").name)){dmg=(float)System.Math.Round(dmgVal.dmgLSaberIni,2);AudioManager.instance.Play("LSaberHit");}
            if(other.gameObject.name.Contains(GameAssets.instance.Get("LClaws").name)){dmg=(float)System.Math.Round(dmgVal.dmgLClawsTouch,2);AudioManager.instance.Play("LClawsHit");Player.instance.energy-=1f;}
            if(other.gameObject.name.Contains(GameAssets.instance.Get("LClawsVFX").name)){dmg=dmgVal.dmgLClaws;AudioManager.instance.Play("LClawsHit");}
            if(other.gameObject.name.Contains(GameAssets.instance.Get("GloomyScythe").name)){dmg=dmgVal.dmgGloomyScythes;
            if(GameRules.instance.dmgGloomyScythes_player!=0){Player.instance.Damage(dmgVal.dmgGloomyScythes_player,dmgType.shadow);}}
            if(other.gameObject.name.Contains(GameAssets.instance.Get("ShadowBt").name)){dmg=dmgVal.dmgShadowBT;
            if(GameRules.instance.dmgShadowBT_player!=0){Player.instance.Damage(dmgVal.dmgShadowBT_player,dmgType.shadow);}}
            if(other.gameObject.name.Contains(GameAssets.instance.Get("CBullet").name)){dmg=dmgVal.dmgCBullet;AudioManager.instance.Play("CStreamHit");}
            if(other.gameObject.name.Contains(GameAssets.instance.Get("QRocket").name)){dmg=dmgVal.dmgQRocket;AudioManager.instance.Play("QRocketHit");
                GameAssets.instance.VFX("ExplosionSmall", new Vector2(transform.position.x,transform.position.y-0.5f),0.3f);
            }
            if(other.gameObject.name.Contains(GameAssets.instance.Get("PRocket").name)){dmg=dmgVal.dmgPRocket;}
            if(other.gameObject.name.Contains(GameAssets.instance.Get("Plasma").name)){dmg=dmgVal.dmgPRocketExpl;transform.GetComponent<Rigidbody2D>().velocity=Vector2.up*6f;transform.GetComponent<Enemy>().yeeted=true;}
            if(other.gameObject.name.Contains(GameAssets.instance.Get("PLaser").name)){dmg=other.GetComponent<DamageOverDist>().dmg;if(transform.GetComponent<EnemyHacked>()==null){transform.gameObject.AddComponent<EnemyHacked>();}Destroy(other.gameObject,0.01f);AudioManager.instance.Play("PLaserHit");}
            if(other.gameObject.name.Contains(GameAssets.instance.Get("MPulse").name)){dmg=dmgVal.dmgMPulse;AudioManager.instance.Play("MLaserHit");}

            if(other.gameObject.name.Contains(GameAssets.instance.GetVFX("BFlameDMG").name)){dmg=dmgVal.dmgShipFlame;}
            if(other.gameObject.name.Contains(GameAssets.instance.GetVFX("BFlameBlueDMG").name)&&!transform.gameObject.name.Contains("Comet")){dmg=-dmgVal.dmgBlueFlame;}else if(other.gameObject.name.Contains(GameAssets.instance.GetVFX("BFlameBlueDMG").name)&&transform.gameObject.name.Contains("Comet")){dmg=0;}
            if(other.gameObject.name.Contains(GameAssets.instance.Get("Phaser").name)){dmg=dmgVal.dmgPhaser;AudioManager.instance.Play("PhaserHit");}
            if(other.gameObject.name.Contains(GameAssets.instance.Get("LSaber").name)){dmg=dmgVal.dmgLSaber;AudioManager.instance.Play("EnemyHit");}
            if(other.gameObject.name.Contains(GameAssets.instance.Get("Plasma").name)){dmg=dmgVal.dmgPRocketExpl;}//AudioSource.PlayClipAtPoint(procketHitSFX, new Vector2(transform.position.x, transform.position.y));}
            if(other.gameObject.name.Contains(GameAssets.instance.Get("CBullet").name)){dmg=dmgVal.dmgCBullet;AudioManager.instance.Play("CStreamHit");}
            if(other.gameObject.name.Contains(GameAssets.instance.Get("LClaws").name)){dmg=(float)System.Math.Round(dmgVal.dmgLSaber/3,2);Player.instance.energy-=0.1f;}//AudioSource.PlayClipAtPoint(lclawsHitSFX, new Vector2(transform.position.x, transform.position.y));}
            //if(other.gameObject.name.Contains(GameAssets.instance.Get("MPulse").name)){dmg=0;AudioManager.instance.Play("PRocketHit");}
            if(other.gameObject.name.Contains(GameAssets.instance.GetVFX("BFlameDMG").name)){dmg=dmgVal.dmgShipFlame;}
            if(other.gameObject.name.Contains(GameAssets.instance.GetVFX("BFlameBlueDMG").name)&&!transform.gameObject.name.Contains("Comet")){dmg=-dmgVal.dmgBlueFlame;}else if(other.gameObject.name.Contains(GameAssets.instance.GetVFX("BFlameBlueDMG").name)&&transform.gameObject.name.Contains("Comet")){dmg=0;}
        #endregion
        }if(collis.Contains(colliTypes.enemies)){
        #region//Enemies bodies
            Player player=null;
            if(transform.GetComponent<Player>()!=null){player=transform.GetComponent<Player>();}

            if(other.gameObject.name.Contains(GameAssets.instance.Get("Comet").name)){if(other.GetComponent<CometRandomProperties>()!=null){if(other.GetComponent<CometRandomProperties>().damageBySpeedSize){dmg=(float)System.Math.Round(dmgVal.dmgComet*Mathf.Abs(other.GetComponent<Rigidbody2D>().velocity.y)*other.transform.localScale.x,1);}else{dmg=dmgVal.dmgComet;}}else{dmg=dmgVal.dmgComet;}}
            if(other.gameObject.name.Contains(GameAssets.instance.Get("Bat").name)){dmg=dmgVal.dmgBat;}
            if(other.gameObject.name.Contains(GameAssets.instance.Get("EnShip").name)){dmg=dmgVal.dmgEnemyShip1;}
            if(other.gameObject.name.Contains(GameAssets.instance.Get("EnComb").name)){dmg=0;}
            if(other.gameObject.name.Contains(GameAssets.instance.Get("Goblin").name)){dmg=dmgVal.dmgGoblin;if(player!=null){player.Hack(dmgVal.efxGoblinBt.x*0.75f);}}
            if(other.gameObject.name.Contains(GameAssets.instance.Get("HDrone").name)){dmg=dmgVal.dmgHealDrone;}
            if(other.gameObject.name.Contains(GameAssets.instance.Get("Vortex").name)){dmg=dmgVal.dmgVortex;}
            if(other.gameObject.name.Contains(GameAssets.instance.Get("Stinger").name)){dmg=dmgVal.dmgStinger;if(player!=null){player.Weaken(dmgVal.efxStinger.x,dmgVal.efxStinger.y);}}
            if(other.gameObject.name.Contains(GameAssets.instance.Get("GlareDevil").name)){dmg=dmgVal.dmgGlareDev;if(player!=null){player.Blind(dmgVal.efxGlareDev.x,dmgVal.efxGlareDev.y);player.Weaken(dmgVal.efxGlareDev.x,dmgVal.efxGlareDev.y);}}
            if(other.gameObject.name.Contains(GameAssets.instance.Get("Leech").name)){dmg=dmgVal.dmgLeech;AudioManager.instance.Play("LeechBite");}
            if(other.gameObject.name.Contains(GameAssets.instance.Get("HLaser").name)){dmg=dmgVal.dmgHLaser;}
            if(other.gameObject.name.Contains(GameAssets.instance.Get("VLaser").name)){dmg=dmgVal.dmgVLaser;}
        #endregion
        }if(collis.Contains(colliTypes.enemyWeapons)){
        #region//Enemy Weapons
            Player player=null;
            if(transform.GetComponent<Player>()!=null){player=transform.GetComponent<Player>();}

            if(other.gameObject.name.Contains(GameAssets.instance.Get("Soundwave").name)){dmg=dmgVal.dmgSoundwave;AudioManager.instance.Play("SoundwaveHit");}
            if(other.gameObject.name.Contains(GameAssets.instance.Get("EnBt").name)){dmg=dmgVal.dmgEBt;if(player!=null)player.Hack(4);}
            if(other.gameObject.name.Contains(GameAssets.instance.Get("EnSaber").name)){dmg=dmgVal.dmgEnSaber;}
            if(other.gameObject.name.Contains("StickBomb")){dmg=0;}
            
            if(other.gameObject.name.Contains(GameAssets.instance.Get("GoblinBt").name)){dmg=dmgVal.dmgGoblinBt; if(player!=null){/*player.Blind(3,2);*//*player.Fragile(dmgVal.efxGoblinBt.x,dmgVal.efxGoblinBt.y);player.Hack(dmgVal.efxGoblinBt.x);}AudioManager.instance.Play("GoblinBtHit");;}
            /*if(other.gameObject.name.Contains(GameAssets.instance.Get("EnSaber").name)){dmg=dmgVal.dmgEnSaber;}
            if(other.gameObject.name.Contains(GameAssets.instance.GetVFX("BFlameBlueDMG").name)){dmg=-dmgVal.dmgBlueFlame;}
            if(other.gameObject.name.Contains("StickBomb")){dmg=0;}
        #endregion
        }*/
        #endregion
        return dmg;
    }

    public static void DMG_VFX(int type,Collider2D other, Transform transform, float dmg, int colorDef=ColorInt32.dmgColor){
    if(other.GetComponent<Player>()==null&&other.GetComponent<Tag_Collectible>()==null&&other.GetComponent<Shredder>()==null){
        DamageValues dmgVal=UniCollider.GetDmgVal(other.gameObject.name);
        if(type==0){//Enemy - TriggerEnter
            GameAssets.instance.VFX("FlareHit", new Vector2(transform.position.x,transform.position.y-0.5f),0.3f);
            if(GameSession.instance.dmgPopups==true&&dmg>0){
                if(dmgVal!=null&&dmgVal.dispDmgCount){
                    if(transform.GetComponent<Enemy>()!=null){
                    transform.GetComponent<Enemy>().dmgCount+=dmg;
                    if(transform.GetComponent<Enemy>().dmgCounted==false)transform.GetComponent<Enemy>().DispDmgCount(other.transform.position);
                    }
                }else{
                    GameCanvas.instance.DMGPopup(dmg,other.transform.position,ColorInt32.Int2Color(colorDef));
                }
            }else if(GameSession.instance.dmgPopups==true&&dmg<0){
                GameCanvas.instance.DMGPopup(dmg,other.transform.position,ColorInt32.Int2Color(ColorInt32.dmgHealColor));
            }
        }else if(type==1){//Enemy - TriggerStay
            GameAssets.instance.VFX("FlareHit", new Vector2(transform.position.x,transform.position.y-0.5f),0.3f);
            if(GameSession.instance.dmgPopups==true&&dmg>0){
                GameCanvas.instance.DMGPopup(dmg,other.transform.position,ColorInt32.Int2Color(ColorInt32.dmgPhaseColor));
            }else if(GameSession.instance.dmgPopups==true&&dmg<0){
                GameCanvas.instance.DMGPopup(-dmg,other.transform.position,ColorInt32.Int2Color(ColorInt32.dmgHealColor));
            }
        }else if(type==2){//Player - TriggerEnter
            var player=Player.instance;
            if(GameSession.instance.dmgPopups==true&&dmg!=0&&!player.gclover&&!player.dashing){
                GameCanvas.instance.DMGPopup(dmg,other.transform.position,ColorInt32.Int2Color(ColorInt32.dmgPlayerColor),2,true);
            }
        }else if(type==3){//Player - TriggerStay
            var player=Player.instance;
            if(GameSession.instance.dmgPopups==true&&dmg>0&&!player.gclover&&!player.dashing){GameCanvas.instance.DMGPopup(dmg,other.transform.position,Color.red,2,true);}
            else if(dmg<0){GameCanvas.instance.DMGPopup(dmg,other.transform.position,ColorInt32.Int2Color(ColorInt32.dmgHealColor),1.5f,true);}
        }
    }}
    public static DamageValues GetDmgVal(string objName){
        DamageValues dmgVal=null;
        List<GObject> assets=new List<GObject>();
        foreach(GObject gobj in GameAssets.instance.objects){assets.Add(gobj);}
        foreach(GObject vfx in GameAssets.instance.vfx){assets.Add(vfx);}
        var asset=assets.Find(x=>objName.Contains(x.gobj.name));
        if(asset!=null)dmgVal=GameRules.instance.dmgValues.Find(x=>x.name==asset.name);
        if(dmgVal==null){dmgVal=GetDmgValAbs(objName.Split('_')[0]);}
        if(dmgVal!=null)return dmgVal;
        else Debug.LogWarning("DamageValues not defined for "+objName);return null;
    }
    public static DamageValues GetDmgValAbs(string dmgValName){
        DamageValues dmgVal=null;
        List<GObject> assets=new List<GObject>();
        dmgVal=GameRules.instance.dmgValues.Find(x=>x.name==dmgValName);
        if(dmgVal!=null)return dmgVal;
        else Debug.LogWarning("DamageValuesAbs not defined for "+dmgValName);return null;
    }
}

public enum colliTypes{player,playerWeapons,enemies,enemyWeapons,world,zone}