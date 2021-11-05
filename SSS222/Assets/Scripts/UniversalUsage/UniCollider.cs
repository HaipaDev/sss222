using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniCollider : MonoBehaviour{
    public static List<colliTypes> colliTypesForEn=new List<colliTypes>{colliTypes.player,colliTypes.playerWeapons};
    public static List<colliTypes> colliTypesForPl=new List<colliTypes>{colliTypes.enemies,colliTypes.enemyWeapons};
    //public float dmgTimer;
    public static float[] TriggerEnter(Collider2D other, Transform transform, List<colliTypes> collis){
        var damageValues=DamageValues.instance;
        float dmg=0;
        bool en=true;
        bool destroy=true;
        if(collis.Contains(colliTypes.player)){
            
        }
        if(collis.Contains(colliTypes.playerWeapons)){
        #region//Player Weapons
            if(other.gameObject.name.Contains(GameAssets.instance.Get("Laser").name)){dmg=damageValues.dmgLaser;Destroy(other.gameObject);AudioManager.instance.Play("EnemyHit");}
            if(other.gameObject.name.Contains(GameAssets.instance.Get("MLaser").name)){
                    AudioManager.instance.Play("MLaserHit");
                    /*var mlaserHitSound=other.GetComponent<RandomSound>().sound;
                    if(other.GetComponent<RandomSound>().playLimitForThis==true){mlaserHitSound=other.GetComponent<RandomSound>().sound2;}
                    AudioSource.PlayClipAtPoint(mlaserHitSound, new Vector2(transform.position.x, transform.position.y));*/
                dmg=damageValues.dmgMiniLaser;Destroy(other.gameObject);
            }
            if (other.gameObject.name.Contains(GameAssets.instance.Get("HRocket").name)){dmg=damageValues.dmgHRocket;Destroy(other.gameObject);AudioManager.instance.Play("HRocketHit");
                GameAssets.instance.VFX("ExplosionSmall", new Vector2(transform.position.x,transform.position.y-0.5f),0.3f);
            }
            if(other.gameObject.name.Contains(GameAssets.instance.Get("LSaber").name)){dmg=(float)System.Math.Round(damageValues.dmgLSaberIni,2);AudioManager.instance.Play("LSaberHit");}
            if(other.gameObject.name.Contains(GameAssets.instance.Get("LClaws").name)){dmg=(float)System.Math.Round(damageValues.dmgLClawsTouch,2);AudioManager.instance.Play("LClawsHit");Player.instance.energy-=1f;}
            if(other.gameObject.name.Contains(GameAssets.instance.Get("LClawsVFX").name)){dmg=damageValues.dmgLClaws;AudioManager.instance.Play("LClawsHit");}
            if(other.gameObject.name.Contains(GameAssets.instance.Get("GloomyScythe").name)){dmg=damageValues.dmgGloomyScythes;
            if(GameRules.instance.dmgGloomyScythes_player!=0){Player.instance.Damage(damageValues.dmgGloomyScythes_player,dmgType.shadow);}}
            if(other.gameObject.name.Contains(GameAssets.instance.Get("ShadowBt").name)){dmg=damageValues.dmgShadowBT;
            if(GameRules.instance.dmgShadowBT_player!=0){Player.instance.Damage(damageValues.dmgShadowBT_player,dmgType.shadow);}}
            if(other.gameObject.name.Contains(GameAssets.instance.Get("CBullet").name)){dmg=damageValues.dmgCBullet;AudioManager.instance.Play("CStreamHit");}
            if(other.gameObject.name.Contains(GameAssets.instance.Get("QRocket").name)){dmg=damageValues.dmgQRocket;Destroy(other.gameObject);AudioManager.instance.Play("QRocketHit");
                GameAssets.instance.VFX("ExplosionSmall", new Vector2(transform.position.x,transform.position.y-0.5f),0.3f);
            }
            if(other.gameObject.name.Contains(GameAssets.instance.Get("PRocket").name)){dmg=damageValues.dmgPRocket;}
            if(other.gameObject.name.Contains(GameAssets.instance.Get("PLaser").name)){dmg=other.GetComponent<DamageOverDist>().dmg;if(transform.GetComponent<EnemyHacked>()==null){transform.gameObject.AddComponent<EnemyHacked>();}Destroy(other.gameObject,0.01f);AudioManager.instance.Play("PLaserHit");}
            if(other.gameObject.name.Contains(GameAssets.instance.Get("Plasma").name)){dmg=damageValues.dmgPRocketExpl;transform.GetComponent<Rigidbody2D>().velocity=Vector2.up*6f;transform.GetComponent<Enemy>().yeeted=true;}
            if(other.gameObject.name.Contains(GameAssets.instance.Get("MPulse").name)){dmg=damageValues.dmgMPulse;AudioManager.instance.Play("MLaserHit");}

            if(other.gameObject.name.Contains(GameAssets.instance.GetVFX("BFlameDMG").name)){dmg=damageValues.dmgFlame;}
            if(other.gameObject.name.Contains(GameAssets.instance.GetVFX("BFlameBlueDMG").name)&&!transform.gameObject.name.Contains("Comet")){dmg=-damageValues.dmgBlueFlame;}else if(other.gameObject.name.Contains(GameAssets.instance.GetVFX("BFlameBlueDMG").name)&&transform.gameObject.name.Contains("Comet")){dmg=0;}
        #endregion
        }if(collis.Contains(colliTypes.enemies)){
        #region//Enemies bodies
            Player player=null;
            if(transform.GetComponent<Player>()!=null){player=transform.GetComponent<Player>();}

            if(other.GetComponent<Tag_DmgPhaseFreq>()!=null)destroy=false;
            if(other.gameObject.name.Contains(GameAssets.instance.Get("Comet").name)){if(other.GetComponent<CometRandomProperties>()!=null){if(other.GetComponent<CometRandomProperties>().damageBySpeedSize){dmg=(float)System.Math.Round(damageValues.dmgComet*Mathf.Abs(other.GetComponent<Rigidbody2D>().velocity.y)*other.transform.localScale.x,1);}else{dmg=damageValues.dmgComet;}}else{dmg=damageValues.dmgComet;}}
            if(other.gameObject.name.Contains(GameAssets.instance.Get("Bat").name)){dmg=damageValues.dmgBat;}
            if(other.gameObject.name.Contains(GameAssets.instance.Get("EnShip").name)){dmg=damageValues.dmgEnemyShip1;}
            if(other.gameObject.name.Contains(GameAssets.instance.Get("EnComb").name)){destroy=false;}
            if(other.gameObject.name.Contains(GameAssets.instance.Get("Goblin").name)){dmg=damageValues.dmgGoblin;if(player!=null){player.Hack(damageValues.efxGoblinBt.x*0.75f);}}
            if(other.gameObject.name.Contains(GameAssets.instance.Get("HDrone").name)){dmg=damageValues.dmgHealDrone;}
            if(other.gameObject.name.Contains(GameAssets.instance.Get("Vortex").name)){dmg=damageValues.dmgVortex;}
            if(other.gameObject.name.Contains(GameAssets.instance.Get("Stinger").name)){dmg=damageValues.dmgStinger;if(player!=null){player.Weaken(damageValues.efxStinger.x,damageValues.efxStinger.y);}}
            if(other.gameObject.name.Contains(GameAssets.instance.Get("GlareDevil").name)){dmg=damageValues.dmgGlareDev;if(player!=null){player.Blind(damageValues.efxGlareDev.x,damageValues.efxGlareDev.y);player.Weaken(damageValues.efxGlareDev.x,damageValues.efxGlareDev.y);}}
        #endregion
        }if(collis.Contains(colliTypes.enemyWeapons)){
        #region//Enemy Weapons
            Player player=null;
            if(transform.GetComponent<Player>()!=null){player=transform.GetComponent<Player>();}

            if(other.gameObject.name.Contains(GameAssets.instance.Get("Soundwave").name)){dmg=damageValues.dmgSoundwave; AudioManager.instance.Play("SoundwaveHit");en=false;}
            if(other.gameObject.name.Contains(GameAssets.instance.Get("EnBt").name)){dmg=damageValues.dmgEBt;en=false;if(player!=null)player.Hack(4);}
            if(other.gameObject.name.Contains(GameAssets.instance.Get("EnSaber").name)){dmg=damageValues.dmgEnSaber;en=false;}
            if(other.gameObject.name.Contains("StickBomb")){dmg=0;en=false;destroy=false;}
            
            if(other.gameObject.name.Contains(GameAssets.instance.Get("GoblinBt").name)){dmg=damageValues.dmgGoblinBt; if(player!=null){/*player.Blind(3,2);*/player.Fragile(damageValues.efxGoblinBt.x,damageValues.efxGoblinBt.y);player.Hack(damageValues.efxGoblinBt.x*0.9f);}AudioManager.instance.Play("GoblinBtHit");en=false;}
        #endregion
        }

        float destroyF=1;if(destroy==false)destroyF=0;
        float enF=1;if(en==false)enF=0;
        return new float[]{dmg,destroyF,enF};
    }
    public static float TriggerStay(Collider2D other, Transform transform, List<colliTypes> collis){
        var damageValues=DamageValues.instance;
        float dmg=0;
        if(collis.Contains(colliTypes.player)){
            
        }if(collis.Contains(colliTypes.playerWeapons)){
        #region//PlayerWeapons
            if(other.gameObject.name.Contains(GameAssets.instance.Get("Phaser").name)){dmg=damageValues.dmgPhaser;AudioManager.instance.Play("PhaserHit");}
            if(other.gameObject.name.Contains(GameAssets.instance.Get("LSaber").name)){dmg=damageValues.dmgLSaber;AudioManager.instance.Play("EnemyHit");}
            if(other.gameObject.name.Contains(GameAssets.instance.Get("Plasma").name)){dmg=damageValues.dmgPRocketExpl;}//AudioSource.PlayClipAtPoint(procketHitSFX, new Vector2(transform.position.x, transform.position.y));}
            if(other.gameObject.name.Contains(GameAssets.instance.Get("CBullet").name)){dmg=damageValues.dmgCBullet;AudioManager.instance.Play("CStreamHit");}
            if(other.gameObject.name.Contains(GameAssets.instance.Get("LClaws").name)){dmg=(float)System.Math.Round(damageValues.dmgLSaber/3,2);Player.instance.energy-=0.1f;}//AudioSource.PlayClipAtPoint(lclawsHitSFX, new Vector2(transform.position.x, transform.position.y));}
            if(other.gameObject.name.Contains(GameAssets.instance.Get("MPulse").name)){dmg=0;AudioManager.instance.Play("PRocketHit");}
            if(other.gameObject.name.Contains(GameAssets.instance.GetVFX("BFlameDMG").name)){dmg=damageValues.dmgFlame;}
            if(other.gameObject.name.Contains(GameAssets.instance.GetVFX("BFlameBlueDMG").name)&&!transform.gameObject.name.Contains("Comet")){dmg=-damageValues.dmgBlueFlame;}else if(other.gameObject.name.Contains(GameAssets.instance.GetVFX("BFlameBlueDMG").name)&&transform.gameObject.name.Contains("Comet")){dmg=0;}
        #endregion
        }if(collis.Contains(colliTypes.enemies)){
        #region//Enemies bodies
            if(other.gameObject.name.Contains(GameAssets.instance.Get("Leech").name)){dmg=damageValues.dmgLeech;AudioManager.instance.Play("LeechBite");}
            if(other.gameObject.name.Contains(GameAssets.instance.Get("HLaser").name)){dmg=damageValues.dmgHLaser;}
            if(other.gameObject.name.Contains(GameAssets.instance.Get("VLaser").name)){dmg=damageValues.dmgVLaser;}
        #endregion
        }if(collis.Contains(colliTypes.enemyWeapons)){
        #region//Enemy Weapons
            if(other.gameObject.name.Contains(GameAssets.instance.Get("EnSaber").name)){dmg=damageValues.dmgEnSaber;}
            if(other.gameObject.name.Contains(GameAssets.instance.GetVFX("BFlameBlueDMG").name)){dmg=-damageValues.dmgBlueFlame;}
            if(other.gameObject.name.Contains("StickBomb")){dmg=0;}
        #endregion
        }

        return dmg;
    }

    public static void DMG_VFX(int type,Collider2D other, Transform transform, float dmg, int colorDef=ColorInt32.dmgColor){
        if(type==0){//Enemy - TriggerEnter
            GameAssets.instance.VFX("FlareHit", new Vector2(transform.position.x,transform.position.y-0.5f),0.3f);
            if(GameSession.instance.dmgPopups==true&&dmg>0){
                if(!other.gameObject.name.Contains(GameAssets.instance.Get("PLaser").name)){
                    GameCanvas.instance.DMGPopup(dmg,other.transform.position,ColorInt32.Int2Color(colorDef));
                }else{
                    if(transform.GetComponent<Enemy>()!=null){
                    transform.GetComponent<Enemy>().dmgCount+=dmg;
                    if(transform.GetComponent<Enemy>().dmgCounted==false)transform.GetComponent<Enemy>().DispDmgCount(other.transform.position);
                    }
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
    }
}


public enum colliTypes{
    player,playerWeapons,enemies,enemyWeapons
}