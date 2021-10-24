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
            if(other.gameObject.name.Contains(GameAssets.instance.Get("Laser").name)){dmg=damageValues.GetDmgLaser();Destroy(other.gameObject);AudioManager.instance.Play("EnemyHit");}
            if(other.gameObject.name.Contains(GameAssets.instance.Get("MLaser").name)){
                    AudioManager.instance.Play("MLaserHit");
                    /*var mlaserHitSound=other.GetComponent<RandomSound>().sound;
                    if(other.GetComponent<RandomSound>().playLimitForThis==true){mlaserHitSound=other.GetComponent<RandomSound>().sound2;}
                    AudioSource.PlayClipAtPoint(mlaserHitSound, new Vector2(transform.position.x, transform.position.y));*/
                dmg=damageValues.GetDmgMiniLaser();Destroy(other.gameObject);
            }
            if (other.gameObject.name.Contains(GameAssets.instance.Get("HRocket").name)){dmg=damageValues.GetDmgHRocket();Destroy(other.gameObject);AudioManager.instance.Play("HRocketHit");
                GameAssets.instance.VFX("ExplosionSmall", new Vector2(transform.position.x,transform.position.y-0.5f),0.3f);
            }
            if(other.gameObject.name.Contains(GameAssets.instance.Get("LSaber").name)){dmg=(float)System.Math.Round(damageValues.GetDmgLSaberIni(),2);AudioManager.instance.Play("LSaberHit");}
            if(other.gameObject.name.Contains(GameAssets.instance.Get("LClaws").name)){dmg=(float)System.Math.Round(damageValues.GetDmgLClawsTouch(),2);AudioManager.instance.Play("LClawsHit"); if(Player.instance!=null)Player.instance.energy-=1f;}
            if(other.gameObject.name.Contains(GameAssets.instance.Get("LClawsVFX").name)){dmg=damageValues.GetDmgLClaws();AudioManager.instance.Play("LClawsHit");}
            if(other.gameObject.name.Contains(GameAssets.instance.Get("ShadowBt").name)){dmg=damageValues.GetDmgShadowBT();Player.instance.Damage(1,dmgType.shadow);}
            if(other.gameObject.name.Contains(GameAssets.instance.Get("CBullet").name)){dmg=damageValues.GetDmgCBullet();AudioManager.instance.Play("CStreamHit");}
            if(other.gameObject.name.Contains(GameAssets.instance.Get("QRocket").name)){dmg=damageValues.GetDmgQRocket();Destroy(other.gameObject);AudioManager.instance.Play("QRocketHit");
                GameAssets.instance.VFX("ExplosionSmall", new Vector2(transform.position.x,transform.position.y-0.5f),0.3f);
            }
            if(other.gameObject.name.Contains(GameAssets.instance.Get("PRocket").name)){dmg=damageValues.GetDmgPRocket();}
            if(other.gameObject.name.Contains(GameAssets.instance.Get("PLaser").name)){dmg=other.GetComponent<DamageOverDist>().dmg;if(transform.GetComponent<EnemyHacked>()==null){transform.gameObject.AddComponent<EnemyHacked>();}Destroy(other.gameObject,0.01f);AudioManager.instance.Play("PLaserHit");}
            if(other.gameObject.name.Contains(GameAssets.instance.Get("Plasma").name)){dmg=damageValues.GetDmgPRocketExpl();transform.GetComponent<Rigidbody2D>().velocity=Vector2.up*6f;transform.GetComponent<Enemy>().yeeted=true;}
            if(other.gameObject.name.Contains(GameAssets.instance.Get("MPulse").name)){dmg=damageValues.GetDmgMPulse();AudioManager.instance.Play("MLaserHit");}

            if(other.gameObject.name.Contains(GameAssets.instance.GetVFX("BFlameDMG").name)){dmg=damageValues.GetDmgFlame();}
            if(other.gameObject.name.Contains(GameAssets.instance.GetVFX("BFlameBlueDMG").name)&&!transform.gameObject.name.Contains("Comet")){dmg=-damageValues.GetDmgBlueFlame();}else if(other.gameObject.name.Contains(GameAssets.instance.GetVFX("BFlameBlueDMG").name)&&transform.gameObject.name.Contains("Comet")){dmg=0;}
        #endregion
        }if(collis.Contains(colliTypes.enemies)){
        #region//Enemies bodies
            var player=transform.GetComponent<Player>();

            if(other.GetComponent<Tag_DmgPhaseFreq>()!=null)destroy=false;
            if(other.gameObject.name.Contains(GameAssets.instance.Get("Comet").name)){if(other.GetComponent<CometRandomProperties>()!=null){if(other.GetComponent<CometRandomProperties>().damageBySpeedSize){dmg=(float)System.Math.Round(damageValues.GetDmgComet()*Mathf.Abs(other.GetComponent<Rigidbody2D>().velocity.y)*other.transform.localScale.x,1);}else{dmg=damageValues.GetDmgComet();}}else{dmg=damageValues.GetDmgComet();}}
            if(other.gameObject.name.Contains(GameAssets.instance.Get("Bat").name)){dmg=damageValues.GetDmgBat();}
            if(other.gameObject.name.Contains(GameAssets.instance.Get("EnShip").name)){dmg=damageValues.GetDmgEnemyShip1();}
            if(other.gameObject.name.Contains(GameAssets.instance.Get("EnComb").name)){destroy=false;}
            if(other.gameObject.name.Contains(GameAssets.instance.Get("Goblin").name)){dmg=damageValues.GetDmgGoblin();}
            if(other.gameObject.name.Contains(GameAssets.instance.Get("HDrone").name)){dmg=damageValues.GetDmgHealDrone();}
            if(other.gameObject.name.Contains(GameAssets.instance.Get("Vortex").name)){dmg=damageValues.GetDmgVortex();}
            if(other.gameObject.name.Contains(GameAssets.instance.Get("Stinger").name)){dmg=damageValues.GetDmgStinger();player.Weaken(damageValues.GetEfxStinger().x,damageValues.GetEfxStinger().y);}
            if(other.gameObject.name.Contains(GameAssets.instance.Get("GlareDevil").name)){dmg=damageValues.GetDmgGoblin();player.Fragile(damageValues.GetEfxGlareDev().x,damageValues.GetEfxGlareDev().y); player.Weaken(damageValues.GetEfxGlareDev().x,damageValues.GetEfxGlareDev().y);}
        #endregion
        }if(collis.Contains(colliTypes.enemyWeapons)){
        #region//Enemy Weapons
            var player=transform.GetComponent<Player>();

            if(other.gameObject.name.Contains(GameAssets.instance.Get("Soundwave").name)){dmg=damageValues.GetDmgSoundwave(); AudioManager.instance.Play("SoundwaveHit");en=false;}
            if(other.gameObject.name.Contains(GameAssets.instance.Get("EnBt").name)){dmg=damageValues.GetDmgEBt();en=false;player.Hack(4);}
            if(other.gameObject.name.Contains(GameAssets.instance.Get("EnSaber").name)){dmg=damageValues.GetDmgEnSaber();en=false;}
            if(other.gameObject.name.Contains("StickBomb")){dmg=0;en=false;destroy=false;}
            
            if(other.gameObject.name.Contains(GameAssets.instance.Get("GoblinBt").name)){dmg=damageValues.GetDmgGoblinBt(); /*player.Blind(3,2);*/player.Fragile(damageValues.GetEfxGoblinBt().x,damageValues.GetEfxGoblinBt().y); player.Hack(damageValues.GetEfxGoblinBt().x*0.9f); AudioManager.instance.Play("GoblinBtHit");en=false;}
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
            if(other.gameObject.name.Contains(GameAssets.instance.Get("Phaser").name)){dmg=damageValues.GetDmgPhaser();AudioManager.instance.Play("PhaserHit");}
            if(other.gameObject.name.Contains(GameAssets.instance.Get("LSaber").name)){dmg=damageValues.GetDmgLSaber();AudioManager.instance.Play("EnemyHit");}
            if(other.gameObject.name.Contains(GameAssets.instance.Get("Plasma").name)){dmg=damageValues.GetDmgPRocketExpl();}//AudioSource.PlayClipAtPoint(procketHitSFX, new Vector2(transform.position.x, transform.position.y));}
            if(other.gameObject.name.Contains(GameAssets.instance.Get("CBullet").name)){dmg=damageValues.GetDmgCBullet();AudioManager.instance.Play("CStreamHit");}
            if(other.gameObject.name.Contains(GameAssets.instance.Get("LClaws").name)){dmg=(float)System.Math.Round(damageValues.GetDmgLSaber()/3,2); Player.instance.energy-=0.1f;}//AudioSource.PlayClipAtPoint(lclawsHitSFX, new Vector2(transform.position.x, transform.position.y));}
            if(other.gameObject.name.Contains(GameAssets.instance.Get("MPulse").name)){dmg=0;AudioManager.instance.Play("PRocketHit");}
            if(other.gameObject.name.Contains(GameAssets.instance.GetVFX("BFlameDMG").name)){dmg=damageValues.GetDmgFlame();}
            if(other.gameObject.name.Contains(GameAssets.instance.GetVFX("BFlameBlueDMG").name)&&!transform.gameObject.name.Contains("Comet")){dmg=-damageValues.GetDmgBlueFlame();}else if(other.gameObject.name.Contains(GameAssets.instance.GetVFX("BFlameBlueDMG").name)&&transform.gameObject.name.Contains("Comet")){dmg=0;}
        #endregion
        }if(collis.Contains(colliTypes.enemies)){
        #region//Enemies bodies
            if(other.gameObject.name.Contains(GameAssets.instance.Get("Leech").name)){dmg=damageValues.GetDmgLeech();AudioManager.instance.Play("LeechBite");}
            if(other.gameObject.name.Contains(GameAssets.instance.Get("HLaser").name)){dmg=damageValues.GetDmgHLaser();}
            if(other.gameObject.name.Contains(GameAssets.instance.Get("VLaser").name)){dmg=damageValues.GetDmgVLaser();}
        #endregion
        }if(collis.Contains(colliTypes.enemyWeapons)){
        #region//Enemy Weapons
            if(other.gameObject.name.Contains(GameAssets.instance.Get("EnSaber").name)){dmg=damageValues.GetDmgEnSaber();}
            if(other.gameObject.name.Contains(GameAssets.instance.GetVFX("BFlameBlueDMG").name)){dmg=-damageValues.GetDmgBlueFlame();}
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